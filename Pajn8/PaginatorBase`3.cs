using Pajn8.Comparers;
using System;

namespace Pajn8
{
    internal abstract class PaginatorBase<TKey, TValue, TComparer> : IPaginator<TValue>
        where TComparer : IComparer2<TKey>
    {
        protected readonly TValue[] values;
        protected readonly int offset;
        protected readonly int length;
        protected TComparer comparer;
        protected bool faulted;
        protected readonly PartitionNode rootNode;
        protected PartitionNode[] partitionsToSort = new PartitionNode[16];
        protected int numPartitionsToSort = 0;

        public int Count => length;

        protected PaginatorBase(TValue[] values, int offset, int length, in TComparer comparer)
        {
            this.values = values;
            this.offset = offset;
            this.length = length;
            this.comparer = comparer;
            rootNode = new PartitionNode(offset, offset + length, 0);
        }

        public ArraySegment<TValue> GetPage(int offset, int length)
        {
            if ((uint)offset >= (uint)this.length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if ((uint)(offset + length) > (uint)this.length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return GetPageImpl(offset, offset + length, length);
        }

        public ArraySegment<TValue> GetPage(Range range)
        {
            ExtractFromRange(range, out int length, out int start, out int end);

            if ((uint)end > (uint)length || (uint)start > (uint)end)
                throw new ArgumentOutOfRangeException(nameof(range));

            return GetPageImpl(start, end, end - start);

            void ExtractFromRange(Range range, out int length, out int start, out int end)
            {
                length = values.Length;
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

        private ArraySegment<TValue> GetPageImpl(int start, int end, int length)
        {
            if (length == 0)
                return default;

            if (faulted)
                throw new InvalidOperationException(Strings.InvalidOperation_PaginatorFaulted);

            int play = Math.Max(Math.Min(length, SortUtils.PartitionMaximumPlay), SortUtils.PartitionMinimumPlay);

            bool wrapException = true;
            try
            {
                DivideAndSort(start, end, play, ref wrapException);
            }
            catch (Exception ex)
            {
                faulted = true;
                if (wrapException)
                    throw new InvalidOperationException(Strings.InvalidOperation_IComparerFailed, ex);
                throw;
            }

            return new(values, start, length);
        }

        protected abstract void DivideAndSort(int start, int end, int play, ref bool wrapException);
    }
}
