using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class WhereWhereBenchmarks : BenchmarksBase
    {
        [BenchmarkCategory("Range")]
        [Benchmark(Baseline = true)]
        public int Linq_Range() 
        { 
            var count = 0;
            foreach(var item in System.Linq.Enumerable.Where(System.Linq.Enumerable.Where(linqRange, _ => true), _ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Queue")]
        [Benchmark(Baseline = true)]
        public int Linq_Queue() 
        { 
            var count = 0;
            foreach(var item in System.Linq.Enumerable.Where(System.Linq.Enumerable.Where(queue, _ => true), _ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Array")]
        [Benchmark(Baseline = true)]
        public int Linq_Array() 
        { 
            var count = 0;
            foreach(var item in System.Linq.Enumerable.Where(System.Linq.Enumerable.Where(array, _ => true), _ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("List")]
        [Benchmark(Baseline = true)]
        public int Linq_List() 
        { 
            var count = 0;
            foreach(var item in System.Linq.Enumerable.Where(System.Linq.Enumerable.Where(list, _ => true), _ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark(Baseline = true)]
        public int Linq_Enumerable_Reference() 
        { 
            var count = 0;
            foreach(var item in System.Linq.Enumerable.Where(System.Linq.Enumerable.Where(enumerableReference, _ => true), _ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark(Baseline = true)]
        public int Linq_Enumerable_Value()
        { 
            var count = 0;
            foreach(var item in System.Linq.Enumerable.Where(System.Linq.Enumerable.Where(enumerableValue, _ => true), _ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Range")]
        [Benchmark]
        public int Hyperlinq_Range() 
        { 
            var count = 0;
            foreach(var item in hyperlinqRange.Where(_ => true).Where(_ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Queue")]
        [Benchmark]
        public int Hyperlinq_Queue() 
        { 
            var count = 0;
            foreach(var item in queue.Where(_ => true).Where(_ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Array")]
        [Benchmark]
        public int Hyperlinq_Array() 
        { 
            var count = 0;
            foreach(ref readonly var item in array.Where(_ => true).Where(_ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("List")]
        [Benchmark]
        public int Hyperlinq_List() 
        { 
            var count = 0;
            foreach(var item in list.Where(_ => true).Where(_ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark]
        public int Hyperlinq_Enumerable_Reference()
        { 
            var count = 0;
            foreach(var item in enumerableReference.Where(_ => true).Where(_ => true))
                count++;
            return count;
        }

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark]
        public int Hyperlinq_Enumerable_Value()
        { 
            var count = 0;
            foreach(var item in enumerableValue
                .Where<TestEnumerable.Enumerable, TestEnumerable.Enumerable.Enumerator, int>(_ => true)
                .Where<Enumerable.WhereEnumerable<TestEnumerable.Enumerable, TestEnumerable.Enumerable.Enumerator, int>, Enumerable.WhereEnumerable<TestEnumerable.Enumerable, TestEnumerable.Enumerable.Enumerator, int>.ValueEnumerator, int>(_ => true))
                count++;
            return count;
        }
    }
}
