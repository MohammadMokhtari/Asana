using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Asana.Domain.Entities.Categories;
using Asana.Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asana.Application.Common.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMemoryCache _memoryCache;

        public CategoryService(IGenericRepository<Category> categoryRepositoy,
            IMapper mapper,
            ILogger<CategoryService> logger, IApplicationDbContext dbContext,
            IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepositoy;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }

        private readonly string categoriesNavigationCacheKey = "CategoryCacheKey";
        private readonly string maincategoriesNavigationCacheKey = "MainCategoryCacheKey";

        public async Task<(Result result, IEnumerable<NavigationCategoriesDto> navigationCategoriesDtos)> GetNavigationCategories()
        {
            try
            {
                if (_memoryCache.TryGetValue(categoriesNavigationCacheKey, out IEnumerable<NavigationCategoriesDto> categoriesDtoCashed))
                {
                    _logger.LogInformation("Get Categories Navigation from MemoryCache to be successful");
                    return (Result.Success(), categoriesDtoCashed);
                }

                //use StoreProcedure and Recersive CTE
                List<Category> categories = await _dbContext.Categories
                    .FromSqlRaw("EXECUTE dbo.sp_GetNavigationCategories")
                     .IgnoreQueryFilters()
                         .ToListAsync();

                categories = categories.Where(c => c.ParentId == null).ToList();

                var categoriesDto = _mapper.Map<IEnumerable<NavigationCategoriesDto>>(categories);

                var chachOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromHours(12))
                   .SetAbsoluteExpiration(TimeSpan.FromDays(1));

                _memoryCache.Set(categoriesNavigationCacheKey, categoriesDto, chachOptions);
                _logger.LogInformation("Set NavigationCategories to MemoryCache has successful");

                _logger.LogInformation("Get NavigationCategories to be successfull");
                return (Result.Success(), categoriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCategories Failed !");
                return (Result.Failure("COULD_NOT_GET_NAVGATION_CATEGORIES"), null);
            }
        }

        public async Task<(Result result, CategoryDetailsDto categoryDetailsDto)> GetCategory(long categoryId)
        {
            try
            {
                Category category = await _categoryRepository.GetEntitiesQuery()
                    .Include(c => c.MediaFile)
                     .Include(c => c.Categories)
                      .ThenInclude(c => c.MediaFile)
                       .SingleOrDefaultAsync(c => c.Id == categoryId);

                var categoryInfoDto = _mapper.Map<CategoryInfoDto>(category);

                var menuCategories = await GetNavigationSubCategories(categoryId);

                var categorDetailDto = new CategoryDetailsDto()
                {
                    categoryInfo = categoryInfoDto,
                    ChildCategories = menuCategories.navigationCategoriesDtos
                };


                _logger.LogInformation("Category to be successfull");
                return (Result.Success(), categorDetailDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Category Failed !");
                return (Result.Failure("COULD_NOT_GET_CATEGORY"), null);
            }
        }

        public async Task<(Result result, IEnumerable<MainCategoryDto> mainCategoryDtos)> GetMainCategories()
        {
            try
            {
                if (_memoryCache.TryGetValue(maincategoriesNavigationCacheKey, out IEnumerable<MainCategoryDto> mainCatsCached))
                {
                    _logger.LogInformation("Get MainCategories from MemoryCache to be successful");
                    return (Result.Success(), mainCatsCached);
                }

                var mainCats = await _categoryRepository.GetEntitiesQuery()
                   .Where(c => c.ParentId == null)
                   .Include(C => C.MediaFile)
                      .AsNoTracking()
                       .ToListAsync();

                var mainCatDto = _mapper.Map<IEnumerable<MainCategoryDto>>(mainCats);
                 
                var chachOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromHours(12))
                   .SetAbsoluteExpiration(TimeSpan.FromDays(1));

                _memoryCache.Set(maincategoriesNavigationCacheKey, mainCatDto, chachOptions);

                _logger.LogInformation("Get Main Categories to be successfull");
                return (Result.Success(), mainCatDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Main Categories Failed!");
                return (Result.Failure("COULD_NOT_GET_MAIN_CATEGORIES"), null);
            }
        }

        public async Task<(Result result, NavigationCategoriesDto navigationCategoriesDtos)> GetNavigationSubCategories(long categoryId)
        {
            try
            {
                //use StoreProcedure and Recersive CTE
                var navigationCategories = await _dbContext.Categories
                    .FromSqlInterpolated($"EXECUTE dbo.sp_GetNavigationSubCategory @Id = {categoryId}")
                     .IgnoreQueryFilters()
                      .ToListAsync();

                var navigationCategory = navigationCategories.SingleOrDefault(c => c.Id == categoryId);
                var navigationCategoriesDto = _mapper.Map<NavigationCategoriesDto>(navigationCategory);

                _logger.LogInformation("Get GetNavigationSubCategories to be successfull");
                return (Result.Success(), navigationCategoriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetNavigationSubCategories Failed!");
                return (Result.Failure("COULD_NOT_GET_NAVGATION_CATEGORIES"), null);
            }
        }
    }
}
