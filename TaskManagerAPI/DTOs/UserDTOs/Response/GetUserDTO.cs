using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.DTOs.UserDTOs.Response
{
    public class GetUserDTO
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public Models.Enums.UserRoles Role { get; set; }
        public string? ProfileImage { get; set; }
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}
