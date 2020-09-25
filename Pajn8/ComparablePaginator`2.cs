using System;

namespace Pajn8
{
    internal sealed class ComparablePaginator<TKey, TValue> : PaginatorBase<TValue>
        where TKey : IComparable<TKey>
    {
        private readonly Paginator<TKey, TValue, ComparableComparer<TKey>> impl;

        internal override int Length => impl.Length;

        public ComparablePaginator(TKey[] keys, TValue[] values)
        {
            impl = new Paginator<TKey, TValue, ComparableComparer<TKey>>(keys, values, default);
        }

        internal override ArraySegment<TValue> GetPageInternal(int start, int end, int pageSize) => impl.GetPageInternal(start, end, pageSize);
    }
}
