using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Gerulus.Core.Crypto;

public sealed partial class DHFG
{
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

        public override bool Equals(object? obj)
        {
            if (obj is Parameters parameters)
            {
                return P.SequenceEqual(parameters.P) &&
                        G.SequenceEqual(parameters.G);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(P, G);
        }

        public static bool operator ==(Parameters left, Parameters right)
            => left.Equals(right);

        public static bool operator !=(Parameters left, Parameters right)
            => !(left == right);
    }
}
