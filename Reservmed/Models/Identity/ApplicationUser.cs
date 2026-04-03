using Microsoft.AspNetCore.Identity;

namespace Reservmed.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool isActive { get; set; } = false;
    }
}
