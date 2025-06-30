namespace Afisha.Application.DTO.Elastics;

public class ElasticLocation
{
    public required long Id { get; set; }
    // Date - позволяет индексу сразу появиться в Discover
    public required string Date { get; set; }
}
