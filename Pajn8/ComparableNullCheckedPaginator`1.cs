using System;

namespace Pajn8
{
    internal sealed class ComparableNullCheckedPaginator<T> : IPaginator<T>
        where T : IComparable<T>
    {
        private readonly Paginator<T, ComparableNullCheckedComparer<T>> impl;

        public ComparableNullCheckedPaginator(T[] items)
        {
            impl = new Paginator<T, ComparableNullCheckedComparer<T>>(items, default);
        }

        public ArraySegment<T> GetPage(int offset, int pageSize) => impl.GetPage(offset, pageSize); // let impl do argument checks
        public ArraySegment<T> GetPage(Range range) => impl.GetPage(range); // let impl do argument checks
    }
}
