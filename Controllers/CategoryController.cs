using FinanceManager.Data.DataTransferObjects;
using FinanceManager.Data.Response;
using FinanceManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinanceManager.Controllers
{
    [ApiController]
    [Route("api/category/")]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Create(CategoryDto category)
        {
            var response = await _categoryService.Create(category);
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GetCategories()
        {
            var response = await _categoryService.GetCategoriesForUser();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{categoryId}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> DeleteCategory(long categoryId)
        {
            var response = await _categoryService.DeleteCategory(categoryId);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
