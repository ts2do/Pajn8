using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Pajn8
{
    internal sealed class Paginator<TKey, TValue, TComparer> : PaginatorBase<TValue>
        where TComparer : IComparer<TKey>
    {
        private readonly TKey[] keys;
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain mutable value types")]
        private TComparer comparer;
        private bool faulted;
        private readonly IComparer<TKey> boxedComparer;
        private readonly PartitionNode rootNode;

        internal Paginator(TKey[] keys, TValue[] values, int offset, int length, TComparer comparer)
            : base(values, offset, length)
        {
            this.keys = keys;
            this.comparer = comparer;
            boxedComparer = comparer;
            rootNode = new PartitionNode(offset, offset + length);
        }

        protected override void DivideAndSort(int start, int end, int length)
        {
            if (faulted)
                throw new InvalidOperationException(Strings.InvalidOperation_PaginatorFaulted);

            try
            {
                PartitionNode p;
                for (int position = start; position < end; position = p.EndIndex)
                {
                    p = rootNode.Find(position);
                    if (!p.IsSorted)
                    {
                        while (end < p.EndIndex - length || start - length > p.StartIndex)
                        {
                            int k = PickPivotAndPartition(keys, items, ref comparer, p.StartIndex, p.EndIndex - 1);
                            p.Split(k);
                            p = k > position ? p.LeftNode : p.RightNode;
                        }

                        Array.Sort(keys, items, p.StartIndex, p.Count, boxedComparer);
                        p.IsSorted = true;
                    }
                }
            }
            catch
            {
                faulted = true;
                throw;
            }
        }

        private static int PickPivotAndPartition(TKey[] keys, TValue[] items, ref TComparer comparer, int first, int last)
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
                MedianOfThree(keys, items, ref comparer, first + step, first, first + step * 2);
                MedianOfThree(keys, items, ref comparer, mid - step, mid, mid + step);
                MedianOfThree(keys, items, ref comparer, last - step * 2, last, last - step);
            }
            MedianOfThree(keys, items, ref comparer, first, mid, last);

            TKey pivot;
            {
                // Move (mid) to (last - 1)
                ref TKey key1 = ref keys[mid];
                pivot = key1;
                ref TKey key2 = ref keys[last - 1];
                key1 = key2;
                key2 = pivot;

                ref TValue value1 = ref items[mid];
                TValue tmpValue = value1;
                ref TValue value2 = ref items[last - 1];
                value1 = value2;
                value2 = tmpValue;
            }

            while (left < right)
            {
                while (comparer.Compare(keys[++left], pivot) < 0)
                    if (left == last)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                while (comparer.Compare(keys[--right], pivot) > 0)
                    if (right == first)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));

                if (left < right)
                {
                    // Swap (left) and (right)
                    ref TKey key1 = ref keys[left];
                    TKey tmpKey = key1;
                    ref TKey key2 = ref keys[right];
                    key1 = key2;
                    key2 = tmpKey;

                    ref TValue value1 = ref items[left];
                    TValue tmpValue = value1;
                    ref TValue value2 = ref items[right];
                    value1 = value2;
                    value2 = tmpValue;
                }
            }

            {
                // Move pivot to destination
                ref TKey key1 = ref keys[last - 1];
                ref TKey key2 = ref keys[left];
                key1 = key2;
                key2 = pivot;

                ref TValue value1 = ref items[last - 1];
                TValue tmpValue = value1;
                ref TValue value2 = ref items[left];
                value1 = value2;
                value2 = tmpValue;
            }

            return left;
        }

        private static void MedianOfThree(TKey[] keys, TValue[] items, ref TComparer comparer, int first, int mid, int last)
        {
            ref TKey firstKey = ref keys[first];
            ref TKey midKey = ref keys[mid];
            ref TKey lastKey = ref keys[last];

            ref TValue firstValue = ref items[first];
            ref TValue midValue = ref items[mid];

            TKey tmpKey;
            TValue tmpValue;

            if (comparer.Compare(firstKey, midKey) > 0)
            {
                // Swap (first) and (mid)
                tmpKey = firstKey;
                firstKey = midKey;
                midKey = tmpKey;

                tmpValue = firstValue;
                firstValue = midValue;
                midValue = tmpValue;
            }

            if (comparer.Compare(midKey, lastKey) > 0)
            {
                {
                    // Swap (mid) and (last)
                    tmpKey = midKey;
                    midKey = lastKey;
                    lastKey = tmpKey;

                    ref TValue lastValue = ref items[last];
                    tmpValue = midValue;
                    midValue = lastValue;
                    lastValue = tmpValue;
                }

                if (comparer.Compare(firstKey, midKey) > 0)
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
    }
}
