using System.Diagnostics.CodeAnalysis;

namespace Pajn8
{
    internal struct Indexed<T>
    {
        [AllowNull]
        public T Value;
        public int Index;

        public void Set([AllowNull] T value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}
