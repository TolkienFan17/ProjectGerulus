using Gerulus.Core.Auth;
using Sharprompt;

namespace Gerulus.Standalone.UserForms;

public class LoginUserForm : IUserForm
{
    public const int MaxLoginAttempts = 10;
    private int LoginAttempts { get; set; } = 0;

    public required IAuthenticationService Authentication { get; init; }
    public required InboxUserForm Inbox { get; init; }
    public required LocalUserState UserState { get; init; }

    public LoginUserForm(IAuthenticationService auth, InboxUserForm inbox, LocalUserState state)
    {
        Authentication = auth;
        Inbox = inbox;
        UserState = state;
    }

    public async Task ExecuteAsync()
    {
        var username = Prompt.Input<string>("Username");
        var password = Prompt.Password("Password", passwordChar: "");

        if (await Authentication.AuthenticateAsync(username, password))
        {
            await Inbox.ExecuteAsync();
        }
        else if (LoginAttempts >= MaxLoginAttempts)
        {
            Console.WriteLine("Invalid login information. Maximum login attempts reached. Please try again.");
        }
        else
        {
            Console.Write($"Invalid login information. {++LoginAttempts} login attempts so far. ");
            await ExecuteAsync();
        }
    }
}
