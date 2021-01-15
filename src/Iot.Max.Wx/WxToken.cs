using Iot.Max.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iot.Max.Wx
{
    public class WxToken
    {
        private static DateTime? GetAccessToken_Time;
        /// <summary>
        /// 过期时间为7200秒
        /// </summary>
        private static int Expires_Period = 7200;


        private static string mAccessToken;
        /// <summary>
        /// access_token(所有接口都需要此凭据)
        /// </summary>
        public static string Access_Token
        {
            get
            {
                //如果为空，或者过期，需要重新获取
                if (string.IsNullOrEmpty(mAccessToken) || HasExpired())
                {
                    //获取
                    mAccessToken = GetAccessToken();
                }

                return mAccessToken;
            }
        }
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            string result = HttpService.Get($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={Models.WxBaseInfo.AppID}&secret={Models.WxBaseInfo.AppSecret}");
            if (result != null)
            {
                //JavaScriptSerializer Jss = new JavaScriptSerializer();
                //Dictionary<string, object> resDic = (Dictionary<string, object>)Jss.DeserializeObject(result);

                Dictionary<string, object> resDic = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(result);
                var access_token = resDic["access_token"];
                var expires_in = resDic["expires_in"];

                GetAccessToken_Time = DateTime.Now;
                if (access_token != null)
                {
                    GetAccessToken_Time = DateTime.Now;

                    if (expires_in != null)
                    {
                        Expires_Period = int.Parse(Convert.ToString(expires_in));
                    }
                    return access_token.ToString();
                }
                else
                {
                    GetAccessToken_Time = DateTime.MinValue;
                }
            }
            return null;
        }
        /// <summary>
        /// 判断Access_token是否过期
        /// </summary>
        /// <returns>bool</returns>
        private static bool HasExpired()
        {
            if (GetAccessToken_Time != null)
            {
                //过期时间，允许有一定的误差，一分钟。获取时间消耗
                if (DateTime.Now > ((DateTime)GetAccessToken_Time).AddSeconds(Expires_Period).AddSeconds(-60))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
