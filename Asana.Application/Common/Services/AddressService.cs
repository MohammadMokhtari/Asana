﻿using Asana.Application.Common.Interfaces;
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

        public async Task<(Result result,IEnumerable<AddressDto> addressDtos)> GetAddressesAsync()
        {
            _logger.LogInformation("GetAddressAsync Executed");
            try
            {
                var addressesDtos = await _addressRepository.GetEntitiesQuery()
                    .Where(a => a.UserId == _currentUserService.GuidUserId)
                    .ProjectTo<AddressDto>(_mapper.ConfigurationProvider).ToListAsync();
                _logger.LogInformation("GetAddresses to be Successful");
                return (Result.Success(),addressesDtos);
            }
            catch (Exception ex)
            {
              _logger.LogError(ex,"GetAddressAsycn Failed!");
              return (Result.Failure("CAN_NOT_GET_ADDRESSES"), null);
            }
        }

        public async Task<(Result result, AddressDto addressDto)> SetDefaultAddressAsync(long addressId)
        {
            _logger.LogInformation("SetDefaultAddresAsync Executed!");

            try
            {
                var addresses = await this._addressRepository.GetEntitiesQuery()
                    .Where(a => a.UserId == _currentUserService.GuidUserId).ToListAsync();

                if (addresses is not null)
                {
                    try
                    {
                        var addressDto = await ChangeDefaultAddress(addresses, addressId);
                        return (Result.Success(), addressDto);
                    }
                    catch (Exception)
                    {
                        return (Result.Failure("COULD_NOT_SET_DEFAULT_ADDRESS"), null);
                    }
                }

                return (Result.Failure("COULD_NOT_SET_DEFAULT_ADDRESS"), null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SetDefaultAddress Failed!");
                return (Result.Failure("COULD_NOT_SET_DEFAULT_ADDRESS"), null);
            }
        }
        
        
        public async Task<Result> CreateAddressAsync(AddressCreateDto addressDto)
        {
            _logger.LogInformation("CreateAddressAsync Executed");
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
                _logger.LogInformation("new Address Successfully Created");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Create new Address Failed!");
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteAddressAsync(long addressId)
        {
            _logger.LogInformation("DeleteAddressAsync Executed");
            
            try
            {
                await _addressRepository.RemoveEntityAsync(addressId);
                await _addressRepository.SaveChangeAsync();
                
                _logger.LogInformation("Deleted Address to be Successful");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Deleted Address Failed!");
                return Result.Failure("CAN_NOT_DELETE_ADDRESS");
            }
        }

        public async Task<(Result result,IEnumerable<ProvinceDto> provinceDtos)> AllProvinceAsync()
        {
            try
            {
                var provinceDtos = await _provinceRepository.GetEntitiesQuery()
                    .ProjectTo<ProvinceDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                _logger.LogInformation("Get AllProvince to be successful");

                return (Result.Success() , provinceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get AllProvinces Failed!");

                return (Result.Failure("CAN_NOT_GET_ALL_PROVINCE"),null);
            }
        }

        public async Task<(Result result,IEnumerable<CityDto> cityDtos)> AllCityAsync()
        {
            try
            {
                var cityDtos = await _cityRepository.GetEntitiesQuery()
                    .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                
                _logger.LogInformation("Get AllCities to be successful");
                
                return (Result.Success(),cityDtos);

            }
            catch (Exception ex)
            {              
                _logger.LogError(ex, "Get AllCities Failed!");
                return( Result.Failure("CAN_NOT_GET_ALL_CITIES"),null);
            }
        }

        public async Task<(Result result, CreateInitAddressDto createInitAddressDto)> InitCreatedAddress()
        {
            _logger.LogInformation("InitCreatedAddress Executed");
            try
            {
                var (cityResult, cityDtos) = await AllCityAsync();
                var (provinceResult,provinceDtos) = await AllProvinceAsync();

                if (cityResult.Succeeded && provinceResult.Succeeded)
                {
                    var addressInitialCreateDto = new CreateInitAddressDto()
                    {
                        Cities = cityDtos,
                        Provincs = provinceDtos
                    };
                    _logger.LogInformation("InitCreatedAddress to be successful");
                    return (Result.Success(), addressInitialCreateDto);
                }

                return (Result.Failure(), null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"InitCreatedAddress Failed!");
                return (Result.Failure(), null);
            }
        }

        public async Task<Result> UpdateAddressAsync(AddressUpdateDto addressDto)
        {
            _logger.LogInformation("UpdateAddressAsync Executed");
            
            try
            {
                var address = await _addressRepository.GetEntitiesQuery()
                    .Where(a=>a.UserId == _currentUserService.GuidUserId && a.Id == addressDto.Id)
                    .FirstOrDefaultAsync();

                if (address is null)
                {
                    _logger.LogWarning("not found address!");
                    return Result.Failure("NOT_FOUND_ADDRESS");
                }

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
                
                _logger.LogInformation("Update Address to be successful");
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError("Update address Failed!");
                return Result.Failure("CAN_NOT_UPDATE_ADDRESS");
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
        
    }
}
