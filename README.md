# Pajn8
Leverage partitioning to efficiently sort a subset of an array.

It's unnecessary to sort an entire collection when only a small range of items is desired.
This project demonstrates partitioning an input array to help fulfill calls to ``GetPage`` and memoizing the partitions to leverage them for subsequent calls to ``GetPage``.

The ``Create``, ``CreateStable``, and ``CreateDirect`` method overloads in the ``Paginator`` factory class return implementations of ``IPaginator<T>``, the interface shown below. Manually populating arrays and calling ``CreateDirect`` with a ``struct`` implementation of ``IComparer<>`` is recommended for best performance.
```csharp
public interface IPaginator<T>
{
    ArraySegment<T> GetPage(int offset, int length);
    ArraySegment<T> GetPage(Range range);
}
```
