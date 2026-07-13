namespace TaskManagerAPI.DTOs.CategoryDTOs.Response
{
    public class FetchCategoryDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

    }
}
