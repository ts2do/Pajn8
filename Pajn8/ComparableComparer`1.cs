using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pajn8
{
    internal struct ComparableComparer<TKey> : IComparer<TKey>
        where TKey : IComparable<TKey>
    {
        public int Compare([AllowNull] TKey x, [AllowNull] TKey y) => x?.CompareTo(y) ?? (y is null ? 0 : -1);
    }
}