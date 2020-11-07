using Pajn8.Utils;
using System;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct NullableComparableStableComparer<T> : IComparer2<Indexed<T?>>
        where T : struct, IComparable<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref Indexed<T?> x, ref Indexed<T?> y)
        {
            int c = Compare(ref x, ref y);
            bool result = c < 0;
            if (c == 0)
                result = x.Index < y.Index;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref Indexed<T?> x, ref Indexed<T?> y)
        {
            int c = Compare(ref x, ref y);
            bool result = c > 0;
            if (c == 0)
                result = x.Index > y.Index;
            return result;
        }

        private static int Compare(ref Indexed<T?> x, ref Indexed<T?> y)
        {
            int c;
            if (x.Value is T xValue)
            {
                c = 1;
                if (y.Value is T yValue)
                    c = CompareUtils.Compare(ref xValue, ref yValue);
            }
            else
            {
                c = 0;
                if (!(y.Value is null))
                    c = -1;
            }
            return c;
        }
    }
}
