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
                TextMsg msg = ser.Received_TextToModel(context);
                 string xmlstr= ser.SendText(msg);
                 context.Response.Write(xmlstr);
                
            }
            context.Response.Write("我是经过HelloHttpHandler处理的！");

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

            string appid = ConfigurationManager.AppSettings["appid"];
            string secret = ConfigurationManager.AppSettings["appsecret"];

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            //自定义菜单token的获取 是用 下面的两个参数 获取的 不能直接用 公众平台的token
            string to = GetData(url);
            // 得到的就是 token 或者 获取 json的token
            to = to.Substring(17, to.Length - 37);
            string i = GetPage("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + to, weixin1);

        }



        private string GetData(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        public string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
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