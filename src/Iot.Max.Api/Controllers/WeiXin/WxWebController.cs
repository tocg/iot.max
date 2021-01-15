/*
 * 微信网页授权
 * 
 * **/
using Dapper;
using Iot.Max.Common;
using Iot.Max.Lib;
using Iot.Max.Services;
using Iot.Max.Wx;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.WeiXin
{
    [Route("wx/web")]
    [ApiController]
    public class WxWebController : ControllerBase
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(WxWebController));

        private readonly DapperClientHelper _dapper;
        IServices _services;
        public WxWebController(IDapperFactory dapperFactory, IServices services)
        {
            _dapper = dapperFactory.CreateClient();
            _services = services;
            _services.DapperClient = _dapper;
        }

        [HttpGet("auth")]
        public IActionResult Auth(string code, string isDetail = "1")
        {

            var result = new PageResultDto();

            if (string.IsNullOrEmpty(code))
            {
                result.Code = (int)ResultCode.UNAUTHORIZED;
                result.Msg = "请求参数不合理";
            }
            else
            {
                var data = WxWebAuthorize.InitUserInfo(code, isDetail == "1");

                if (!data.TryGetValue("openid", out _))
                {
                    result.Code = (int)ResultCode.FAIL;
                    result.Msg = "获取用户信息失败";
                }
                else
                {


                    try
                    {
                        //var jsonString = "{\"openid\":\"oHw8dt_flDX4QfN5ryhEcPO8U8PA\",\"nickname\":\"Tomax\",\"sex\":1,\"language\":\"zh_CN\",\"city\":\"\u6768\u6D66\",\"province\":\"\u4E0A\u6D77\",\"country\":\"\u4E2D\u56FD\",\"headimgurl\":\"https://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTJO30GWRefUunTbaWkX4ibMTz9lywiaYoRkz3CVEtwMmjKkD07xfr5wFxunAeXZVfDqwS6UiaXn1nwGQ/132\",\"privilege\":[]}";
                        //log.Info($"用户信息：{jsonString}");

                        var jsonString = JsonSerializer.Serialize(data);
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        };
                        var user = JsonSerializer.Deserialize<Wx.Models.WxUserInfo>(jsonString, options);

                        //log.Info($"保存到数据库的用户信息：{System.Text.Json.JsonSerializer.Serialize(user)}");


                        SnowFlakeWork snowFlake = new SnowFlakeWork(1);//雪花ID

                        DynamicParameters parm = new DynamicParameters();
                        parm.Add("id", snowFlake.NextID().ToString());
                        parm.Add("openid", user.OpenID);
                        parm.Add("nickname", user.NickName);
                        parm.Add("sex", Convert.ToString(user.Sex));
                        parm.Add("province", user.Province);
                        parm.Add("city", user.City);
                        parm.Add("country", user.Country);
                        parm.Add("headimgurl", user.HeadImgUrl);
                        parm.Add("privilege", "");
                        parm.Add("unionid", "");
                        parm.Add("@_id", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

                        var par = new PageParameters<Model.Models.WxUser>();
                        par.Proc = new PageProc { ProcName = "pr_wx_user", ProcParm = parm, ProcOutName = "_id" };

                        int _r = _services.InsertProcOut<Model.Models.WxUser>(par, out string id);

                        if (_r > -1)
                        {
                            //返回用户ID
                            result.Data = id;
                        }
                        else
                        {
                            result.Msg = "用户信息保存失败";
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Code = (int)ResultCode.INTERNAL_SERVER_ERROR;
                        result.Msg = "内部操作错误，请联系管理员或查看错误日志。";
                        log.Error($"/{System.Reflection.MethodBase.GetCurrentMethod().Name}方法/错误信息：【{ex.Message}】");
                    }
                }
            }
            return Ok(result);
        }
    }
}
