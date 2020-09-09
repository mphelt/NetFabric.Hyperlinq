using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public static partial class ArrayExtensions
    {
        public static bool Contains<TList, TSource>(this in ArraySegment<TSource> source, [AllowNull] TSource value)
            where TSource : struct
        {
            var result = false;
            if (source.Any())
            {
                if (source.IsWhole())
                {
                    var array = source.Array;
                    var end = array.Length - 1;
                    for (var index = 0; index < array.Length; index++)
                    {
                        result = EqualityComparer<TSource>.Default.Equals(array![index], value!);
                        index = result.Conditional(end, index);
                    }
                }
                else
                {
                    var array = source.Array;
                    var end = source.Count + source.Offset - 1;
                    for (var index = source.Offset; index <= end; index++)
                    {
                        result = EqualityComparer<TSource>.Default.Equals(array![index], value!);
                        index = result.Conditional(end, index);
                    }
                }
            }
            return result;
        }

        public static bool Contains<TSource>(this in ArraySegment<TSource> source, [AllowNull] TSource value, IEqualityComparer<TSource>? comparer = null)
        {
            if (source.Count == 0)
                return false;

            if (Utils.UseDefault(comparer))
                return DefaultContains(source, value!);

            comparer ??= EqualityComparer<TSource>.Default;
            return ComparerContains(source, value, comparer);

            static bool DefaultContains(in ArraySegment<TSource> source, [AllowNull] TSource value)
            {
                var result = false;
                if (source.Any())
                {
                    if (source.IsWhole())
                    {
                        var array = source.Array;
                        var end = array.Length - 1;
                        for (var index = 0; index < array.Length; index++)
                        {
                            result = EqualityComparer<TSource>.Default.Equals(array![index], value!);
                            index = result.Conditional(end, index);
                        }
                    }
                    else
                    {
                        var array = source.Array;
                        var end = source.Count + source.Offset - 1;
                        for (var index = source.Offset; index <= end; index++)
                        {
                            result = EqualityComparer<TSource>.Default.Equals(array![index], value!);
                            index = result.Conditional(end, index);
                        }
                    }
                }
                return result;
            }

            static bool ComparerContains(in ArraySegment<TSource> source, [AllowNull] TSource value, IEqualityComparer<TSource> comparer)
            {
                var result = false;
                if (source.Any())
                {
                    if (source.IsWhole())
                    {
                        var array = source.Array;
                        var end = array.Length - 1;
                        for (var index = 0; index < array.Length; index++)
                        {
                            result = comparer.Equals(array![index], value!);
                            index = result.Conditional(end, index);
                        }
                    }
                    else
                    {
                        var array = source.Array;
                        var end = source.Count + source.Offset - 1;
                        for (var index = source.Offset; index <= end; index++)
                        {
                            result = comparer.Equals(array![index], value!);
                            index = result.Conditional(end, index);
                        }
                    }
                }
                return result;
            }
        }


        static bool Contains<TSource, TResult>(this in ArraySegment<TSource> source, [AllowNull] TResult value, NullableSelector<TSource, TResult> selector)
        {
            return source.Count switch
            {
                0 => false,
                _ => Utils.IsValueType<TResult>()
                    ? ValueContains(source, value, selector)
                    : ReferenceContains(source, value, selector)
            };

            static bool ValueContains(in ArraySegment<TSource> source, [AllowNull] TResult value, NullableSelector<TSource, TResult> selector)
            {
                var result = false;
                if (source.Any())
                {
                    if (source.IsWhole())
                    {
                        var array = source.Array;
                        var end = array.Length - 1;
                        for (var index = 0; index < array.Length; index++)
                        {
                            result = EqualityComparer<TResult>.Default.Equals(selector(array![index])!, value!);
                            index = result.Conditional(end, index);
                        }
                    }
                    else
                    {
                        var array = source.Array;
                        var end = source.Count + source.Offset - 1;
                        for (var index = source.Offset; index <= end; index++)
                        {
                            result = EqualityComparer<TResult>.Default.Equals(selector(array![index])!, value!);
                            index = result.Conditional(end, index);
                        }
                    }
                }
                return result;
            }

            static bool ReferenceContains(in ArraySegment<TSource> source, [AllowNull] TResult value, NullableSelector<TSource, TResult> selector)
            {
                var defaultComparer = EqualityComparer<TResult>.Default;

                var result = false;
                if (source.Any())
                {
                    if (source.IsWhole())
                    {
                        var array = source.Array;
                        var end = array.Length - 1;
                        for (var index = 0; index < array.Length; index++)
                        {
                            result = defaultComparer.Equals(selector(array![index])!, value!);
                            index = result.Conditional(end, index);
                        }
                    }
                    else
                    {
                        var array = source.Array;
                        var end = source.Count + source.Offset - 1;
                        for (var index = source.Offset; index <= end; index++)
                        {
                            result = defaultComparer.Equals(selector(array![index])!, value!);
                            index = result.Conditional(end, index);
                        }
                    }
                }
                return result;
            }
        }


        static bool Contains<TSource, TResult>(this in ArraySegment<TSource> source, [AllowNull] TResult value, NullableSelectorAt<TSource, TResult> selector)
        {
            return source.Count switch
            {
                0 => false,
                _ => Utils.IsValueType<TResult>()
                    ? ValueContains(source, value, selector)
                    : ReferenceContains(source, value, selector),
            };

            static bool ValueContains(in ArraySegment<TSource> source, [AllowNull] TResult value, NullableSelectorAt<TSource, TResult> selector)
            {
                var result = false;
                if (source.Any())
                {
                    if (source.IsWhole())
                    {
                        var array = source.Array;
                        var end = array.Length - 1;
                        for (var index = 0; index < array.Length; index++)
                        {
                            result = EqualityComparer<TResult>.Default.Equals(selector(array![index], index)!, value!);
                            index = result.Conditional(end, index);
                        }
                    }
                    else
                    {
                        var end = source.Count - 1;
                        if (source.Offset == 0)
                        {
                            var array = source.Array;
                            for (var index = 0; index <= end; index++)
                            {
                                result = EqualityComparer<TResult>.Default.Equals(selector(array![index], index)!, value!);
                                index = result.Conditional(end, index);
                            }
                        }
                        else
                        {
                            var array = source.Array;
                            var offset = source.Offset;
                            for (var index = 0; index <= end; index++)
                            {
                                result = EqualityComparer<TResult>.Default.Equals(selector(array![index + offset], index)!, value!);
                                index = result.Conditional(end, index);
                            }
                        }
                    }
                }
                return result;
            }

            static bool ReferenceContains(in ArraySegment<TSource> source, [AllowNull] TResult value, NullableSelectorAt<TSource, TResult> selector)
            {
                var defaultComparer = EqualityComparer<TResult>.Default;

                var result = false;
                if (source.Any())
                {
                    if (source.IsWhole())
                    {
                        var array = source.Array;
                        var end = array.Length - 1;
                        for (var index = 0; index < array.Length; index++)
                        {
                            result = defaultComparer.Equals(selector(array![index], index)!, value!);
                            index = result.Conditional(end, index);
                        }
                    }
                    else
                    {
                        var end = source.Count - 1;
                        if (source.Offset == 0)
                        {
                            var array = source.Array;
                            for (var index = 0; index <= end; index++)
                            {
                                result = defaultComparer.Equals(selector(array![index], index)!, value!);
                                index = result.Conditional(end, index);
                            }
                        }
                        else
                        {
                            var array = source.Array;
                            var offset = source.Offset;
                            for (var index = 0; index <= end; index++)
                            {
                                result = defaultComparer.Equals(selector(array![index + offset], index)!, value!);
                                index = result.Conditional(end, index);
                            }
                        }
                    }
                }
                return result;
            }
        }
    }
}
