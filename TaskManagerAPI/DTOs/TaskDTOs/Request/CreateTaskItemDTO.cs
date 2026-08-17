namespace TaskManagerAPI.DTOs.TaskDTOs.Request
{
    public class CreateTaskItemDTO
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? CategoryId { get; set; }
        public required Guid UserId { get; set; }

    }
}
