using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AguasNico.Controllers
{
    public class BaseController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (bool.Parse(_configuration["BlockPage"]))
            {
                filterContext.Result = new ViewResult { ViewName = "~/Views/PageBlocked.cshtml" };
                return;
            }
            ViewBag.PaidPage = _configuration["PaidPage"];
        }
    }
}
