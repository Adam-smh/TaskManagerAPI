using TaskManagerAPI.Controllers;
using TaskManagerAPI.DTOs.CategoryDTOs.Request;
using TaskManagerAPI.DTOs.CategoryDTOs.Response;
using TaskManagerAPI.DTOs.TaskDTOs.Request;
using TaskManagerAPI.DTOs.TaskDTOs.Response;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Repositories.CategoryRepository;

namespace TaskManagerAPI.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _repo;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository repo, ILogger<CategoryService> logger)
        {
            _repo = repo;   
            _logger = logger;
        }

        public async Task<List<FetchCategoryDTO>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _repo.GetAllAsync();

                return categories.Select(c => new FetchCategoryDTO
                { 
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch categories");
                throw;
            }

        }

        public async Task<FetchCategoryDTO> GetCategoryById(Guid id)
        {
            var cat = await _repo.GetByIdAsync(id);

            var result = new FetchCategoryDTO()
            {
                Id = cat.Id,
                Name = cat.Name, 
                Description = cat.Description
            };

            return result;
        }

        public async Task CreateCategoryAsync(CreateCategoryDTO req)
        {
            if (string.IsNullOrEmpty(req.Name)) 
            {
                _logger.LogWarning("Category Creation Failed: Name required");
                throw new Exception("Name is required");
            }

            var cat = new Category 
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Description = req.Description,
            };

            try
            {
                await _repo.AddAsync(cat);
                await _repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Category Creation Failed: {Name}", cat.Name);
                throw;
            }

        }

        public async Task UpdateCategoryAsync(Guid id, UpdateCategoryDTO req)
        {

            var cat = await _repo.GetByIdAsync(id);

            if (cat == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            if (req.Name != null)
                cat.Name = req.Name;

            if (req.Description != null)
                cat.Description = req.Description;

            await _repo.SaveChangesAsync();

        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            Category? cat = await _repo.GetByIdAsync(id);

            if (cat == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            _repo.Delete(cat);
            await _repo.SaveChangesAsync();
        }

    }
}
