/*
 * 与微信服务验证
 * 
 * 
 * **/
using Iot.Max.Api.Controllers.WeiXin.MsgHandler;
using Iot.Max.Common;
using Iot.Max.Wx.Models;
using Iot.Max.Wx.WxMsg;
using Iot.Max.Wx.WxMsg.Handler;
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

        private readonly ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(WxController));

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

            //log.Info($"1、获取请求方式：get= {isGet},post= {isPost}");


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
                    return "";
                }
                #endregion
            }
            else if (isPost)
            {
                #region Post请求
                try
                {
                    var sr = new StreamReader(HttpContext.Request.Body);
                    var body = await sr.ReadToEndAsync();

                    #region 消息体解密（默认无加密，所以注释）

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

                    //log.Info($"2、获得post请求内容： {body}");

                    if (!string.IsNullOrEmpty(body))
                    {
                        XDocument doc = XDocument.Parse(body);
                        var MsgType = doc.Element("xml").Element("MsgType");

                        if (MsgType == null)
                        {
                            return "";
                        }

                        //log.Info($"3、获取请求消息的类型：{MsgType}");

                        //result是业务逻辑处理完成之后的待返回结果
                        var result = "";
                        try
                        {
                            var msgTypeValue = MsgType.Value;

                            if (string.IsNullOrEmpty(msgTypeValue))
                                return "";

                            //log.Info($"4、获取请求消息类型的值msgTypeValue = {msgTypeValue}");

                            //这里根据body中的MsgType和Even来区分消息，然后来处理不同的业务逻辑
                            //如果处理逻辑需要花费较长时间，可以这里先返回空(""),然后使用异步去处理业务逻辑，
                            //异步处理完后，调用微信的客服消息接口通知微信服务器
                            switch (msgTypeValue)
                            {
                                case WxMsgType.REQUEST_MSGTYPE_TEXT:
                                    result = new MsgHandler.TextMsg(doc).InitContent();
                                    log.Debug($"5、调试输入内容：{result}");
                                    break;
                                case WxMsgType.REQUEST_MSGTYPE_EVENT:
                                    var _event = doc.Element("xml").Element("Event").Value;
                                    switch (_event.ToLower().Trim())
                                    {
                                        case "click"://菜单点击
                                            result = new NewsMsg(doc).InitContent();
                                            log.Debug($"5、调试输入内容：{result}");
                                            break;
                                        case "view"://菜单链接
                                            result = "";
                                            break;
                                        case "subscribe": //关注事件
                                            //这里还有扫描推送事件（分未关注及已关注）
                                            //https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Receiving_event_pushes.html

                                            var eventkey = doc.Element("xml").Element("EventKey").Value;
                                            if (eventkey.StartsWith("qrscene_"))
                                            {
                                                //未关注时扫码，qrscene_xxxxx（xxxxx表示二维码的参数）
                                            }
                                            else
                                            {
                                                //已关注扫码
                                            }

                                            break;
                                        case "unsubscribe": //取消关注
                                            //取消关注，可以把数据库中的用户信息删除
                                            break;
                                        case "location": //上报地理位置
                                            break;
                                        default:
                                            result = "";
                                            break;
                                    }
                                    break;
                                default:
                                    result = "";
                                    break;
                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            log.Error($"Post Error 1：{ ex.Message}");
                            return "";
                        }

                        #region 解密（需要可以启用）

                        //if (!string.IsNullOrEmpty(result))
                        //{
                        //    if (isEncrypt)
                        //    {
                        //        result = EncryptHelper.AESEncrypt(result, Wx.Models.WxBaseInfo.EncodingAESKey, Wx.Models.WxBaseInfo.AppID);
                        //        var _msg_signature = MakeMsgSign(nonce, timestamp, result, Wx.Models.WxBaseInfo.AppToken);
                        //        result = $@"<xml>
                        //                            <Encrypt><![CDATA[{result}]]></Encrypt>
                        //                            <MsgSignature>{_msg_signature}</MsgSignature>
                        //                            <TimeStamp>{timestamp}</TimeStamp>
                        //                            <Nonce>{nonce}</Nonce>
                        //                        </xml>";
                        //    }
                        //    return result;
                        //}
                        #endregion


                    }

                    return "";
                }
                catch (Exception ex)
                {
                    log.Error($"Post Error 0：{ex.Message}");
                    return "";
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
