using System;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct NullCheckedComparableStableComparer<T> : IComparer2<Indexed<T>>
        where T : IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref Indexed<T> x, ref Indexed<T> y)
        {
            int c = 0;
            if (!(x.Value is null))
                c = x.Value.CompareTo(y.Value);
            else if (!(y.Value is null))
                c = -y.Value.CompareTo(x.Value);
            bool result = c < 0;
            if (c == 0)
                result = x.Index < y.Index;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref Indexed<T> x, ref Indexed<T> y)
        {
            int c = 0;
            if (!(x.Value is null))
                c = x.Value.CompareTo(y.Value);
            else if (!(y.Value is null))
                c = -y.Value.CompareTo(x.Value);
            bool result = c > 0;
            if (c == 0)
                result = x.Index > y.Index;
            return result;
        }
    }
}
