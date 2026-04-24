using Reservmed.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Reservmed.Models.Domain
{
    public class Doctor
    {

        public int Id { get; set; }

        public required string ApplicationUserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        [StringLength(20)]
        public required string LicenseNumber { get; set; }

        public ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();

        public string? ProfilePictureUrl { get; set; }
        public string? Description { get; set; }
        // navigation property
        public ApplicationUser? User { get; set; }
        public bool IsVerifiedByAdmin { get; set; } = false;
    }
}
