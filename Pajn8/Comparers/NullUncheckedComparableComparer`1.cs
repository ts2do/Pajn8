using Pajn8.Utils;
using System;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct NullUncheckedComparableComparer<T> : IComparer2<T>
        where T : IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref T x, ref T y) => CompareUtils.LessThanNullUnchecked(ref x, ref y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref T x, ref T y) => CompareUtils.GreaterThanNullUnchecked(ref x, ref y);
    }
}