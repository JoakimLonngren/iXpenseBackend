using iXpenseBackend.Data.Entities;
using iXpenseBackend.Layers.Repositories;

namespace iXpenseBackend.Layers.Services
{
    public class CategoryService
    {
        private readonly CategoryRepo _categoryRepo;

        public CategoryService(CategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryRepo.GetCategoryByIdAsync(categoryId);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepo.GetAllCategoriesAsync();
        }
    }
}
