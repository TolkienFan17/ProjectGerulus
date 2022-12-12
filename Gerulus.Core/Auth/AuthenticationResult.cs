namespace Gerulus.Core.Auth;

public struct AuthenticationResult
{
    public required bool IsSuccessful { get; init; }
    public required User? User { get; init; }

    public static AuthenticationResult FromFailure() => new()
    {
        IsSuccessful = false,
        User = null
    };

    public static AuthenticationResult FromSuccess(User user) => new()
    {
        IsSuccessful = true,
        User = user
    };
}