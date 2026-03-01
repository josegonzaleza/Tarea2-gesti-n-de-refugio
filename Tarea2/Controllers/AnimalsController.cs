using System;
using System.Web.Mvc;
using Tarea2.Filters;
using Tarea2.Models;
using Tarea2.Services;

namespace Tarea2.Controllers
{
    [RequireAuth]
    public class AnimalsController : Controller
    {
        private readonly JsonAnimalStore _animals = new JsonAnimalStore();
        private readonly JsonDraftStore _drafts = new JsonDraftStore();

        [HttpGet]
        public ActionResult List()
        {
            var list = _animals.GetAll();
            return View(list);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var a = _animals.GetById(id);
            if (a == null) return RedirectToAction("List");
            return View(a);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveJson(AnimalRecord updated)
        {
            try
            {
                _animals.Update(updated);
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }

        [HttpGet]
        [RequireRole("Admin")]
        public ActionResult Register(int step = 1)
        {
            if (step < 1) step = 1;
            if (step > 3) step = 3;

            var s = (UserSession)Session["user"];
            var draft = _drafts.Get(s.Email);

            ViewBag.Step = step;
            return View(draft);
        }

        [HttpPost]
        [RequireRole("Admin")]
        public JsonResult SaveDraft(AnimalDraft draft)
        {
            try
            {
                var s = (UserSession)Session["user"];
                _drafts.SaveMerged(s.Email, draft ?? new AnimalDraft());
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }

        [HttpPost]
        [RequireRole("Admin")]
        public JsonResult FinalizeDraft()
        {
            try
            {
                var s = (UserSession)Session["user"];
                var d = _drafts.Get(s.Email);

                if (string.IsNullOrWhiteSpace(d.Especie) || string.IsNullOrWhiteSpace(d.EdadAprox))
                    return Json(new { ok = false, msg = "Faltan datos del Paso 1." });

                if (string.IsNullOrWhiteSpace(d.MotivoIngreso) ||
                    string.IsNullOrWhiteSpace(d.Temperamento) ||
                    string.IsNullOrWhiteSpace(d.NecesidadesMedicas) ||
                    string.IsNullOrWhiteSpace(d.EstadoSaludIngreso))
                    return Json(new { ok = false, msg = "Faltan datos del Paso 2." });

                if (string.IsNullOrWhiteSpace(d.RescatistaNombre) || string.IsNullOrWhiteSpace(d.RescatistaTelefono))
                    return Json(new { ok = false, msg = "Faltan datos del Paso 3." });

                var id = _animals.CreateFromDraft(d, s.Email);
                _drafts.Clear(s.Email);

                return Json(new { ok = true, id = id });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }
    }
}