using Pajn8.Utils;
using System;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct NullUncheckedComparableStableComparer<T> : IComparer2<Indexed<T>>
        where T : IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref Indexed<T> x, ref Indexed<T> y)
        {
            int c = CompareUtils.Compare(ref x.Value, ref y.Value);
            bool result = c < 0;
            if (c == 0)
                result = x.Index < y.Index;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref Indexed<T> x, ref Indexed<T> y)
        {
            int c = CompareUtils.Compare(ref x.Value, ref y.Value);
            bool result = c > 0;
            if (c == 0)
                result = x.Index > y.Index;
            return result;
        }
    }
}
