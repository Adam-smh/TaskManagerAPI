namespace TaskManagerAPI.DTOs.UserDTOs.Request
{
    public class UpdateUserPasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
