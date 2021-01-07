using Pajn8.Utils;
using System;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct NullableComparableComparer<T> : IComparer2<T?>
        where T : struct, IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref T? x, ref T? y)
            => y is T yValue && (x is not T xValue || CompareUtils.LessThan(ref xValue, ref yValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref T? x, ref T? y)
            => x is T xValue && (y is not T yValue || CompareUtils.GreaterThan(ref xValue, ref yValue));
    }
}