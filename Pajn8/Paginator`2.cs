using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal sealed class Paginator<T, TComparer> : IPaginator<T>
        where TComparer : IComparer<T>
    {
#if DEBUG
        private readonly T[] sortedItems;
#endif
        internal readonly T[] items;
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain value types")]
        private TComparer comparer;
        private PartSet partSet;

        internal Paginator(T[] items, TComparer comparer)
        {
#if DEBUG
            sortedItems = items[..];
            Array.Sort(sortedItems, comparer);
#endif
            this.items = items;
            this.comparer = comparer;
            partSet.Init(items.Length);
        }

        public ReadOnlySpan<T> GetPage(int offset, int pageSize)
        {
            if ((uint)offset >= (uint)items.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if ((uint)(offset + pageSize) > (uint)items.Length)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return GetPageInternal(offset, offset + pageSize, pageSize);
        }

        public ReadOnlySpan<T> GetPage(Range range)
        {
            ExtractFromRange(range, out int length, out int start, out int end);

            if ((uint)end > (uint)length || (uint)start > (uint)end)
                throw new ArgumentOutOfRangeException(nameof(range));

            return GetPageInternal(start, end, end - start);
        }

        internal void ExtractFromRange(Range range, out int length, out int start, out int end)
        {
            length = items.Length;
            Index startIndex = range.Start;
            start = startIndex.Value;
            if (startIndex.IsFromEnd)
                start = length - start;

            Index endIndex = range.End;
            end = endIndex.Value;
            if (endIndex.IsFromEnd)
                end = length - end;
        }

        internal ReadOnlySpan<T> GetPageInternal(int start, int end, int pageSize)
        {
            if (pageSize == 0)
                return default;

            if (!IsSorted(start, end))
                DivideAndSort(start, end, pageSize);

            return items.AsSpan(start, pageSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsSorted(int start, int end)
        {
            for (int position = start; position < end; )
            {
                ref Part p = ref partSet[position];
                if (!p.IsSorted)
                    return false;

                position = p.End;
            }

            return true;
        }

        private void DivideAndSort(int start, int end, int pageSize)
        {
            ref T itemsHead = ref items[0];
            for (int position = start; position < end; )
            {
                ref Part p = ref partSet[position];
                if (p.IsSorted)
                {
                    position = p.End;
                    continue;
                }

                ref T itemsX = ref Unsafe.Add(ref itemsHead, p.Start);

                if (end >= p.End - pageSize && start - pageSize <= p.Start)
                {
                    //Array.Sort(keys, values, p.Start, p.Count, comparer);
                    Sorting<T, TComparer>.Sort(ref itemsX, ref comparer, p.Count);
                    p.IsSorted = true;
                    position = p.End;
                }
                else
                {
                    int k = Sorting<T, TComparer>.PickPivotAndPartition(ref itemsX, ref comparer, p.Count) + p.Start;
                    partSet.Split(ref p, k);
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
