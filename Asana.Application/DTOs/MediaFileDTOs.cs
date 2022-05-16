using Asana.Application.Common.Mappings;
using Asana.Domain.Entities.Media;
using AutoMapper;

namespace Asana.Application.DTOs
{
    public class MediaFileDto : IMapFrom<MediaFile>
    {

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MediaFile, MediaFileDto>()
                .ForMember(m => m.Url, opt => opt.MapFrom<MediaFileUrlResolver>());
        }

        public string Alt { get; set; }

        public string Url { get; set; }

        public MediaType Type { get; set; }
    }
}
