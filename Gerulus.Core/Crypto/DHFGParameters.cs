using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Gerulus.Core.Crypto;

public class DHFGParameters
{
    public byte[] P { get; }
    public byte[] G { get; }

    public DHFGParameters(byte[] p, byte[] g)
    {
        P = p;
        G = g;
    }

    public DHFGParameters(DHParameters parameters)
    {
        P = parameters.P.ToByteArray();
        G = parameters.G.ToByteArray();
    }

    public DHParameters ToBouncyCastle()
    {
        return new DHParameters(new BigInteger(P), new BigInteger(G));
    }
}
