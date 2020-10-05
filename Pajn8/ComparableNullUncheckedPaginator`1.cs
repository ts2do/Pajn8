using System;

namespace Pajn8
{
    internal sealed class ComparableNullUncheckedPaginator<T> : IPaginator<T>
        where T : IComparable<T>
    {
        private readonly Paginator<T, ComparableNullUncheckedComparer<T>> impl;

        public ComparableNullUncheckedPaginator(T[] items, int offset, int length)
        {
            impl = new Paginator<T, ComparableNullUncheckedComparer<T>>(items, offset, length, default);
        }

        public ArraySegment<T> GetPage(int offset, int length) => impl.GetPage(offset, length); // let impl do argument checks
        public ArraySegment<T> GetPage(Range range) => impl.GetPage(range); // let impl do argument checks
    }
}
