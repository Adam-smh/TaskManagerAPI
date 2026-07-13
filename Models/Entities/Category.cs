namespace TaskManagerAPI.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string? Description { get; set; }
    }
}
