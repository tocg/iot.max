using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg
{
    public class WxMsgFactory : IWxMsg
    {
        private string _type { get; set; }
        private IContentTemplate _template { get; set; }

        public WxMsgFactory(string type) {
            _type = type.ToLower();
        }
        public string InitContent(string data)
        {
            string result;
            switch (_type)
            {
                case WxMsgType.MSGTYPE_TEXT:
                    try
                    {
                        WxMsgTextContent content = System.Text.Json.JsonSerializer.Deserialize<WxMsgTextContent>(data);
                        _template = new TextContentTemplate(content);
                        result = _template.TemplateContent();
                    }
                    catch (Exception)
                    {
                        throw;
                    }   
                    break;
                case WxMsgType.MSGTYPE_EVENT:

                    try
                    {
                        WxMsgNewsContent content = System.Text.Json.JsonSerializer.Deserialize<WxMsgNewsContent>(data);
                        _template = new NewsContentTemplate(content);
                        result = _template.TemplateContent();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    break;
                case WxMsgType.MSGTYPE_SUBSCRIBE:
                    result = "";
                    break;
                case WxMsgType.MSGTYPE_UNSUBSCRIBE:
                    result = "";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }
    }
}
