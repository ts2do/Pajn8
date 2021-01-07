namespace Pajn8.Comparers
{
    internal interface IComparer2<T>
    {
        bool LessThan(ref T x, ref T y);
        bool GreaterThan(ref T x, ref T y);
    }
}
