using System;
using System.Web.Mvc;
using Tarea2.Filters;
using Tarea2.Services;

namespace Tarea2.Controllers
{
    [RequireAuth]
    [RequireRole("Admin")]
    public class MedsController : Controller
    {
        private readonly JsonMedsStore _meds = new JsonMedsStore();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListJson()
        {
            return Json(_meds.GetAll(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddJson(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return Json(new { ok = false, msg = "Nombre obligatorio." });

                _meds.Add(name.Trim());
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteJson(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return Json(new { ok = false, msg = "Id requerido." });

                _meds.Delete(id);
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }
    }
}