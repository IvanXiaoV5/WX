using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Controller;

namespace web.test
{
    /// <summary>
    /// Webserver 的摘要说明
    /// </summary>
    public class Webserver : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request.QueryString["method"];
            string response = "";
            switch(method)
            {
                case "pushMenu":
                    response= PushMenu(context);
                    break;
            }
            context.Response.Write(response);
        }

        public string PushMenu(HttpContext context)
        {
            string menudata= context.Request.Params["menudata"];

            Controller.MsgCtrl col = new MsgCtrl();
            //菜单
            string appid = ConfigurationManager.AppSettings["appid"];
            string appsecret = ConfigurationManager.AppSettings["appsecret"];
            string tmp= col.PushMenu(appid, appsecret, menudata);
            return tmp;

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}