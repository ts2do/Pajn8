using System;
using System.Diagnostics;

namespace Pajn8.Test
{
    [DebuggerDisplay("({x}, {y})")]
    internal struct PartialComparable : IComparable<PartialComparable>, IEquatable<PartialComparable>
    {
        private readonly int x, y;

        public PartialComparable(int x, int y) => (this.x, this.y) = (x, y);

        public int CompareTo(PartialComparable other) => x.CompareTo(other.x);

        public bool Equals(PartialComparable other) => x == other.x && y == other.y;

        public override bool Equals(object obj) => obj is PartialComparable other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(x, y);
    }
}
