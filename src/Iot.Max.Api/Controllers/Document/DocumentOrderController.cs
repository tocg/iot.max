using Dapper;
using Iot.Max.Common;
using Iot.Max.Lib;
using Iot.Max.Model.Dtos;
using Iot.Max.Model.ExtenModels;
using Iot.Max.Model.Models;
using Iot.Max.Model.Models.Document;
using Iot.Max.Services;
using Iot.Max.Services.Document;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.Document
{
    /// <summary>
    /// 文档
    /// </summary>
    [Route("document/order")]
    [ApiController]


    public class DocumentOrderController : ControllerBase
    {
        private readonly ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(DocumentOrderController));
        private readonly DapperClientHelper _dapper;
        private IWebHostEnvironment _host;
        readonly DocumentOrderServices _services;
        public DocumentOrderController(IDapperFactory dapperFactory,
            IWebHostEnvironment host,
            DocumentOrderServices services)
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

                var par = new PageParameters<DocumentOrderDto>();
                par.Proc = new PageProc { ProcName = "pr_document_order_list", ProcParm = parm, ProcOutName = "count" };

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
                result.Data = _services.QueryFirst<DocumentOrder>(id);
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
                List<DocumentOrder> parm = new List<DocumentOrder>();
                foreach (var item in ids)
                {
                    parm.Add(new DocumentOrder { ID = item });
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
        public async Task<IActionResult> Edit()
        {
            var result = new PageResultDto();
            var models = new InsertDocumentAnnexModels();
            SnowFlakeWork snowFlake = new SnowFlakeWork(1);//雪花ID
            string _id = snowFlake.NextID().ToString();

            var files = Request.Form.Files;
            var para = Request.Form["data"];

            //将表单数据转为实体对象
            var model = System.Text.Json.JsonSerializer.Deserialize<DocumentOrder>(para);
            model.IsOpen = model.IsOpen == "on" ? "1" : "0";

            //当业务数据必要字段为空，则直接返回
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                result.Code = (int)ResultCode.UNAUTHORIZED;
                result.Msg = "参数无效";

                return Ok(result);
            }

            //当业务id为不空时，则表示是修改数据，则暂时保存一下该Id ,用作上传附件关联
            if (!string.IsNullOrEmpty(model.ID) && !model.ID.ToString().ToLower().Equals("string"))
            {
                _id = model.ID;
            }

            //上传文档附件
            if (files != null && files.Count > 0)
            {
                var list = await new Common.FileUpload(_host).UploadAsync(files, "document");
                List<DocumentAnnex> annexs = new List<DocumentAnnex>();
                var i = 0;
                foreach (var item in list)
                {
                    DocumentAnnex annex = new DocumentAnnex
                    {
                        DocumentID = _id,
                        ID = snowFlake.NextID().ToString(),
                        Path = item,
                        Title = files[i].FileName
                    };
                    i++;
                    annexs.Add(annex);
                }
                models.Annex = annexs;
            }
            models.Document = model;

            int res;
            try
            {
                if (!string.IsNullOrEmpty(model.ID) && !model.ID.ToString().ToLower().Equals("string"))
                {
                    //res = _services.Update(model);
                    res = 0;
                }
                else
                {
                    model.ID = _id;
                    model.CreateDate = System.DateTime.Now;
                    res = _services.Insert(models);
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
