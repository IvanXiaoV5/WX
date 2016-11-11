using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Helper
{
    public class WriteLog
    {
        public static  void Write(string msg,TypeEmum te)
        {
            string tmp = System.Configuration.ConfigurationManager.AppSettings["LogPath"]+"";
            if(string.IsNullOrWhiteSpace(tmp))
            {
                tmp = "/xiaoxiong/log/";
            }
            string filename= HttpContext.Current.Server.MapPath(tmp);
            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
            filename+= DateTime.Now.ToString("D") + "log.txt";

            //每天创建一个日志文件
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                switch(te)
                {
                    case TypeEmum.Request:
                        sw.WriteLine("================================Request Start==========================================");
                        sw.WriteLine(DateTime.Now);
                        sw.Write(msg);
                        sw.WriteLine("================================Request End==========================================");
                        break;
                    case TypeEmum.Response:
                        sw.WriteLine("================================Response Start==========================================");
                        sw.WriteLine(DateTime.Now);
                        sw.Write(msg);
                        sw.WriteLine("");
                        sw.WriteLine("================================Response End==========================================");
                        sw.WriteLine("");
                        break;
                }

                

            }
        }

        public static string ReadLogForHtml()
        {
            string tmp = System.Configuration.ConfigurationManager.AppSettings["LogPath"] + "";
            if (string.IsNullOrWhiteSpace(tmp))
            {
                tmp = "/xiaoxiong/log/";
            }
            string filename = HttpContext.Current.Server.MapPath(tmp);

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
            filename += DateTime.Now.ToString("D") + "log.txt";


            if(File.Exists(filename))
            {
                string html = string.Empty;
                using (StreamReader sr = new StreamReader(filename))
                {
                    //return sr.ReadToEnd();
                    StringBuilder sb = new StringBuilder();
                    
                    while (!string.IsNullOrWhiteSpace(html = sr.ReadLine()))
                    {
                        sb.Append(HttpContext.Current.Server.HtmlEncode( html)+"<br/>");
                    }
                    return sb.ToString();
                }
                
                    
            }

            return filename+"(文件不存在!)";
            

        }


        public static bool DelTodayLog()
        {
            string tmp = System.Configuration.ConfigurationManager.AppSettings["LogPath"] + "";
            if (string.IsNullOrWhiteSpace(tmp))
            {
                tmp = "/xiaoxiong/log/";
            }
            string filename = HttpContext.Current.Server.MapPath(tmp);

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
            filename += DateTime.Now.ToString("D") + "log.txt";

            if(File.Exists(filename))
            {
                //删除文件
                FileInfo file = new FileInfo(filename);
                try
                {
                    file.Delete();
                }
                catch
                {
                    return false;
                }
                
            }

            return true;
        }
    }
}
