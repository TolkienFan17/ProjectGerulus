namespace Gerulus.Standalone.UserForms;

public class InboxUserForm : IUserForm
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("Render inbox");
        return Task.CompletedTask;
    }
}
