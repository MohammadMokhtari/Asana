using Asana.Application.Common.Mappings;
using Asana.Domain.Entities.Addresses;
using AutoMapper;
using System.Collections.Generic;

namespace Asana.Application.DTOs
{
    public class ProvinceOptionDtoResponse
    {
        public List<ProvinceOptionDto> ProvinceOptions { get; set; }

        public List<CityOptionDto> CityOptions { get; set; }
    }

    public class ProvinceOptionDto : IMapFrom<State>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<State, ProvinceOptionDto>()
                .ForMember(s => s.Lable, d => d.MapFrom(s => s.Name.Replace("-", " ")))
                .ForMember(s => s.Value, opt => opt.MapFrom(s => s.Name));
        }

        public string Lable { get; set; }

        public string Value { get; set; }

    }

    public class CityOptionDto : IMapFrom<City>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<City, CityOptionDto>()
                .ForMember(s => s.Lable, d => d.MapFrom(s => s.Name.Replace("-", " ")))
                .ForMember(s => s.Value, opt => opt.MapFrom(s => s.Name))
                .ForMember(s => s.ParentId, opt => opt.MapFrom(s => s.StateName));
        }

        public string Lable { get; set; }

        public string Value { get; set; }

        public string ParentId { get; set; }

    }

}
