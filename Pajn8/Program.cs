using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pajn8
{
    class Program
    {
        public static void Main()
        {
            const int count = 50000000;

            var keys = new int[count];
            var values = new (int, int)[count];

            var random = new Random(0);
            for (int i = 0; i < count; ++i)
            {
                keys[i] = random.Next();
                values[i] = (i, random.Next());
            }

            var sw = new Stopwatch();

            // Generate 20 "random" pages
            var paginator = new ComparablePaginator<int, (int, int)>(keys[..], values[..]);
            random = new Random(10);
            sw.Start();
            for (int i = 0; i < 20; ++i)
                paginator.GetPage(random.Next(0, count / 10) * 10, 10);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Generate the first 100 pages
            paginator = new ComparablePaginator<int, (int, int)>(keys[..], values[..]);
            sw.Restart();
            for (int i = 0; i < 100; ++i)
                paginator.GetPage(i * 10, 10);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Sort the whole darn thing
            sw.Restart();
            Array.Sort(keys, values, Comparer<int>.Default);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
