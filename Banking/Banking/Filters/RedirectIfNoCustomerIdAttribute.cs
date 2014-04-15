using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Filters
{
    using System.Web.Mvc;

    using Banking.Application.Web.Controllers;

    public class RedirectIfNoCustomerIdAttribute : ActionFilterAttribute
    {
        private string customerIdSessionKey;

        public RedirectIfNoCustomerIdAttribute(string customerIdSessionKey)
        {
            this.customerIdSessionKey = customerIdSessionKey;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Session[customerIdSessionKey] == null)
            {
                var controller = (TellerController)filterContext.Controller;
                controller.RedirectToAction("Home", "Teller");
            }
        }
    }
}