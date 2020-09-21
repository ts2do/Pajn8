using System;
using System.Collections.Generic;
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
            ref TKey keysHead = ref keys[0];
            ref TValue valuesHead = ref values[0];
            for (int position = start; position < end;)
            {
                PartitionNode p = rootNode.Find(position);
                if (p.IsSorted)
                {
                    position = p.EndIndex;
                    continue;
                }

                ref TKey keysX = ref Unsafe.Add(ref keysHead, p.StartIndex);
                ref TValue valuesX = ref Unsafe.Add(ref valuesHead, p.StartIndex);

                if (end >= p.EndIndex - pageSize && start - pageSize <= p.StartIndex)
                {
                    //Array.Sort(keys, values, p.Start, p.Count, comparer);
                    Sorting<TKey, TValue, TComparer>.Sort(ref keysX, ref comparer, ref valuesX, p.Count);
                    p.IsSorted = true;
                    position = p.EndIndex;
                }
                else
                {
                    int k = Sorting<TKey, TValue, TComparer>.PickPivotAndPartition(ref keysX, ref comparer, ref valuesX, p.Count) + p.StartIndex;
                    p.Split(k);
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
    }
}
