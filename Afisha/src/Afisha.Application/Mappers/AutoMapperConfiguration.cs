using Afisha.Application.Mappers.UserMappper;
using AutoMapper;

namespace Afisha.Application.Mappers;

public class AutoMapperConfiguration
{
    public MapperConfiguration Configure()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AfishaMappingProfile>();
            cfg.AddProfile<UserMapper>();
        });
        return config;
    }
}
