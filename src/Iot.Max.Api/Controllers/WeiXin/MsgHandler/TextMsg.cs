using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Iot.Max.Api.Controllers.WeiXin.MsgHandler
{
    public class TextMsg
    {
        readonly MsgReplyTextModel content;
        public TextMsg(XDocument doc)
        {
            if (content == null)
            {
                content = new MsgReplyTextModel
                {
                    //注意：给用户反馈回复消息touser和fromuser要对调  （所以没有用xml直接转实体）
                    FromUserName = doc.Element("xml").Element("ToUserName")?.Value,
                    ToUserName = doc.Element("xml").Element("FromUserName")?.Value,
                    Content = doc.Element("xml").Element("Content")?.Value,
                };
            }
        }

        public string InitContent()
        {
            //可以根据用户请求的Content内容去做业务处理（此处只输出固定的文本提示）
            string str = $"系统暂时不支持关键字检索功能。";
            return InitContent(str);
        }

        public string InitContent(string strContent)
        {
            content.Content = strContent;
            return new MsgReplyTextHandler().InitContent(content);
        }
    }
}
