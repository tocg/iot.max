using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx.Models
{
    public class WxBaseInfo
    {
        IConfiguration _config;
        public WxBaseInfo() { }

        /// <summary>
        /// AppID
        /// </summary>
        public static string AppID {
            get
            {
                return "wx7064f042a67b0cd4";
                    //return _config.GetConnectionString("appid");
            }
        }

        /// <summary>
        /// AppSecret
        /// </summary>
        public static string AppSecret
        {
            get
            {
                return "25719f5bd3b8e82eec43f3354358b78c";
                //return _config.GetConnectionString("appsecret");
            }
        }

        /// <summary>
        /// 自定义Token
        /// </summary>
        public static string AppToken
        {
            get { return "mytestweixintoken"; }
        }

        /// <summary>
        /// 自定义加密码
        /// （测试号，不需要加密）
        /// </summary>
        public static string EncodingAESKey
        {
            get
            {
                return ""; 
            }
        }
    }
}
