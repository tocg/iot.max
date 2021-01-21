
using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Handler
{
    public class MsgReplyTextHandler: IWxMsgHandler<MsgReplyTextModel>
    {
        public string InitContent(MsgReplyTextModel model)
        {
            IReplyTemplate template = new ReplyTextTemplate(model);
            return template.TemplateContent();
        }
    }
}
