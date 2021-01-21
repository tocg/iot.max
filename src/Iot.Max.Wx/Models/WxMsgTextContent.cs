using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    //文本消息
    public class WxMsgTextContent : WxMsgBaseContent
    {
        public string MsgType { get; set; } = "text";
        public string Content { get; set; }
    }


    //图片消息
    public class WxMsgImageContent : WxMsgBaseContent
    {
        public string MsgType { get; set; } = "image";
        public string PicUrl { get; set; }
        public string MediaId { get; set; }
    }

    //语音消息
    public class WxMsgVoiceContent : WxMsgBaseContent
    {
        public string MsgType { get; set; } = "voice";
        public string Format { get; set; }
        public string MediaId { get; set; }
    }

    //视频消息
    public class WxMsgVideoContent : WxMsgBaseContent
    {
        public string MsgType { get; set; } = "video";
        public string ThumbMediaId { get; set; }
        public string MediaId { get; set; }
    }

    //小视频 Shortvideo
    public class WxMsgShortVideoContent : WxMsgBaseContent
    {
        public string MsgType { get; set;  } = "shortvideo";
        public string ThumbMediaId { get; set; }
        public string MediaId { get; set; }
    }

    //地理位置 Location
    public class WxMsgLocationContent : WxMsgBaseContent
    {
        public string MsgType { get; set; } = "location";
        public int Location_X { get; set; }
        public int Location_Y { get; set; }
        public string Scale { get; set; }
        public string Label { get; set; }
    }

    //链接消息 Link
    public class WxMsgLinkContent : WxMsgBaseContent
    {
        public string MsgType { get; set; } = "link";
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

    //图文消息
    public class WxMsgNewsContent : WxMsgBaseContent
    {
        public string MsgType { get; set; } = "news";
        public int ArticleCount { get; set; }

        //最多10条，可以判断
        public List<WxMsgNewsItemContent> Articles { get; set; }
    }

    //图文消息明细
    public class WxMsgNewsItemContent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicUrl { get; set; }
        public string Url { get; set; }
    }
}
