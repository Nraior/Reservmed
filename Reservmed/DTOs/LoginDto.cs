using System.ComponentModel.DataAnnotations;
namespace Reservmed.DTOs
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
