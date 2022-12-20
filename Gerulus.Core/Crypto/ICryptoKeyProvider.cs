namespace Gerulus.Core.Crypto;

public interface ICryptoKeyProvider
{
    Task<CryptographicKeyPair> GenerateKeyPairAsync();
    Task<byte[]> ComputeSharedKeyAsync(CryptographicKeyPair initiator, CryptographicKeyPair otherKey);
}