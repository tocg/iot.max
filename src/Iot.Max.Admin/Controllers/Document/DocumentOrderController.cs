﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Admin.Controllers.Document
{
    public class DocumentOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
