using Pajn8.Utils;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct NullableComparableComparer<T> : IComparer2<T?>
        where T : struct, IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan([AllowNull] ref T? x, [AllowNull] ref T? y)
            => y is T yValue && (!(x is T xValue) || CompareUtils.LessThanNullUnchecked(ref xValue, ref yValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan([AllowNull] ref T? x, [AllowNull] ref T? y)
            => x is T xValue && (!(y is T yValue) || CompareUtils.GreaterThanNullUnchecked(ref xValue, ref yValue));
    }
}