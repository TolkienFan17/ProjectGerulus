using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Gerulus.Core.Crypto;

public class DHFGParameters
{
    public DHParameters Parameters { get; }

    public byte[] P { get; }
    public byte[] G { get; }

    public DHFGParameters(byte[] p, byte[] g)
    {
        P = p;
        G = g;

        Parameters = new DHParameters(new BigInteger(p), new BigInteger(g));
    }

    public DHFGParameters(DHParameters parameters)
    {
        Parameters = parameters;
        P = parameters.P.ToByteArray();
        G = parameters.G.ToByteArray();
    }
}
