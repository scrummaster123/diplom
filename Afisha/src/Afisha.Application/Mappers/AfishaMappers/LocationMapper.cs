using Afisha.Application.DTO.Outputs;
using Afisha.Domain.Entities;
using AutoMapper;

namespace Afisha.Application.Mappers.AfishaMappers;

public class LocationMapper : Profile
{
    public LocationMapper()
    {
        // Маппинг OutputLocationBase в Location
        CreateMap<Location, OutputLocationBase>()
            .ForMember(dst => dst.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Pricing,
                opt => opt.MapFrom(src => src.Pricing))
            .ForMember(dst => dst.IsWarmPlace,
                opt => opt.MapFrom(src => src.IsWarmPlace))
            ;
    }
}
