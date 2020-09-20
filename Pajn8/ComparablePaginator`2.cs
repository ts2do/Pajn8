using System;

namespace Pajn8
{
    internal sealed class ComparablePaginator<TKey, TValue> : IPaginator<TValue>
        where TKey : IComparable<TKey>
    {
        private readonly Paginator<TKey, TValue, ComparableComparer<TKey>> impl;

        public ComparablePaginator(TKey[] keys, TValue[] values)
        {
            impl = new Paginator<TKey, TValue, ComparableComparer<TKey>>(keys, values, default);
        }

        public ReadOnlySpan<TValue> GetPage(int offset, int pageSize)
        {
            if ((uint)offset >= (uint)impl.keys.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if ((uint)(offset + pageSize) > (uint)impl.keys.Length)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return impl.GetPageInternal(offset, offset + pageSize, pageSize);
        }

        public ReadOnlySpan<TValue> GetPage(Range range)
        {
            impl.ExtractFromRange(range, out int length, out int start, out int end);

            if ((uint)end > (uint)length || (uint)start > (uint)end)
                throw new ArgumentOutOfRangeException(nameof(range));

            return impl.GetPageInternal(start, end, end - start);
        }
    }
}
