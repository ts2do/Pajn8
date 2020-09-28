using System;

namespace Pajn8
{
    internal sealed class ComparableNullUncheckedPaginator<TKey, TValue> : IPaginator<TValue>
        where TKey : IComparable<TKey>
    {
        private readonly Paginator<TKey, TValue, ComparableNullUncheckedComparer<TKey>> impl;

        public ComparableNullUncheckedPaginator(TKey[] keys, TValue[] values)
        {
            impl = new Paginator<TKey, TValue, ComparableNullUncheckedComparer<TKey>>(keys, values, default);
        }

        public ArraySegment<TValue> GetPage(int offset, int pageSize) => impl.GetPage(offset, pageSize); // let impl do argument checks
        public ArraySegment<TValue> GetPage(Range range) => impl.GetPage(range); // let impl do argument checks
    }
}
