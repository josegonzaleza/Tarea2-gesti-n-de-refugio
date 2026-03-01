using System.Collections.Generic;
using Tarea2.Models;

namespace Tarea2.Services
{
    public class JsonDraftStore : JsonStoreBase
    {
        private string DraftsPath => Map("~/App_Data/drafts.json");

        public AnimalDraft Get(string userEmail)
        {
            var dict = Read(DraftsPath, new Dictionary<string, AnimalDraft>());
            if (dict.ContainsKey(userEmail) && dict[userEmail] != null) return dict[userEmail];
            return new AnimalDraft();
        }

        public void SaveMerged(string userEmail, AnimalDraft incoming)
        {
            var dict = Read(DraftsPath, new Dictionary<string, AnimalDraft>());

            AnimalDraft current = new AnimalDraft();
            if (dict.ContainsKey(userEmail) && dict[userEmail] != null)
                current = dict[userEmail];

            var d = current;

            d.Nombre = Pick(incoming.Nombre, d.Nombre);
            d.Especie = Pick(incoming.Especie, d.Especie);
            d.Raza = Pick(incoming.Raza, d.Raza);
            d.EdadAprox = Pick(incoming.EdadAprox, d.EdadAprox);
            d.Sexo = Pick(incoming.Sexo, d.Sexo);
            d.Color = Pick(incoming.Color, d.Color);

            d.MotivoIngreso = Pick(incoming.MotivoIngreso, d.MotivoIngreso);
            d.Temperamento = Pick(incoming.Temperamento, d.Temperamento);
            d.NecesidadesMedicas = Pick(incoming.NecesidadesMedicas, d.NecesidadesMedicas);
            d.HistorialVacunas = Pick(incoming.HistorialVacunas, d.HistorialVacunas);
            d.EstadoSaludIngreso = Pick(incoming.EstadoSaludIngreso, d.EstadoSaludIngreso);

            d.RescatistaNombre = Pick(incoming.RescatistaNombre, d.RescatistaNombre);
            d.RescatistaTelefono = Pick(incoming.RescatistaTelefono, d.RescatistaTelefono);
            d.RescatistaCorreo = Pick(incoming.RescatistaCorreo, d.RescatistaCorreo);

            dict[userEmail] = d;
            Write(DraftsPath, dict);
        }

        public void Clear(string userEmail)
        {
            var dict = Read(DraftsPath, new Dictionary<string, AnimalDraft>());
            if (dict.ContainsKey(userEmail)) dict.Remove(userEmail);
            Write(DraftsPath, dict);
        }

        private string Pick(string incoming, string fallback)
        {
            if (incoming == null) return fallback;
            var t = incoming.Trim();
            if (t.Length == 0) return fallback;
            return t;
        }
    }
}