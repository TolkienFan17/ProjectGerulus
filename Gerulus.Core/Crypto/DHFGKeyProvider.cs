using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Gerulus.Core.Crypto;

public class DHFGKeyProvider : ICryptoKeyProvider
{
    public required ICryptoParameterProvider<DHFGParameters> ParameterProvider { get; init; }

    public Task<CryptographicKeyPair> GenerateKeyPairAsync()
    {
        if (Parameters is null) throw new InvalidOperationException();

        var generator = GeneratorUtilities.GetKeyPairGenerator("DH");
        generator.Init(new DHKeyGenerationParameters(new SecureRandom(), Parameters.Value.Parameters));
        var pair = generator.GenerateKeyPair();
        var publicPart = ((DHPublicKeyParameters)pair.Public).Y.ToByteArray();
        var privatePart = ((DHPrivateKeyParameters)pair.Private).X.ToByteArray();
        return Task.FromResult(new CryptographicKeyPair(publicPart, privatePart));
    }

    public Task<byte[]> ComputeSharedKeyAsync(CryptographicKeyPair initiator, CryptographicKeyPair otherKey)
    {
        if (Parameters is null) throw new InvalidOperationException();

        var agreement = AgreementUtilities.GetBasicAgreement("DH");
        agreement.Init(new DHPrivateKeyParameters(new BigInteger(otherKey.PrivateKey), Parameters.Value.Parameters));
        var publicKey = new DHPublicKeyParameters(new BigInteger(initiator.PublicKey), Parameters.Value.Parameters);
        var secret = agreement.CalculateAgreement(publicKey);
        return Task.FromResult(secret.ToByteArray());
    }

}
