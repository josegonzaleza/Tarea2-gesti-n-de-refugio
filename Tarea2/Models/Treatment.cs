using System;

namespace Tarea2.Models
{
    public class Treatment
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }         
        public string Type { get; set; }           
        public string MedicineId { get; set; }     
        public string MedicineName { get; set; }
        public string Dose { get; set; }           
        public string Observations { get; set; }   
        public string CaregiverEmail { get; set; } 
    }
}