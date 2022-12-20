using Gerulus.Core.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace Gerulus.Core.Crypto.DHFG;

public class FileParameterProvider : ICryptoParameterProvider<DHFGParameters>
{
    public bool IsInitialized { get; private set; }
    public DHFGParameters? Parameters { get; private set; }

    public Task<DHFGParameters> GenerateParametersAsync()
    {
        var generator = new DHParametersGenerator();
        generator.Init(4096, 100, new SecureRandom());
        Parameters = new DHFGParameters(generator.GenerateParameters());
        return Task.FromResult((DHFGParameters)Parameters);
    }

    public Task InitializeParametersAsync(DHFGParameters parameters)
    {
        Parameters = parameters;
        return Task.CompletedTask;
    }
}