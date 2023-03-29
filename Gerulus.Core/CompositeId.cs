using System.Runtime.CompilerServices;

namespace Gerulus.Core;

public static class CompositeId
{
    public static CompositeId<TFirst, TSecond> From<TFirst, TSecond>(TFirst first, TSecond second)
        where TFirst : IEquatable<TFirst>
        where TSecond : IEquatable<TSecond>
    {
        return new CompositeId<TFirst, TSecond>(first, second);
    }

    public static CompositeId<TFirst, TSecond, TThird> From<TFirst, TSecond, TThird>(TFirst first, TSecond second, TThird third)
        where TFirst : IEquatable<TFirst>
        where TSecond : IEquatable<TSecond>
        where TThird : IEquatable<TThird>
    {
        return new CompositeId<TFirst, TSecond, TThird>(first, second, third);
    }
}

public readonly record struct CompositeId<TFirst, TSecond>(TFirst First, TSecond second)
    : IEquatable<CompositeId<TFirst, TSecond>>
        where TFirst : IEquatable<TFirst>
        where TSecond : IEquatable<TSecond>;

public readonly record struct CompositeId<TFirst, TSecond, TThird>(TFirst First, TSecond second, TThird third)
    : IEquatable<CompositeId<TFirst, TSecond, TThird>>
        where TFirst : IEquatable<TFirst>
        where TSecond : IEquatable<TSecond>
        where TThird : IEquatable<TThird>;
