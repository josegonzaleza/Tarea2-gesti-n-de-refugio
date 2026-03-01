using System.Web.Mvc;

namespace Tarea2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => RedirectToAction("Login", "Auth");
    }
}