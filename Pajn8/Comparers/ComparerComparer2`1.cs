using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pajn8.Comparers
{
    internal struct ComparerComparer2<T, TComparer> : IComparer2<T>
        where TComparer : IComparer<T>
    {
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "May contain value types")]
        private TComparer impl;

        public ComparerComparer2(in TComparer impl)
        {
            this.impl = impl;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LessThan(ref T left, ref T right) => impl.Compare(left, right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GreaterThan(ref T left, ref T right) => impl.Compare(left, right) > 0;
    }
}
