using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
            ref T itemsHead = ref items[0];
            for (int position = start; position < end;)
            {
                PartitionNode p = rootNode.Find(position);
                if (p.IsSorted)
                {
                    position = p.EndIndex;
                    continue;
                }

                ref T itemsX = ref Unsafe.Add(ref itemsHead, p.StartIndex);

                if (end >= p.EndIndex - pageSize && start - pageSize <= p.StartIndex)
                {
                    //Array.Sort(keys, values, p.Start, p.Count, comparer);
                    Sorting<T, TComparer>.Sort(ref itemsX, ref comparer, p.Count);
                    p.IsSorted = true;
                    position = p.EndIndex;
                }
                else
                {
                    int k = Sorting<T, TComparer>.PickPivotAndPartition(ref itemsX, ref comparer, p.Count) + p.StartIndex;
                    p.Split(k);
                }
            }

#if DEBUG
            Verify(start, end);

            void Verify(int start, int end)
            {
                for (int i = start; i < end; ++i)
                {
                    if (comparer.Compare(items[i], sortedItems[i]) != 0)
                        throw new Exception("Verification failed");
                    //if (!valueComparer.Equals(values[i], sortedValues[i]))
                    //    throw new Exception("Verification failed");
                }
            }
#endif
        }
    }
}
