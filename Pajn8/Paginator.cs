using System;
using System.Collections.Generic;
using System.Linq;

namespace Pajn8
{
    public static class Paginator
    {
        #region CreateDirect
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirect<T>(T[] items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (default(T) == null)
                return new ComparableNullCheckedPaginator<T>(items, 0, items.Length);
            else
                return new ComparableNullUncheckedPaginator<T>(items, 0, items.Length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on a range in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <param name="offset">Offset of range in array</param>
        /// <param name="length">Length of range in array</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirect<T>(T[] items, int offset, int length)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (items.Length < length + offset) throw new ArgumentException(nameof(items) + " array shorter than expected", nameof(items));
            if (default(T) == null)
                return new ComparableNullCheckedPaginator<T>(items, offset, length);
            else
                return new ComparableNullUncheckedPaginator<T>(items, offset, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirect<T, TComparer>(T[] items, TComparer comparer)
            where TComparer : IComparer<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return new Paginator<T, TComparer>(items, 0, items.Length, comparer);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on a range in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <param name="offset">Offset of range in array</param>
        /// <param name="length">Length of range in array</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirect<T, TComparer>(T[] items, int offset, int length, TComparer comparer)
            where TComparer : IComparer<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (items.Length < length + offset) throw new ArgumentException(nameof(items) + " array shorter than expected", nameof(items));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return new Paginator<T, TComparer>(items, offset, length, comparer);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateDirect<TKey, TValue>(TKey[] keys, TValue[] values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            int length = keys.Length;
            if (length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (default(TKey) == null)
                return new ComparableNullCheckedPaginator<TKey, TValue>(keys, values, 0, length);
            else
                return new ComparableNullUncheckedPaginator<TKey, TValue>(keys, values, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on a range in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <param name="offset">Offset of range in arrays</param>
        /// <param name="length">Length of range in arrays</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateDirect<TKey, TValue>(TKey[] keys, TValue[] values, int offset, int length)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (keys.Length < length + offset) throw new ArgumentException(nameof(keys) + " array shorter than expected", nameof(keys));
            if (values.Length < length + offset) throw new ArgumentException(nameof(values) + " array shorter than expected", nameof(values));
            if (default(TKey) == null)
                return new ComparableNullCheckedPaginator<TKey, TValue>(keys, values, 0, length);
            else
                return new ComparableNullUncheckedPaginator<TKey, TValue>(keys, values, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateDirect<TKey, TValue, TComparer>(TKey[] keys, TValue[] values, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            int length = keys.Length;
            if (length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new Paginator<TKey, TValue, TComparer>(keys, values, 0, length, comparer);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on a range in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <param name="offset">Offset of range in arrays</param>
        /// <param name="length">Length of range in arrays</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateDirect<TKey, TValue, TComparer>(TKey[] keys, TValue[] values, int offset, int length, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (keys.Length < length + offset) throw new ArgumentException(nameof(keys) + " array shorter than expected", nameof(keys));
            if (values.Length < length + offset) throw new ArgumentException(nameof(values) + " array shorter than expected", nameof(values));
            return new Paginator<TKey, TValue, TComparer>(keys, values, offset, length, comparer);
        }
        #endregion

        #region CreateDirectNoNulls
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// Elements in <paramref name="items"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateDirect{T}(T[])"/> if <typeparamref name="T"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirectNoNulls<T>(T[] items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            return new ComparableNullUncheckedPaginator<T>(items, 0, items.Length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on a range in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// Elements in <paramref name="items"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateDirect{T}(T[])"/> if <typeparamref name="T"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <param name="offset">Offset of range in array</param>
        /// <param name="length">Length of range in array</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirectNoNulls<T>(T[] items, int offset, int length)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (items.Length < length + offset) throw new ArgumentException(nameof(items) + " array shorter than expected", nameof(items));
            return new ComparableNullUncheckedPaginator<T>(items, offset, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// Elements in <paramref name="keys"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateDirect{TKey, TValue}(TKey[], TValue[])"/> if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateDirectNoNulls<TKey, TValue>(TKey[] keys, TValue[] values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            int length = keys.Length;
            if (length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new ComparableNullUncheckedPaginator<TKey, TValue>(keys, values, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates directly on a range in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// Elements in <paramref name="keys"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateDirect{TKey, TValue}(TKey[], TValue[])"/> if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <param name="offset">Offset of range in arrays</param>
        /// <param name="length">Length of range in arrays</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateDirectNoNulls<TKey, TValue>(TKey[] keys, TValue[] values, int offset, int length)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (keys.Length < length + offset) throw new ArgumentException(nameof(keys) + " array shorter than expected", nameof(keys));
            if (values.Length < length + offset) throw new ArgumentException(nameof(values) + " array shorter than expected", nameof(values));
            return new ComparableNullUncheckedPaginator<TKey, TValue>(keys, values, 0, length);
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> Create<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T[] itemArray = items.ToArray();
            if (default(T) == null)
                return new ComparableNullCheckedPaginator<T>(itemArray, 0, itemArray.Length);
            else
                return new ComparableNullUncheckedPaginator<T>(itemArray, 0, itemArray.Length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> Create<T, TComparer>(IEnumerable<T> items, TComparer comparer)
            where TComparer : IComparer<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            T[] itemArray = items.ToArray();
            return new Paginator<T, TComparer>(itemArray, 0, itemArray.Length, comparer);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            TKey[] keyArray = keys.ToArray();
            TValue[] valueArray = values.ToArray();
            int length = keyArray.Length;
            if (length != valueArray.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (default(TKey) == null)
                return new ComparableNullCheckedPaginator<TKey, TValue>(keyArray, valueArray, 0, length);
            else
                return new ComparableNullUncheckedPaginator<TKey, TValue>(keyArray, valueArray, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue, TComparer>(IEnumerable<TKey> keys, IEnumerable<TValue> values, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            TKey[] keyArray = keys.ToArray();
            TValue[] valueArray = values.ToArray();
            int length = keyArray.Length;
            if (length != valueArray.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new Paginator<TKey, TValue, TComparer>(keyArray, valueArray, 0, length, comparer);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (default(TKey) == null)
                return CreateImpl(values.ToArray(), keySelector, default(ComparableNullCheckedComparer<TKey>));
            else
                return CreateImpl(values.ToArray(), keySelector, default(ComparableNullUncheckedComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue, TComparer>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return CreateImpl(values.ToArray(), keySelector, comparer);
        }
        #endregion

        #region CreateNoNulls
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// Elements in <paramref name="items"/> will be assumed to be non-null.
        /// Prefer calling <see cref="Create{T}(IEnumerable{T})"/> if <typeparamref name="T"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateNoNulls<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T[] itemArray = items.ToArray();
            return new ComparableNullUncheckedPaginator<T>(itemArray, 0, itemArray.Length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// Elements in <paramref name="keys"/> will be assumed to be non-null.
        /// Prefer calling <see cref="Create{TKey, TValue}(IEnumerable{TKey}, IEnumerable{TValue})"/>
        /// if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateNoNulls<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            TKey[] keyArray = keys.ToArray();
            TValue[] valueArray = values.ToArray();
            int length = keyArray.Length;
            if (length != valueArray.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new ComparableNullUncheckedPaginator<TKey, TValue>(keyArray, valueArray, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// Keys returned by <paramref name="keySelector"/> will be assumed to be non-null.
        /// Prefer calling <see cref="Create{TKey, TValue}(IEnumerable{TValue}, Func{TValue, TKey})"/>
        /// if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateNoNulls<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            return CreateImpl(values.ToArray(), keySelector, default(ComparableNullUncheckedComparer<TKey>));
        }
        #endregion

        #region CreateStable
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateStable<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T[] values = items.ToArray();
            int length = values.Length;
            if (default(T) == null)
                return new ComparableNullUncheckedPaginator<StableNullCheckedComparable<T>, T>(
                    values.ToIndexedArray<T, StableNullCheckedComparable<T>>(length), values, 0, length);
            else
                return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<T>, T>(
                    values.ToIndexedArray<T, StableNullUncheckedComparable<T>>(length), values, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateStable<T, TComparer>(IEnumerable<T> items, TComparer comparer)
            where TComparer : IComparer<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            T[] values = items.ToArray();
            int length = values.Length;
            return new Paginator<Indexed<T>, T, StableComparer<T, TComparer>>(
                values.ToIndexedArray<T, Indexed<T>>(length), values, 0, length, new StableComparer<T, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            TValue[] valueArray = values.ToArray();
            int length = valueArray.Length;
            if (default(TKey) == null)
                return new ComparableNullUncheckedPaginator<StableNullCheckedComparable<TKey>, TValue>(
                    keys.ToIndexedArray<TKey, StableNullCheckedComparable<TKey>>(length), valueArray, 0, length);
            else
                return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<TKey>, TValue>(
                    keys.ToIndexedArray<TKey, StableNullUncheckedComparable<TKey>>(length), valueArray, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <param name="comparer">Comparer by which to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue, TComparer>(IEnumerable<TKey> keys, IEnumerable<TValue> values, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            TValue[] valueArray = values.ToArray();
            int length = valueArray.Length;
            return new Paginator<Indexed<TKey>, TValue, StableComparer<TKey, TComparer>>(
                keys.ToIndexedArray<TKey, Indexed<TKey>>(length), valueArray, 0, length, new StableComparer<TKey, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            TValue[] valueArray = values.ToArray();
            if (default(TKey) == null)
                return CreateImpl<TKey, TValue, StableNullCheckedComparable<TKey>>(valueArray, keySelector);
            else
                return CreateImpl<TKey, TValue, StableNullUncheckedComparable<TKey>>(valueArray, keySelector);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <typeparam name="TComparer">Type of comparer by which to sort</typeparam>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue, TComparer>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            TValue[] valueArray = values.ToArray();
            int length = valueArray.Length;
            var keys = ArrayUtils.AllocateArray<Indexed<TKey>>(length);
            for (int i = 0; i < length; ++i)
                keys[i].Set(keySelector(valueArray[i]), i);
            return new Paginator<Indexed<TKey>, TValue, StableComparer<TKey, TComparer>>(
                keys, valueArray, 0, length, new StableComparer<TKey, TComparer>(comparer));
        }
        #endregion

        #region CreateStableNoNulls
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// Elements in <paramref name="items"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateStable{T}(IEnumerable{T})"/> if <typeparamref name="T"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateStableNoNulls<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T[] itemArray = items.ToArray();
            int length = itemArray.Length;
            return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<T>, T>(
                itemArray.ToIndexedArray<T, StableNullUncheckedComparable<T>>(length), itemArray, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// Elements in <paramref name="keys"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateStable{TKey, TValue}(IEnumerable{TKey}, IEnumerable{TValue})"/>
        /// if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateStableNoNulls<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            TValue[] valueArray = values.ToArray();
            int length = valueArray.Length;
            return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<TKey>, TValue>(
                keys.ToIndexedArray<TKey, StableNullUncheckedComparable<TKey>>(length), valueArray, 0, length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// Keys returned by <paramref name="keySelector"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateStable{TKey, TValue}(IEnumerable{TValue}, Func{TValue, TKey})"/>
        /// if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<TValue> CreateStableNoNulls<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            TValue[] valueArray = values.ToArray();
            return CreateImpl<TKey, TValue, StableNullUncheckedComparable<TKey>>(valueArray, keySelector);
        }
        #endregion

        #region Implementation
        private static IPaginator<TValue> CreateImpl<TKey, TValue, TComparer>(TValue[] values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            int length = values.Length;
            var keys = ArrayUtils.AllocateArray<TKey>(length);
            for (int i = 0; i < length; ++i)
                keys[i] = keySelector(values[i]);
            return new Paginator<TKey, TValue, TComparer>(keys, values, 0, length, comparer);
        }

        private static IPaginator<TValue> CreateImpl<TKey, TValue, TStableComparable>(TValue[] values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
            where TStableComparable : IIndexed<TKey>, IComparable<TStableComparable>
        {
            int length = values.Length;
            var keys = ArrayUtils.AllocateArray<TStableComparable>(length);
            for (int i = 0; i < length; ++i)
                keys[i].Set(keySelector(values[i]), i);
            return new Paginator<TStableComparable, TValue, ComparableNullUncheckedComparer<TStableComparable>>(keys, values, 0, length, default);
        }
        #endregion
    }
}
