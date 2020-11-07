using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct StableComparer<T, TComparer> : IComparer2<Indexed<T>>
        where TComparer : IComparer<T>
    {
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain value types")]
        private TComparer impl;

        public StableComparer(TComparer impl)
        {
            this.impl = impl;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref Indexed<T> x, ref Indexed<T> y)
        {
            int c = impl.Compare(x.Value, y.Value);
            bool result = c < 0;
            if (c == 0)
                result = x.Index < y.Index;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref Indexed<T> x, ref Indexed<T> y)
        {
            int c = impl.Compare(x.Value, y.Value);
            bool result = c > 0;
            if (c == 0)
                result = x.Index > y.Index;
            return result;
        }
    }
}
