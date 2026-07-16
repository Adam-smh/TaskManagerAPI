namespace TaskManagerAPI.DTOs.CategoryDTOs.Request
{
    public class UpdateCategoryDTO
    {
        public required string? Name { get; set; }
        public string? Description { get; set; }
    }
}
