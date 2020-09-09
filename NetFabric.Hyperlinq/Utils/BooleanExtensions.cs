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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Conditional(this bool condition, int consequent, int alternative)
        {
            var mask = -condition.AsByte() >> 31; // test ? 1..1 : 0..0
            return (mask & consequent) + (~mask & alternative); // test ? end : index == (test ? end : 0) + (test ? 0 : index)
        }
    }
}
