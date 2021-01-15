/*
 * 面试邀约
 * 
 * **/
using Dapper;
using Iot.Max.Common;
using Iot.Max.Lib;
using Iot.Max.Model.Models;
using Iot.Max.Services;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.WeiXin
{
    [Route("wx/invite")]
    [ApiController]
    public class WxInviteController : ControllerBase
    {

        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(WxInviteController));
        private IWebHostEnvironment _host;
        private readonly DapperClientHelper _dapper;
        IServices _services;
        public WxInviteController(IDapperFactory dapperFactory,
            IWebHostEnvironment host,
            IServices services)
        {
            _host = host;
            _dapper = dapperFactory.CreateClient();
            _services = services;
            _services.DapperClient = _dapper;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            //返回值对象
            var result = new PageResultDto();

            //获取表单提交的数据（文件和文本框等）
            var files = Request.Form.Files;
            var para = Request.Form["data"];

            //将表单数据转为实体对象
            var model = JsonSerializer.Deserialize<InterInvite>(para);

            if (model == null || string.IsNullOrEmpty(model.CompanyName) || string.IsNullOrEmpty(model.CreateBy))
            {
                result.Code = (int)ResultCode.UNAUTHORIZED;
                result.Msg = "参数无效";

                return Ok(result);
            }

            if (files != null && files.Count > 0)
            {
                var list = await new Common.FileUpload(_host).UploadAsync(files, "invite");
                if (list != null && list.Count > 0)
                    model.Images = list.FirstOrDefault();
            }
            try
            {
                SnowFlakeWork snowFlake = new SnowFlakeWork(1);//雪花ID
                DynamicParameters parm = new DynamicParameters();
                parm.Add("companyid", snowFlake.NextID().ToString());
                parm.Add("companyname", model.CompanyName);
                parm.Add("address","");
                parm.Add("inviteid", snowFlake.NextID().ToString());
                parm.Add("times", model.Times);
                parm.Add("images", model.Images);
                parm.Add("createby", model.CreateBy);

                var parameters = new PageParameters<InterInvite>
                {
                    Proc = new PageProc { ProcName = "pr_inter_invite_insert", ProcParm = parm }
                };
                int _rs = _services.InsertProc(parameters);
                result.Count = _rs;
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
