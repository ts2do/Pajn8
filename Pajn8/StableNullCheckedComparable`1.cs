using System;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct StableNullCheckedComparable<T> : IIndexed<T>, IComparable<StableNullCheckedComparable<T>>
        where T : IComparable<T>
    {
        private T value;
        private int index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(T value, int index)
        {
            this.value = value;
            this.index = index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(StableNullCheckedComparable<T> other)
        {
            int c;
            if (value is null)
            {
                c = -1;
                if (other.value is null)
                    c = 0;
            }
            else
            {
                c = value.CompareTo(other.value);
            }
            if (c == 0)
                c = index - other.index;
            return c;
        }
    }
}
