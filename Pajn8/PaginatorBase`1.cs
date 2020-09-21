using System;

namespace Pajn8
{
    internal abstract class PaginatorBase<T> : IPaginator<T>
    {
        internal abstract int Length { get; }
        internal abstract ReadOnlySpan<T> GetPageInternal(int start, int end, int pageSize);

        public ReadOnlySpan<T> GetPage(int offset, int pageSize)
        {
            int length = Length;
            if ((uint)offset >= (uint)length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if ((uint)(offset + pageSize) > (uint)length)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return GetPageInternal(offset, offset + pageSize, pageSize);
        }

        public ReadOnlySpan<T> GetPage(Range range)
        {
            ExtractFromRange(range, out int length, out int start, out int end);

            if ((uint)end > (uint)length || (uint)start > (uint)end)
                throw new ArgumentOutOfRangeException(nameof(range));

            return GetPageInternal(start, end, end - start);
        }

        internal void ExtractFromRange(Range range, out int length, out int start, out int end)
        {
            length = Length;
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
}
