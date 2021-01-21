using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Template
{
    public class TextContentTemplate : IContentTemplate
    {
        private Models.WxMsgTextContent _content;
        public TextContentTemplate(Models.WxMsgTextContent content)
        {
            _content = content;
        }
        public virtual string TemplateContent()
        {
            return $"<xml>" +
                $"<ToUserName><![CDATA[{_content.ToUserName}]]></ToUserName>" +
                $"<FromUserName><![CDATA[{_content.FromUserName}]]></FromUserName>" +
                $"<CreateTime>{_content.CreateTime}</CreateTime>" +
                $"<MsgType><![CDATA[{_content.MsgType}]]></MsgType>" +
                $"<Content><![CDATA[{_content.Content}]]></Content>" +
                $"<MsgId>{_content.MsgId}</MsgId>" +
                $"</xml>";
        }
    }
}
