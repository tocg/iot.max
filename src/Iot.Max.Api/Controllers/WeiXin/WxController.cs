/*
 * 与微信服务验证
 * 
 * 
 * **/
using Iot.Max.Common;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Iot.Max.Api.Controllers.WeiXin
{
    [Route("wx")]
    [ApiController]
    public class WxController : ControllerBase
    {

        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(WxController));

        /// <summary>
        /// 微信回调统一接口
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        [Route("service")]
        public async Task<string> Service()
        {
            //获取配置文件中的数据
            //var token = "mytestweixintoken";
            //var encodingAESKey = ""; //测试号是明文，不需要解密
            //var appId = "wx7064f042a67b0cd4";
            //var appSerect = "25719f5bd3b8e82eec43f3354358b78c";

            bool isGet = string.Equals(HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase);
            bool isPost = string.Equals(HttpContext.Request.Method, HttpMethod.Post.Method, StringComparison.OrdinalIgnoreCase);
            if (!isGet && !isPost)
            {
                return "";
            }


            bool isEncrypt = false;
            string msg_signature = "", nonce = "", timestamp = "", encrypt_type = "", signature = "", echostr = "";

            if (isGet)
            {
                #region Get请求
                try
                {
                    var query = HttpContext.Request.QueryString.ToString();

                    if (!string.IsNullOrEmpty(query))//需要验证签名
                    {
                        var collection = HttpUtility.ParseQueryString(query);
                        msg_signature = collection["msg_signature"]?.Trim();
                        nonce = collection["nonce"]?.Trim();
                        timestamp = collection["timestamp"]?.Trim();
                        encrypt_type = collection["encrypt_type"]?.Trim();
                        signature = collection["signature"]?.Trim();
                        echostr = collection["echostr"]?.Trim();

                        if (!string.IsNullOrEmpty(encrypt_type))//有使用加密
                        {
                            if (!string.Equals(encrypt_type, "aes", StringComparison.OrdinalIgnoreCase))//只支持AES加密方式
                            {
                                return "";
                            }
                            isEncrypt = true;
                        }
                    }

                    //先验证签名
                    if (!string.IsNullOrEmpty(signature))
                    {
                        //字符加密
                        var sha1 = MakeSign(nonce, timestamp, Wx.Models.WxBaseInfo.AppToken);
                        if (!sha1.Equals(signature, StringComparison.OrdinalIgnoreCase))//验证不通过
                        {
                            return "";
                        }

                        if (isGet)//是否Get请求，如果true,那么就认为是修改服务器回调配置信息
                        {
                            return echostr;
                        }
                    }
                    else
                    {
                        return "";//没有签名，请求直接返回
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Get请求Error：" + ex.Message);
                    throw;
                }
                #endregion
            }
            else if (isPost)
            {
                #region Post请求
                try
                {
                    //var body = new StreamReader(HttpContext.Request.Body).ReadToEnd();

                    var sr = new StreamReader(HttpContext.Request.Body);
                    var body = await sr.ReadToEndAsync();

                    #region 消息体解密

                    //if (isEncrypt)
                    //{
                    //    XDocument doc = XDocument.Parse(body);
                    //    var encrypt = doc.Element("xml").Element("Encrypt");

                    //    //验证消息签名
                    //    if (!string.IsNullOrEmpty(msg_signature))
                    //    {
                    //        //消息加密
                    //        var sha1 = MakeMsgSign(nonce, timestamp, encrypt.Value, Wx.Models.WxBaseInfo.AppToken);
                    //        if (!sha1.Equals(msg_signature, StringComparison.OrdinalIgnoreCase))//验证不通过
                    //        {
                    //            return "";
                    //        }
                    //    }

                    //    body = EncryptHelper.AESDecrypt(encrypt.Value, Wx.Models.WxBaseInfo.EncodingAESKey);//解密
                    //}
                    #endregion

                    log.Info($"body={body}");

                    if (!string.IsNullOrEmpty(body))
                    {
                        XDocument doc = XDocument.Parse(body);
                        var MsgType = doc.Element("xml").Element("MsgType");

                        if (MsgType == null)
                        {
                            return "";
                        }


                        log.Info($"MsgType={MsgType.ToString()}");

                        //
                        //在这里根据body中的MsgType和Even来区分消息，然后来处理不同的业务逻辑
                        //
                        //

                        //result是上面逻辑处理完成之后的待返回结果，如返回文本消息：
                        var result = @"<xml>
                                      <ToUserName><![CDATA[toUser]]></ToUserName>
                                      <FromUserName><![CDATA[fromUser]]></FromUserName>
                                      <CreateTime>12345678</CreateTime>
                                      <MsgType><![CDATA[text]]></MsgType>
                                      <Content><![CDATA[你好]]></Content>
                                    </xml>";
                        if (!string.IsNullOrEmpty(result))
                        {
                            if (isEncrypt)
                            {
                                result = EncryptHelper.AESEncrypt(result, Wx.Models.WxBaseInfo.EncodingAESKey, Wx.Models.WxBaseInfo.AppID);
                                var _msg_signature = MakeMsgSign(nonce, timestamp, result, Wx.Models.WxBaseInfo.AppToken);
                                result = $@"<xml>
                                                    <Encrypt><![CDATA[{result}]]></Encrypt>
                                                    <MsgSignature>{_msg_signature}</MsgSignature>
                                                    <TimeStamp>{timestamp}</TimeStamp>
                                                    <Nonce>{nonce}</Nonce>
                                                </xml>";
                            }
                            return result;
                        }

                        //如果这里我们的处理逻辑需要花费较长时间，可以这里先返回空(""),然后使用异步去处理业务逻辑，
                        //异步处理完后，调用微信的客服消息接口通知微信服务器
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Post请求Error：{ex.Message}");
                    throw;
                }
                #endregion
            }

            return "";
        }


        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [NonAction]
        private string MakeSign(params string[] args)
        {
            //字典排序
            Array.Sort(args);
            string tmpStr = string.Join("", args);
            //字符加密
            var sha1 = EncryptHelper.Sha1Encrypt(tmpStr);
            return sha1;
        }

        /// <summary>
        /// 生成消息签名
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [NonAction]
        private string MakeMsgSign(params string[] args)
        {
            //字典排序
            Array.Sort(args, new CharSort());
            string tmpStr = string.Join("", args);
            //字符加密
            var sha1 = EncryptHelper.Sha1Encrypt(tmpStr);
            return sha1;
        }
    }
}
