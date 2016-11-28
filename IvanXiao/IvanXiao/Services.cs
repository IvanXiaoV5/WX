using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IvanXiao
{
    public class Services
    {

        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <param name="servicesName">服务的名字</param>
        /// <returns></returns>
        public bool ExitsServices(string servicesName)
        {

            bool ExistFlag = false;
            System.ServiceProcess.ServiceController[] ser = System.ServiceProcess.ServiceController.GetServices();
            foreach (var sp in ser)
            {
                if (sp.ServiceName == servicesName)
                {
                    ExistFlag = true;
                }
                Console.WriteLine(sp.ServiceName + "-----------" + sp.DisplayName);
            }
            return ExistFlag;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="servicesName">服务的名字</param>
        /// <returns></returns>
        public bool StartService(string servicesName)
        {
            System.ServiceProcess.ServiceController sr = GetServiceByName(servicesName);
            if (sr.Status != System.ServiceProcess.ServiceControllerStatus.Running)
            {
                try
                {
                    sr.Start();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private System.ServiceProcess.ServiceController GetServiceByName(string ServiceName)
        {
            System.ServiceProcess.ServiceController sr = null;
            if (!string.IsNullOrEmpty(ServiceName))
            {
                sr = new System.ServiceProcess.ServiceController(ServiceName);
            }
            return sr;
        }

        /// <summary>
        /// 获取服务的状态
        /// </summary>
        /// <param name="serviceName">服务的名称</param>
        /// <returns>0: 服务没有启动,1: 服务正在运行</returns>
        public int ServiceStatus(string serviceName)
        {
            int status = 0;
            System.ServiceProcess.ServiceController sr = GetServiceByName(serviceName);
            switch (sr.Status)
            {
                case System.ServiceProcess.ServiceControllerStatus.Running:
                    status = 1;
                    break;
                case System.ServiceProcess.ServiceControllerStatus.Stopped:
                    status = 0;
                    break;

            }

            return status;
        }
        /// <summary>
        /// 设置服务启动类型
        /// </summary>
        /// <param name="serviceName">服务名字</param>
        public void SetAutoRun(string serviceName)
        {
            string componentsKeyName = "SYSTEM\\CurrentControlSet\\services\\" + serviceName;
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(componentsKeyName, true);
            rk.SetValue("Start", 2);
        }
    }
}
