using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Models;
using System.Net;

namespace Helper
{
    public class WXServices
    {
        /// <summary>
        /// 判断是否为微信服务器，通过timestamp,nonce,token字典排序以后SHA1加密得到的字符串和signature对比，如果一样表示为微信服务器，如果不一样表示伪造的消息
        /// </summary>
        /// <param name="timestamp">微信get过来的时间戳</param>
        /// <param name="nonce">微信get过来的随机数</param>
        /// <param name="token">留给微信的token字符串</param>
        /// <param name="signature">微信get过来的加密字符串</param>
        /// <returns>true 微信服务器</returns>
        public bool IsWXserver(string timestamp,string nonce,string token,string signature)
        {
            string[] arrTmp = { token, timestamp, nonce };
            Array.Sort(arrTmp);
            string tmpStr = string.Join("",arrTmp);
            tmpStr = EncryptHelper.Sha1(tmpStr);
            tmpStr = tmpStr.ToLower();
            signature = signature.ToLower();
            if(tmpStr.Equals(signature))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetPostString(HttpContext context)
        {
            if(!context.Request.HttpMethod.ToLower().Equals("post"))
            {
                //如果不是post就返回空字符串
                return "";
            }

            using(Stream stream=context.Request.InputStream)
            {
                Byte[] postByte=new Byte[stream.Length];
                stream.Read(postByte, 0, postByte.Length);
                string postString= Encoding.UTF8.GetString(postByte);
                return postString;
            }
        }

       

        public TextMsg Received_TextToModel(HttpContext context)
        {
            //微信被动消息会post过来xml
            string xmlstr= GetPostString(context);
            XmlDocument postObj = new XmlDocument();
            postObj.LoadXml(xmlstr);
            //解析xml到实体
            TextMsg textmsg = new TextMsg();
            XmlNodeList xmllist_msgtype = postObj.GetElementsByTagName("MsgType");
            XmlNodeList xmllist_tousername = postObj.GetElementsByTagName("ToUserName");
            XmlNodeList xmllist_fromUsername = postObj.GetElementsByTagName("FromUserName");
            XmlNodeList xmllist_createtime = postObj.GetElementsByTagName("CreateTime");
            XmlNodeList xmllist_Content = postObj.GetElementsByTagName("Content");
            XmlNodeList xmllist_MsgId = postObj.GetElementsByTagName("MsgId");
            try
            {
                textmsg.MsgType = xmllist_msgtype[0].InnerText;
                textmsg.ToUserName = xmllist_tousername[0].InnerText;
                textmsg.FromUserName = xmllist_fromUsername[0].InnerText;
                textmsg.CreateTime =Convert.ToInt32( xmllist_createtime[0].InnerText);
                textmsg.Content = xmllist_Content[0].InnerText;
                textmsg.MsgId = Convert.ToInt64( xmllist_MsgId[0].InnerText);
            }
            catch
            {
                //todo:解析文本消息异常处理
            }

            return textmsg;
        }

        public string SendText(TextMsg msg)
        {
            string textpl = string.Empty;
            textpl = "<xml>" +
                     "<ToUserName><![CDATA[" + msg.FromUserName + "]]></ToUserName>" +
                     "<FromUserName><![CDATA[" + msg.ToUserName + "]]></FromUserName>" +
                     "<CreateTime>" + msg.CreateTime + "</CreateTime>" +
                     "<MsgType><![CDATA[text]]></MsgType>" +
                     "<Content><![CDATA[" + msg.Content + "]]></Content>" +
                     "</xml>";

            return textpl;
        }


        public string GetAccess_token(string appid,string appsecret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
            string context = PushGetData(url);
            context=context.Substring(17, context.Length - 37);
            return context;


        }


        public string PushMenu(string Access_token, string MenuJson)
        {
            string posturl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token="+Access_token;
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(MenuJson);
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

        private string PushGetData(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }
    }
}
