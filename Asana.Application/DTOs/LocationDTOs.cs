using Asana.Application.Common.Mappings;
using Asana.Domain.Entities.Addresses;

namespace Asana.Application.DTOs
{
    public class LocationDto :IMapFrom<Address>
    {
        public long Id { get; set; }

        public string AddressLine { get; set; }

        public string PostalCode { get; set; }

        public string NumberPlate { get; set; }

        public byte UnitNumber { get; set; }

        public bool IsDefault { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string FullAddress
        { get 
        {
                return $"{StateName} - {CityName} - {AddressLine} - پلاک {NumberPlate}";     
        } 
        }
    }
}
