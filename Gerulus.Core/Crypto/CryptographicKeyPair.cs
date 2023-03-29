using Org.BouncyCastle.Math;

namespace Gerulus.Core.Crypto;

public readonly struct CryptographicKeyPair
{
    public byte[] PublicKey { get; }
    public byte[] PrivateKey { get; }

    public CryptographicKeyPair(byte[] publicKey, byte[] privateKey)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }
}