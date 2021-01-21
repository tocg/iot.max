using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Handler
{
    public class NewsMsgHandler : IWxMsgHandler<WxMsgNewsContent>
    {
        public string InitContent(WxMsgNewsContent content)
        {
            IContentTemplate template = new NewsContentTemplate(content);
            return template.TemplateContent();
        }
    }
}
