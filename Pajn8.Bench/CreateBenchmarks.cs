using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace Pajn8.Bench
{
    [MemoryDiagnoser]
    public class CreateBenchmarks
    {
        private int[] intArray;
        private IEnumerable<int> intEnumerable;

        [Params(1000)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            intArray = new int[N];
            intEnumerable = GetInts();
        }

        private IEnumerable<int> GetInts()
        {
            int i = N;
            while (--i >= 0)
                yield return i;
        }

        [Benchmark]
        public void CreateFromArray() => Paginator.Create(intArray);

        [Benchmark]
        public void CreateFromEnumerable() => Paginator.Create(intEnumerable);

        [Benchmark]
        public void CreateDirect() => Paginator.CreateDirect(intArray);

        [Benchmark]
        public void CreateStable() => Paginator.CreateStable(intArray);
    }
}
