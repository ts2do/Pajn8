using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct ComparableNullCheckedComparer<TKey> : IComparer<TKey>
        where TKey : IComparable<TKey>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare([AllowNull] TKey x, [AllowNull] TKey y)
        {
            int c = 0;
            if (!(x is null))
                c = x.CompareTo(y);
            else if (!(y is null))
                c = -y.CompareTo(x);
            return c;
        }
    }
}