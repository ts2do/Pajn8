using BenchmarkDotNet.Attributes;
using System;

namespace Pajn8.Bench
{
    [MemoryDiagnoser]
    public class SortBenchmarks
    {
        private int[] intArray;
        private int[] intArrayCopy;

        [Params(10000)]
        public int N;

        [Params(0, 1)]
        public int Seed;

        [GlobalSetup]
        public void GlobalSetup()
        {
            intArray = new int[N];
            intArrayCopy = new int[N];

            Random random = new(Seed);
            int i = N;
            while (--i >= 0)
                intArray[i] = random.Next();
        }

        [Benchmark]
        public void CopyArray() => Array.Copy(intArray, intArrayCopy, N);

        [Benchmark]
        public void ArraySort()
        {
            CopyArray();
            Array.Sort(intArrayCopy);
        }

        [Benchmark]
        public void GetPage()
        {
            CopyArray();
            Paginator.CreateDirect(intArrayCopy).GetPage(0, 100);
        }
    }
}
