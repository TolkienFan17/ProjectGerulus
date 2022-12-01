using System.ComponentModel.DataAnnotations;
using Sharprompt;

namespace Gerulus.Standalone.UserForms;

public class MainMenuForm : IUserForm
{
    public required LoginUserForm Login { get; init; }
    public required CreateAccountUserForm CreateAccount { get; init; }

    public async Task ExecuteAsync()
    {
        var option = Prompt.Select<MainMenuSelection>("Choose an option to proceed");

        switch (option)
        {
            case MainMenuSelection.CreateNew:
                await CreateAccount.ExecuteAsync();
                return;
            case MainMenuSelection.Login:
                await Login.ExecuteAsync();
                return;
            default:
                Console.WriteLine("Invalid option. ");
                await ExecuteAsync();
                return;
        }
    }
}


public enum MainMenuSelection
{
    [Display(Name = "Create a new account")]
    CreateNew,

    [Display(Name = "Log in")]
    Login
}
