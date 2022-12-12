namespace Gerulus.Core.Auth;

public interface IAuthenticationService
{
    Task<User> CreateAccountAsync(string username, string password);
    Task<AuthenticationResult> AuthenticateAsync(string username, string password);
}