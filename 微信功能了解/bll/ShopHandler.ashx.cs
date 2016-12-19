using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.bll
{
    /// <summary>
    /// ShopHandler 的摘要说明
    /// </summary>
    public class ShopHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.QueryString["method"];
            

            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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