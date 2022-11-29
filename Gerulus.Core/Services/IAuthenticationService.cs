namespace Gerulus.Core.Services;

public interface IAuthenticationService
{
    Task CreateAccountAsync(string username, string password);
    Task<bool> AuthenticateAsync(string username, string password);
}