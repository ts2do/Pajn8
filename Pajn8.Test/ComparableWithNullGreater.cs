using System;
using System.Diagnostics.CodeAnalysis;

namespace Pajn8.Test
{
    internal sealed class ComparableWithNullGreater : IComparable<ComparableWithNullGreater>
    {
        private readonly int value;
        public ComparableWithNullGreater(int value) { this.value = value; }
        public int CompareTo([AllowNull] ComparableWithNullGreater other) => other != null ? value.CompareTo(other.value) : -1;
    }
}
