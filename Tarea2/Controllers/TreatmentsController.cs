using System;
using System.Linq;
using System.Web.Mvc;
using Tarea2.Filters;
using Tarea2.Models;
using Tarea2.Services;

namespace Tarea2.Controllers
{
    [RequireAuth]
    public class TreatmentsController : Controller
    {
        private readonly JsonAnimalStore _animals = new JsonAnimalStore();
        private readonly JsonMedsStore _meds = new JsonMedsStore();

        private static readonly string[] AllowedTypes = new[] { "Medicamento", "Cirugía", "Vacuna", "Curación" };

        [HttpGet]
        public ActionResult Add(string animalId)
        {
            ViewBag.AnimalId = animalId;
            ViewBag.Meds = _meds.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string animalId, string treatmentId)
        {
            var t = _animals.GetTreatment(animalId, treatmentId);
            if (t == null) return RedirectToAction("Details", "Animals", new { id = animalId });

            ViewBag.AnimalId = animalId;
            ViewBag.Meds = _meds.GetAll();
            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddJson(string animalId, string date, string type, string medicineId, string dose, string observations)
        {
            try
            {
                var u = (UserSession)Session["user"];
                var meds = _meds.GetAll();
                var med = meds.FirstOrDefault(m => m.id == medicineId);

                DateTime dt;
                if (!DateTime.TryParse(date, out dt)) return Json(new { ok = false, msg = "Fecha inválida." });
                if (dt.Date > DateTime.Today) return Json(new { ok = false, msg = "La fecha no puede ser futura." });

                if (string.IsNullOrWhiteSpace(type) || !AllowedTypes.Contains(type))
                    return Json(new { ok = false, msg = "Tipo inválido." });

                if (string.IsNullOrWhiteSpace(medicineId) || med == null)
                    return Json(new { ok = false, msg = "Medicamento obligatorio." });

                if (string.IsNullOrWhiteSpace(dose))
                    return Json(new { ok = false, msg = "Dosis obligatoria." });

                var t = new Treatment
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Date = dt.Date,
                    Type = type,
                    MedicineId = medicineId,
                    MedicineName = med.name,
                    Dose = dose,
                    Observations = observations,
                    CaregiverEmail = u.Email
                };

                _animals.AddTreatment(animalId, t);
                return Json(new { ok = true, treatmentId = t.Id });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditJson(string animalId, string id, string date, string type, string medicineId, string dose, string observations)
        {
            try
            {
                var u = (UserSession)Session["user"];
                var meds = _meds.GetAll();
                var med = meds.FirstOrDefault(m => m.id == medicineId);

                DateTime dt;
                if (!DateTime.TryParse(date, out dt)) return Json(new { ok = false, msg = "Fecha inválida." });
                if (dt.Date > DateTime.Today) return Json(new { ok = false, msg = "La fecha no puede ser futura." });

                if (string.IsNullOrWhiteSpace(type) || !AllowedTypes.Contains(type))
                    return Json(new { ok = false, msg = "Tipo inválido." });

                if (string.IsNullOrWhiteSpace(medicineId) || med == null)
                    return Json(new { ok = false, msg = "Medicamento obligatorio." });

                if (string.IsNullOrWhiteSpace(dose))
                    return Json(new { ok = false, msg = "Dosis obligatoria." });

                var t = new Treatment
                {
                    Id = id,
                    Date = dt.Date,
                    Type = type,
                    MedicineId = medicineId,
                    MedicineName = med.name,
                    Dose = dose,
                    Observations = observations,
                    CaregiverEmail = u.Email
                };

                _animals.UpdateTreatment(animalId, t);
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }
    }
}