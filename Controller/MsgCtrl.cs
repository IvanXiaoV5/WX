using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Models;
using Helper;

namespace Controller
{
    public class MsgCtrl
    {
        private WXServices ser = new WXServices();

        /// <summary>
        /// 把接收到的文本消息又发送回去
        /// </summary>
        /// <param name="context"></param>
        public void RecAndSend(HttpContext context)
        {
            TextMsg msg = ser.Received_TextToModel(context);
            string xmlstr = ser.SendText(msg);
            context.Response.Write(xmlstr);
        }

        public string PushMenu(string appid,string appsecret,string MenuJson)
        {
            
            string weixin1 = "";
            weixin1 += "{\n";
            weixin1 += "\"button\":[\n";
            weixin1 += "{\n";
            weixin1 += "\"type\":\"click\",\n";
            weixin1 += "\"name\":\"今日歌曲\",\n";
            weixin1 += "\"key\":\"V1001_TODAY_MUSIC123eee\"\n";
            weixin1 += "},\n";
            weixin1 += "{\n";
            weixin1 += "\"type\":\"click\",\n";
            weixin1 += "\"name\":\"歌手简介\",\n";
            weixin1 += "\"key\":\"V1001_TODAY_SINGER123eee\"\n";
            weixin1 += "},\n";
            weixin1 += "{\n";
            weixin1 += "\"name\":\"菜单\",\n";
            weixin1 += "\"sub_button\":[\n";
            weixin1 += "{\n";
            weixin1 += "\"type\":\"click\",\n";
            weixin1 += "\"name\":\"hello word\",\n";
            weixin1 += "\"key\":\"V1001_HELLO_WORLD123eee\"\n";
            weixin1 += "},\n";
            weixin1 += "{\n";
            weixin1 += "\"type\":\"click\",\n";
            weixin1 += "\"name\":\"赞一下我们\",\n";
            weixin1 += "\"key\":\"V1001_GOOD123eee\"\n";
            weixin1 += "}]\n";
            weixin1 += "}]\n";
            weixin1 += "}\n";

            MenuJson = MenuJson == "" ? weixin1 : MenuJson;
            string access = ser.GetAccess_token(appid, appsecret);
            string context= ser.PushMenu(access, MenuJson);
            return context;
        }
    }
}
