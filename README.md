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

# Benchmark

The table below shows relative times which compares the medians of M operations for determining the first 1000 sorted elements of x, a randomly generated int[N], where ``x.AsSpan().Sort()`` is used as the baseline. Because the source data is randomly generated, results will vary due to the nature of the partitioning algorithm.

| M      | N          | x.AsSpan().Sort() | Array.Sort(x) | Paginator.CreateDirect(x).GetPage(0, 10) |
| :--    | :--        |               --: |           --: |                                      --: |
| 10,000 | 1,700      |             1.000 |         1.030 |                                    1.008 |
| 10,000 | 2,500      |             1.000 |         1.014 |                                    0.727 |
| 10,000 | 5,000      |             1.000 |         1.003 |                                    0.435 |
| 10,000 | 10,000     |             1.000 |         1.001 |                                    0.296 |
| 1,000  | 100,000    |             1.000 |         1.001 |                                    0.162 |
| 51     | 1,000,000  |             1.000 |         0.996 |                                    0.126 |
| 11     | 10,000,000 |             1.000 |         0.969 |                                    0.104 |
