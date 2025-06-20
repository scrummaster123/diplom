using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Domain.Entities;
using AutoMapper;

namespace Afisha.Application.Mappers;

public class AfishaMappingProfile : Profile
{
    public AfishaMappingProfile()
    {
        CreateMap<Event, CreateEvent>().ReverseMap();
        CreateMap<Event, OutputEvent>()
            .ForMember(dst => dst.Participants, opt =>
            opt.MapFrom(src => src.EventParticipants.Select(x => x.User).ToList())).ReverseMap();
        CreateMap<Location, OutputEventLocation>().ReverseMap();
        CreateMap<User, OutputEventUser>().ReverseMap();
    }
}
