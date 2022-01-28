using System.ComponentModel.DataAnnotations;

namespace Chat.Persistence.DTOs
{
    public class UserForLoginDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
