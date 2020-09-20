using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal sealed class Paginator<TKey, TValue, TComparer> : IPaginator<TValue>
        where TComparer : IComparer<TKey>
    {
#if DEBUG
        private readonly TKey[] sortedKeys;
        private readonly TValue[] sortedValues;
#endif
        internal readonly TKey[] keys;
        private readonly TValue[] values;
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain value types")]
        private TComparer comparer;
        private PartSet partSet;

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
            partSet.Init(keys.Length);
        }

        public ReadOnlySpan<TValue> GetPage(int offset, int pageSize)
        {
            if ((uint)offset >= (uint)keys.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if ((uint)(offset + pageSize) > (uint)keys.Length)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return GetPageInternal(offset, offset + pageSize, pageSize);
        }

        public ReadOnlySpan<TValue> GetPage(Range range)
        {
            ExtractFromRange(range, out int length, out int start, out int end);

            if ((uint)end > (uint)length || (uint)start > (uint)end)
                throw new ArgumentOutOfRangeException(nameof(range));

            return GetPageInternal(start, end, end - start);
        }

        internal void ExtractFromRange(Range range, out int length, out int start, out int end)
        {
            length = keys.Length;
            Index startIndex = range.Start;
            start = startIndex.Value;
            if (startIndex.IsFromEnd)
                start = length - start;

            Index endIndex = range.End;
            end = endIndex.Value;
            if (endIndex.IsFromEnd)
                end = length - end;
        }

        internal ReadOnlySpan<TValue> GetPageInternal(int start, int end, int pageSize)
        {
            if (pageSize == 0)
                return default;

            if (!IsSorted(start, end))
                DivideAndSort(start, end, pageSize);

            return values.AsSpan(start, pageSize);
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
            ref TKey keysHead = ref keys[0];
            ref TValue valuesHead = ref values[0];
            for (int position = start; position < end; )
            {
                ref Part p = ref partSet[position];
                if (p.IsSorted)
                {
                    position = p.End;
                    continue;
                }

                ref TKey keysX = ref Unsafe.Add(ref keysHead, p.Start);
                ref TValue valuesX = ref Unsafe.Add(ref valuesHead, p.Start);

                if (end >= p.End - pageSize && start - pageSize <= p.Start)
                {
                    //Array.Sort(keys, values, p.Start, p.Count, comparer);
                    Sorting<TKey, TValue, TComparer>.Sort(ref keysX, ref comparer, ref valuesX, p.Count);
                    p.IsSorted = true;
                    position = p.End;
                }
                else
                {
                    int k = Sorting<TKey, TValue, TComparer>.PickPivotAndPartition(ref keysX, ref comparer, ref valuesX, p.Count) + p.Start;
                    partSet.Split(ref p, k);
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
