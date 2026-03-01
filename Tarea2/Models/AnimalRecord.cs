using System;
using System.Collections.Generic;

namespace Tarea2.Models
{
    public class AnimalRecord
    {
        public string Id { get; set; }
        public string Expediente { get; set; }     
        public DateTime FechaIngreso { get; set; }
        public string RegistradoPor { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }       
        public string Raza { get; set; }
        public string EdadAprox { get; set; }
        public string Sexo { get; set; }
        public string Color { get; set; }
        public string MotivoIngreso { get; set; }
        public string Temperamento { get; set; }
        public string NecesidadesMedicas { get; set; }
        public string HistorialVacunas { get; set; }
        public string EstadoSaludIngreso { get; set; } 
        public RescuerInfo Rescatista { get; set; }
        public List<Treatment> Tratamientos { get; set; }
    }
}