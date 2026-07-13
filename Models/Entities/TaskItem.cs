namespace TaskManagerAPI.Models.Entities
{
    public class TaskItem
    {
        public int Id {  get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Models.Enums.TaskStatus Status { get; set; } = Models.Enums.TaskStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int? CategoryId {  get; set; }
        public Category? Category { get; set; }
    }
}
