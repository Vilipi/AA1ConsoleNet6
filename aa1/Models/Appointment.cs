namespace aa1.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Specialist Specialist { get; set; }
        public Patient Patient { get; set; }
        public DateTime AppointmentCreationDate { get; set; }
        public bool IsCompleted { get; set; } // lo marca el specilaista. Inicialmente false
        public decimal Price { get; set; }
        public string SpecialistComment { get; set; } // al terminar por el specialista. Inicialmente null
    }
}