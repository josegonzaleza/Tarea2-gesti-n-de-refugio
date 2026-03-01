using System;
using System.Collections.Generic;
using System.Linq;
using Tarea2.Models;

namespace Tarea2.Services
{
    public class JsonMedsStore : JsonStoreBase
    {
        private string MedsPath => Map("~/App_Data/meds.json");

        public List<MedItem> GetAll()
        {
            var list = Read(MedsPath, new List<MedItem>());
            if (list == null) list = new List<MedItem>();
            return list;
        }

        public void Add(string name)
        {
            var list = GetAll();
            list.Add(new MedItem { id = Guid.NewGuid().ToString("N"), name = name });
            Write(MedsPath, list);
        }

        public void Delete(string id)
        {
            var list = GetAll();
            list = list.Where(m => m.id != id).ToList();
            Write(MedsPath, list);
        }

        public void EnsureSeed()
        {
            var list = GetAll();
            if (list.Count > 0) return;
            //Medicamentos quemados
            list.Add(new MedItem { id = Guid.NewGuid().ToString("N"), name = "Ivermectina" });
            list.Add(new MedItem { id = Guid.NewGuid().ToString("N"), name = "Amoxicilina" });
            list.Add(new MedItem { id = Guid.NewGuid().ToString("N"), name = "Vacuna antirrábica" });
            Write(MedsPath, list);
        }
    }
}