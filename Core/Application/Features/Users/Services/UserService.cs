namespace Application.Features.Users.Services;

public sealed class UserService : IUserService
{
    public IEnumerable<string> GetAll() =>
    [
        "Administrator",
        "Operator",
        "Guest"
    ];
}
