﻿/*
 * 短信服务
 * 发送一条、多条
 * 
 * **/

using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.Sms
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [Route("sms")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(SMSController));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("send")]
        public IActionResult Send(string phone)
        {
            try
            {
                return Ok(new { phone, state = true });
            }
            catch (Exception ex)
            {
                log.Error("sms/send/ error : "+ex.Message);
            }

            return Ok(phone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phones"></param>
        /// <returns></returns>
        [HttpPost("sendmany")]
        public IActionResult SendMany(List<string> phones)
        {
            return Ok(phones);
        }
    }
}
