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
using Dos.WeChat;



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


            string xml = ser.GetPostString(context);
            WriteLog.Write(xml, TypeEmum.Request);

            if (context.Request.HttpMethod.ToLower()=="get")
            {
                //验证微信服务器
                //string signature = context.Request.QueryString["signature"] + "";
                //string timestamp = context.Request.QueryString["timestamp"] + "";
                //string nonce = context.Request.QueryString["nonce"] + "";
                //string echostr = context.Request.QueryString["echostr"] + "";
                //string token = ConfigurationManager.AppSettings["token"];
                
                //bool iswx = ser.IsWXserver(timestamp, nonce, token, signature);
                //if (iswx)
                //{
                //    context.Response.Write(echostr);
                //}
                if(!JoinToken.Join(new web.bll.MsgCall()))
                {
                    
                    //签名未通过
                }
            }
            
            
            if(context.Request.HttpMethod.ToLower()=="post")
            {
                
                //发送消息
                //col.RecAndSend(context);

               

                

                    web.bll.MsgCall msg = new web.bll.MsgCall();

                ResponseMsg resmsg = new ResponseMsg();
                 ReceiveMsg getmsg=   Dos.WeChat.ReceiveMsg.Parse(xml);
                switch(getmsg.MsgType)
                {
                    case EnumHelper.MsgType.Text:
                        resmsg = msg.TextMsgCall((RecTextMsg)getmsg);
                        break;
                    case EnumHelper.MsgType.Voice:
                        resmsg = msg.VoiceMsgCall((RecVoiceMsg)getmsg);
                        break;
                    case EnumHelper.MsgType.Image:
                        resmsg = msg.ImageMsgCall((RecImgMsg)getmsg);
                        break;
                    case EnumHelper.MsgType.Event:
                        resmsg = msg.EventMsgCall((RecEventMsg)getmsg);
                        break;
                }
                
                 xml= "<xml>"+resmsg.InnerToXml()+"</xml>";
                //把发送的信息也记录下来
                Helper.WriteLog.Write(xml, TypeEmum.Response);
                context.Response.Write(xml);
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