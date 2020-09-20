using System;

namespace Pajn8
{
    public interface IPaginator<T>
    {
        ReadOnlySpan<T> GetPage(int offset, int pageSize);
        ReadOnlySpan<T> GetPage(Range range);
    }
}
