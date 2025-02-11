using Afisha.Application.DTO;
using Afisha.Domain.Entities;
using AutoMapper;

namespace Afisha.Application.Mappers;

public class AfishaMappingProfile : Profile
{
    public AfishaMappingProfile()
    {
        CreateMap<Event, CreateEvent>().ReverseMap();
        CreateMap<Event, OutputEvent>().ReverseMap();
        CreateMap<Location, OutputEventLocation>().ReverseMap();
        CreateMap<User, OutputEventUser>().ReverseMap();
    }
}
