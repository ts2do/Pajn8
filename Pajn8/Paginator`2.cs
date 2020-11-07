using Pajn8.Comparers;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal sealed class Paginator<T, TComparer> : PaginatorBase<T, T, TComparer>
        where TComparer : IComparer2<T>
    {
        internal Paginator(T[] values, int offset, int length, in TComparer comparer)
            : base(values, offset, length, comparer)
        {
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
                    int k = Partition(values, ref comparer, p.StartIndex, p.EndIndex - 1, ref wrapException);
                    p.Split(k);
                    p = k > position ? p.LeftNode : p.RightNode;
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

                // gather contiguous partitions and sort
                int minDepth = p.Depth;
                int rangeStart = p.StartIndex, rangeEnd = p.EndIndex;
                while (++i < numPartitionsToSort)
                {
                    p = partitionsToSort[i];
                    if (p.StartIndex != rangeEnd)
                    {
                        break;
                    }

                    rangeEnd = p.EndIndex;
                    if (minDepth > p.Depth)
                        minDepth = p.Depth;
                    p.IsSorted = true;
                }
                if (rangeEnd - rangeStart > 1)
                    IntroSort(values, ref comparer, rangeStart, rangeEnd - 1, SortUtils.DepthLimit(values.Length) - minDepth, ref wrapException);
            }

            numPartitionsToSort = 0;
        }

        private static int Partition(T[] values, ref TComparer comparer, int first, int last, ref bool wrapException)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(first <= last);

            int left = first, right = last - 1;
            int count = last - first;
            int mid = first + (count / 2);
            if (count > SortUtils.TukeyNintherThreshold)
            {
                int step = (count + 1) / 8;
                MedianOfThree(values, ref comparer, first + step, first, first + step * 2);
                MedianOfThree(values, ref comparer, mid - step, mid, mid + step);
                MedianOfThree(values, ref comparer, last - step * 2, last, last - step);
            }
            MedianOfThree(values, ref comparer, first, mid, last);

            T pivot;
            {
                // Move (mid) to (last - 1)
                ref T value1 = ref values[mid];
                pivot = value1;
                ref T value2 = ref values[last - 1];
                value1 = value2;
                value2 = pivot;
            }

            // Partition (first + 1)..(last - 2)
            while (left < right)
            {
                while (comparer.GreaterThan(ref pivot, ref values[++left]))
                {
                    if (left == last)
                    {
                        wrapException = false;
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                    }
                }

                while (comparer.LessThan(ref pivot, ref values[--right]))
                {
                    if (right == first)
                    {
                        wrapException = false;
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                    }
                }

                if (left < right)
                    Swap(values, left, right);
            }

            {
                // Move pivot to destination
                ref T value1 = ref values[last - 1];
                ref T value2 = ref values[left];
                value1 = value2;
                value2 = pivot;
            }

            return left;
        }

        private static void MedianOfThree(T[] values, ref TComparer comparer, int first, int mid, int last)
        {
            ref T firstValue = ref values[first];
            ref T midValue = ref values[mid];
            ref T lastValue = ref values[last];

            T tmpValue;

            if (comparer.GreaterThan(ref firstValue, ref midValue))
            {
                // Swap (first) and (mid)
                tmpValue = firstValue;
                firstValue = midValue;
                midValue = tmpValue;
            }

            if (comparer.GreaterThan(ref midValue, ref lastValue))
            {
                {
                    // Swap (mid) and (last)
                    tmpValue = midValue;
                    midValue = lastValue;
                    lastValue = tmpValue;
                }

                if (comparer.GreaterThan(ref firstValue, ref midValue))
                {
                    // Swap (first) and (mid)
                    tmpValue = firstValue;
                    firstValue = midValue;
                    midValue = tmpValue;
                }
            }
        }

        private static void SwapIfGreater(T[] values, ref TComparer comparer, int a, int b)
        {
            if (a != b)
            {
                ref T valuesA = ref values[a], valuesB = ref values[b];
                T valueA = valuesA, valueB = valuesB;
                if (comparer.GreaterThan(ref valueA, ref valueB))
                {
                    valuesA = valueB;
                    valuesB = valueA;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InsertionSort(T[] values, ref TComparer comparer, int lo, int hi)
        {
            int partitionSize = hi - lo + 1;
            if (partitionSize < 2)
            {
                return;
            }

            SwapIfGreater(values, ref comparer, lo, lo + 1);

            if (partitionSize > 2)
            {
                SwapIfGreater(values, ref comparer, lo, lo + 2);
                SwapIfGreater(values, ref comparer, lo + 1, lo + 2);

                if (partitionSize > 3)
                {
                    InsertionSort(values, ref comparer, lo + 2, lo, hi);
                }
            }
        }

        private static void IntroSort(T[] values, ref TComparer comparer, int lo, int hi, int depthLimit, ref bool wrapException)
        {
            while (hi > lo)
            {
                int partitionSize = hi - lo + 1;
                if (partitionSize <= SortUtils.InsertionSortThreshold)
                {
                    InsertionSort(values, ref comparer, lo, hi);
                    break;
                }

                if (depthLimit == 0)
                {
                    Heapsort(values, ref comparer, lo, hi);
                    break;
                }

                --depthLimit;

                int p = Partition(values, ref comparer, lo, hi, ref wrapException);
                IntroSort(values, ref comparer, p + 1, hi, depthLimit, ref wrapException);
                hi = p - 1;
            }
        }

        private static void Heapsort(T[] values, ref TComparer comparer, int lo, int hi)
        {
            int n = hi - lo + 1;
            for (int i = n >> 1; i >= 1; --i)
            {
                DownHeap(values, ref comparer, i, n, lo);
            }

            for (int i = n - 1; i >= 1; --i)
            {
                Swap(values, lo, lo + i);
                DownHeap(values, ref comparer, 1, i, lo);
            }
        }

        private static void DownHeap(T[] values, ref TComparer comparer, int i, int n, int lo)
        {
            T d = values[lo + i - 1];
            while (i <= n >> 1)
            {
                int child = 2 * i;
                if (child < n && comparer.LessThan(ref values[lo + child - 1], ref values[lo + child]))
                {
                    ++child;
                }
                if (!comparer.LessThan(ref d, ref values[lo + child - 1]))
                    break;
                values[lo + i - 1] = values[lo + child - 1];
                i = child;
            }
            values[lo + i - 1] = d;
        }

        private static void InsertionSort(T[] values, ref TComparer comparer, int i, int lo, int hi)
        {
            if (i == hi)
                return;

            int j = i;
            T insertingItem = values[j + 1];
            while (comparer.LessThan(ref insertingItem, ref values[j]) && --j >= lo)
                ;

            ++j;
            for (int k = i; k >= j; --k)
                values[k + 1] = values[k];
            values[j] = insertingItem;

            InsertionSort(values, ref comparer, i + 1, lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(T[] values, int i, int j)
        {
            ref T valuesI = ref values[i];
            T tmpValue = valuesI;
            ref T valuesJ = ref values[j];
            valuesI = valuesJ;
            valuesJ = tmpValue;
        }
    }
}
