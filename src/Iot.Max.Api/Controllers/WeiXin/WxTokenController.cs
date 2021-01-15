using Iot.Max.Wx;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.WeiXin
{
    [Route("wx/token")]
    [ApiController]
    public class WxTokenController : ControllerBase
    {
        [HttpGet("access")]
        public IActionResult GetAccessToken()
        {
            return Ok(new { token = WxToken.Access_Token });
        }
    }
}
