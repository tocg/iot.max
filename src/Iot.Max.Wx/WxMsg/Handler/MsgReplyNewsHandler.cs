using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Handler
{
    public class MsgReplyNewsHandler : IWxMsgHandler<MsgReplyNewsModel>
    {
        public string InitContent(MsgReplyNewsModel model)
        {
            IReplyTemplate template = new ReplyNewsTemplate(model);
            return template.TemplateContent();
        }
    }
}
