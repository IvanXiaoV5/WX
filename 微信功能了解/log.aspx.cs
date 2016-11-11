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
        protected void Page_Load(object sender, EventArgs e)
        {
            string log= Helper.WriteLog.ReadLogForHtml();

            ViewState.Add("log", log);
        }

        protected void btn_dellog_Click(object sender, EventArgs e)
        {
            Helper.WriteLog.DelTodayLog();
            //刷新页面
            Response.Write("<script language=javascript>window.location.href=document.URL;</script>");
        }
    }
}