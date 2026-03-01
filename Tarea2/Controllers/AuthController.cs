using System.Web.Mvc;
using Tarea2.Models;
using Tarea2.Services;

namespace Tarea2.Controllers
{
    public class AuthController : Controller
    {
        private readonly JsonUserStore _users = new JsonUserStore();

        [HttpGet]
        public ActionResult Login()
        {
            DataStore.EnsureSeed();

            var s = Session["user"] as UserSession;
            if (s != null) return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            DataStore.EnsureSeed();

            var genericError = "Credenciales inválidas.";

            email = (email ?? "").Trim();
            password = (password ?? "").Trim();

            var res = _users.Validate(email, password);
            if (!res.Ok)
            {
                ViewBag.Error = genericError;
                return View();
            }

            Session["user"] = new UserSession { Email = email, Role = res.Role };
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login");
        }
    }
}