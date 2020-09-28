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
        public int Compare([AllowNull] TKey x, [AllowNull] TKey y) => x?.CompareTo(y) ?? (y is null ? 0 : -1);
    }
}