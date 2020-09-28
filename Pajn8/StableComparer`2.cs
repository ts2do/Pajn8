using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal sealed class StableComparer<T, TComparer> : IComparer<Indexed<T>>
        where TComparer : IComparer<T>
    {
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain mutable value types")]
        private TComparer impl;

        public StableComparer(TComparer impl)
        {
            this.impl = impl;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(Indexed<T> x, Indexed<T> y)
        {
            int c = impl.Compare(x.Value, y.Value);
            if (c == 0)
                c = x.Index - y.Index;
            return c;
        }
    }
}
