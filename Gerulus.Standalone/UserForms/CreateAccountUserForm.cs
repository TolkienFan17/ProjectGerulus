using Gerulus.Core.Auth;
using Sharprompt;

namespace Gerulus.Standalone.UserForms;

public class CreateAccountUserForm : IUserForm
{
    public required IAuthenticationService Authentication { get; init; }
    public required InboxUserForm Inbox { get; init; }
    public required LocalUserState UserState { get; init; }

    public CreateAccountUserForm(IAuthenticationService auth, InboxUserForm inbox, LocalUserState state)
    {
        Authentication = auth;
        Inbox = inbox;
        UserState = state;
    }

    public async Task ExecuteAsync()
    {
        var username = Prompt.Input<string>("Choose your username");
        var password = Prompt.Password("Enter your password", "");

        await Authentication.CreateAccountAsync(username, password);
        await Inbox.ExecuteAsync();
    }
}
