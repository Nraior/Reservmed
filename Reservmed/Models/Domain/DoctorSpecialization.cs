namespace Reservmed.Models.Domain
{
    public class DoctorSpecialization
    {
        public int SpecializationId { get; set; }
        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }
        public Specialization? Specialization { get; set; }

        public DateTime? DateAcquired { get; set; }
    }
}
