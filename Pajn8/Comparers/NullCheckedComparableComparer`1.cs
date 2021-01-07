using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    [SuppressMessage("Style", "IDE0075:Simplify conditional expression", Justification = "Better codegen")]
    internal struct NullCheckedComparableComparer<T> : IComparer2<T>
        where T : IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref T x, ref T y)
        {
            if (x is not null)
                return x.CompareTo(y) < 0 ? true : false;
            if (y is not null)
                return y.CompareTo(x) > 0 ? true : false;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref T x, ref T y)
        {
            if (x is not null)
                return x.CompareTo(y) > 0 ? true : false;
            if (y is not null)
                return y.CompareTo(x) < 0 ? true : false;
            return false;
        }
    }
}