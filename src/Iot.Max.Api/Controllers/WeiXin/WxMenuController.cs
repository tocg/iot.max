/*
 * 自定义微信菜单
 * 
 * **/
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.WeiXin
{
    [Route("wx/menu")]
    [ApiController]
    public class WxMenuController : ControllerBase
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(WxMenuController));

        [HttpGet("create")]
        public IActionResult GetCreate()
        {
            //post提交生成菜单链接及access_token(该access_token可数据库保存)
            var token = Iot.Max.Wx.WxToken.Access_Token;
            string postUrl = $" https://api.weixin.qq.com/cgi-bin/menu/create?access_token={token}";

            log.Info($"生成菜单需要的token={token}");

            string menuInfo = TestMenu();

            log.Info($"生成的菜单信息={menuInfo}");

            string Alert = Iot.Max.Common.HttpService.SendPostHttpRequest(postUrl, "post", menuInfo);       

            return Ok(Alert);
        }


        /// <summary>
        /// 菜单数据
        /// </summary>
        /// <returns>输出微信的json格式</returns>
        private static string TestMenu()
        {

            //菜单view是指单击后跳转到指定的页面(要获取openid需微信中设置回调域名【功能服务】-【网页账号】-修改)
            //菜单click是指单击后，根据获取到的key获取内容并在微信内显示

            //string url = "http://test.sundaydesign.cn/wxhandler/_view/testview.aspx?id=test";
            //string _url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={}&redirect_uri={}&response_type=code&scope=snsapi_base&state=1#wechat_redirect";
            #region 测试菜单

            string strMenu = @" {  
                            ""button"":[  
                            {    
                                ""name"":""面试上传"",  
                                ""sub_button"":[
                                {
                                    ""type"":""view"",
                                    ""name"":""上传邀约"",
                                    ""url"":""{0}http://wx.iot.lcvue.com/invite/upload{1}""
                                },{
                                    ""type"":""view"",
                                    ""name"":""上传offer"",
                                    ""url"":""{0}http://wx.iot.lcvue.com/offer/upload{1}""
                                }]
                            },  
                            {  
                                ""name"":""面试宝典"",  
                                ""sub_button"":[  
                                {  
                                    ""type"":""click"",  
                                    ""name"":""技能复习"",  
                                    ""key"":""BUTTON_SKILL""
                                },{  
                                    ""type"":""view"",  
                                    ""name"":""面试计划"",  
                                    ""url"":""{0}http://wx.iot.lcvue.com/inter/plan{1}""
                                }] 
                            },  
                            {  
                                ""name"":""个人中心"",  
                                ""sub_button"":[
                                {
                                    ""type"":""click"",
                                    ""name"":""2021愿望"",
                                    ""key"":""BUTTON_CENTER_2021""
                                    },{
                                    ""type"":""click"",
                                    ""name"":""个人资料"",
                                    ""key"":""BUTTON_CENTER_INFO""
                                    }
                                ]
                            }
                        ]}";

            #endregion

            string strUrl0 = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={Iot.Max.Wx.Models.WxBaseInfo.AppID}&redirect_uri=";
            string strUrl1 = "&response_type=code&scope=snsapi_base&state=1#wechat_redirect";
            strMenu = strMenu.Replace("{0}", strUrl0).Replace("{1}", strUrl1);

            //服务号菜单拼接后的链接格式：
            //https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx7064f042a67b0cd4&redirect_uri=http://test.sundaydesign.cn/test/default.aspx&response_type=code&scope=snsapi_base&state=1#wechat_redirect
            return strMenu;
        }

    }
}
