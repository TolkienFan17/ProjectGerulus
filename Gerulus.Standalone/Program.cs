using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata;
using Gerulus.Core;
using Microsoft.Data.Sqlite;
using SecureMessenger.Services;
using Sharprompt;

namespace SecureMessenger.Terminal;

public class Program
{
    private static IAuthenticationService Authentication = new AuthenticationService();

    public static async Task Main()
    {
        var selection = Prompt.Select<LogInMenuSelection>("Welcome! Choose an option to proceed");

        switch (selection)
        {
            case LogInMenuSelection.CreateNew:
                var username = Prompt.Input<string>("Choose your username");
                var password = Prompt.Password("Enter your password", "");

                await Authentication.CreateAccountAsync(username, password);
                Console.WriteLine("Account created!");
                break;
            case LogInMenuSelection.Login: await LoginAsync(); break;
            default: Console.WriteLine("Invalid selection"); break;
        };
    }

    public static async Task LoginAsync()
    {
        var username = Prompt.Input<string>("Username");
        var password = Prompt.Password("Password", passwordChar: "");

        if (await Authentication.AuthenticateAsync(username, password))
            Console.WriteLine("Successfully authenticated!");
        else
            Console.WriteLine("Access denied");
    }
}

public enum LogInMenuSelection
{
    [Display(Name = "Create a new account")]
    CreateNew,

    [Display(Name = "Log in")]
    Login
}
