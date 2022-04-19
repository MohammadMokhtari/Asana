using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Asana.Domain.Entities.Addresses;
using Asana.Domain.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asana.Application.Common.Services
{
    public class AddressService : IAddressService
    {
        private readonly IGenericRepository<Address> _addressRepository;
        private readonly IGenericRepository<Province> _provinceRepository;
        private readonly IGenericRepository<City> _cityRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;
        
        public AddressService(IGenericRepository<Address> addressRepository,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IGenericRepository<Province> provinceRepository,
            IGenericRepository<City> cityGenericRepository,
            ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
            _provinceRepository = provinceRepository;
            _cityRepository = cityGenericRepository;
        }

        public async Task<Result> GetAddressesAsync()
        {
            var addresses = await _addressRepository.GetEntitiesQuery()
                .Where(a => a.UserId == _currentUserService.GuidUserId)
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider).ToListAsync();

            if (addresses is null)
                return Result.Failure("COULD_NOT_GET_USER_ADDRESSES");

            return Result.Success(addresses);
        }

        public async Task<Result> SetDefaultAddressAsync(long addressId)
        {
            var addresses = await this._addressRepository.GetEntitiesQuery()
                .Where(a => a.UserId == _currentUserService.GuidUserId).ToListAsync();

            if (addresses is not null)
            {
                try
                {
                    var result = await ChangeDefaultAddress(addresses, addressId);
                    return Result.Success(result);
                }
                catch (Exception)
                {
                    return Result.Failure("COULD_NOT_SET_DEFAULT_ADDRESS" );
                }
            }

            return Result.Failure("COULD_NOT_SET_DEFAULT_ADDRESS");
        }

        public async Task<Result> CreateAddressAsync(AddressCreateDto addressDto)
        {
            try
            {
                var newAddress = new Address();
                newAddress.AddressLine = addressDto.AddressLine;
                newAddress.CityName = addressDto.CityName;
                newAddress.ProvinceName = addressDto.StateName;
                newAddress.UnitNumber = addressDto.UnitNumber;
                newAddress.PostalCode = addressDto.PostalCode;
                newAddress.NumberPlate = addressDto.NumberPlate;
                newAddress.RecipientFirstName = addressDto.RecipientFirstName;
                newAddress.RecipientLastName = addressDto.RecipientLastName;
                newAddress.RecipientNationalCode = addressDto.RecipientNationalCode;
                newAddress.RecipientPhoneNumber = addressDto.RecipientPhoneNumber;
                newAddress.UserId = _currentUserService.GuidUserId;

                await _addressRepository.AddEntityAsync(newAddress);
                await _addressRepository.SaveChangeAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {

                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteAddressAsync(long addressId)
        {
            try
            {
                await _addressRepository.RemoveEntityAsync(addressId);
                await _addressRepository.SaveChangeAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateAddressAsync(AddressUpdateDto addressDto)
        {
            var address = await _addressRepository.GetEntitiesQuery()
                .Where(a=>a.UserId == _currentUserService.GuidUserId && a.Id == addressDto.Id)
                .FirstOrDefaultAsync();

            if (address is null)
            {
                return Result.Failure("NOT_FOUND_ADDRESS");
            }

            try
            {
                address.AddressLine = addressDto.AddressLine;
                address.CityName = addressDto.CityName;
                address.ProvinceName = address.ProvinceName;
                address.UnitNumber = addressDto.UnitNumber;
                address.PostalCode = addressDto.PostalCode;
                address.NumberPlate = addressDto.NumberPlate;
                address.RecipientFirstName = address.RecipientFirstName;
                address.RecipientLastName = addressDto.RecipientLastName;
                address.RecipientNationalCode = addressDto.RecipientNationalCode;
                address.RecipientPhoneNumber = addressDto.RecipientPhoneNumber;

                _addressRepository.UpdateEntity(address);
                await _addressRepository.SaveChangeAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {

                return Result.Failure(ex.Message);
            }

        }

        private async Task<AddressDto> ChangeDefaultAddress(IEnumerable<Address> addresses,long addressId)
        {
            try
            {
                foreach (var ad in addresses)
                {
                    ad.IsDefault = false;
                }

                this._addressRepository.UpdateRangeEntity(addresses);

                var address = addresses.FirstOrDefault(a => a.Id == addressId);

                address.IsDefault = true;

                _addressRepository.UpdateEntity(address);

                await _addressRepository.SaveChangeAsync();

                return _mapper.Map<AddressDto>(address);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Result> GetAllProvinceCityOptionsAsync()
        {
            try
            {
                var provinceOptions = await _provinceRepository.GetEntitiesQuery()
                  .ProjectTo<ProvinceOptionDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                var cityOptions = await _cityRepository.GetEntitiesQuery()
                    .ProjectTo<CityOptionDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                var response = new ProvinceOptionDtoResponse();
                response.ProvinceOptions = provinceOptions;
                response.CityOptions = cityOptions;

                _logger.LogInformation("Get AllProvinceOption to be successful");

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get AllProvinceOption Faildee!");

                return Result.Failure("CAN_NOT_GET_ALL_PROVINCE");
            }
        }
    }
}
