using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Repositories.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllAsync(Guid userId,
            string? searchTitle, 
            Guid? categoryId, 
            Models.Enums.TaskStatus? status)
        {
            IQueryable<TaskItem> query = _context.Tasks.Where(t => t.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTitle))
            {
                query = query.Where(t =>
                    t.Title.Contains(searchTitle));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(t =>
                    t.CategoryId == categoryId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(t =>
                    t.Status == status.Value);
            }

            return await query
                .Include(t => t.Category)
                .ToListAsync();
        }
        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
        }

        //public void Update(TaskItem task)
        //{
        //    _context.Tasks.Update(task);
        //}

        public void Delete(TaskItem task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
