﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

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

        private string GetPostString(HttpContext context)
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

        public void XmlToEntity(string xml)
        {
            XmlDocument postObj = new XmlDocument();
            postObj.LoadXml(xml);
            
        }

        public string ReceivedText(HttpContext context)
        {
            //获取post的xml
            string postString= GetPostString(context);

            XmlDocument postObj = new XmlDocument();
            postObj.LoadXml(postString);

            XmlNodeList xmllist_msgtype = postObj.GetElementsByTagName("MsgType");
            XmlNodeList xmllist_tousername = postObj.GetElementsByTagName("ToUserName");
            XmlNodeList xmllist_fromUsername = postObj.GetElementsByTagName("FromUserName");
            XmlNodeList xmllist_createtime = postObj.GetElementsByTagName("CreateTime");
            XmlNodeList xmllist_Content = postObj.GetElementsByTagName("Content");
            if(xmllist_msgtype.Count<=0||xmllist_msgtype.Count>1)
            {
                //TODO:发送解析错误的消息
            }
            Models.Msg msg = new Models.Msg();

            msg.MsgType = xmllist_msgtype[0].InnerText;
            msg.ToUserName = xmllist_tousername[0].InnerText;
            msg.FromUserName = xmllist_fromUsername[0].InnerText;
            msg.CreateTime = Convert.ToInt32( xmllist_createtime[0].InnerText);
            msg.Content = "珊珊,笑脸，对不起";





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
    }
}