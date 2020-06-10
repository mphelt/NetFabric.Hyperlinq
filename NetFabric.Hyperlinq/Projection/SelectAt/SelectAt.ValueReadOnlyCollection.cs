using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueReadOnlyCollection
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TResult> Select<TEnumerable, TEnumerator, TSource, TResult>(
            this TEnumerable source, 
            SelectorAt<TSource, TResult> selector)
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            if(selector is null) Throw.ArgumentNullException(nameof(selector));

            return new SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TResult>(in source, selector);
        }

        [GeneratorMapping("TSource", "TResult")]
        public readonly partial struct SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TResult>
            : IValueReadOnlyCollection<TResult, SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TResult>.Enumerator>
            , ICollection<TResult>
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            readonly TEnumerable source;
            readonly SelectorAt<TSource, TResult> selector;

            internal SelectAtEnumerable(in TEnumerable source, SelectorAt<TSource, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            public readonly int Count => source.Count;

            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly Enumerator GetEnumerator() => new Enumerator(in this);
            readonly IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => new Enumerator(in this);
            readonly IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

            bool ICollection<TResult>.IsReadOnly  
                => true;

            void ICollection<TResult>.CopyTo(TResult[] array, int arrayIndex) 
            {
                if (source.Count == 0)
                    return;

                checked
                {
                    using var enumerator = source.GetEnumerator();
                    for (var index = 0; enumerator.MoveNext(); index++)
                        array[index + arrayIndex] = selector(enumerator.Current, index);
                }
            }

            void ICollection<TResult>.Add(TResult item) 
                => throw new NotSupportedException();
            void ICollection<TResult>.Clear() 
                => throw new NotSupportedException();
            bool ICollection<TResult>.Contains(TResult item) 
                => throw new NotSupportedException();
            bool ICollection<TResult>.Remove(TResult item) 
                => throw new NotSupportedException();

            public struct Enumerator
                : IEnumerator<TResult>
            {
                [SuppressMessage("Style", "IDE0044:Add readonly modifier")]
                TEnumerator enumerator; // do not make readonly
                readonly SelectorAt<TSource, TResult> selector;
                int index;

                internal Enumerator(in SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TResult> enumerable)
                {
                    enumerator = enumerable.source.GetEnumerator();
                    selector = enumerable.selector;
                    index = -1;
                }

                public readonly TResult Current
                    => selector(enumerator.Current, index);
                readonly object? IEnumerator.Current 
                    => selector(enumerator.Current, index);

                public bool MoveNext()
                {
                    if (enumerator.MoveNext())
                    {
                        checked { index++; }
                        return true;
                    }
                    Dispose();
                    return false;
                }

                [ExcludeFromCodeCoverage]
                public readonly void Reset() 
                    => throw new NotSupportedException();

                public void Dispose() => enumerator.Dispose();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Count != 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Contains(TResult value, IEqualityComparer<TResult>? comparer = null)
                => ValueReadOnlyCollection.Contains<TEnumerable, TEnumerator, TSource, TResult>(source, value, comparer, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ValueReadOnlyCollection.SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TSelectorResult> Select<TSelectorResult>(Selector<TResult, TSelectorResult> selector)
                => ValueReadOnlyCollection.Select<TEnumerable, TEnumerator, TSource, TSelectorResult>(source, Utils.Combine(this.selector, selector));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ValueReadOnlyCollection.SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TSelectorResult> Select<TSelectorResult>(SelectorAt<TResult, TSelectorResult> selector)
                => ValueReadOnlyCollection.Select<TEnumerable, TEnumerator, TSource, TSelectorResult>(source, Utils.Combine(this.selector, selector));


            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TResult> ElementAt(int index)
                => ValueReadOnlyCollection.ElementAt<TEnumerable, TEnumerator, TSource, TResult>(source, index, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TResult> First()
                => ValueReadOnlyCollection.First<TEnumerable, TEnumerator, TSource, TResult>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TResult> Single()
                => ValueReadOnlyCollection.Single<TEnumerable, TEnumerator, TSource, TResult>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TResult[] ToArray()
                => ValueReadOnlyCollection.ToArray<TEnumerable, TEnumerator, TSource, TResult>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<TResult> ToList()
                => ValueReadOnlyCollection.ToList<TEnumerable, TEnumerator, TSource, TResult>(source, selector);

            public Dictionary<TKey, TResult> ToDictionary<TKey>(Selector<TResult, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
                => ToDictionary<TKey>(keySelector, comparer);

            public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Selector<TResult, TKey> keySelector, Selector<TResult, TElement> elementSelector, IEqualityComparer<TKey>? comparer = null)
                => ToDictionary<TKey, TElement>(keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TEnumerable, TEnumerator, TSource, TResult>(this SelectAtEnumerable<TEnumerable, TEnumerator, TSource, TResult> source)
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.Count;
    }
}
