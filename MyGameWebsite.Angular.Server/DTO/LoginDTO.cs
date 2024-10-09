using System.ComponentModel.DataAnnotations;

namespace MyGameWebsite.Server.DTO
{
    public class LoginDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username must be less than 50 characters.")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }
    }
}
