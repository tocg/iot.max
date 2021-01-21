using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg.Template
{
    /*
     * 
     * 模板格式文档官网：
     * https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Passive_user_reply_message.html#5
     * 
     * **/

    public interface IContentTemplate
    {
        string TemplateContent();
    }
}
