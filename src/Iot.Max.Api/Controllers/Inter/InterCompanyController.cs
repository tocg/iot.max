/*
 * 面试公司控制器
 * Date：2021/1/4
 * Author：max
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
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.Inter
{
    [Route("inter/company")]
    [ApiController]


    public class InterCompanyController : ControllerBase
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(InterInviteController));
        private readonly IWebHostEnvironment _host;
        private readonly DapperClientHelper _dapper;
        readonly IServices _services;
        public InterCompanyController(IDapperFactory dapperFactory,
            IWebHostEnvironment host,
            IServices services)
        {
            _host = host;
            _dapper = dapperFactory.CreateClient();
            _services = services;
            _services.DapperClient = _dapper;
        }

        [HttpGet("query")]
        public IActionResult Query(int page = 1, int limit = 20, string search = "")
        {
            var result = new PageResultDto();
            try
            {
                DynamicParameters parm = new DynamicParameters();
                parm.Add("search", search);
                parm.Add("page", page);
                parm.Add("size", limit);
                parm.Add("@count", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var par = new PageParameters<InterCompany>();
                par.Proc = new PageProc { ProcName = "pr_inter_company_list", ProcParm = parm, ProcOutName = "count" };
                var list = _services.Query(par, out int outCount);

                result.Data = list;
                result.Count = outCount;
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
                result.Data = _services.QueryFirst<InterCompany>(id);
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

        [HttpPost("edit")]
        public  IActionResult EditAsync()
        {
            //返回值对象
            var result = new PageResultDto();

            //获取表单提交的数据（文件和文本框等）
            var files = Request.Form.Files;
            var para = Request.Form["data"];

            //将表单数据转为实体对象
            var model = System.Text.Json.JsonSerializer.Deserialize<InterCompany>(para);

            if (model == null || string.IsNullOrEmpty(model.CompanyName))
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
                    res = _services.Update<InterCompany>(model);
                }
                else
                {
                    SnowFlakeWork snowFlake = new SnowFlakeWork(1);//雪花ID
                    model.ID = snowFlake.NextID().ToString();
                    res = _services.Insert<InterCompany>(model);
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

        [HttpPost("delete")]
        public IActionResult Delete(List<string> ids)
        {
            var result = new PageResultDto();
            try
            {
                List<InterCompany> parm = new List<InterCompany>();
                foreach (var item in ids)
                {
                    parm.Add(new InterCompany { ID = item });
                }
                var i = _services.Delete<InterCompany>(parm);

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

        [HttpPost("update")]
        public IActionResult Update(InterCompany model)
        {
            var result = new PageResultDto();
            try
            {
                var i = _services.Update<InterCompany>(model);
                result.Code = i == 0 ? (int)ResultCode.SUCCESS : (int)ResultCode.INTERNAL_SERVER_ERROR;
                result.Count = 1;
                result.Data = model;
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
