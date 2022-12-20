using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace Gerulus.Core.Crypto;

public class DHFGFileParameterProvider : ICryptoParameterProvider<DHFGParameters>
{
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