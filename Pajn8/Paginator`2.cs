using System;
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
        private readonly T[] items;
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain value types")]
        private TComparer comparer;
        private readonly PartitionNode rootNode;

        internal override int Length => items.Length;

        internal Paginator(T[] items, TComparer comparer)
        {
#if DEBUG
            sortedItems = items[..];
            Array.Sort(sortedItems, comparer);
#endif
            this.items = items;
            this.comparer = comparer;
            rootNode = new PartitionNode(0, items.Length);
        }

        internal override ReadOnlySpan<T> GetPageInternal(int start, int end, int pageSize)
        {
            if (pageSize == 0)
                return default;

            DivideAndSort(start, end, pageSize);

            return items.AsSpan(start, pageSize);
        }

        private void DivideAndSort(int start, int end, int pageSize)
        {
            PartitionNode p;
            for (int position = start; position < end; position = p.EndIndex)
            {
                p = rootNode.Find(position);
                if (!p.IsSorted)
                {
                    while (end < p.EndIndex - pageSize || start - pageSize > p.StartIndex)
                    {
                        int k = PickPivotAndPartition(items, ref comparer, p.StartIndex, p.EndIndex - 1);
                        p.Split(k);
                        p = k >= position ? p.LeftNode : p.RightNode;
                    }

                    Array.Sort(items, p.StartIndex, p.Count, comparer);
                    p.IsSorted = true;
                }
            }

#if DEBUG
            for (int i = start; i < end; ++i)
                if (comparer.Compare(items[i], sortedItems[i]) != 0)
                    throw new Exception("Verification failed");
#endif
        }

        private int PickPivotAndPartition(T[] items, ref TComparer comparer, int first, int last)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(items.Length > 0);

            int left = first, right = last - 1;
            int count = last - first;
            int mid = first + (count / 2);
            T pivot = MedianOfThree(items, ref comparer, first, mid, last);

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
                // Swap (left) and (last - 1)
                ref T item1 = ref items[left];
                T tmpItem = item1;
                ref T item2 = ref items[last - 1];
                item1 = item2;
                item2 = pivot;
            }
#if DEBUG
            for (int i = first; i < left; ++i)
                Debug.Assert(comparer.Compare(items[i], pivot) <= 0);
            for (int i = left + 1; i <= last; ++i)
                Debug.Assert(comparer.Compare(items[i], pivot) >= 0);
            Debug.Assert(comparer.Compare(pivot, sortedItems[left]) == 0);
#endif
            return left;
        }

        private static T MedianOfThree(T[] items, ref TComparer comparer, int first, int mid, int last)
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

            {
                // Move (mid) to (last - 1)
                ref T item1 = ref midItem;
                T pivot = item1;
                ref T item2 = ref items[last - 1];
                item1 = item2;
                item2 = pivot;

                return pivot;
            }
        }
    }
}
