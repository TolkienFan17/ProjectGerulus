using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Gerulus.Core.Crypto.DHFG;

public class Parameters
{
    public byte[] P { get; }
    public byte[] G { get; }

    public Parameters(byte[] p, byte[] g)
    {
        P = p;
        G = g;
    }

    public Parameters(DHParameters parameters)
    {
        P = parameters.P.ToByteArray();
        G = parameters.G.ToByteArray();
    }

    public DHParameters ToBouncyCastle()
    {
        return new DHParameters(new BigInteger(P), new BigInteger(G));
    }
}
