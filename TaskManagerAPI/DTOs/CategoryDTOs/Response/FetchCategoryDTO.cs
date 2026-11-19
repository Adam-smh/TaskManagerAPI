namespace TaskManagerAPI.DTOs.CategoryDTOs.Response
{
    public class FetchCategoryDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

    }
}
