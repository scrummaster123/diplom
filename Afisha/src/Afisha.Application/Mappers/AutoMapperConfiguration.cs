using Afisha.Application.Mappers.AfishaMappers;
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
