using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    /// <summary>
    /// 文本
    /// </summary>
    public class MsgReceiveTextModel : MsgReceiveModel
    {
        /// <summary>
        /// 用户输入的内容
        /// </summary>
        public string Content { get; set; }
    }
}
