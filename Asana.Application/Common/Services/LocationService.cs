using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Asana.Domain.Entities.Addresses;
using Asana.Domain.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asana.Application.Common.Services
{
    public class LocationService : ILocationService
    {
        private readonly IGenericRepository<Address> _repository;
        private readonly IMapper _mapper;

        public LocationService(IGenericRepository<Address> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> GetLocationsAsync(Guid userId)
        {
            var Locations = await _repository.GetEntitiesQuery()
                .Where(a => a.UserId == userId)
                .ProjectTo<LocationDto>(_mapper.ConfigurationProvider).ToListAsync();

            if (Locations is null)
            {
                return Result.Failure(new string[] { "CAN NOT GET USER LOCATION !" });
            }
            return Result.Success(Locations);
        }

        public async Task<Result> SetDefaultLocationAsync(long locationId, Guid userId)
        {
            var Locations = await this._repository.GetEntitiesQuery()
                .Where(a => a.UserId == userId).ToListAsync();

            if(Locations is not null)
            {
                try
                {
                   var result =  await ChangeDefaultLocation(Locations, locationId);
                    return Result.Success(result);
                }
                catch (Exception)
                {
                    return Result.Failure(new string[] {"CAN NOT SET DEFAULT LOCATION !"});
                }               
            }

            return Result.Failure(new string[] { "CAN NOT SET DEFAULT LOCATION !" });

        }

        private async Task<IEnumerable<LocationDto>> ChangeDefaultLocation( IEnumerable<Address> locations,long locationId)
        {
            try
            {
                foreach (var loc in locations)
                {
                    loc.IsDefault = false;
                }

                this._repository.UpdateRangeEntity(locations);

                var address = locations.FirstOrDefault(a => a.Id == locationId);

                address.IsDefault = true;
                _repository.UpdateEntity(address);
                await _repository.SaveChangeAsync();
                return _mapper.ProjectTo<LocationDto>(locations.AsQueryable());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
