# Pajn8
Leverage partitioning to efficiently sort a subset of an array.

It's unnecessary to sort an entire collection when only a small range of items is desired.
This project demonstrates partitioning an input array to help fulfill calls to ``GetPage`` and memoizing the partitions to leverage them for subsequent calls to ``GetPage``.

The ``Paginator.Create`` and ``Paginator.CreateDirect`` method overloads return implementations of ``IPaginator<T>``, the interface shown below.
```csharp
public interface IPaginator<T>
{
    ArraySegment<T> GetPage(int offset, int pageSize);
    ArraySegment<T> GetPage(Range range);
}
```
