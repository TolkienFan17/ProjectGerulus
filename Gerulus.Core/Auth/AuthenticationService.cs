using System.Security.Cryptography;
using System.Text;
using Gerulus.Core;
using Gerulus.Core.Database;
using Konscious.Security.Cryptography;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core.Auth;

public class AuthenticationService : IAuthenticationService
{

    public required GerulusContext Context { get; init; }

    public async Task<User> CreateAccountAsync(string username, string password)
    {
        var salt = RandomNumberGenerator.GetBytes(32);
        var hash = await ComputeHashAsync(Encoding.UTF8.GetBytes(password), salt);
        var user = new User()
        {
            Username = username,
            Password = hash,
            Salt = salt
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        return user;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password)
    {
        var user = await Context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));
        if (user is null)
        {
            return AuthenticationResult.FromFailure();
        }

        byte[] hash = await ComputeHashAsync(Encoding.UTF8.GetBytes(password), user.Salt);
        if (hash.SequenceEqual(user.Password))
        {
            return AuthenticationResult.FromSuccess(user);
        }
        else
        {
            return AuthenticationResult.FromFailure();
        }
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