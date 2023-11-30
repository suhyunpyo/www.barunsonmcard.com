using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Controllers
{
    public class CustomErrorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("CustomErrors/{StatusCode}")]
        public IActionResult StatusCodeHandle(int statusCode)
        {
            //switch (statusCode)
            //{
            //    case 404:
            //        ViewBag.ErrorMessasge = $"I am having {statusCode}" + " Error code Message";
            //        break;


            //}
            return View(statusCode);
        }
    }
}
