using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.Pay
{
    [Route("alipay")]
    [ApiController]
    public class AliPayController : ControllerBase
    {
        [HttpGet,Route("test")]
        public IActionResult Test()
        {
            DefaultAopClient client = new DefaultAopClient(AliPayConfig.gatewayUrl,
                AliPayConfig.app_id, AliPayConfig.private_key, "json", "1.0",
                AliPayConfig.sign_type, AliPayConfig.alipay_public_key, AliPayConfig.charset, false);

            // 外部订单号，商户网站订单系统中唯一的订单号
            string out_trade_no = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString();

            // 订单名称
            string subject = ".NetCore 支付宝沙箱测试";

            // 付款金额
            string total_amout = "0.01";

            // 商品描述
            string body = "NetCore 支付宝沙箱测试，直接使用Demo";

            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.Body = body;
            model.Subject = subject;
            model.TotalAmount = total_amout;
            model.OutTradeNo = out_trade_no;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl("http://localhost:64127/");
            // 设置异步通知接收地址
            request.SetNotifyUrl("");
            // 将业务model载入到request
            request.SetBizModel(model);

            AlipayTradePagePayResponse response = null;
            try
            {
                response = client.PageExecute(request, null, "post");
                //Response.WriteAsync(response.Body);

                return Content(response.Body);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
