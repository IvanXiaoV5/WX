using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Helper;
using System.Net;
using System.IO;
using System.Text;
using Models;
using Controller;


namespace 微信功能了解.test
{
    /// <summary>
    /// token 的摘要说明
    /// </summary>
    public class token : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            WXServices ser = new WXServices();
            Controller.MsgCtrl col = new MsgCtrl();
            if(context.Request.HttpMethod.ToLower()=="get")
            {
                string signature = context.Request.QueryString["signature"] + "";
                string timestamp = context.Request.QueryString["timestamp"] + "";
                string nonce = context.Request.QueryString["nonce"] + "";
                string echostr = context.Request.QueryString["echostr"] + "";
                string token = ConfigurationManager.AppSettings["token"];
                
                bool iswx = ser.IsWXserver(timestamp, nonce, token, signature);
                if (iswx)
                {
                    context.Response.Write(echostr);
                }
            }
            
            if(context.Request.HttpMethod.ToLower()=="post")
            {
                col.RecAndSend(context);
            }
            

            ////菜单
            //string appid=ConfigurationManager.AppSettings["appid"];
            //string appsecret=ConfigurationManager.AppSettings["appsecret"];
            //col.PushMenu(appid, appsecret,"");

            
            

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