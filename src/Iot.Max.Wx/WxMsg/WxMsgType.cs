using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.WxMsg
{
    public class WxMsgType
    {
        /*
         * 请求消息类型
         * **/
        public const string REQUEST_MSGTYPE_TEXT = "text";
        public const string REQUEST_MSGTYPE_EVENT = "event";
        public const string REQUEST_MSGTYPE_IMAGE = "image";
        public const string REQUEST_MSGTYPE_VOICE = "voice";
        public const string REQUEST_MSGTYPE_VIDEO = "video";

        /*
         * 回复消息类型
         * **/
        public const string RESPONSE_MSGTYPE_TEXT = "text";
        public const string RESPONSE_MSGTYPE_IMAGE = "image";
        public const string RESPONSE_MSGTYPE_VOICE = "voice";
        public const string RESPONSE_MSGTYPE_VIDEO = "video";
        public const string RESPONSE_MSGTYPE_MUSIC = "music"; 
        public const string RESPONSE_MSGTYPE_NEWS = "news";

        public const string RESPONSE_MSGTYPE_LINK = "link";
        public const string RESPONSE_MSGTYPE_SUBSCRIBE = "subscribe";
        public const string RESPONSE_MSGTYPE_UNSUBSCRIBE = "unsubscribe";
        public const string RESPONSE_MSGTYPE_SHORTVIDEO = "shortvideo";
        public const string RESPONSE_MSGTYPE_LOCATION = "location";

        /*
         * 事件消息类型
         * **/
        public const string EVENT_MSGTYPE_SUBSCRIBE = "subscribe";
    }
}
