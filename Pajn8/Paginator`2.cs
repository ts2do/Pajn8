﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Pajn8
{
    internal sealed class Paginator<T, TComparer> : PaginatorBase<T>
        where TComparer : IComparer<T>
    {
#if DEBUG
        private readonly T[] sortedItems;
#endif
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain mutable value types")]
        private TComparer comparer;
        private readonly IComparer<T> boxedComparer;
        private readonly PartitionNode rootNode;

        internal Paginator(T[] items, int offset, int length, TComparer comparer)
            : base(items, offset, length)
        {
#if DEBUG
            sortedItems = (T[])items.Clone();
            Array.Sort(sortedItems, offset, length, comparer);
#endif
            this.comparer = comparer;
            boxedComparer = comparer;
            rootNode = new PartitionNode(offset, offset + length);
        }

        protected override void DivideAndSort(int start, int end, int length)
        {
            PartitionNode p;
            for (int position = start; position < end; position = p.EndIndex)
            {
                p = rootNode.Find(position);
                if (!p.IsSorted)
                {
                    while (end < p.EndIndex - length || start - length > p.StartIndex)
                    {
                        int k = PickPivotAndPartition(items, ref comparer, p.StartIndex, p.EndIndex - 1);
#if DEBUG
                        T pivot = items[k];
                        T[] lowerItems = items[p.StartIndex..k];
                        T[] upperKeys = items[(k + 1)..p.EndIndex];
                        Array.Sort(lowerItems, comparer);
                        Array.Sort(upperKeys, comparer);
                        foreach (T x in lowerItems)
                            Debug.Assert(comparer.Compare(x, pivot) <= 0);
                        foreach (T x in upperKeys)
                            Debug.Assert(comparer.Compare(x, pivot) >= 0);
                        Debug.Assert(comparer.Compare(pivot, sortedItems[k]) == 0);
#endif
                        p.Split(k);
                        p = k > position ? p.LeftNode : p.RightNode;
                    }

                    Array.Sort(items, p.StartIndex, p.Count, boxedComparer);
                    p.IsSorted = true;
                }
            }

#if DEBUG
            for (int i = start; i < end; ++i)
                if (comparer.Compare(items[i], sortedItems[i]) != 0)
                    throw new Exception("Verification failed");
#endif
        }

        private static int PickPivotAndPartition(T[] items, ref TComparer comparer, int first, int last)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(first <= last);

            int left = first, right = last - 1;
            int count = last - first;
            int mid = first + (count / 2);
            if (count > 40)
            {
                // John Tukey's ninther
                int step = (count + 1) / 8;
                MedianOfThree(items, ref comparer, first + step, first, first + step * 2);
                MedianOfThree(items, ref comparer, mid - step, mid, mid + step);
                MedianOfThree(items, ref comparer, last - step * 2, last, last - step);
            }
            MedianOfThree(items, ref comparer, first, mid, last);

            T pivot;
            {
                // Move (mid) to (last - 1)
                ref T key1 = ref items[mid];
                pivot = key1;
                ref T key2 = ref items[last - 1];
                key1 = key2;
                key2 = pivot;
            }

            while (left < right)
            {
                while (comparer.Compare(items[++left], pivot) < 0)
                    if (left == last)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                while (comparer.Compare(pivot, items[--right]) < 0)
                    if (right == first)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));

                if (left < right)
                {
                    // Swap (left) and (right)
                    ref T item1 = ref items[left];
                    T tmpItem = item1;
                    ref T item2 = ref items[right];
                    item1 = item2;
                    item2 = pivot;
                }
            }

            {
                // Move pivot to destination
                ref T item1 = ref items[last - 1];
                ref T item2 = ref items[left];
                item1 = item2;
                item2 = pivot;
            }

            return left;
        }

        private static void MedianOfThree(T[] items, ref TComparer comparer, int first, int mid, int last)
        {
            ref T firstItem = ref items[first];
            ref T midItem = ref items[mid];
            ref T lastItem = ref items[last];

            T tmpItem;

            if (comparer.Compare(firstItem, midItem) > 0)
            {
                // Swap (first) and (mid)
                tmpItem = firstItem;
                firstItem = midItem;
                midItem = tmpItem;
            }

            if (comparer.Compare(midItem, lastItem) > 0)
            {
                {
                    // Swap (mid) and (last)
                    tmpItem = midItem;
                    midItem = lastItem;
                    lastItem = tmpItem;
                }

                if (comparer.Compare(firstItem, midItem) > 0)
                {
                    // Swap (first) and (mid)
                    tmpItem = firstItem;
                    firstItem = midItem;
                    midItem = tmpItem;
                }
            }
        }
    }
}
