
using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Handler
{
    public class TextMsgHandler: IWxMsgHandler<WxMsgTextContent>
    {
        public string InitContent(WxMsgTextContent content)
        {
            IContentTemplate template = new TextContentTemplate(content);
            return template.TemplateContent();
        }
    }
}
