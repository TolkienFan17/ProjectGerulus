using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gerulus.Core.Crypto;

namespace Gerulus.Core;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public required string Username { get; set; }
    public required byte[] Password { get; set; }
    public required byte[] Salt { get; set; }

    public byte[]? PublicKey { get; set; }
    public byte[]? PrivateKey { get; set; }

    public CryptographicKeyPair CreateKeyPair()
    {
        if (PublicKey is null)
            throw new ArgumentNullException(nameof(PublicKey));

        if (PrivateKey is null)
            throw new ArgumentNullException(nameof(PrivateKey));

        return new CryptographicKeyPair(PublicKey, PrivateKey);
    }
}