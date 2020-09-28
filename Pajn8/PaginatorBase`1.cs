using System;

namespace Pajn8
{
    internal abstract class PaginatorBase<T> : IPaginator<T>
    {
        protected readonly T[] items;

        protected PaginatorBase(T[] items)
        {
            this.items = items;
        }

        public ArraySegment<T> GetPage(int offset, int pageSize)
        {
            int length = items.Length;
            if ((uint)offset >= (uint)length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if ((uint)(offset + pageSize) > (uint)length)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return GetPageImpl(offset, offset + pageSize, pageSize);
        }

        public ArraySegment<T> GetPage(Range range)
        {
            ExtractFromRange(range, out int length, out int start, out int end);

            if ((uint)end > (uint)length || (uint)start > (uint)end)
                throw new ArgumentOutOfRangeException(nameof(range));

            return GetPageImpl(start, end, end - start);

            void ExtractFromRange(Range range, out int length, out int start, out int end)
            {
                length = items.Length;
                Index startIndex = range.Start;
                start = startIndex.Value;
                if (startIndex.IsFromEnd)
                    start = length - start;

                Index endIndex = range.End;
                end = endIndex.Value;
                if (endIndex.IsFromEnd)
                    end = length - end;
            }
        }

        protected abstract void DivideAndSort(int start, int end, int pageSize);

        private ArraySegment<T> GetPageImpl(int start, int end, int pageSize)
        {
            if (pageSize == 0)
                return default;

            DivideAndSort(start, end, pageSize);

            return new ArraySegment<T>(items, start, pageSize);
        }
    }
}
