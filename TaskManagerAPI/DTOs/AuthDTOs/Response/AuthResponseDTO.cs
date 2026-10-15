namespace TaskManagerAPI.DTOs.AuthDTOs.Response
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;

        //public Guid UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
