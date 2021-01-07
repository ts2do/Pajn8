using Pajn8.Comparers;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal sealed class Paginator<TKey, TValue, TComparer> : PaginatorBase<TKey, TValue, TComparer>
        where TComparer : IComparer2<TKey>
    {
        private readonly TKey[] keys;

        internal Paginator(TKey[] keys, TValue[] values, int offset, int length, in TComparer comparer)
            : base(values, offset, length, comparer)
        {
            this.keys = keys;
        }

        protected override void DivideAndSort(int start, int end, int play, ref bool wrapException)
        {
            SplitPartitions(start, end, play, ref wrapException);
            SortPartitions(ref wrapException);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SplitPartitions(int start, int end, int play, ref bool wrapException)
        {
            PartitionNode p;
            for (int position = start; position < end; position = p.EndIndex)
            {
                p = rootNode.Find(position);
                if (!p.IsSorted)
                {
                    p = SplitPartition(p, start, end, play, ref wrapException, position);
                }
            }

            PartitionNode SplitPartition(PartitionNode p, int start, int end, int play, ref bool wrapException, int position)
            {
                int depthLimit = SortUtils.DepthLimit(values.Length) - p.Depth;
                while (depthLimit > 0 && p.Count > SortUtils.InsertionSortThreshold && (end < p.EndIndex - play || start - play > p.StartIndex))
                {
                    int k = Partition(keys, values, ref comparer, p.StartIndex, p.EndIndex - 1, ref wrapException);
                    p.Split(k);
                    p = k > position ? p.LeftNode! : p.RightNode!;
                    --depthLimit;
                }

                if (numPartitionsToSort == partitionsToSort.Length)
                {
                    Array.Resize(ref partitionsToSort, numPartitionsToSort * 2);
                }
                partitionsToSort[numPartitionsToSort++] = p;
                return p;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SortPartitions(ref bool wrapException)
        {
            int i = 0;
            while (i < numPartitionsToSort)
            {
                PartitionNode p = partitionsToSort[i];
                if (p.IsSorted)
                {
                    ++i;
                    continue;
                }

                int minDepth = p.Depth;
                int rangeStart = p.StartIndex, rangeEnd = p.EndIndex;
                while (++i < numPartitionsToSort)
                {
                    p = partitionsToSort[i];
                    if (p.IsSorted || p.StartIndex != rangeEnd)
                    {
                        break;
                    }

                    rangeEnd = p.EndIndex;
                    p.IsSorted = true;
                    if (minDepth > p.Depth)
                        minDepth = p.Depth;
                }
                if (rangeEnd - rangeStart > 1)
                    IntroSort(keys, values, ref comparer, rangeStart, rangeEnd - 1, SortUtils.DepthLimit(values.Length) - minDepth, ref wrapException);
            }

            numPartitionsToSort = 0;
        }

        private static int Partition(TKey[] keys, TValue[] values, ref TComparer comparer, int first, int last, ref bool wrapException)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(first <= last);

            int left = first, right = last - 1;
            int count = last - first;
            int mid = first + (count / 2);
            if (count > SortUtils.TukeyNintherThreshold)
            {
                int step = (count + 1) / 8;
                MedianOfThree(keys, values, ref comparer, first + step, first, first + step * 2);
                MedianOfThree(keys, values, ref comparer, mid - step, mid, mid + step);
                MedianOfThree(keys, values, ref comparer, last - step * 2, last, last - step);
            }
            MedianOfThree(keys, values, ref comparer, first, mid, last);

            TKey pivot;
            {
                // Move (mid) to (last - 1)
                ref TKey key1 = ref keys[mid];
                pivot = key1;
                ref TKey key2 = ref keys[last - 1];
                key1 = key2;
                key2 = pivot;

                ref TValue value1 = ref values[mid];
                TValue tmpValue = value1;
                ref TValue value2 = ref values[last - 1];
                value1 = value2;
                value2 = tmpValue;
            }

            // Partition (first + 1)..(last - 2)
            while (left < right)
            {
                while (comparer.GreaterThan(ref pivot, ref keys[++left]))
                {
                    if (left == last)
                    {
                        wrapException = false;
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                    }
                }

                while (comparer.LessThan(ref pivot, ref keys[--right]))
                {
                    if (right == first)
                    {
                        wrapException = false;
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                    }
                }

                if (left < right)
                    Swap(keys, values, left, right);
            }

            {
                // Move pivot to destination
                ref TKey key1 = ref keys[last - 1];
                ref TKey key2 = ref keys[left];
                key1 = key2;
                key2 = pivot;

                ref TValue value1 = ref values[last - 1];
                TValue tmpValue = value1;
                ref TValue value2 = ref values[left];
                value1 = value2;
                value2 = tmpValue;
            }

            return left;
        }

        private static void MedianOfThree(TKey[] keys, TValue[] values, ref TComparer comparer, int first, int mid, int last)
        {
            ref TKey firstKey = ref keys[first];
            ref TKey midKey = ref keys[mid];
            ref TKey lastKey = ref keys[last];

            ref TValue firstValue = ref values[first];
            ref TValue midValue = ref values[mid];

            TKey tmpKey;
            TValue tmpValue;

            if (comparer.GreaterThan(ref firstKey, ref midKey))
            {
                // Swap (first) and (mid)
                tmpKey = firstKey;
                firstKey = midKey;
                midKey = tmpKey;

                tmpValue = firstValue;
                firstValue = midValue;
                midValue = tmpValue;
            }

            if (comparer.GreaterThan(ref midKey, ref lastKey))
            {
                {
                    // Swap (mid) and (last)
                    tmpKey = midKey;
                    midKey = lastKey;
                    lastKey = tmpKey;

                    ref TValue lastValue = ref values[last];
                    tmpValue = midValue;
                    midValue = lastValue;
                    lastValue = tmpValue;
                }

                if (comparer.GreaterThan(ref firstKey, ref midKey))
                {
                    // Swap (first) and (mid)
                    tmpKey = firstKey;
                    firstKey = midKey;
                    midKey = tmpKey;

                    tmpValue = firstValue;
                    firstValue = midValue;
                    midValue = tmpValue;
                }
            }
        }

        private static void SwapIfGreater(TKey[] keys, TValue[] values, ref TComparer comparer, int a, int b)
        {
            if (a != b)
            {
                ref TKey keysA = ref keys[a], keysB = ref keys[b];
                TKey keyA = keysA, keyB = keysB;
                if (comparer.GreaterThan(ref keyA, ref keyB))
                {
                    keysA = keyB;
                    keysB = keyA;

                    ref TValue valuesA = ref values[a];
                    TValue value = valuesA;
                    ref TValue valuesB = ref values[b];
                    valuesA = valuesB;
                    valuesB = value;
                }
            }
        }

        private static void InsertionSort(TKey[] keys, TValue[] values, ref TComparer comparer, int lo, int hi)
        {
            int partitionSize = hi - lo + 1;
            if (partitionSize < 2)
            {
                return;
            }

            SwapIfGreater(keys, values, ref comparer, lo, lo + 1);

            if (partitionSize > 2)
            {
                SwapIfGreater(keys, values, ref comparer, lo, lo + 2);
                SwapIfGreater(keys, values, ref comparer, lo + 1, lo + 2);

                if (partitionSize > 3)
                {
                    InsertionSort(keys, values, ref comparer, lo + 2, lo, hi);
                }
            }
        }

        private static void IntroSort(TKey[] keys, TValue[] values, ref TComparer comparer, int lo, int hi, int depthLimit, ref bool wrapException)
        {
            while (hi > lo)
            {
                int partitionSize = hi - lo + 1;
                if (partitionSize <= SortUtils.InsertionSortThreshold)
                {
                    InsertionSort(keys, values, ref comparer, lo, hi);
                    break;
                }

                if (depthLimit == 0)
                {
                    Heapsort(keys, values, ref comparer, lo, hi);
                    break;
                }

                --depthLimit;

                int p = Partition(keys, values, ref comparer, lo, hi, ref wrapException);
                IntroSort(keys, values, ref comparer, p + 1, hi, depthLimit, ref wrapException);
                hi = p - 1;
            }
        }

        private static void Heapsort(TKey[] keys, TValue[] values, ref TComparer comparer, int lo, int hi)
        {
            int n = hi - lo + 1;
            for (int i = n >> 1; i >= 1; --i)
            {
                DownHeap(keys, values, ref comparer, i, n, lo);
            }

            for (int i = n - 1; i >= 1; --i)
            {
                Swap(keys, values, lo, lo + i);
                DownHeap(keys, values, ref comparer, 1, i, lo);
            }
        }

        private static void DownHeap(TKey[] keys, TValue[] values, ref TComparer comparer, int i, int n, int lo)
        {
            int baseIdx = lo - 1;
            TKey d = keys[baseIdx + i];
            TValue dValue = values[baseIdx + i];
            while (i <= n >> 1)
            {
                int child = 2 * i;
                if (child < n && comparer.LessThan(ref keys[baseIdx + child], ref keys[baseIdx + child + 1]))
                {
                    ++child;
                }
                if (!comparer.LessThan(ref d, ref keys[baseIdx + child]))
                    break;
                keys[baseIdx + i] = keys[baseIdx + child];
                values[baseIdx + i] = values[baseIdx + child];
                i = child;
            }
            keys[baseIdx + i] = d;
            values[baseIdx + i] = dValue;
        }

        private static void InsertionSort(TKey[] keys, TValue[] values, ref TComparer comparer, int i, int lo, int hi)
        {
            if (i == hi)
                return;

            int j = i;
            TKey insertingKey = keys[j + 1];
            TValue insertingValue = values[j + 1];
            while (comparer.LessThan(ref insertingKey, ref keys[j]) && --j >= lo)
                ;

            ++j;
            for (int k = i; k >= j; --k)
            {
                keys[k + 1] = keys[k];
                values[k + 1] = values[k];
            }
            keys[j] = insertingKey;
            values[j] = insertingValue;

            InsertionSort(keys, values, ref comparer, i + 1, lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(TKey[] keys, TValue[] values, int i, int j)
        {
            ref TKey keysI = ref keys[i];
            TKey tmpKey = keysI;
            ref TKey keysJ = ref keys[j];
            keysI = keysJ;
            keysJ = tmpKey;

            ref TValue valuesI = ref values[i];
            TValue tmpValue = valuesI;
            ref TValue valuesJ = ref values[j];
            valuesI = valuesJ;
            valuesJ = tmpValue;
        }
    }
}
