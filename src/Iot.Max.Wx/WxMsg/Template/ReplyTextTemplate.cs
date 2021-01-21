using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Template
{
    public class ReplyTextTemplate : IReplyTemplate
    {
        private Models.MsgReplyTextModel _model;
        public ReplyTextTemplate(Models.MsgReplyTextModel model)
        {
            _model = model;
        }
        public virtual string TemplateContent()
        {
            return $"<xml>" +
                $"<ToUserName><![CDATA[{_model.ToUserName}]]></ToUserName>" +
                $"<FromUserName><![CDATA[{_model.FromUserName}]]></FromUserName>" +
                $"<CreateTime>{_model.CreateTime}</CreateTime>" +
                $"<MsgType><![CDATA[{_model.MsgType}]]></MsgType>" +
                $"<Content><![CDATA[{_model.Content}]]></Content>" +
                $"</xml>";
        }
    }
}
