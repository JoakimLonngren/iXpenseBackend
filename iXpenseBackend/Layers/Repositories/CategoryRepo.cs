using iXpenseBackend.Data.DbContext;
using iXpenseBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace iXpenseBackend.Layers.Repositories
{
    public class CategoryRepo
    {
        private readonly Context _context;

        public CategoryRepo(Context context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
