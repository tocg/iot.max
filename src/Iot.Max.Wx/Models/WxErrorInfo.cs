using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    /*
     * 调用微信服务器，返回错误信息
     * 
     * **/
    public class WxErrorInfo
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
    }
}
