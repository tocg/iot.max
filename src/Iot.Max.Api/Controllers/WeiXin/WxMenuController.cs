/*
 * 自定义微信菜单
 * 
 * **/
using Dapper;
using Iot.Max.Common;
using Iot.Max.Lib;
using Iot.Max.Model.Models;
using Iot.Max.Services;
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
        private readonly DapperClientHelper _dapper;
        readonly IServices _services;
        public WxMenuController(IDapperFactory dapperFactory, IServices services)
        {
            _dapper = dapperFactory.CreateClient();
            _services = services;
            _services.DapperClient = _dapper;
        }

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
        /// 菜单数据(测试数据)
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
                                    ""type"":""view"",  
                                    ""name"":""高频问题"",  
                                    ""url"":""{0}http://wx.iot.lcvue.com/question/index{1}""
                                },
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

        //从数据库中获取数菜单数据(拼接成json格式)
        private  string DbMenuToJson()
        {
            string result = "";
            var par = new PageParameters<WxMenu>();
            par.Model = new WxMenu();
            var list = _services.Query(par);

            if (list == null || list.Count == 0)
                return result;

            return result;
        }


        [HttpGet("query")]
        public IActionResult Query()
        {
            var result = new PageResultDto();
            try
            {
                var par = new PageParameters<WxMenu>();
                par.Model = new WxMenu();

                var list = _services.Query(par);

                result.Data = list;
                result.Count = list.Count;
            }
            catch (Exception ex)
            {
                result.Code = (int)ResultCode.INTERNAL_SERVER_ERROR;
                result.Msg = "内部操作错误，请联系管理员或查看错误日志。";
                log.Error($"/{System.Reflection.MethodBase.GetCurrentMethod().Name}方法/错误信息：【{ex.Message}】");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Query(string id)
        {
            var result = new PageResultDto();
            try
            {
                result.Code = (int)ResultCode.SUCCESS;
                result.Data = _services.QueryFirst<WxMenu>(id);
                result.Count = 1;
            }
            catch (Exception ex)
            {
                result.Code = (int)ResultCode.INTERNAL_SERVER_ERROR;
                result.Msg = "内部操作错误，请联系管理员或查看错误日志。";
                log.Error($"/{System.Reflection.MethodBase.GetCurrentMethod().Name}方法/错误信息：【{ex.Message}】");
            }
            return Ok(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(List<string> ids)
        {
            var result = new PageResultDto();
            try
            {
                List<WxMenu> parm = new List<WxMenu>();
                foreach (var item in ids)
                {
                    parm.Add(new WxMenu { ID = item });
                }
                var i = _services.Delete(parm);

                result.Code = i == 0 ? (int)ResultCode.SUCCESS : (int)ResultCode.INTERNAL_SERVER_ERROR;
                result.Count = ids.Count;
                result.Data = ids;
            }
            catch (Exception ex)
            {
                result.Code = (int)ResultCode.INTERNAL_SERVER_ERROR;
                result.Msg = "内部操作错误，请联系管理员或查看错误日志。";
                log.Error($"/{System.Reflection.MethodBase.GetCurrentMethod().Name}方法/错误信息：【{ex.Message}】");
            }
            return Ok(result);
        }

        [HttpPost("edit")]
        public IActionResult Edit(WxMenu model)
        {
            var result = new PageResultDto();

            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                result.Code = (int)ResultCode.UNAUTHORIZED;
                result.Msg = "参数无效";

                return Ok(result);
            }

            model.State = model.State == "on" ? "0" : "1";

            int res;
            try
            {
                if (!string.IsNullOrEmpty(model.ID) && !model.ID.ToString().ToLower().Equals("string"))
                {
                    res = _services.Update(model);
                }
                else
                {
                    SnowFlakeWork snowFlake = new SnowFlakeWork(1);
                    model.ID = snowFlake.NextID().ToString();
                    res = _services.Insert(model);
                }
                result.Count = res > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                result.Code = (int)ResultCode.INTERNAL_SERVER_ERROR;
                result.Msg = "内部操作错误，请联系管理员或查看错误日志。";
                log.Error($"/{System.Reflection.MethodBase.GetCurrentMethod().Name}方法/错误信息：【{ex.Message}】");
            }

            return Ok(result);
        }
    }
}
