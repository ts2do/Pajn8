using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct NullCheckedComparableComparer<T> : IComparer2<T>
        where T : IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan([AllowNull] ref T x, [AllowNull] ref T y)
        {
            if (!(x is null))
                return x.CompareTo(y) < 0 ? true : false;
            if (!(y is null))
                return y.CompareTo(x) > 0 ? true : false;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan([AllowNull] ref T x, [AllowNull] ref T y)
        {
            if (!(x is null))
                return x.CompareTo(y) > 0 ? true : false;
            if (!(y is null))
                return y.CompareTo(x) < 0 ? true : false;
            return false;
        }
    }
}