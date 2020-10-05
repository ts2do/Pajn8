using System;

namespace Pajn8
{
    internal abstract class PaginatorBase<T> : IPaginator<T>
    {
        protected readonly T[] items;
        protected readonly int offset;
        protected readonly int length;

        protected PaginatorBase(T[] items, int offset, int length)
        {
            this.items = items;
            this.offset = offset;
            this.length = length;
        }

        public ArraySegment<T> GetPage(int offset, int length)
        {
            if ((uint)offset >= (uint)this.length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if ((uint)(offset + length) > (uint)this.length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return GetPageImpl(offset, offset + length, length);
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

        protected abstract void DivideAndSort(int start, int end, int length);

        private ArraySegment<T> GetPageImpl(int start, int end, int length)
        {
            if (length == 0)
                return default;

            DivideAndSort(start, end, length);

            return new ArraySegment<T>(items, start, length);
        }
    }
}
