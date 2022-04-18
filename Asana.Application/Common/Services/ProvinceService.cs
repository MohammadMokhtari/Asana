using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Domain.Entities.Addresses;
using Asana.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Asana.Application.DTOs;

namespace Asana.Application.Common.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IGenericRepository<State> _genericRepository;
        private readonly IGenericRepository<City> _cityGenericRepository;
        private readonly IMapper _mapper;


        public ProvinceService(IGenericRepository<State> genericRepository , IGenericRepository<City> cityGenericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _cityGenericRepository = cityGenericRepository;
        }


        public async Task<Result> AllProvinceOptionAsync()
        {
            var provinceOptions = await _genericRepository.GetEntitiesQuery()
                .ProjectTo<ProvinceOptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var cityOptions = await _cityGenericRepository.GetEntitiesQuery()
                .ProjectTo<CityOptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var response = new ProvinceOptionDtoResponse();
            response.ProvinceOptions = provinceOptions;
            response.CityOptions = cityOptions;

            return Result.Success(response);
        }
    }
}
