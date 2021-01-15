using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Iot.Max.Wx.View.Controllers
{
    public class OfferController : Controller
    {
        private readonly ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(OfferController));
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upload()
        {
            //try
            //{
            //    var query = HttpContext.Request.QueryString.ToString();

            //    log.Info(query);

            //    var collection = HttpUtility.ParseQueryString(query);
            //    var code = collection["code"]?.Trim();
            //    log.Info(code);

            //    var url = $"http://api.iot.lcvue.com/wx/web/auth?code={code}&isdetail=1";
            //    using (var client = new HttpClient())
            //    {
            //        var response = client.GetAsync(url);
            //        var result = response.Result.Content.ReadAsStringAsync().Result;
            //        log.Info($"upload 页面请求，用户信息：{result}");

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            var des = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(result);
            //            ViewBag.Msg = des["Msg"];
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //    throw;
            //}
            return View();
        }
    }
}
