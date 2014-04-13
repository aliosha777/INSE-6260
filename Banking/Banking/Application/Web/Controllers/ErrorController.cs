using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banking.Application.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult General(string message)
        {
            return View();
        }
    }
}
