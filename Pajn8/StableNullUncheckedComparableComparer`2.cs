using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct StableNullUncheckedComparableComparer<T> : IComparer<Indexed<T>>
        where T : IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(Indexed<T> x, Indexed<T> y)
        {
            int c = x.Value.CompareTo(y.Value);
            if (c == 0)
                c = x.Index - y.Index;
            return c;
        }
    }
}
