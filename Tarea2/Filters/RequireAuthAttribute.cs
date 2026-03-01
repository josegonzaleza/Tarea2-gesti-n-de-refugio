using System.Web.Mvc;
using Tarea2.Models;

namespace Tarea2.Filters
{
    public class RequireAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Session["user"] as UserSession;
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Auth", action = "Login" }
                    )
                );
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}