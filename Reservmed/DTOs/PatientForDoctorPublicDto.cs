namespace Reservmed.DTOs
{
    public class PatientForDoctorPublicDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }

        public int Age => DateTime.UtcNow.Year - BirthDate.Year;
    }
}
