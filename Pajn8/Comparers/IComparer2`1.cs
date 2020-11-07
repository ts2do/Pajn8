using System.Diagnostics.CodeAnalysis;

namespace Pajn8.Comparers
{
    public interface IComparer2<T>
    {
        bool LessThan([AllowNull] ref T x, [AllowNull] ref T y);
        bool GreaterThan([AllowNull] ref T x, [AllowNull] ref T y);
    }
}
