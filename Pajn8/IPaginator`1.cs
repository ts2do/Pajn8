using System;
using System.Collections.Generic;

namespace Pajn8
{
    public interface IPaginator<T>
    {
        /// <summary>
        /// The number of elements over which this <see cref="IPaginator{T}"/> instance is operating.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get a range of sorted items.
        /// </summary>
        /// <param name="offset">Offset of range</param>
        /// <param name="length">Length of range</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative.
        /// -or-
        /// <paramref name="offset"/> + <paramref name="length"/> exceeds <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="IComparer{T}"/> implementation used by this instance returned inconsistent values.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A prior invocation of <see cref="GetPage"/> resulted in an exception.
        /// </exception>
        /// <returns>View of sorted items</returns>
        ArraySegment<T> GetPage(int offset, int length);

        /// <summary>
        /// Get a range of sorted items.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"
        /// <param name="range">
        /// <paramref name="range"/> is not contained by the range [0, <see cref="Count"/>).
        /// </param>
        /// <exception cref="ArgumentException">
        /// The <see cref="IComparer{T}"/> implementation used by this instance returned inconsistent values.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A prior invocation of <see cref="GetPage"/> resulted in an exception.
        /// </exception>
        /// <returns>View of sorted items</returns>
        ArraySegment<T> GetPage(Range range);
    }
}
