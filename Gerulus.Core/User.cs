using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gerulus.Core.Crypto;

namespace Gerulus.Core;

public class User : AggregateRoot<UserId>
{
    public required string Username { get; set; }
    public required byte[] Password { get; set; }
    public required byte[] Salt { get; set; }

    public CryptographicKeyPair KeyPair { get; set; }

    public User(UserId? id = null) : base(id ?? new UserId())
    {
    }

    public async Task<CryptographicKeyPair> GenerateKeyPairAsync(ICryptoKeyProvider keyProvider)
    {
        var pair = await keyProvider.GenerateKeyPairAsync();
        KeyPair = pair;
        return pair;
    }
}
