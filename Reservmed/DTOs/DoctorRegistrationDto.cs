using System.ComponentModel.DataAnnotations;

namespace Reservmed.DTOs
{
    public class DoctorRegistrationDto
    {
        [Required]
        [MaxLength(40)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(40)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [StringLength(20)]
        public string LicenseNumber { get; set; }

        public string? Description { get; set; }

        public List<String> Specializations { get; set; } = new List<String>();
        public string? ProfilePictureUrl { get; set; }

        // Auth

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

    }
}
