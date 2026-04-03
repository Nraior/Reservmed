using Reservmed.Models.Identity;

namespace Reservmed.DTOs.Internal
{
    public class ApplicationUserCreationDto
    {
        public ApplicationUser? UserIdentity { get; set; }
        public bool IsNewUser { get; set; }
    }
}
