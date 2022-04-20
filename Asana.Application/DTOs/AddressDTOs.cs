using System.Collections.Generic;
using Asana.Application.Common.Mappings;
using Asana.Domain.Entities.Addresses;

namespace Asana.Application.DTOs
{
    public class AddressDto :IMapFrom<Address>
    {
        public long Id { get; set; }

        public string AddressLine { get; set; }

        public string PostalCode { get; set; }

        public string NumberPlate { get; set; }

        public byte UnitNumber { get; set; }

        public bool IsDefault { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string RecipientFirstName { get; set; }

        public string RecipientLastName { get; set; }

        public string RecipientNationalCode { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public string FullAddress { get => $"{StateName} - {CityName} - {AddressLine} - پلاک {NumberPlate}";}
    }

    public class AddressCreateDto
    {
        public string AddressLine { get; set; }

        public string PostalCode { get; set; }

        public string NumberPlate { get; set; }

        public byte UnitNumber { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string RecipientFirstName { get; set; }

        public string RecipientLastName { get; set; }

        public string RecipientNationalCode { get; set; }

        public string RecipientPhoneNumber { get; set; }

    }

    public class AddressUpdateDto
    {
        public long Id { get; set; }

        public string AddressLine { get; set; }

        public string PostalCode { get; set; }

        public string NumberPlate { get; set; }

        public byte UnitNumber { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string RecipientFirstName { get; set; }

        public string RecipientLastName { get; set; }

        public string RecipientNationalCode { get; set; }

        public string RecipientPhoneNumber { get; set; }

    }
    
    public class CreateInitAddressDto 
    {
        public IEnumerable<ProvinceDto> Provincs { get; set; }

        public IEnumerable<CityDto> Cities { get; set; }
    }

    public class ProvinceDto : IMapFrom<Province>
    {
        public string Name { get; set; }
    }

    public class CityDto : IMapFrom<City>
    {
        public string Name { get; set; }
        
        public string ProvinceName { get; set; }
    }
}
