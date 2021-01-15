/*
 * 微信Web网页授权，获取用户信息
 * 
 * **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Wx
{
    public class WxWebAuthorize
    {
        public static Dictionary<string,object> InitUserInfo(string code, bool isDetail)
        {
            try
            {
                var UserInfo = new Models.WxUserInfo();
                var _url = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={Wx.Models.WxBaseInfo.AppID}&secret={Wx.Models.WxBaseInfo.AppSecret}&code={code}&grant_type=authorization_code";
                //HttpClient _client = new HttpClient();
                //HttpRequestMessage _httpRequest = new HttpRequestMessage(HttpMethod.Get, _url);
                //var _result = _client.Send(_httpRequest).Content.ReadAsStringAsync().Result;
                ////释放
                //_client.Dispose();

                var _result = Iot.Max.Common.HttpService.Get(_url);
                var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(_result);

                object _access_token;
                if(!data.TryGetValue("access_token",out _access_token))
                {
                    return data;
                }

                if (isDetail) 
                {
                    _url = $"https://api.weixin.qq.com/sns/userinfo?access_token={_access_token}&openid={Wx.Models.WxBaseInfo.AppID}&lang=zh_CN";
                    _result = Common.HttpService.Get(_url);
                    data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string,object>>(_result);
                }

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
