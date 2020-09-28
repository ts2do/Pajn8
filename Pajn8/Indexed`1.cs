using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct Indexed<T> : IIndexed<T>
    {
        public T Value;
        public int Index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(T value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}
