﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AguasNico.Controllers
{
    public class BaseController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_configuration["BlockPage"] == "true")
            {
                filterContext.Result = new ViewResult { ViewName = "~/Views/PageBlocked.cshtml" };
                return;
            }
            ViewBag.PaidPage = _configuration["PaidPage"] == "true" ? true : false;
        }
    }
}
