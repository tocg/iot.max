using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Template
{
    public class NewsContentTemplate : IContentTemplate
    {
        private Models.WxMsgNewsContent _content;
        public NewsContentTemplate(Models.WxMsgNewsContent content)
        {
            _content = content;
        }
        public virtual string TemplateContent()
        {
            StringBuilder items = new StringBuilder();
            foreach (var item in _content.Articles)
            {
                items.Append($"<item> " +
                    $"<Title><![CDATA[{item.Title}]]></Title>" +
                    $"<Description><![CDATA[{item.Description}]]></Description>" +
                    $"<PicUrl><![CDATA[{item.PicUrl}]]></PicUrl>" +
                    $"<Url><![CDATA[{item.Url}]]></Url>" +
                    $"</item>");
            }

            return $"<xml>" +
                    $"<ToUserName><![CDATA[{_content.ToUserName}]]></ToUserName>" +
                    $"<FromUserName><![CDATA[{_content.FromUserName}]]></FromUserName>" +
                    $"<CreateTime>{_content.CreateTime}</CreateTime>" +
                    $"<MsgType><![CDATA[{_content.MsgType}]]></MsgType>" +
                    $"<ArticleCount>{_content.Articles.Count}</ArticleCount>" +
                    $"<Articles>" +
                    $"{items}"+
                    $"</Articles>" +
                    $"</xml>";
        }
    }
}
