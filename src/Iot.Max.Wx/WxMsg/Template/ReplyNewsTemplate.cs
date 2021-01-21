using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Template
{
    public class ReplyNewsTemplate : IReplyTemplate
    {
        private Models.MsgReplyNewsModel _model;
        public ReplyNewsTemplate(Models.MsgReplyNewsModel model)
        {
            _model = model;
        }
        public virtual string TemplateContent()
        {
            StringBuilder items = new StringBuilder();
            foreach (var item in _model.Articles)
            {
                items.Append($"<item> " +
                    $"<Title><![CDATA[{item.Title}]]></Title>" +
                    $"<Description><![CDATA[{item.Description}]]></Description>" +
                    $"<PicUrl><![CDATA[{item.PicUrl}]]></PicUrl>" +
                    $"<Url><![CDATA[{item.Url}]]></Url>" +
                    $"</item>");
            }

            return $"<xml>" +
                    $"<ToUserName><![CDATA[{_model.ToUserName}]]></ToUserName>" +
                    $"<FromUserName><![CDATA[{_model.FromUserName}]]></FromUserName>" +
                    $"<CreateTime>{_model.CreateTime}</CreateTime>" +
                    $"<MsgType><![CDATA[{_model.MsgType}]]></MsgType>" +
                    $"<ArticleCount>{_model.Articles.Count}</ArticleCount>" +
                    $"<Articles>" +
                    $"{items}" +
                    $"</Articles>" +
                    $"</xml>";
        }
    }
}
