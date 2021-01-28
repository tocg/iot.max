using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Admin.Controllers
{
    public class BaseCrudController : Controller
    {
        public IActionResult Index<T>()
        {
            return View();
        }
    }
}
