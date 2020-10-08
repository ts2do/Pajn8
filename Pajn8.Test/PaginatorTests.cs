using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Pajn8.Test
{
    public class PaginatorTests
    {
        [Fact]
        public void NullChecks()
        {
            string[] nullArray = null, validArray = Array.Empty<string>();
            Comparer<string> nullComparer = null, validComparer = Comparer<string>.Default;
            Func<string, string> nullKeySelector = null, validKeySelector = x => x;

            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(nullArray, 0, 0));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(nullArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(validArray, nullComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(nullArray, 0, 0, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(validArray, 0, 0, nullComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(nullArray, validArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(validArray, nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(nullArray, validArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(validArray, nullArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirect(validArray, validArray, nullComparer));

            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirectNoNulls(nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirectNoNulls(nullArray, 0, 0));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirectNoNulls(nullArray, validArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateDirectNoNulls(validArray, nullArray));

            Assert.Throws<ArgumentNullException>(() => Paginator.Create(nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(nullArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(validArray, nullComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(nullArray, validArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(validArray, nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(nullArray, validArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(validArray, nullArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(validArray, validArray, nullComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(nullArray, validKeySelector));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(validArray, nullKeySelector));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(nullArray, validKeySelector, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(validArray, nullKeySelector, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.Create(validArray, validKeySelector, nullComparer));

            Assert.Throws<ArgumentNullException>(() => Paginator.CreateNoNulls(nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateNoNulls(nullArray, validArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateNoNulls(validArray, nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateNoNulls(nullArray, validKeySelector));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateNoNulls(validArray, nullKeySelector));

            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(nullArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(validArray, nullComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(nullArray, validArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(validArray, nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(nullArray, validArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(validArray, nullArray, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(validArray, validArray, nullComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(nullArray, validKeySelector));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(validArray, nullKeySelector));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(nullArray, validKeySelector, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(validArray, nullKeySelector, validComparer));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStable(validArray, validKeySelector, nullComparer));

            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStableNoNulls(nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStableNoNulls(nullArray, validArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStableNoNulls(validArray, nullArray));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStableNoNulls(nullArray, validKeySelector));
            Assert.Throws<ArgumentNullException>(() => Paginator.CreateStableNoNulls(validArray, nullKeySelector));
        }

        [Fact]
        public void BasicRangeChecks()
        {
            var array = Array.Empty<string>();
            var comparer = Comparer<string>.Default;

            Assert.Throws<ArgumentOutOfRangeException>(() => Paginator.CreateDirect(array, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Paginator.CreateDirect(array, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Paginator.CreateDirect(array, -1, 0, comparer));
            Assert.Throws<ArgumentOutOfRangeException>(() => Paginator.CreateDirect(array, 0, -1, comparer));

            Assert.Throws<ArgumentOutOfRangeException>(() => Paginator.CreateDirectNoNulls(array, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Paginator.CreateDirectNoNulls(array, 0, -1));
        }

        [Fact]
        public void ArrayRangeChecks()
        {
            var array = Array.Empty<string>();
            var comparer = Comparer<string>.Default;

            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array, 1, 0));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array, 0, 1));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array, 1, 0, comparer));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array, 0, 1, comparer));

            Assert.Throws<ArgumentException>(() => Paginator.CreateDirectNoNulls(array, 1, 0));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirectNoNulls(array, 0, 1));
        }

        [Fact]
        public void KeysAndValuesLengthChecks()
        {
            var comparer = Comparer<string>.Default;
            string[] array1 = { "" }, array2 = { "", "" };

            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array1, array2));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array2, array1));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array1, array2, comparer));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array2, array1, comparer));

            Assert.Throws<ArgumentException>(() => Paginator.CreateDirectNoNulls(array1, array2));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirectNoNulls(array2, array1));

            Assert.Throws<ArgumentException>(() => Paginator.Create(array1, array2));
            Assert.Throws<ArgumentException>(() => Paginator.Create(array2, array1));
            Assert.Throws<ArgumentException>(() => Paginator.Create(array1, array2, comparer));
            Assert.Throws<ArgumentException>(() => Paginator.Create(array2, array1, comparer));

            Assert.Throws<ArgumentException>(() => Paginator.CreateNoNulls(array1, array2));
            Assert.Throws<ArgumentException>(() => Paginator.CreateNoNulls(array2, array1));

            Assert.Throws<ArgumentException>(() => Paginator.CreateStable(array1, array2));
            Assert.Throws<ArgumentException>(() => Paginator.CreateStable(array2, array1));
            Assert.Throws<ArgumentException>(() => Paginator.CreateStable(array1, array2, comparer));
            Assert.Throws<ArgumentException>(() => Paginator.CreateStable(array2, array1, comparer));

            Assert.Throws<ArgumentException>(() => Paginator.CreateStableNoNulls(array1, array2));
            Assert.Throws<ArgumentException>(() => Paginator.CreateStableNoNulls(array2, array1));
        }

        [Fact]
        public void KeysAndValuesDifferChecks()
        {
            string[] array0 = Array.Empty<string>(), array1 = { "" };
            var comparer = Comparer<string>.Default;

            Paginator.CreateDirect(array0, array0);
            Paginator.CreateDirect(array0, array0, comparer);

            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array1, array1));
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirect(array1, array1, comparer));

            Paginator.CreateDirectNoNulls(array0, array0);
            Assert.Throws<ArgumentException>(() => Paginator.CreateDirectNoNulls(array1, array1));
        }

        [Fact]
        public void GetPageRangeChecks()
        {
            var paginator = Paginator.CreateDirect(Array.Empty<int>());

            Assert.Throws<ArgumentOutOfRangeException>(() => paginator.GetPage(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => paginator.GetPage(1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => paginator.GetPage(1..^0));
            Assert.Throws<ArgumentOutOfRangeException>(() => paginator.GetPage(^1..^0));
            Assert.Throws<ArgumentOutOfRangeException>(() => paginator.GetPage(1..^0));
            Assert.Throws<ArgumentOutOfRangeException>(() => paginator.GetPage(^1..^1));
        }

        [Fact]
        public void GetPageBadComparer()
        {
            var enumerable = Enumerable.Range(1, 100);
            var badComparer = Comparer<int>.Create((a, b) => 1);
            var paginator1 = Paginator.Create(enumerable, badComparer);
            var paginator2 = Paginator.Create(enumerable, enumerable, badComparer);

            Assert.Throws<ArgumentException>(() => paginator1.GetPage(10, 10));
            Assert.Throws<ArgumentException>(() => paginator2.GetPage(10, 10));
        }

        [Fact]
        public void PartitionAndSort()
        {
            for (int seed = 0; seed < 10; ++seed)
            {
                var random = new Random(seed);
                var input = new int[100];
                for (int i = 0; i < 100; ++i)
                    input[i] = random.Next();
                var paginator = Paginator.Create(input);
                int offset = 25, length = 25;
                Assert.Equal(input.OrderBy(x => x).Skip(offset).Take(length), paginator.GetPage(offset, length));
            }

            for (int seed = 0; seed < 10; ++seed)
            {
                var random = new Random(seed);
                var input = new int[100];
                for (int i = 0; i < 100; ++i)
                    input[i] = random.Next();
                var paginator = Paginator.Create(input, input);
                int offset = 25, length = 25;
                Assert.Equal(input.OrderBy(x => x).Skip(offset).Take(length), paginator.GetPage(offset, length));
            }
        }

        [Fact]
        public void StableSort()
        {
            var chunk1 = Enumerable.Range(1, 50).Select(i => (x: 1, y: i));
            var chunk2 = Enumerable.Range(1, 50).Select(i => (x: 0, y: i));
            var comparer = Comparer<(int x, int y)>.Create((a, b) => a.x.CompareTo(b.x));
            var paginator = Paginator.CreateStable(Enumerable.Concat(chunk1, chunk2), comparer);
            Assert.Equal(Enumerable.Concat(chunk2, chunk1), paginator.GetPage(0, 100).Array);
        }
    }
}
