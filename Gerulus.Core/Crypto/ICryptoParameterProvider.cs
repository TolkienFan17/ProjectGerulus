namespace Gerulus.Core.Crypto;

public interface ICryptoParameterProvider<TParameters>
{
    bool IsInitialized { get; }
    TParameters? Parameters { get; }

    Task<TParameters> GenerateParametersAsync();
    Task SaveParametersAsync();
    Task<bool> LoadParametersAsync();
}
