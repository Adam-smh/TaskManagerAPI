using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TaskManagerAPI.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string Password { get; set; }
        public Models.Enums.UserRoles Role { get; set; } = Models.Enums.UserRoles.member;
        public string? ProfileImage { get; set; }
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}