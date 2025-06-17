using Afisha.Application.DTO.Elastics;
using Afisha.Application.DTO.Outputs;
using Afisha.Domain.Entities;

namespace Afisha.Application.Services.Interfaces;

public interface IElasticService
{
    Task<IEnumerable<ElasticLocation>> GetAsync(string search);

    Task WriteAsync(ElasticLocation location);
}
