using Afisha.Application.DTO.Elastics;
using Afisha.Application.Services.Interfaces;
using Nest;

namespace Afisha.Application.Services.Managers;

public class ElasticService : IElasticService
{
    private ElasticClient _client { get; set; }
    private const string INDEX_NAME = "locations";

    public ElasticService()
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex(INDEX_NAME);
        _client = new ElasticClient(settings);
    }

    public async Task<IEnumerable<ElasticLocation>> GetAsync(string search)
    {
        var response = await _client.SearchAsync<ElasticLocation>(s => s
            .Query(q => q
                .Bool(b => b
                    .Should(
                        m => m.Wildcard(w => w
                            .Field(f => f.Date.Suffix("keyword"))
                            .Value($"*{search.ToLower()}*")
                        )
                    )
                    .MinimumShouldMatch(1)
                )
            )
        );

        return response.Documents;
    }

    public async Task WriteAsync(ElasticLocation location) =>
        await _client.IndexDocumentAsync(location);
}
