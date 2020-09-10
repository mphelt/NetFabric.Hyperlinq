using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    static class BooleanExtensions
    {
        // Based on https://twitter.com/rameel_b/status/1302632990737080321
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(this bool value)
            => Unsafe.As<bool, byte>(ref value);

        // condition ? consequent : alternative
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Conditional(this bool condition, int consequent, int alternative)
        {
            var mask = -condition.AsByte() >> 31; // condition ? 0b1..1 : 0b0..0
            return (mask & consequent) + (~mask & alternative); // (condition ? consequent : 0) + (condition ? 0 : alternative)
        }
    }
}
