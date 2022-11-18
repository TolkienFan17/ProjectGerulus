using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.Data.Sqlite;
using Sharprompt;

namespace SecureMessenger.Terminal;

public class Program
{
    public const string ConnectionString = "Data Source=EncryptedMessenger.db";

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

        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            using (var command = new SqliteCommand($"SELECT PASSWORD FROM USERS WHERE USERNAME = '{username}';", conn))
            {
                Console.WriteLine(command.CommandText);
                var correctPassword = command.ExecuteScalar();
                if (correctPassword.Equals(password))
                    Console.WriteLine("Authenticated");
                else
                    Console.WriteLine("Bad login");
            }

            conn.Close();
        }
    }
}

public enum LogInMenuSelection
{
    [Display(Name = "Create a new account")]
    CreateNew,

    [Display(Name = "Log in")]
    Login
}
