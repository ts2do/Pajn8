using System;

namespace Pajn8
{
    internal sealed class ComparablePaginator<T> : IPaginator<T>
        where T : IComparable<T>
    {
        private readonly Paginator<T, ComparableComparer<T>> impl;

        public ComparablePaginator(T[] items)
        {
            impl = new Paginator<T, ComparableComparer<T>>(items, default);
        }

        public ReadOnlySpan<T> GetPage(int offset, int pageSize)
        {
            if ((uint)offset >= (uint)impl.items.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if ((uint)(offset + pageSize) > (uint)impl.items.Length)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return impl.GetPageInternal(offset, offset + pageSize, pageSize);
        }

        public ReadOnlySpan<T> GetPage(Range range)
        {
            impl.ExtractFromRange(range, out int length, out int start, out int end);

            if ((uint)end > (uint)length || (uint)start > (uint)end)
                throw new ArgumentOutOfRangeException(nameof(range));

            return impl.GetPageInternal(start, end, end - start);
        }
    }
}
