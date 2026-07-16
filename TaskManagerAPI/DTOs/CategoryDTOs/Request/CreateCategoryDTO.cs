namespace TaskManagerAPI.DTOs.CategoryDTOs.Request
{
    public class CreateCategoryDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
