using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Pajn8
{
    internal sealed class Paginator<TKey, TValue, TComparer> : PaginatorBase<TValue>
        where TComparer : IComparer<TKey>
    {
#if DEBUG
        private readonly TKey[] sortedKeys;
        private readonly TValue[] sortedValues;
#endif
        private readonly TKey[] keys;
        private readonly TValue[] values;
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain value types")]
        private TComparer comparer;
        private readonly PartitionNode rootNode;

        internal override int Length => keys.Length;

        internal Paginator(TKey[] keys, TValue[] values, TComparer comparer)
        {
#if DEBUG
            sortedKeys = keys[..];
            sortedValues = values[..];
            Array.Sort(sortedKeys, sortedValues, comparer);
#endif
            this.keys = keys;
            this.values = values;
            this.comparer = comparer;
            rootNode = new PartitionNode(0, keys.Length);
        }

        internal override ArraySegment<TValue> GetPageInternal(int start, int end, int pageSize)
        {
            if (pageSize == 0)
                return default;

            DivideAndSort(start, end, pageSize);

            return new ArraySegment<TValue>(values, start, pageSize);
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
                        int k = PickPivotAndPartition(keys, ref comparer, values, p.StartIndex, p.EndIndex - 1);
                        p.Split(k);
                        p = k >= position ? p.LeftNode : p.RightNode;
                    }

                    Array.Sort(keys, values, p.StartIndex, p.Count, comparer);
                    p.IsSorted = true;
                }
            }

#if DEBUG
            for (int i = start; i < end; ++i)
                if (comparer.Compare(keys[i], sortedKeys[i]) != 0)
                    throw new Exception("Verification failed");
#endif
        }

        private int PickPivotAndPartition(TKey[] keys, ref TComparer comparer, TValue[] values, int first, int last)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(keys.Length > 0);

            int left = first, right = last - 1;
            int count = last - first;
            int mid = first + (count / 2);
            if (count > 40)
            {
                // John Tukey's ninther
                int step = (count + 1) / 8;
                MedianOfThree(keys, ref comparer, values, first + step, first, first + step * 2);
                MedianOfThree(keys, ref comparer, values, mid - step, mid, mid + step);
                MedianOfThree(keys, ref comparer, values, last - step * 2, last, last - step);
            }
            MedianOfThree(keys, ref comparer, values, first, mid, last);

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

            while (left < right)
            {
                while (comparer.Compare(keys[++left], pivot) < 0)
                    if (left == last)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                while (comparer.Compare(pivot, keys[--right]) < 0)
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

                    ref TValue value1 = ref values[left];
                    TValue tmpValue = value1;
                    ref TValue value2 = ref values[right];
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

                ref TValue value1 = ref values[last - 1];
                TValue tmpValue = value1;
                ref TValue value2 = ref values[left];
                value1 = value2;
                value2 = tmpValue;
            }

#if DEBUG
            TKey[] lowerKeys = keys[first..left];
            TKey[] upperKeys = keys[left..(last + 1)];
            Array.Sort(lowerKeys, comparer);
            Array.Sort(upperKeys, comparer);
            foreach (TKey x in lowerKeys)
                Debug.Assert(comparer.Compare(x, pivot) <= 0);
            foreach (TKey x in upperKeys)
                Debug.Assert(comparer.Compare(x, pivot) >= 0);
            Debug.Assert(comparer.Compare(pivot, sortedKeys[left]) == 0);
#endif

            return left;
        }

        private static void MedianOfThree(TKey[] keys, ref TComparer comparer, TValue[] values, int first, int mid, int last)
        {
            ref TKey firstKey = ref keys[first];
            ref TKey midKey = ref keys[mid];
            ref TKey lastKey = ref keys[last];

            ref TValue firstValue = ref values[first];
            ref TValue midValue = ref values[mid];

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

                    ref TValue lastValue = ref values[last];
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
