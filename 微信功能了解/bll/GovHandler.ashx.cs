using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.bll
{
    /// <summary>
    /// GovHandler 的摘要说明
    /// </summary>
    public class GovHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request.QueryString["method"];
            switch(method)
            {
                //注册管局
                

            }
            context.Response.Write("Hello World");
        }


        private void GovReg(HttpContext context)
        {
            string username = context.Request.Params["username"];
            string password = context.Request.Params["password"];
            string govname = context.Request.Params["govname"];
            string address = context.Request.Params["address"];
            ///todo: 数据验证
            ///

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