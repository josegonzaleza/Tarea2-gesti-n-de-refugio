using System;
using System.Linq;
using System.Web.Mvc;
using Tarea2.Models;

namespace Tarea2.Filters
{
    public class RequireRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;
        public RequireRoleAttribute(params string[] roles) { _roles = roles ?? new string[0]; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Session["user"] as UserSession;
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Auth", action = "Login" })
                );
                return;
            }

            if (_roles.Length > 0 && !_roles.Contains(user.Role))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Dashboard", action = "Index" })
                );
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}