using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    /// <summary>
    /// 微信用户的基本信息(scope=snsapi_userinfo)
    /// </summary>
    public class WxUserInfo
    {
        public string OpenID { get; set; }
        public string NickName { get; set; }
        public int Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string HeadImgUrl { get; set; }

        public dynamic Privilege { get; set; }
        public string UnionidID { get; set; }
    }
}
