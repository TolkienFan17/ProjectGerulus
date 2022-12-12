namespace Gerulus.Standalone.UserForms;

public class InboxUserForm : IUserForm
{
    public required LocalUserState UserState { get; init; }

    public InboxUserForm(LocalUserState userState)
    {
        UserState = userState;
    }

    public Task ExecuteAsync()
    {
        Console.WriteLine($"Welcome, {UserState.CurrentUser!.Username}! You have no messages in your inbox.");
        return Task.CompletedTask;
    }
}
