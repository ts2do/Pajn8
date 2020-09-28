using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct ComparableNullUncheckedComparer<TKey> : IComparer<TKey>
        where TKey : IComparable<TKey>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(TKey x, TKey y) => x.CompareTo(y);
    }
}