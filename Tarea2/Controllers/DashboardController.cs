using System.Web.Mvc;
using Tarea2.Filters;
using Tarea2.Models;

namespace Tarea2.Controllers
{
    [RequireAuth]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            var s = Session["user"] as UserSession;
            ViewBag.Role = s.Role;
            ViewBag.Email = s.Email;
            return View();
        }
    }
}