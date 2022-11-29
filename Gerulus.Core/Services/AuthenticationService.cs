using System.Security.Cryptography;
using System.Text;
using Gerulus.Core;
using Konscious.Security.Cryptography;
using Microsoft.Data.Sqlite;

namespace Gerulus.Core.Services;

public class AuthenticationService : IAuthenticationService
{

    public async Task CreateAccountAsync(string username, string password)
    {
        var salt = RandomNumberGenerator.GetBytes(32);
        var hash = await ComputeHashAsync(Encoding.UTF8.GetBytes(password), salt);
        var user = new User()
        {
            Username = username,
            Password = hash,
            Salt = salt
        };

        using var context = new GerulusContext();
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        using var conn = new SqliteConnection("Data Source=Gerulus.db");
        await conn.OpenAsync();

        using var command = new SqliteCommand($"SELECT PASSWORD, SALT FROM USERS WHERE USERNAME = '{username}';", conn);
        /*using var command = new SqliteCommand("SELECT PASSWORD, SALT FROM USERS WHERE USERNAME = @user;", conn);

        command.Parameters.AddWithValue("user", username);
        await command.PrepareAsync();*/

        using var passwordReader = await command.ExecuteReaderAsync();
        if (!passwordReader.HasRows)
            return false;

        await passwordReader.ReadAsync();

        byte[] salt = (byte[])passwordReader["Salt"];
        byte[] hash = await ComputeHashAsync(Encoding.UTF8.GetBytes(password), salt);

        return hash.SequenceEqual((byte[])passwordReader["Password"]);
    }

    private static Task<byte[]> ComputeHashAsync(byte[] password, byte[] salt)
    {
        using var argon = new Argon2id(password);

        argon.DegreeOfParallelism = 1;
        argon.Iterations = 2;
        argon.MemorySize = 15360;
        argon.Salt = salt;

        return argon.GetBytesAsync(32);
    }
}