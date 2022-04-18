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
    public class AddressService : IAddressService
    {
        private readonly IGenericRepository<Address> _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AddressService(IGenericRepository<Address> repository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result> GetAddressesAsync()
        {
            var addresses = await _repository.GetEntitiesQuery()
                .Where(a => a.UserId == _currentUserService.GuidUserId)
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider).ToListAsync();

            if (addresses is null)
                return Result.Failure("COULD_NOT_GET_USER_ADDRESSES");

            return Result.Success(addresses);
        }

        public async Task<Result> SetDefaultAddressAsync(long addressId)
        {
            var addresses = await this._repository.GetEntitiesQuery()
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
                newAddress.StateName = addressDto.StateName;
                newAddress.UnitNumber = addressDto.UnitNumber;
                newAddress.PostalCode = addressDto.PostalCode;
                newAddress.NumberPlate = addressDto.NumberPlate;
                newAddress.RecipientFirstName = addressDto.RecipientFirstName;
                newAddress.RecipientLastName = addressDto.RecipientLastName;
                newAddress.RecipientNationalCode = addressDto.RecipientNationalCode;
                newAddress.RecipientPhoneNumber = addressDto.RecipientPhoneNumber;
                newAddress.UserId = _currentUserService.GuidUserId;

                await _repository.AddEntityAsync(newAddress);
                await _repository.SaveChangeAsync();
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
                await _repository.RemoveEntityAsync(addressId);
                await _repository.SaveChangeAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateAddressAsync(AddressUpdateDto addressDto)
        {
            var address = await _repository.GetEntitiesQuery()
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
                address.StateName = address.StateName;
                address.UnitNumber = addressDto.UnitNumber;
                address.PostalCode = addressDto.PostalCode;
                address.NumberPlate = addressDto.NumberPlate;
                address.RecipientFirstName = address.RecipientFirstName;
                address.RecipientLastName = addressDto.RecipientLastName;
                address.RecipientNationalCode = addressDto.RecipientNationalCode;
                address.RecipientPhoneNumber = addressDto.RecipientPhoneNumber;

                _repository.UpdateEntity(address);
                await _repository.SaveChangeAsync();

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

                this._repository.UpdateRangeEntity(addresses);

                var address = addresses.FirstOrDefault(a => a.Id == addressId);

                address.IsDefault = true;

                _repository.UpdateEntity(address);

                await _repository.SaveChangeAsync();

                return _mapper.Map<AddressDto>(address);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
