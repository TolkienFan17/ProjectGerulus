using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.Data.Sqlite;
using Sharprompt;

namespace SecureMessenger.Terminal;

public class Program
{


    public static void Main()
    {
        var selection = Prompt.Select<LogInMenuSelection>("Welcome! Choose an option to proceed");

        switch (selection)
        {
            case LogInMenuSelection.CreateNew: Login(); break;
            case LogInMenuSelection.Login: Login(); break;
            default: Console.WriteLine("Invalid selection"); break;
        };
    }

    public static void Login()
    {
        var username = Prompt.Input<string>("Username");
        var password = Prompt.Password("Password");


    }
}

public enum LogInMenuSelection
{
    [Display(Name = "Create a new account")]
    CreateNew,

    [Display(Name = "Log in")]
    Login
}
