namespace Application.Features.Clients.Services;

public sealed class ClientService : IClientService
{
    public IEnumerable<string> GetAll() =>
    [
        "John Smith",
        "Jane Doe",
        "Michael Johnson"
    ];

    /*
    public static KeyValuePair<string, string>[] ConfigureServices()
    {
        return
        [
            new("Priority", "1"),
            new("Lifetime", "Singleton")
        ];
    }
    */
}
