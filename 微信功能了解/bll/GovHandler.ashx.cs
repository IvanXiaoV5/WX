using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Web.SessionState;

namespace web.bll
{
    /// <summary>
    /// GovHandler 的摘要说明
    /// </summary>
    public class GovHandler : IHttpHandler, IRequiresSessionState
    {
        private Verify Verify = new Verify();
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string jsonStr = "";
            string method = context.Request.QueryString["method"];
            switch(method)
            {
                //注册管局
                case "govreg":
                    jsonStr= GovReg(context);
                    break;


            }
            context.Response.Write(jsonStr);
        }


        private string GovReg(HttpContext context)
        {
            hbxlsEntities dbContext = new hbxlsEntities();
            string username = context.Request.Params["username"];
            string password = context.Request.Params["password"];
            string govname = context.Request.Params["govname"];
            string address = context.Request.Params["address"];
            ///todo: 数据验证
            ///
            if(!(Verify.StringLength(username,2,64)&&Verify.StringLength(password,0,64)&&Verify.StringLength(govname,2,64)&&Verify.StringLength(address,0,256)))
            {
                
                return "{\"type\":\""+Common.Type.Error+"\",\"value\":\"输入错误！\"}";
            }


            //判断用户名和单位名称是否重复
            var govlist= dbContext.wx_xx_government.Where<wx_xx_government>(gov => (gov.Username == username||gov.Name==govname));
            if(govlist.Count<wx_xx_government>()>0)
            {
                return "{\"type\":\"" + Common.Type.Error + "\",\"value\":\"用户或者单位已经存在！\"}";
            }

            //保存用户
           
            wx_xx_government govuser = new wx_xx_government();
            govuser.Address = address;
            govuser.Username = username;
            govuser.Password = password;
            govuser.Name = govname;
            try
            {
                dbContext.wx_xx_government.Add(govuser);

                if(dbContext.SaveChanges()>0)
                {
                    HttpContext.Current.Session["login"]=govuser;
                    //context.Response.Redirect("main.aspx");
                    return "{\"type\":\"" + Common.Type.Success + "\",\"value\":\"main.aspx\"}";
                }
                else
                {
                    return "{\"type\":\"" + Common.Type.Error + "\",\"value\":\"写入数据库错误！\"}";
                }
                
            }
            catch(Exception msg)
            {
                return "{\"type\":\"" + Common.Type.Error + "\",\"value\":\""+msg.Message+"\"}";
            }
            
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