using System;
using System.Collections.Generic;
using System.Linq;
using Tarea2.Models;

namespace Tarea2.Services
{
    public class JsonAnimalStore : JsonStoreBase
    {
        private string AnimalsPath => Map("~/App_Data/animals.json");
        private string CountersPath => Map("~/App_Data/counters.json");

        private static readonly string[] AllowedSpecies = new[] { "Perro", "Gato", "Roedor", "Conejo" };

        public List<AnimalRecord> GetAll()
        {
            var list = Read(AnimalsPath, new List<AnimalRecord>());
            if (list == null) list = new List<AnimalRecord>();
            foreach (var a in list) Normalize(a);
            return list;
        }

        public AnimalRecord GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            var a = GetAll().FirstOrDefault(x => x != null && x.Id == id);
            Normalize(a);
            return a;
        }

        public string CreateFromDraft(AnimalDraft d, string registeredBy)
        {
            if (d == null) throw new Exception("Draft vacío.");

            
            if (string.IsNullOrWhiteSpace(d.Especie) || !AllowedSpecies.Contains(d.Especie))
                throw new Exception("Especie inválida. Solo: Perro, Gato, Roedor o Conejo.");

            var animals = GetAll();
            if (animals == null) animals = new List<AnimalRecord>();

            var id = Guid.NewGuid().ToString("N");
            var expediente = GenerateExpediente();

            var record = new AnimalRecord
            {
                Id = id,
                Expediente = expediente,
                FechaIngreso = DateTime.Now,
                RegistradoPor = registeredBy,

                Nombre = d.Nombre,
                Especie = d.Especie,
                Raza = d.Raza,
                EdadAprox = d.EdadAprox,
                Sexo = d.Sexo,
                Color = d.Color,

                MotivoIngreso = d.MotivoIngreso,
                Temperamento = d.Temperamento,
                NecesidadesMedicas = d.NecesidadesMedicas,
                HistorialVacunas = d.HistorialVacunas,
                EstadoSaludIngreso = d.EstadoSaludIngreso,

                Rescatista = new RescuerInfo
                {
                    FullName = d.RescatistaNombre,
                    Phone = d.RescatistaTelefono,
                    Email = d.RescatistaCorreo
                },

                Tratamientos = new List<Treatment>()
            };

            Normalize(record);

            animals.Add(record);
            Write(AnimalsPath, animals);

            return id;
        }

        public void Update(AnimalRecord updated)
        {
            if (updated == null) throw new Exception("Datos vacíos.");

            
            if (string.IsNullOrWhiteSpace(updated.Especie) || !AllowedSpecies.Contains(updated.Especie))
                throw new Exception("Especie inválida. Solo: Perro, Gato, Roedor o Conejo.");

            var animals = GetAll();
            var existing = animals.FirstOrDefault(x => x.Id == updated.Id);
            if (existing == null) return;

            Normalize(existing);
            if (updated.Rescatista == null) updated.Rescatista = new RescuerInfo();

            existing.Nombre = updated.Nombre;
            existing.Especie = updated.Especie;
            existing.Raza = updated.Raza;
            existing.EdadAprox = updated.EdadAprox;
            existing.Sexo = updated.Sexo;
            existing.Color = updated.Color;

            existing.MotivoIngreso = updated.MotivoIngreso;
            existing.Temperamento = updated.Temperamento;
            existing.NecesidadesMedicas = updated.NecesidadesMedicas;
            existing.HistorialVacunas = updated.HistorialVacunas;

            existing.EstadoSaludIngreso = updated.EstadoSaludIngreso;

            existing.Rescatista.FullName = updated.Rescatista.FullName;
            existing.Rescatista.Phone = updated.Rescatista.Phone;
            existing.Rescatista.Email = updated.Rescatista.Email;

            Write(AnimalsPath, animals);
        }

        public void AddTreatment(string animalId, Treatment t)
        {
            var animals = GetAll();
            var a = animals.FirstOrDefault(x => x.Id == animalId);
            if (a == null) throw new Exception("Animal no encontrado.");

            Normalize(a);
            a.Tratamientos.Add(t);

            Write(AnimalsPath, animals);
        }

        public void UpdateTreatment(string animalId, Treatment t)
        {
            var animals = GetAll();
            var a = animals.FirstOrDefault(x => x.Id == animalId);
            if (a == null) throw new Exception("Animal no encontrado.");

            Normalize(a);
            var existing = a.Tratamientos.FirstOrDefault(x => x.Id == t.Id);
            if (existing == null) throw new Exception("Tratamiento no encontrado.");

            existing.Date = t.Date;
            existing.Type = t.Type;
            existing.MedicineId = t.MedicineId;
            existing.MedicineName = t.MedicineName;
            existing.Dose = t.Dose;
            existing.Observations = t.Observations;
            existing.CaregiverEmail = t.CaregiverEmail;

            Write(AnimalsPath, animals);
        }

        public Treatment GetTreatment(string animalId, string treatmentId)
        {
            var a = GetById(animalId);
            if (a == null) return null;
            Normalize(a);
            return a.Tratamientos.FirstOrDefault(x => x.Id == treatmentId);
        }

        private string GenerateExpediente()
        {
            var counters = Read(CountersPath, new Dictionary<string, int>());
            if (counters == null) counters = new Dictionary<string, int>();

            var year = DateTime.Now.Year.ToString();
            if (!counters.ContainsKey(year)) counters[year] = 0;
            counters[year]++;

            Write(CountersPath, counters);

            return year + "-" + counters[year].ToString("00000");
        }

        private void Normalize(AnimalRecord a)
        {
            if (a == null) return;
            if (a.Rescatista == null) a.Rescatista = new RescuerInfo();
            if (a.Tratamientos == null) a.Tratamientos = new List<Treatment>();
        }
    }
}