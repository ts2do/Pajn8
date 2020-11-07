using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Pajn8
{
    [DebuggerDisplay("({Value}, {Index})")]
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
