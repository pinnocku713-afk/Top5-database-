using System.ComponentModel.DataAnnotations;

namespace Top5.Models
{
    public class LoginInput
    {
        [Required(ErrorMessage = "Give an email")]
        [EmailAddress(ErrorMessage = "That is not an email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Give a password")]
        public string Password { get; set; } = "";
    }
}