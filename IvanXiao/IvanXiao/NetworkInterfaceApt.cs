using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Data;

namespace IvanXiao
{
    public class NetworkInterfaceApt
    {
        /// <summary>
        /// 方法处理完以后的消息
        /// </summary>
        public string log = string.Empty;



        /// <summary>
        /// 获取电脑所有网卡的名称，IP地址，MAC地址
        /// </summary>
        /// <returns></returns>
        public DataTable GetNetworkAdapter()
        {
            //用一个表格保存信息
            DataTable iptable = new DataTable("网卡IP地址表");
            iptable.Columns.Add("网卡名称");
            iptable.Columns.Add("IP地址");
            iptable.Columns.Add("MAC地址");

            // List<string> networkstr = new List<string>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                //显示网络适配器描述信息、名称、类型、速度、MAC 地址  
                //networkstr.Add("名称：" + adapter.Name + "\tIP:" + string.Join(" | ", GetIp(adapter)) + "\tMAC地址：" + adapter.GetPhysicalAddress());
                DataRow dr = iptable.NewRow();
                dr["网卡名称"] = adapter.Name;
                dr["IP地址"] = string.Join(" | ", GetIp(adapter));
                dr["MAC地址"] = adapter.GetPhysicalAddress();
                iptable.Rows.Add(dr);

            }
            log = "已经加载所有网卡，修改IP需要管理员权限，如果修改失败请确认使用管理员权限启动程序！";
            return iptable;


        }

        /// <summary>
        /// 查找到网卡的IPV4地址
        /// </summary>
        /// <param name="apt">NetworkInterface 的对象</param>
        /// <returns></returns>
        private string[] GetIp(NetworkInterface apt)
        {
            //string ipadd = string.Empty;
            List<string> result = new List<string>();

            IPInterfaceProperties ip = apt.GetIPProperties();
            UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
            foreach (UnicastIPAddressInformation uip in ipCollection)
            {

                //MessageBox.Show();
                //uip.Address.AddressFamily
                if (uip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    result.Add(uip.Address.ToString());
                    //有多个IP地址的时候用| 隔开。
                    //ipadd += string.IsNullOrEmpty(ipadd) ? uip.Address.ToString() : "|" + uip.Address.ToString();
                }

            }
            return result.ToArray();
        }

        /// <summary>
        /// 修改指定网卡的IP地址，True表示修改成功，false表示修改失败
        /// </summary>
        /// <param name="adapterID">网卡ID</param>
        /// <param name="ip">需要修改的IP地址</param>
        /// <param name="submark">需要修改的子网掩码</param>
        /// <returns></returns>
        private bool SetInfo(string InterfaceName, string adapterID, string ip, string submark)
        {
            ManagementBaseObject newIp = null;
            ManagementBaseObject outIp = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc2 = mc.GetInstances();
            foreach (ManagementObject mo in moc2)
            {
                if (mo["SettingID"].ToString() == adapterID)
                {
                    string[] ipArray;
                    string[] submarkArray;
                    //如果ID一样，表示是需要修改的网卡。
                    //需要判断是否有多个IP地址
                    if (ip.IndexOf('|') > 0)
                    {
                        ipArray = ip.Split('|');
                        submarkArray = submark.Split('|');
                        newIp = mo.GetMethodParameters("EnableStatic");
                        newIp["IPAddress"] = new string[] { ipArray[0] };
                        newIp["SubnetMask"] = new string[] { submarkArray[0] };
                        outIp = mo.InvokeMethod("EnableStatic", newIp, null);

                        for (int i = 1; i < ipArray.Length; i++)
                        {
                            string cmd = string.Format("netsh interface ip add address {0} {1} {2}", InterfaceName, ipArray[i], submarkArray[i]);
                            Process p = new Process();
                            p.StartInfo.FileName = "cmd.exe";
                            p.StartInfo.UseShellExecute = false;   //關閉Shell的使用
                            p.StartInfo.RedirectStandardInput = true;  //重定向標準輸入
                            p.StartInfo.RedirectStandardOutput = true;   //重定向標準輸出
                            p.StartInfo.CreateNoWindow = true;  //設置不顯示窗口
                            p.Start();
                            p.StandardInput.WriteLine(cmd);
                            p.StandardInput.WriteLine("exit");

                        }
                    }
                    else
                    {
                        ipArray = new string[] { ip };
                        submarkArray = new string[] { submark };
                        newIp = mo.GetMethodParameters("EnableStatic");
                        newIp["IPAddress"] = ipArray;
                        newIp["SubnetMask"] = submarkArray;
                        outIp = mo.InvokeMethod("EnableStatic", newIp, null);
                    }
                    mo.Dispose();
                    //判断是否修改成功，如果现在网卡的IP和参数里面的IP地址一样，那么表示修改成功。
                    Thread.Sleep(500);
                    bool l = IsConfig(adapterID, ipArray);
                    //bool l= outIp["IPAddress"] == newIp["IPAddress"]?true:false;

                    return l;
                }
                mo.Dispose();
            }

            return false;

        }

        /// <summary>
        /// 判断两个IP地址是否一样，一样为TRUE 不一样为FALSE
        /// </summary>
        /// <param name="adapterID">网卡的ID</param>
        /// <param name="ip">需要和网卡对比的IP地址</param>
        /// <returns></returns>
        private bool IsConfig(string adapterID, string[] ip)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface apt in adapters)
            {
                if (apt.Id == adapterID)
                {

                    string[] newiparry = GetIp(apt);
                    if (newiparry.Length == ip.Length)
                    {

                        IEnumerable<string> result = newiparry.Except(ip);
                        return result.ToArray().Length == 0 ? true : false;

                    }


                }
            }
            return false;
        }

        /// <summary>
        /// 外部使用方法，修改IP地址
        /// </summary>
        /// <param name="InterfaceName">网卡的名字</param>
        /// <param name="IPaddress">需要修改的IP地址</param>
        /// <param name="submark">需要修改的子网掩码</param>
        public void SetNetworkAdapter(string InterfaceName, string IPaddress, string submark)
        {

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.Name == InterfaceName)
                {

                    log = SetInfo(InterfaceName, adapter.Id, IPaddress, submark) ? adapter.Name + "的IP地址已经修改" : adapter.Name + "的IP地址修改失败";

                    return;
                }

            }

            log = "需要修改的网卡" + InterfaceName + "没有找到！请核对网卡名称！";

        }


    }
}
