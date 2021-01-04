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

namespace Iot.Max.Api.Controllers.Retail
{
    /// <summary>
    /// 
    /// </summary>
    [Route("retail/brand")]
    [ApiController]
    public class RetailBrandController : ControllerBase
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(RetailBrandController));
        private IWebHostEnvironment _host;
        private readonly DapperClientHelper _dapper;
        IServices _services;
        public RetailBrandController(IDapperFactory dapperFactory,
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

                var par = new PageParameters<RetailBrand>();
                par.Proc = new PageProc { ProcName = "pr_retail_brand_list", ProcParm = parm, ProcOutName = "count" };
                var list = _services.Query(par, out int outCount);

                //直接调用dapper操作
                //var list = _dapper.QueryProcedureOut<RetailBrand>("pr_retail_brand_list", parm, "count", out int outCount);

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
                result.Data = _services.QueryFirst<RetailBrand>(id);
                //直接用dapper执行
                //_dapper.QueryFirst<RetailBrand>("select * from retail_brand where id=@id",new { id });
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
        public async Task<IActionResult> EditAsync()
        {
            //返回值对象
            var result = new PageResultDto();

            //获取表单提交的数据（文件和文本框等）
            var files = Request.Form.Files;
            var para = Request.Form["data"];

            //将表单数据转为实体对象
            var brand = System.Text.Json.JsonSerializer.Deserialize<RetailBrand>(para);

            if (brand == null || string.IsNullOrEmpty(brand.BrandName))
            {
                result.Code = (int)ResultCode.UNAUTHORIZED;
                result.Msg = "参数无效";

                return Ok(result);
            }

            //string sql = "";

            brand.State = brand.State == "on" ? "0" : "1";
            if (files != null && files.Count > 0)
            {
                var list = await new Common.FileUpload(_host).UploadAsync(files, "brand");
                if (list != null && list.Count > 0)
                    brand.Logo = list.FirstOrDefault();
            }

            int res;
            try
            {
                //注释的sql语句和执行语句为直接采用dapper
                if (!string.IsNullOrEmpty(brand.ID) && !brand.ID.ToString().ToLower().Equals("string"))
                {
                    res = _services.Update<RetailBrand>(brand);
                    //sql = "update retail_brand set brandname=@brandname,description=@description,sort=@sort,state=@state where id=@id";
                }
                else
                {
                    SnowFlakeWork snowFlake = new SnowFlakeWork(1);//雪花ID
                    brand.ID = snowFlake.NextID().ToString();
                    res = _services.Insert<RetailBrand>(brand);
                    //sql = "insert into retail_brand (id,brandname,description,logo,sort,state) " +
                    //    "values(@id,@brandname,@description,@logo,@sort,@state)";
                }

                //res = _dapper.Execute(sql, brand);
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
                //string parm =$" '{string.Join("','",ids.ToArray())}'";
                List<RetailBrand> parm = new List<RetailBrand>();
                foreach (var item in ids)
                {
                    parm.Add(new RetailBrand { ID = item });
                }
                var i = _services.Delete<RetailBrand>(parm);
                //直接用dapper执行
                //var i =  _dapper.Execute("delete from retail_brand where id = @id", parm);

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
        public IActionResult Update(RetailBrand model)
        {
            var result = new PageResultDto();
            try
            {
                var i = _services.Update<RetailBrand>(model);
                //直接用dapper执行
                //var i = _dapper.Execute("update retail_brand set BrandName = @brandname,Description = @description where id = @id", model);

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
