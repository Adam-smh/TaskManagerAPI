using TaskManagerAPI.DTOs.CategoryDTOs.Request;
using TaskManagerAPI.DTOs.CategoryDTOs.Response;

namespace TaskManagerAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<FetchCategoryDTO>> GetAllCategoriesAsync();
        Task<FetchCategoryDTO> GetCategoryById(Guid id);
        Task CreateCategoryAsync(CreateCategoryDTO req);
        Task UpdateCategoryAsync(Guid id, UpdateCategoryDTO req);
        Task DeleteCategoryAsync(Guid id);
    }
}
