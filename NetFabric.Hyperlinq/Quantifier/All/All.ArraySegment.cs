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

            var result = false;
            if (source.Any())
            {
                if (source.IsWhole())
                {
                    var array = source.Array;
                    var end = array.Length - 1;
                    for (var index = 0; index < array.Length; index++)
                    {
                        result = !predicate(array![index]);
                        index = result.Conditional(end, index); // result ? end : index
                    }
                }
                else
                {
                    var array = source.Array;
                    var end = source.Count + source.Offset - 1;
                    for (var index = source.Offset; index <= end; index++)
                    {
                        result = !predicate(array![index]);
                        index = result.Conditional(end, index); // result ? end : index
                    }
                }
            }
            return !result;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All<TSource>(this in ArraySegment<TSource> source, PredicateAt<TSource> predicate)
        {
            if (predicate is null)
                Throw.ArgumentNullException(nameof(predicate));

            var result = false;
            if (source.Any())
            {
                if (source.IsWhole())
                {
                    var array = source.Array;
                    var end = array.Length - 1;
                    for (var index = 0; index < array.Length; index++)
                    {
                        result = !predicate(array![index], index);
                        index = result.Conditional(end, index); // result ? end : index
                    }
                }
                else
                {
                    var array = source.Array;
                    var end = source.Count + source.Offset - 1;
                    for (var index = source.Offset; index <= end; index++)
                    {
                        result = !predicate(array![index], index);
                        index = result.Conditional(end, index); // result ? end : index
                    }
                }
            }
            return !result;
        }
    }
}
