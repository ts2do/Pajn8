using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct StableNullCheckedComparableComparer<T> : IComparer<Indexed<T>>
        where T : IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(Indexed<T> x, Indexed<T> y)
        {
            int c = 0;
            if (!(x.Value is null))
                c = x.Value.CompareTo(y.Value);
            else if (!(y.Value is null))
                c = y.Value.CompareTo(x.Value);
            if (c == 0)
                c = x.Index - y.Index;
            return c;
        }
    }
}
