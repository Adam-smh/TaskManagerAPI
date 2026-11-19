namespace TaskManagerAPI.Models.Entities
{
    public class TaskItem
    {
        public Guid Id {  get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Models.Enums.TaskStatus Status { get; set; } = Models.Enums.TaskStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public Guid? CategoryId {  get; set; }
        public Category? Category { get; set; }

        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
