using Reservmed.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Reservmed.Models.Domain
{
    public class Patient
    {

        public int Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        // navigation property
        public ApplicationUser User { get; set; }
    }
}
