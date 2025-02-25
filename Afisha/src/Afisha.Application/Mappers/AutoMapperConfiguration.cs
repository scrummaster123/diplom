using AutoMapper;

namespace Afisha.Application.Mappers;

public class AutoMapperConfiguration
{
    public MapperConfiguration Configure()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AfishaMappingProfile>();
        });
        return config;
    }
}
