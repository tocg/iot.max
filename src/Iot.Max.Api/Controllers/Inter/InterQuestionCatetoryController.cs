using AutoMapper;
using Dapper;
using Iot.Max.Common;
using Iot.Max.Lib;
using Iot.Max.Model.Dtos;
using Iot.Max.Model.Models;
using Iot.Max.Services;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.Inter
{
    [Route("inter/question/category")]
    [ApiController]
    public class InterQuestionCatetoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(InterInviteController));
        private readonly DapperClientHelper _dapper;
        readonly IServices _services;
        public InterQuestionCatetoryController(IMapper mapper,
            IDapperFactory dapperFactory,
            IServices services)
        {
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

                var par = new PageParameters<InterQuestionCategory>();
                par.Proc = new PageProc { ProcName = "pr_inter_question_category_list", ProcParm = parm, ProcOutName = "count" };

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

        [HttpGet("dropdown")]
        public IActionResult Query()
        {
            var result = new PageResultDto();
            try
            {
                var par = new PageParameters<InterQuestionCategory>();
                par.Model = new InterQuestionCategory();

                var list = _services.Query<InterQuestionCategory>(par);
                var mapList = _mapper.Map<List<InterQuestionCategoryDto>>(list);

                result.Data = mapList;
                result.Count = mapList.Count;
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
                result.Data = _services.QueryFirst<InterQuestionCategory>(id);
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
                List<InterQuestionCategory> parm = new List<InterQuestionCategory>();
                foreach (var item in ids)
                {
                    parm.Add(new InterQuestionCategory { ID = item });
                }
                var i = _services.Delete<InterQuestionCategory>(parm);

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
        public IActionResult Edit(RetailCategory model)
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
                    res = _services.Update<RetailCategory>(model);
                }
                else
                {
                    SnowFlakeWork snowFlake = new SnowFlakeWork(1);
                    model.ID = snowFlake.NextID().ToString();
                    res = _services.Insert<RetailCategory>(model);
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
