using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs.AuthDTOs.Request
{
    public class RegisterDTO
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        public string? ProfileImage { get; set; }
    }
}
