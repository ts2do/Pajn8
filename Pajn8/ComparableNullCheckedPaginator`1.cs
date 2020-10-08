using System;

namespace Pajn8
{
    internal sealed class ComparableNullCheckedPaginator<T> : IPaginator<T>
        where T : IComparable<T>
    {
        private readonly Paginator<T, ComparableNullCheckedComparer<T>> impl;

        public int Count => impl.Count;

        public ComparableNullCheckedPaginator(T[] items, int offset, int length)
        {
            impl = new Paginator<T, ComparableNullCheckedComparer<T>>(items, offset, length, default);
        }

        public ArraySegment<T> GetPage(int offset, int length) => impl.GetPage(offset, length); // let impl do argument checks
        public ArraySegment<T> GetPage(Range range) => impl.GetPage(range); // let impl do argument checks
    }
}
