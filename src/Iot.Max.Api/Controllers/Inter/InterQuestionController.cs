﻿using AutoMapper;
using Dapper;
using Iot.Max.Common;
using Iot.Max.Lib;
using Iot.Max.Model.Dtos;
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
    [Route("inter/question")]
    [ApiController]
    public class InterQuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(InterInviteController));
        private IWebHostEnvironment _host;
        private readonly DapperClientHelper _dapper;
        readonly IServices _services;
        //readonly InterQuestionServices _services;
        public InterQuestionController(IMapper mapper, IWebHostEnvironment host,
            IDapperFactory dapperFactory,
            IServices services)
        {
            _host = host;
            _mapper = mapper;
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

                var par = new PageParameters<InterQuestion>();
                par.Proc = new PageProc { ProcName = "pr_inter_question_list", ProcParm = parm, ProcOutName = "count" };

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


        [HttpGet("list")]
        public IActionResult List(int page = 1, int limit = 20, string key = "")
        {
            var result = new PageResultDto();
            try
            {
                DynamicParameters parm = new DynamicParameters();
                parm.Add("search", key);
                parm.Add("page", page);
                parm.Add("size", limit);
                parm.Add("@count", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var par = new PageParameters<InterQuestionDto>();
                par.Proc = new PageProc { ProcName = "pr_inter_question_wxlist", ProcParm = parm, ProcOutName = "count" };

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
                result.Data = _services.QueryFirst<InterQuestion>(id);
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
                List<InterQuestion> parm = new List<InterQuestion>();
                foreach (var item in ids)
                {
                    parm.Add(new InterQuestion { ID = item });
                }
                var i = _services.Delete<InterQuestion>(parm);

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
        public async Task<IActionResult> EditAsync( )
        {
            //返回值对象
            var result = new PageResultDto();

            //获取表单提交的数据（文件和文本框等）
            var files = Request.Form.Files;
            var para = Request.Form["data"];

            //将表单数据转为实体对象
            var model = System.Text.Json.JsonSerializer.Deserialize<InterQuestion>(para);


            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                result.Code = (int)ResultCode.UNAUTHORIZED;
                result.Msg = "参数无效";

                return Ok(result);
            }

            if (files != null && files.Count > 0)
            {
                var list = await new Common.FileUpload(_host).UploadAsync(files, "inter/question");

                if (list != null && list.Count > 0)
                    model.Images = string.Join(",", list.ToArray());
            }

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