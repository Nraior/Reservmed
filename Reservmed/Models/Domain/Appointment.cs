namespace Reservmed.Models.Domain
{
    public class Appointment
    {
        public int Id { get; set; }
        public required string DoctorId { get; set; }
        public required string PatientId { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public DateTime AppontmentStart { get; set; }
        public int? ExpectedDurationMinutes { get; set; } = 30;
        public required string Title { get; set; }
        public string? AppointmentSpecialization { get; set; }
        public string? PatientAppointmentInfo { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Created;
    }

    public enum AppointmentStatus
    {
        Created,
        Rescheduled,
        Cancelled,
        Confirmed,
        Completed
    }
}
