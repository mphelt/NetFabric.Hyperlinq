using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ArrayExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All<TSource>(this in ArraySegment<TSource> source, Predicate<TSource> predicate)
        {
            if (predicate is null)
                Throw.ArgumentNullException(nameof(predicate));

            if (source.Any())
            {
                if (source.IsWhole())
                {
                    var array = source.Array;
                    for (var index = 0; index < array.Length; index++)
                    {
                        if (!predicate(array![index]))
                            return false;
                    }
                }
                else
                {
                    var array = source.Array;
                    var end = source.Count + source.Offset - 1;
                    for (var index = source.Offset; index <= end; index++)
                    {
                        if (!predicate(array![index]))
                            return false;
                    }
                }
            }
            return true;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All<TSource>(this in ArraySegment<TSource> source, PredicateAt<TSource> predicate)
        {
            if (predicate is null)
                Throw.ArgumentNullException(nameof(predicate));

            if (source.Any())
            {
                if (source.IsWhole())
                {
                    var array = source.Array;
                    for (var index = 0; index < array.Length; index++)
                    {
                        if (!predicate(array![index], index))
                            return false;
                    }
                }
                else
                {
                    var array = source.Array;
                    var end = source.Count + source.Offset - 1;
                    for (var index = source.Offset; index <= end; index++)
                    {
                        if (!predicate(array![index], index))
                            return false;
                    }
                }
            }
            return true;
        }
    }
}

