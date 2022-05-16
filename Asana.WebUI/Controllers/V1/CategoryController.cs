using Asana.Application.Common.Interfaces;
using Asana.Application.Utilities.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Asana.WebUI.Controllers.V1
{
    [ApiVersion("1.0")]
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (result, categoryResponse) = await _categoryService.GetNavigationCategories();

            return result.Succeeded ? JsonResponseStatus.Success(categoryResponse) :
                                JsonResponseStatus.BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> index(long id)
        {
            var (result, categoryResponse) = await _categoryService.GetCategory(id);

            return result.Succeeded ? JsonResponseStatus.Success(categoryResponse) :
                    JsonResponseStatus.BadRequest(result.Errors);
        }

        [HttpGet("main")]
        public async Task<IActionResult> Main()
        {
            var (result, mainCategories) = await _categoryService.GetMainCategories();

            return result.Succeeded ? JsonResponseStatus.Success(mainCategories) :
                    JsonResponseStatus.BadRequest(result.Errors);
        }
    }
}
