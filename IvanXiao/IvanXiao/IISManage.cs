using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using System.IO;
using System.Xml;
using System.Windows.Forms;


namespace IvanXiao
{
    public class IISManage
    {
        /// <summary>
        /// 创建站点
        /// </summary>
        /// <param name="SiteName">站点的名称</param>
        /// <param name="rootdir">站点的物理地址</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public bool CreateSite(string SiteName, string rootdir, int port = 80)
        {
            ServerManager Manager = new ServerManager();
            //判断网站是否存在
            if (!ExistSite(SiteName))
            {
                try
                {
                    //创建网站
                    Manager.Sites.Add(SiteName, "http", "*:" + port + ":", rootdir);

                    //创建应用程序池
                    Manager.ApplicationPoolDefaults.ManagedRuntimeVersion = "v4.0";
                    Manager.ApplicationPoolDefaults.Enable32BitAppOnWin64 = true;
                    ApplicationPool appPool = Manager.ApplicationPools.Add(SiteName);
                    appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                    appPool.AutoStart = true;


                    //分配应用程序池给网站
                    Manager.Sites[SiteName].Applications[0].ApplicationPoolName = SiteName;
                    Manager.Sites[SiteName].ServerAutoStart = true;
                    Manager.CommitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("因为重复部署,IIS WEB站点或者应用程序池没有删除!\r\n" + e.ToString());
                    return false;
                }

            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 创建虚拟目录
        /// </summary>
        /// <param name="SiteName">站点名称</param>
        /// <param name="virtualPath">虚拟目录的路径例如 放在网站根目录  /image</param>
        /// <param name="physicalPath">虚拟目录的物理路径</param>
        public void CreateVirtualDirectorie(string SiteName, string virtualPath, string physicalPath)
        {
            ServerManager Manager = new ServerManager();
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }
            Site web = Manager.Sites[SiteName];
            //web.Applications.
            Microsoft.Web.Administration.Application App = web.Applications["/"];
            App.VirtualDirectories.Add(
                virtualPath, physicalPath);


            //Application app = web.Applications["/"+SiteName];
            // app.VirtualDirectories.Add(virtualPath, physicalPath);
            //Manager.Sites[SiteName].Applications["/"].VirtualDirectories.Add(virtualPath, physicalPath);
            Manager.CommitChanges();

        }

        /// <summary>
        /// 停止站点
        /// </summary>
        /// <param name="SiteName">站点名称</param>
        /// <returns></returns>
        public bool StopSite(string SiteName)
        {
            if (ExistSite(SiteName))
            {
                try
                {
                    ServerManager Manager = new ServerManager();
                    Site web = Manager.Sites[SiteName];
                    web.Stop();
                    //Site web = Manager.Sites.SingleOrDefault(c => c.Name == SiteName);
                    //if(web!=null)
                    //{
                    //    Manager.Sites[SiteName].Stop();
                    //    Manager.CommitChanges();

                    //}
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 删除站点和对应的应用程序池
        /// </summary>
        /// <param name="SiteName">网站名称</param>
        public bool RemoveSite(string SiteName)
        {
            //先判断WEB站点是否存在,如果存在才可以执行删除
            if (ExistSite(SiteName))
            {
                ServerManager Manager = new ServerManager();

                try
                {
                    //删除站点
                    Site site = Manager.Sites[SiteName];
                    Manager.Sites.Remove(site);
                    //删除应用程序池
                    ApplicationPool pool = Manager.ApplicationPools[SiteName];
                    Manager.ApplicationPools.Remove(pool);
                    Manager.CommitChanges();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            if (ExistSite(SiteName))
            {
                return false;
            }
            else
            {
                return true;
            }



        }


        /// <summary>
        /// 判断网站是否存在
        /// </summary>
        /// <param name="SiteName">站点的名称</param>
        /// <returns></returns>
        public bool ExistSite(string SiteName)
        {
            try
            {
                //通过查看配置文件的方法查看网站是否存在,因为使用Microsoft.web.administrator类来处理的时候,在windows 2008系统上面会报错
                string webconfigfile = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\inetsrv\config\applicationHost.config";
                if (File.Exists(webconfigfile))
                {
                    string[] sitenames = GetSites(webconfigfile);
                    foreach (string name in sitenames)
                    {
                        if (name == SiteName) return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
                //return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// 获得所有的网站名称
        /// </summary>
        /// <param name="iisconfigfilePath">IIS配置文件的路径</param>
        /// <returns></returns>
        public string[] GetSites(string iisconfigfilePath)
        {
            List<string> names = new List<string>();
            if (File.Exists(iisconfigfilePath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreComments = true;
                    XmlReader reader = XmlReader.Create(iisconfigfilePath, settings);
                    xmlDoc.Load(iisconfigfilePath);

                    XmlNodeList sitesList = xmlDoc.SelectNodes("/configuration/system.applicationHost/sites/site");
                    if (sitesList != null)
                    {


                        for (int i = 0; i < sitesList.Count; i++)
                        {
                            names.Add(sitesList[i].Attributes["name"].Value);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            return names.ToArray();
        }

        /// <summary>
        /// 获取所有站点的名称
        /// </summary>
        /// <returns></returns>
        public string[] GetSites()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\inetsrv\config\applicationHost.config";
            return GetSites(path);

        }


    }
}
