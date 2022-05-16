using Asana.Application.Common.Mappings;
using Asana.Domain.Entities.Categories;
using AutoMapper;
using System.Collections.Generic;

namespace Asana.Application.DTOs
{
    public class NavigationCategoriesDto : IMapFrom<Category>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, NavigationCategoriesDto>()
                .ForMember(c => c.ChildCategories, opt => opt.MapFrom(c => c.Categories));
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<NavigationCategoriesDto> ChildCategories { get; set; }
    }

    public class CategoryInfoDto : IMapFrom<Category>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryInfoDto>()
                .ForMember(c => c.ChildCategories, opt => opt.MapFrom(c => c.Categories));
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryInfoDto> ChildCategories { get; set; }

        public MediaFileDto MediaFile { get; set; }
    }

    public class MainCategoryDto : IMapFrom<Category>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public MediaFileDto MediaFile { get; set; }
    }

    public class CategoryDetailsDto
    {
        public CategoryInfoDto categoryInfo { get; set; }

        public NavigationCategoriesDto ChildCategories { get; set; }
    }
}
