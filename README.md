# Pajn8
Leverage partitioning to efficiently sort a subset of an array.

It's unnecessary to sort an entire collection when only a small range of items is desired.
This project demonstrates partitioning an input array to help fulfill calls to ``GetPage`` and memoizing the partitions to leverage them for subsequent calls.

The factory methods in the ``Paginator`` static class return implementations of ``IPaginator<>``, the interface shown below. Manually populating arrays and calling ``CreateDirect`` or ``CreateDirectNoNulls`` with a value type implementation of ``IComparer<>`` is recommended for optimal performance.
```csharp
public interface IPaginator<T>
{
    ArraySegment<T> GetPage(int offset, int length);
    ArraySegment<T> GetPage(Range range);
}
```
