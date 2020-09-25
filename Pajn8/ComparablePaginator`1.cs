using System;

namespace Pajn8
{
    internal sealed class ComparablePaginator<T> : PaginatorBase<T>
        where T : IComparable<T>
    {
        private readonly Paginator<T, ComparableComparer<T>> impl;

        internal override int Length => impl.Length;

        public ComparablePaginator(T[] items)
        {
            impl = new Paginator<T, ComparableComparer<T>>(items, default);
        }

        internal override ArraySegment<T> GetPageInternal(int start, int end, int pageSize) => impl.GetPageInternal(start, end, pageSize);
    }
}
