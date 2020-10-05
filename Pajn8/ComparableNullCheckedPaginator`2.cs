using System;

namespace Pajn8
{
    internal sealed class ComparableNullCheckedPaginator<TKey, TValue> : IPaginator<TValue>
        where TKey : IComparable<TKey>
    {
        private readonly Paginator<TKey, TValue, ComparableNullCheckedComparer<TKey>> impl;

        public ComparableNullCheckedPaginator(TKey[] keys, TValue[] values, int offset, int length)
        {
            impl = new Paginator<TKey, TValue, ComparableNullCheckedComparer<TKey>>(keys, values, offset, length, default);
        }

        public ArraySegment<TValue> GetPage(int offset, int length) => impl.GetPage(offset, length); // let impl do argument checks
        public ArraySegment<TValue> GetPage(Range range) => impl.GetPage(range); // let impl do argument checks
    }
}
