using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gerulus.Core.Crypto;

namespace Gerulus.Core;

public class User
{
    [Key]
    public UserId Id { get; init; }

    public required string Username { get; set; }
    public required byte[] Password { get; set; }
    public required byte[] Salt { get; set; }

    public CryptographicKeyPair KeyPair { get; set; }

    public async Task<CryptographicKeyPair> GenerateKeyPairAsync(ICryptoKeyProvider keyProvider)
    {
        var pair = await keyProvider.GenerateKeyPairAsync();
        KeyPair = pair;
        return pair;
    }
}
