using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.DTOs.TaskDTOs.Response
{
    public class GetTaskItemDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Models.Enums.TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        //ADD LATER
        public int? CategoryId { get; set; }
        //public Category? Category { get; set; }
    }
}
