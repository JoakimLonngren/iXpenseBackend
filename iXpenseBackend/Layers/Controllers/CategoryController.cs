using iXpenseBackend.Layers.Services;
using Microsoft.AspNetCore.Mvc;

namespace iXpenseBackend.Layers.Controllers
{
    [Route("api/[Controller")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
