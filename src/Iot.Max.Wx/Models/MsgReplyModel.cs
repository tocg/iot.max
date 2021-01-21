/*
 * 微信消息回复基础实体
 * 
 * **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    public class MsgReplyModel
    {
        /// <summary>
        /// 在使用过程中，与接收到XML中的FromUserName对应
        /// </summary>
        public string ToUserName { get; set; }


        /// <summary>
        /// 在使用过程中，与接收到XML中的ToUserName对应
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 创建时间（只读，无须设置）
        /// </summary>
        public long CreateTime { get { return DateTime.Now.Ticks; } }
    }
}
