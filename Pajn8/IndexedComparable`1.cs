using System;
using System.Diagnostics.CodeAnalysis;

namespace Pajn8
{
    internal struct IndexedComparable<T> : IComparable<IndexedComparable<T>>
        where T : IComparable<T>
    {
        public T Value;
        public int Index;

        public int CompareTo([AllowNull] IndexedComparable<T> other)
        {
            int c;
            if (Value is null)
            {
                c = -1;
                if (other.Value is null)
                    c = 0;
            }
            else
            {
                c = Value.CompareTo(other.Value);
                if (c == 0)
                {
                    c = Index.CompareTo(other.Index);
                }
            }
            return c;
        }
    }
}
