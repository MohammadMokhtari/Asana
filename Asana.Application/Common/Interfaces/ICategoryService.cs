using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface ICategoryService
    {
        Task<(Result result, IEnumerable<NavigationCategoriesDto> navigationCategoriesDtos)> GetNavigationCategories();

        Task<(Result result, NavigationCategoriesDto navigationCategoriesDtos)> GetNavigationSubCategories(long categoryId);

        Task<(Result result, CategoryDetailsDto categoryDetailsDto)> GetCategory(long categoryId);

        Task<(Result result, IEnumerable<MainCategoryDto> mainCategoryDtos)> GetMainCategories();
    }
}
