using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    public class MsgReplyNewsModel : MsgReplyModel
    {
        public string MsgType { get; set; } = "news";
        public int ArticleCount { get; set; }
        public List<WxMsgReplayNewsItemModel> Articles { get; set; }
    }

    public class WxMsgReplayNewsItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicUrl { get; set; }
        public string Url { get; set; }
    }
}
