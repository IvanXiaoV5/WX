using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace web
{
    public partial class log : System.Web.UI.Page
    {
        public string logstr;
        protected void Page_Load(object sender, EventArgs e)
        {
             logstr= Helper.WriteLog.ReadLogForHtml();

            ViewState.Add("log", logstr);
        }

        protected void btn_dellog_Click(object sender, EventArgs e)
        {
            Helper.WriteLog.DelTodayLog();
            //刷新页面
            Response.Write("<script language=javascript>window.location.href=document.URL;</script>");
        }
    }
}