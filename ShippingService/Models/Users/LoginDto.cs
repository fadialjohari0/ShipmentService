using System.ComponentModel.DataAnnotations;

namespace ShipmentService.API.Models.Users
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "Your Password must be between {2} and {1} characters!", MinimumLength = 6)]
        public string Password { get; set; }
    }
}