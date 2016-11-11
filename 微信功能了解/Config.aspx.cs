using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace web
{
    public partial class Config : System.Web.UI.Page
    {
        //public string access_token;
        protected void Page_Load(object sender, EventArgs e)
        {
            //string appid = ConfigurationManager.AppSettings["WeChatAppId"];
            //string secret = ConfigurationManager.AppSettings["WeChatSecret"];

            ////得到access_token
            //Helper.WXServices ws = new Helper.WXServices();
            //access_token= ws.GetAccess_token(appid,secret);
        }
    }
}