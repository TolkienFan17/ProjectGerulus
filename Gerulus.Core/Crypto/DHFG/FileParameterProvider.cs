using Gerulus.Core.Config;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;

namespace Gerulus.Core.Crypto;

public sealed partial class DHFG
{
    public class FileParameterProvider : ICryptoParameterProvider<Parameters>
    {
        public bool IsInitialized { get; private set; }
        public Parameters? Parameters { get; private set; }

        public required IConfigProvider Config { get; init; }

        public Task<Parameters> GenerateParametersAsync()
        {
            var generator = new DHParametersGenerator();
            generator.Init(Config.Get().ParameterSize, 100, new SecureRandom());

            Parameters = new Parameters(generator.GenerateParameters());
            IsInitialized = true;
            return Task.FromResult(Parameters);
        }

        public async Task SaveParametersAsync()
        {
            if (Parameters is null) throw new InvalidOperationException("Cannot save parameters until they have been generated");

            using var fileWriter = new StreamWriter(Config.Get().ParametersFileLocation);
            var pemWriter = new PemWriter(fileWriter);

            pemWriter.WriteObject(new PemObject("P Value", Parameters.P));
            pemWriter.WriteObject(new PemObject("G Value", Parameters.G));
            await fileWriter.FlushAsync();
        }

        public Task<bool> LoadParametersAsync()
        {
            using var fileReader = new StreamReader(Config.Get().ParametersFileLocation);
            var pemReader = new PemReader(fileReader);

            var pValue = pemReader.ReadPemObject();
            var gValue = pemReader.ReadPemObject();

            Parameters = new Parameters(pValue.Content, gValue.Content);
            IsInitialized = true;
            return Task.FromResult(true);
        }
    }
}