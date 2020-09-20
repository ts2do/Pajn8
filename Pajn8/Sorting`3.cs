using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal static class Sorting<TKey, TValue, TComparer>
        where TComparer : IComparer<TKey>
    {
        public const int IntrosortSizeThreshold = 16;

        public static void Sort(ref TKey keys, ref TComparer comparer, ref TValue values, int length)
        {
            try
            {
                IntrospectiveSort(ref keys, ref comparer, ref values, length);
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

        private static void SwapIfGreaterWithValues(ref TKey keys, ref TComparer comparer, ref TValue values, int length, int i, int j)
        {
            Debug.Assert(0 <= i && i < length);
            Debug.Assert(0 <= j && j < length);

            if (i != j)
            {
                ref TKey keysI = ref Unsafe.Add(ref keys, i);
                TKey keyI = keysI;
                ref TKey keysJ = ref Unsafe.Add(ref keys, j);
                TKey keyJ = keysJ;
                if (comparer.Compare(keyI, keyJ) > 0)
                {
                    keysI = keyJ;
                    keysJ = keyI;

                    ref TValue valuesI = ref Unsafe.Add(ref values, i);
                    ref TValue valuesJ = ref Unsafe.Add(ref values, j);
                    TValue value = valuesI;
                    valuesI = valuesJ;
                    valuesJ = value;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(ref TKey keys, ref TValue values, int i, int j)
        {
            if (i != j)
            {
                ref TKey keysI = ref Unsafe.Add(ref keys, i);
                ref TKey keysJ = ref Unsafe.Add(ref keys, j);
                TKey key = keysI;
                keysI = keysJ;
                keysJ = key;

                ref TValue valuesI = ref Unsafe.Add(ref values, i);
                ref TValue valuesJ = ref Unsafe.Add(ref values, j);
                TValue value = valuesI;
                valuesI = valuesJ;
                valuesJ = value;
            }
        }

        private static void IntrospectiveSort(ref TKey keys, ref TComparer comparer, ref TValue values, int length)
        {
            if (length > 1)
            {
                IntroSort(ref keys, ref comparer, ref values, length, 2 * (32 - BitOperations.LeadingZeroCount((uint)length)));
            }
        }

        private static void IntroSort(ref TKey keys, ref TComparer comparer, ref TValue values, int length, int depthLimit)
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
                        SwapIfGreaterWithValues(ref keys, ref comparer, ref values, length, lo, hi);
                        return;
                    }

                    if (partitionSize == 3)
                    {
                        SwapIfGreaterWithValues(ref keys, ref comparer, ref values, length, lo, hi - 1);
                        SwapIfGreaterWithValues(ref keys, ref comparer, ref values, length, lo, hi);
                        SwapIfGreaterWithValues(ref keys, ref comparer, ref values, length, hi - 1, hi);
                        return;
                    }

                    InsertionSort(ref Unsafe.Add(ref keys, lo), ref comparer, ref Unsafe.Add(ref values, lo), hi - lo + 1);
                    return;
                }

                if (depthLimit == 0)
                {
                    HeapSort(ref Unsafe.Add(ref keys, lo), ref comparer, ref Unsafe.Add(ref values, lo), hi - lo + 1);
                    return;
                }
                --depthLimit;

                int p = PickPivotAndPartition(ref Unsafe.Add(ref keys, lo), ref comparer, ref Unsafe.Add(ref values, lo), hi - lo + 1);

                // Note we've already partitioned around the pivot and do not have to move the pivot again.
                IntroSort(ref Unsafe.Add(ref keys, p + 1), ref comparer, ref Unsafe.Add(ref values, p + 1), hi - p, depthLimit);
                hi = p - 1;
            }
        }

        public static int PickPivotAndPartition(ref TKey keys, ref TComparer comparer, ref TValue values, int length)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(length > 0);

            int lo = 0;
            int hi = length - 1;

            // Compute median-of-three.  But also partition them, since we've done the comparison.
            int middle = lo + ((hi - lo) / 2);

            // Sort lo, mid and hi appropriately, then pick mid as the pivot.
            SwapIfGreaterWithValues(ref keys, ref comparer, ref values, length, lo, middle); // swap the low with the mid point
            SwapIfGreaterWithValues(ref keys, ref comparer, ref values, length, lo, hi); // swap the low with the high
            SwapIfGreaterWithValues(ref keys, ref comparer, ref values, length, middle, hi); // swap the middle with the high

            TKey pivot = Unsafe.Add(ref keys, middle);
            Swap(ref keys, ref values, middle, hi - 1);
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

                    Swap(ref keys, ref values, left, right);
                }
            }

            // Put pivot in the right location.
            Swap(ref keys, ref values, left, hi - 1);
            return left;
        }

        private static void HeapSort(ref TKey keys, ref TComparer comparer, ref TValue values, int length)
        {
            Debug.Assert(length > 0);

            int lo = 0;
            int hi = length - 1;

            int n = hi - lo + 1;
            for (int i = n / 2; i >= 1; --i)
            {
                DownHeap(ref keys, ref comparer, ref values, length, i, n, lo);
            }

            for (int i = n; i > 1; --i)
            {
                Swap(ref keys, ref values, lo, lo + i - 1);
                DownHeap(ref keys, ref comparer, ref values, length, 1, i - 1, lo);
            }
        }

        private static void DownHeap(ref TKey keys, ref TComparer comparer, ref TValue values, int length, int i, int n, int lo)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(lo >= 0);
            Debug.Assert(lo < length);

            keys = ref Unsafe.Add(ref keys, lo);
            values = ref Unsafe.Add(ref values, lo);

            TKey d = Unsafe.Add(ref keys, i - 1);
            TValue dValue = Unsafe.Add(ref values, i - 1);

            while (i <= n / 2)
            {
                int child = 2 * i;
                ref TKey keysChild = ref Unsafe.Add(ref keys, child - 1);
                ref TKey keysChildNext = ref Unsafe.Add(ref keys, child);
                if (child < n && comparer.Compare(keysChild, keysChildNext) < 0)
                {
                    ++child;
                    keysChild = ref keysChildNext;
                    keysChildNext = ref Unsafe.Add(ref keysChildNext, 1);
                }

                if (!(comparer.Compare(d, keysChild) < 0))
                    break;

                Unsafe.Add(ref keys, i - 1) = keysChild;
                Unsafe.Add(ref values, i - 1) = Unsafe.Add(ref values, child - 1);
                i = child;
            }

            Unsafe.Add(ref keys, i - 1) = d;
            Unsafe.Add(ref values, i - 1) = dValue;
        }

        private static void InsertionSort(ref TKey keys, ref TComparer comparer, ref TValue values, int length)
        {
            Debug.Assert(comparer != null);

            for (int i = 1; i < length; ++i)
            {
                TKey t = Unsafe.Add(ref keys, i);
                TValue tValue = Unsafe.Add(ref values, i);

                ref TKey keyB = ref Unsafe.Add(ref keys, i);
                ref TKey keyA = ref Unsafe.Add(ref keyB, -1);

                ref TValue valueB = ref Unsafe.Add(ref values, i);
                ref TValue valueA = ref Unsafe.Add(ref valueB, -1);

                for (int j = i - 1; j >= 0 && comparer.Compare(t, keyA) < 0; --j)
                {
                    keyB = keyA;
                    keyB = ref keyA;
                    keyA = ref Unsafe.Add(ref keyA, -1);

                    valueB = valueA;
                    valueB = ref valueA;
                    valueA = ref Unsafe.Add(ref valueA, -1);
                }

                keyB = t;
                valueB = tValue;
            }
        }
    }
}
