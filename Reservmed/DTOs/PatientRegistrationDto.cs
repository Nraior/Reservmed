using System.ComponentModel.DataAnnotations;

namespace Reservmed.DTOs
{
    public class PatientRegistrationDto
    {
        [Required]
        [MaxLength(40)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(40)]
        public string LastName { get; set; }
        [Required, DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
