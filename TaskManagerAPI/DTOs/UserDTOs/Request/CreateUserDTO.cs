using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs.UserDTOs.Request
{
    public class CreateUserDTO
    {

        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        public string? ProfileImage { get; set; }
    }
}
