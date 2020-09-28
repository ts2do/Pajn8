using System;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct StableNullUncheckedComparable<T> : IIndexed<T>, IComparable<StableNullUncheckedComparable<T>>
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
        public int CompareTo(StableNullUncheckedComparable<T> other)
        {
            int c = value.CompareTo(other.value);
            if (c == 0)
                c = index - other.index;
            return c;
        }
    }
}
