using System.ComponentModel.DataAnnotations;

namespace L2CodePackagingAPI.DTOs
{
    public class AuthRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
