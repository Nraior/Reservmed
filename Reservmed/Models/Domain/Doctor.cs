using Reservmed.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Reservmed.Models.Domain
{
    public class Doctor
    {

        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [StringLength(20)]
        public string LicenseNumber { get; set; }

        public List<String> Specializations { get; set; } = new List<string>();

        public string? ProfilePictureUrl { get; set; }

        public string? Description { get; set; }
        // navigation property
        public ApplicationUser User { get; set; }
        public bool IsVerifiedByAdmin { get; set; } = false;
    }
}
