namespace TaskManagerAPI.DTOs.TaskDTOs.Request
{
    public class UpdateTaskItemDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Models.Enums.TaskStatus? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
