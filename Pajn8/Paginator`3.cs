using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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

        internal override ReadOnlySpan<TValue> GetPageInternal(int start, int end, int pageSize)
        {
            if (pageSize == 0)
                return default;

            DivideAndSort(start, end, pageSize);

            return values.AsSpan(start, pageSize);
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
            Verify(start, end);

            void Verify(int start, int end)
            {
                EqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;
                for (int i = start; i < end; ++i)
                {
                    if (comparer.Compare(keys[i], sortedKeys[i]) != 0)
                        throw new Exception("Verification failed");
                    //if (!valueComparer.Equals(values[i], sortedValues[i]))
                    //    throw new Exception("Verification failed");
                }
            }
#endif
        }

        private static void SwapIfGreater(TKey[] keys, ref TComparer comparer, TValue[] values, int i, int j)
        {
            Debug.Assert(0 <= i && i < keys.Length);
            Debug.Assert(0 <= j && j < keys.Length);

            if (i != j)
            {
                ref TKey keysI = ref keys[i];
                TKey keyI = keysI;
                ref TKey keysJ = ref keys[j];
                TKey keyJ = keysJ;
                if (comparer.Compare(keyI, keyJ) > 0)
                {
                    keysI = keyJ;
                    keysJ = keyI;

                    ref TValue valuesI = ref values[i];
                    ref TValue valuesJ = ref values[j];
                    TValue value = valuesI;
                    valuesI = valuesJ;
                    valuesJ = value;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(TKey[] keys, TValue[] values, int i, int j)
        {
            if (i != j)
            {
                ref TKey keysI = ref keys[i];
                ref TKey keysJ = ref keys[j];
                TKey key = keysI;
                keysI = keysJ;
                keysJ = key;

                ref TValue valuesI = ref values[i];
                ref TValue valuesJ = ref values[j];
                TValue value = valuesI;
                valuesI = valuesJ;
                valuesJ = value;
            }
        }

        public static int PickPivotAndPartition(TKey[] keys, ref TComparer comparer, TValue[] values, int lo, int hi)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(keys.Length > 0);

            int mid = lo + ((hi - lo) / 2);

            SwapIfGreater(keys, ref comparer, values, lo, mid);
            SwapIfGreater(keys, ref comparer, values, lo, hi);
            SwapIfGreater(keys, ref comparer, values, mid, hi);

            TKey pivot = keys[mid];
            Swap(keys, values, mid, hi - 1);
            int left = lo, right = hi - 1;

            while (true)
            {
                while (comparer.Compare(keys[++left], pivot) < 0)
                    if (left == hi)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                while (comparer.Compare(pivot, keys[--right]) < 0)
                    if (right == lo)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));

                if (left >= right)
                    break;

                Swap(keys, values, left, right);
            }

            Swap(keys, values, left, hi - 1);
            return left;
        }
    }
}
