using TaskManagerAPI.DTOs.CategoryDTOs.Request;
using TaskManagerAPI.DTOs.CategoryDTOs.Response;

namespace TaskManagerAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task CreateCategoryAsync(CreateCategoryDTO req);
        Task<List<FetchCategoryDTO>> GetAllCategoriesAsync();
        //Task<FetchCategoryDTO> GetCategoryById(Guid id);
    }
}
