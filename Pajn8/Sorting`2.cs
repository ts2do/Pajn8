using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal static class Sorting<T, TComparer>
        where TComparer : IComparer<T>
    {
        public const int IntrosortSizeThreshold = 16;

        public static void Sort(ref T keys, ref TComparer comparer, int length)
        {
            try
            {
                IntrospectiveSort(ref keys, ref comparer, length);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(Strings.InvalidOperation_IComparerFailed, e);
            }
        }

        private static void SwapIfGreaterWithValues(ref T keys, ref TComparer comparer, int length, int i, int j)
        {
            Debug.Assert(0 <= i && i < length);
            Debug.Assert(0 <= j && j < length);

            if (i != j)
            {
                ref T keysI = ref Unsafe.Add(ref keys, i);
                T keyI = keysI;
                ref T keysJ = ref Unsafe.Add(ref keys, j);
                T keyJ = keysJ;
                if (comparer.Compare(keyI, keyJ) > 0)
                {
                    keysI = keyJ;
                    keysJ = keyI;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(ref T keys, int i, int j)
        {
            if (i != j)
            {
                ref T keysI = ref Unsafe.Add(ref keys, i);
                ref T keysJ = ref Unsafe.Add(ref keys, j);
                T key = keysI;
                keysI = keysJ;
                keysJ = key;
            }
        }

        private static void IntrospectiveSort(ref T keys, ref TComparer comparer, int length)
        {
            if (length > 1)
            {
                IntroSort(ref keys, ref comparer, length, 2 * (32 - BitOperations.LeadingZeroCount((uint)length)));
            }
        }

        private static void IntroSort(ref T keys, ref TComparer comparer, int length, int depthLimit)
        {
            int lo = 0;
            int hi = length - 1;

            while (hi > lo)
            {
                int partitionSize = hi - lo + 1;
                if (partitionSize <= IntrosortSizeThreshold)
                {
                    if (partitionSize == 1)
                    {
                        return;
                    }

                    if (partitionSize == 2)
                    {
                        SwapIfGreaterWithValues(ref keys, ref comparer, length, lo, hi);
                        return;
                    }

                    if (partitionSize == 3)
                    {
                        SwapIfGreaterWithValues(ref keys, ref comparer, length, lo, hi - 1);
                        SwapIfGreaterWithValues(ref keys, ref comparer, length, lo, hi);
                        SwapIfGreaterWithValues(ref keys, ref comparer, length, hi - 1, hi);
                        return;
                    }

                    InsertionSort(ref Unsafe.Add(ref keys, lo), ref comparer, hi - lo + 1);
                    return;
                }

                if (depthLimit == 0)
                {
                    HeapSort(ref Unsafe.Add(ref keys, lo), ref comparer, hi - lo + 1);
                    return;
                }
                --depthLimit;

                int p = PickPivotAndPartition(ref Unsafe.Add(ref keys, lo), ref comparer, hi - lo + 1);

                // Note we've already partitioned around the pivot and do not have to move the pivot again.
                IntroSort(ref Unsafe.Add(ref keys, p + 1), ref comparer, hi - p, depthLimit);
                hi = p - 1;
            }
        }

        public static int PickPivotAndPartition(ref T keys, ref TComparer comparer, int length)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(length > 0);

            int lo = 0;
            int hi = length - 1;

            // Compute median-of-three.  But also partition them, since we've done the comparison.
            int middle = lo + ((hi - lo) / 2);

            // Sort lo, mid and hi appropriately, then pick mid as the pivot.
            SwapIfGreaterWithValues(ref keys, ref comparer, length, lo, middle); // swap the low with the mid point
            SwapIfGreaterWithValues(ref keys, ref comparer, length, lo, hi); // swap the low with the high
            SwapIfGreaterWithValues(ref keys, ref comparer, length, middle, hi); // swap the middle with the high

            T pivot = Unsafe.Add(ref keys, middle);
            Swap(ref keys, middle, hi - 1);
            int left = lo, right = hi - 1; // We already partitioned lo and hi and put the pivot in hi - 1.  And we pre-increment & decrement below.

            if (left < right)
            {
                while (true)
                {
                    while (comparer.Compare(Unsafe.Add(ref keys, ++left), pivot) < 0)
                        ;
                    while (comparer.Compare(pivot, Unsafe.Add(ref keys, --right)) < 0)
                        ;

                    if (left >= right)
                        break;

                    Swap(ref keys, left, right);
                }
            }

            // Put pivot in the right location.
            Swap(ref keys, left, hi - 1);
            return left;
        }

        private static void HeapSort(ref T keys, ref TComparer comparer, int length)
        {
            Debug.Assert(length > 0);

            int lo = 0;
            int hi = length - 1;

            int n = hi - lo + 1;
            for (int i = n / 2; i >= 1; --i)
            {
                DownHeap(ref keys, ref comparer, length, i, n, lo);
            }

            for (int i = n; i > 1; --i)
            {
                Swap(ref keys, lo, lo + i - 1);
                DownHeap(ref keys, ref comparer, length, 1, i - 1, lo);
            }
        }

        private static void DownHeap(ref T keys, ref TComparer comparer, int length, int i, int n, int lo)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(lo < length);

            keys = ref Unsafe.Add(ref keys, lo);

            T d = Unsafe.Add(ref keys, i - 1);

            while (i <= n / 2)
            {
                int child = 2 * i;
                ref T keysChild = ref Unsafe.Add(ref keys, child - 1);
                ref T keysChildNext = ref Unsafe.Add(ref keys, child);
                if (child < n && comparer.Compare(keysChild, keysChildNext) < 0)
                {
                    ++child;
                    keysChild = ref keysChildNext;
                    keysChildNext = ref Unsafe.Add(ref keysChildNext, 1);
                }

                if (!(comparer.Compare(d, keysChild) < 0))
                    break;

                Unsafe.Add(ref keys, i - 1) = keysChild;
                i = child;
            }

            Unsafe.Add(ref keys, i - 1) = d;
        }

        private static void InsertionSort(ref T keys, ref TComparer comparer, int length)
        {
            Debug.Assert(comparer != null);

            for (int i = 1; i < length; ++i)
            {
                T t = Unsafe.Add(ref keys, i);

                ref T keyB = ref Unsafe.Add(ref keys, i);
                ref T keyA = ref Unsafe.Add(ref keyB, -1);

                for (int j = i - 1; j >= 0 && comparer.Compare(t, keyA) < 0; --j)
                {
                    keyB = keyA;
                    keyB = ref keyA;
                    keyA = ref Unsafe.Add(ref keyA, -1);
                }

                keyB = t;
            }
        }
    }
}
