using System;

namespace Pajn8
{
    public interface IPaginator<T>
    {
        ArraySegment<T> GetPage(int offset, int pageSize);
        ArraySegment<T> GetPage(Range range);
    }
}
