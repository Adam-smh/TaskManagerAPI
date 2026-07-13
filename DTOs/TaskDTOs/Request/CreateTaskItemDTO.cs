namespace TaskManagerAPI.DTOs.TaskDTOs.Request
{
    public class CreateTaskItemDTO
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CategoryId { get; set; }


    }
}
