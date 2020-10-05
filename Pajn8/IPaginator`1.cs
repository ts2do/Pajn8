using System;

namespace Pajn8
{
    public interface IPaginator<T>
    {
        /// <summary>
        /// Get a range of sorted items.
        /// </summary>
        /// <param name="offset">Offset of range</param>
        /// <param name="length">Length of range</param>
        /// <returns>View of sorted items</returns>
        ArraySegment<T> GetPage(int offset, int length);
        /// <summary>
        /// Get a range of sorted items.
        /// </summary>
        /// <param name="range">Range</param>
        /// <returns>View of sorted items</returns>
        ArraySegment<T> GetPage(Range range);
    }
}
