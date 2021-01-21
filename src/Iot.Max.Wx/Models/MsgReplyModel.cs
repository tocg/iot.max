using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    public class MsgReplyModel
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public long CreateTime { get { return DateTime.Now.Ticks; } }
    }
}
