using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static void SwapIfGreater(T[] items, ref TComparer comparer, int i, int j)
        {
            Debug.Assert(0 <= i && i < items.Length);
            Debug.Assert(0 <= j && j < items.Length);

            if (i != j)
            {
                ref T itemsI = ref items[i];
                T itemI = itemsI;
                ref T itemsJ = ref items[j];
                T itemJ = itemsJ;
                if (comparer.Compare(itemI, itemJ) > 0)
                {
                    itemsI = itemJ;
                    itemsJ = itemI;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(T[] items, int i, int j)
        {
            Debug.Assert(0 <= i && i < items.Length);
            Debug.Assert(0 <= j && j < items.Length);

            if (i != j)
            {
                ref T itemsI = ref items[i];
                ref T itemsJ = ref items[j];
                T tmp = itemsI;
                itemsI = itemsJ;
                itemsJ = tmp;
            }
        }

        public static int PickPivotAndPartition(T[] items, ref TComparer comparer, int lo, int hi)
        {
            int mid = lo + ((hi - lo) / 2);

            SwapIfGreater(items, ref comparer, lo, mid);
            SwapIfGreater(items, ref comparer, lo, hi);
            SwapIfGreater(items, ref comparer, mid, hi);

            T pivot = items[mid];
            Swap(items, mid, hi - 1);
            int left = lo, right = hi - 1;

            while (true)
            {
                while (comparer.Compare(items[++left], pivot) < 0)
                    if (left == hi)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));
                while (comparer.Compare(pivot, items[--right]) < 0)
                    if (right == lo)
                        throw new ArgumentException(string.Format(Strings.Arg_BogusIComparer, comparer));

                if (left >= right)
                    break;

                Swap(items, left, right);
            }

            Swap(items, left, hi - 1);
            return left;
        }
    }
}
