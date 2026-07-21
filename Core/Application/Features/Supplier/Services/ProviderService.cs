namespace Application.Features.Supplier.Services;

public sealed class ProviderService : IProviderService
{
    public IEnumerable<string> GetAll() =>
    [
            "Pedro Perez",
            "Michael"
    ];


    
    public static KeyValuePair<string, string>[] ConfigureServices() =>
    [
        new("Priority", "2"),
        new("Lifetime", "Singleton")
    ];

}
