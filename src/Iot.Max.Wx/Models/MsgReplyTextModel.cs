/*
 * 微信消息文本实体
 * 
 * **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    public class MsgReplyTextModel : MsgReplyModel
    {
        public string MsgType { get { return "text"; } }
        public string Content { get; set; }
    }
}
