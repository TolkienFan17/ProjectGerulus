namespace Gerulus.Core.Crypto;

public interface ICryptoKeyProvider
{
    Task<CryptographicKeyPair> GenerateKeyPairAsync();
    Task<byte[]> ComputeSharedKeyAsync(CryptographicKeyPair initiator, CryptographicKeyPair otherKey);
}

public interface ICryptoParameterProvider<TParameters>
{
    Task<TParameters> GenerateParametersAsync();
    Task InitializeParametersAsync(TParameters parameters);
}

public interface ICryptoKeyService<TParameters> : ICryptoParameterProvider<TParameters>, ICryptoKeyProvider
{
}