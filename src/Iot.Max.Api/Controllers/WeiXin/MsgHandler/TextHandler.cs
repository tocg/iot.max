using Iot.Max.Wx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.WeiXin.MsgHandler
{
    public class TextHandler
    {
        public string InitContent(WxMsgTextContent content)
        {
            if (content == null || string.IsNullOrEmpty(content.ToUserName))
                return "";

            return $"<xml>" +
                $"<ToUserName><![CDATA[{content.ToUserName}]]></ToUserName>" +
                $"<FromUserName><![CDATA[{content.FromUserName}]]></FromUserName>" +
                $"<CreateTime>{content.CreateTime}</CreateTime>" +
                $"<MsgType><![CDATA[{content.MsgType}]]></MsgType>" +
                $"<Content><![CDATA[你输入的是{content.Content}]]></Content>" +
                $"<MsgId>{content.MsgId}</MsgId>" +
                $"</xml>";
        }
    }
}
