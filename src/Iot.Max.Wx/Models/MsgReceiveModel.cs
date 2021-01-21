using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    /// <summary>
    /// 接收来自微信服务器的消息（基本字段）
    /// </summary>
    public class MsgReceiveModel
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string MsgType { get; set; }
        public string Event { get; set; }
        public string EventKey { get; set; }
    }
}
