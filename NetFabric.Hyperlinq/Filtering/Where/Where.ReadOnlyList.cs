﻿using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ReadOnlyListExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereEnumerable<TList, TSource> Where<TList, TSource>(this TList source, Predicate<TSource> predicate)
            where TList : notnull, IReadOnlyList<TSource>
        {
            if (predicate is null) Throw.ArgumentNullException(nameof(predicate));

            return new WhereEnumerable<TList, TSource>(in source, predicate, 0, source.Count);
        }

        static WhereEnumerable<TList, TSource> Where<TList, TSource>(this TList source, Predicate<TSource> predicate, int offset, int count)
            where TList : notnull, IReadOnlyList<TSource>
            => new WhereEnumerable<TList, TSource>(in source, predicate, offset, count);

        [StructLayout(LayoutKind.Auto)]
        public readonly partial struct WhereEnumerable<TList, TSource>
            : IValueEnumerable<TSource, WhereEnumerable<TList, TSource>.DisposableEnumerator>
            where TList : notnull, IReadOnlyList<TSource>
        {
            readonly TList source;
            readonly Predicate<TSource> predicate;
            readonly int offset;
            readonly int count;

            internal WhereEnumerable(in TList source, Predicate<TSource> predicate, int offset, int count)
            {
                this.source = source;
                this.predicate = predicate;
                (this.offset, this.count) = Utils.SkipTake(source.Count, offset, count);
            }

            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly Enumerator GetEnumerator() => new Enumerator(in this);
            readonly DisposableEnumerator IValueEnumerable<TSource, WhereEnumerable<TList, TSource>.DisposableEnumerator>.GetEnumerator() => new DisposableEnumerator(in this);
            readonly IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new DisposableEnumerator(in this);
            readonly IEnumerator IEnumerable.GetEnumerator() => new DisposableEnumerator(in this);

            [StructLayout(LayoutKind.Sequential)]
            public struct Enumerator
            {
                int index;
                readonly int end;
                readonly TList source;
                readonly Predicate<TSource> predicate;

                internal Enumerator(in WhereEnumerable<TList, TSource> enumerable)
                {
                    source = enumerable.source;
                    predicate = enumerable.predicate;
                    index = enumerable.offset - 1;
                    end = index + enumerable.count;
                }

                [MaybeNull]
                public readonly TSource Current
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => source[index];
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext()
                {
                    while (++index <= end)
                    {
                        if (predicate(source[index]))
                            return true;
                    }
                    return false;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DisposableEnumerator
                : IEnumerator<TSource>
            {
                int index;
                readonly int end;
                readonly TList source;
                readonly Predicate<TSource> predicate;

                internal DisposableEnumerator(in WhereEnumerable<TList, TSource> enumerable)
                {
                    source = enumerable.source;
                    predicate = enumerable.predicate;
                    index = enumerable.offset - 1;
                    end = index + enumerable.count;
                }

                [MaybeNull]
                public readonly TSource Current 
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => source[index];
                }
                readonly TSource IEnumerator<TSource>.Current 
                    => source[index];
                readonly object? IEnumerator.Current
                    => source[index];

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext()
                {
                    while (++index <= end)
                    {
                        if (predicate(source[index]))
                            return true;
                    }
                    return false;
                }

                [ExcludeFromCodeCoverage]
                public readonly void Reset() 
                    => Throw.NotSupportedException();

                public readonly void Dispose() { }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => ReadOnlyListExtensions.Count<TList, TSource>(source, predicate, offset, count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => ReadOnlyListExtensions.Any<TList, TSource>(source, predicate, offset, count);
                
            public ReadOnlyListExtensions.WhereSelecTList<TList, TSource, TResult> Select<TResult>(NullableSelector<TSource, TResult> selector)
            {
                if (selector is null) Throw.ArgumentNullException(nameof(selector));

                return ReadOnlyListExtensions.WhereSelect<TList, TSource, TResult>(source, predicate, selector, offset, count);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyListExtensions.WhereEnumerable<TList, TSource> Where(Predicate<TSource> predicate)
                => ReadOnlyListExtensions.Where<TList, TSource>(source, Utils.Combine(this.predicate, predicate), offset, count);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyListExtensions.WhereAtEnumerable<TList, TSource> Where(PredicateAt<TSource> predicate)
                => ReadOnlyListExtensions.Where<TList, TSource>(source, Utils.Combine(this.predicate, predicate), offset, count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> ElementAt(int index)
                => ReadOnlyListExtensions.ElementAt<TList, TSource>(source, index, predicate, offset, count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> First()
                => ReadOnlyListExtensions.First<TList, TSource>(source, predicate, offset, count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TSource> Single()
                => ReadOnlyListExtensions.Single<TList, TSource>(source, predicate, offset, count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource[] ToArray()
                => ReadOnlyListExtensions.ToArray<TList, TSource>(source, predicate, offset, count);

            public IMemoryOwner<TSource> ToArray(MemoryPool<TSource> pool)
                => ReadOnlyListExtensions.ToArray<TList, TSource>(source, predicate, offset, count, pool);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<TSource> ToList()
                => ReadOnlyListExtensions.ToList<TList, TSource>(source, predicate, offset, count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Dictionary<TKey, TSource> ToDictionary<TKey>(Selector<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = default)
                where TKey : notnull
                => ReadOnlyListExtensions.ToDictionary<TList, TSource, TKey>(source, keySelector, comparer, predicate, offset, count);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Selector<TSource, TKey> keySelector, NullableSelector<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer = default)
                where TKey : notnull
                => ReadOnlyListExtensions.ToDictionary<TList, TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, predicate, offset, count);
        }
    }
}

