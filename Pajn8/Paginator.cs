using Pajn8.Arrays;
using Pajn8.Comparers;
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
            if (default(T) is null)
                return CreateImpl(items, 0, items.Length, default(NullCheckedComparableComparer<T>));
            else
                return CreateImpl(items, 0, items.Length, default(NullUncheckedComparableComparer<T>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T?}"/> instance which operates directly on <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T?}"/> instance</returns>
        public static IPaginator<T?> CreateDirect<T>(T?[] items)
            where T : struct, IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            return CreateImpl(items, 0, items.Length, default(NullableComparableComparer<T>));
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
            if (items.Length < length + offset) throw new ArgumentException(Strings.Arg_ArrayTooShortForOffsetAndLength, nameof(items));
            if (default(T) is null)
                return CreateImpl(items, offset, length, default(NullCheckedComparableComparer<T>));
            else
                return CreateImpl(items, offset, length, default(NullUncheckedComparableComparer<T>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T?}"/> instance which operates directly on a range in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <param name="offset">Offset of range in array</param>
        /// <param name="length">Length of range in array</param>
        /// <returns>The <see cref="IPaginator{T?}"/> instance</returns>
        public static IPaginator<T?> CreateDirect<T>(T?[] items, int offset, int length)
            where T : struct, IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (items.Length < length + offset) throw new ArgumentException(Strings.Arg_ArrayTooShortForOffsetAndLength, nameof(items));
            return CreateImpl(items, offset, length, default(NullableComparableComparer<T>));
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
            return CreateImpl(items, 0, items.Length, new ComparerComparer2<T, TComparer>(comparer));
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
            if (items.Length < length + offset) throw new ArgumentException(Strings.Arg_ArrayTooShortForOffsetAndLength, nameof(items));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return CreateImpl(items, offset, length, new ComparerComparer2<T, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates directly on <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateDirect<TKey, TValue>(TKey[] keys, TValue[] values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            int length = keys.Length;
            if (length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (keys == (object)values)
                return (IPaginator<TValue>)CreateDirect(keys);
            if (default(TKey) is null)
                return CreateImpl(keys, values, 0, length, default(NullCheckedComparableComparer<TKey>));
            else
                return CreateImpl(keys, values, 0, length, default(NullUncheckedComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates directly on <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateDirect<TKey, TValue>(TKey?[] keys, TValue[] values)
            where TKey : struct, IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            int length = keys.Length;
            if (length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (keys == (object)values)
                return (IPaginator<TValue>)CreateDirect(keys);
            return CreateImpl(keys, values, 0, length, default(NullableComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates directly on <paramref name="keys"/> and <paramref name="values"/>.
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
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateDirect<TKey, TValue, TComparer>(TKey[] keys, TValue[] values, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            int length = keys.Length;
            if (length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (keys == (object)values)
                return (IPaginator<TValue>)CreateDirect(keys, comparer);
            return CreateImpl(keys, values, 0, length, new ComparerComparer2<TKey, TComparer>(comparer));
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
            return CreateImpl(items, 0, items.Length, default(NullUncheckedComparableComparer<T>));
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
            if (items.Length < length + offset) throw new ArgumentException(Strings.Arg_ArrayTooShortForOffsetAndLength, nameof(items));
            return CreateImpl(items, offset, length, default(NullUncheckedComparableComparer<T>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates directly on <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// Elements in <paramref name="keys"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateDirect{TKey, TValue}(TKey[], TValue[])"/> if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateDirectNoNulls<TKey, TValue>(TKey[] keys, TValue[] values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            int length = keys.Length;
            if (length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (keys == (object)values)
                return (IPaginator<TValue>)CreateDirectNoNulls(keys);
            return CreateImpl(keys, values, 0, length, default(NullUncheckedComparableComparer<TKey>));
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
            if (default(T) is null)
                return CreateImpl(itemArray, 0, itemArray.Length, default(NullCheckedComparableComparer<T>));
            else
                return CreateImpl(itemArray, 0, itemArray.Length, default(NullUncheckedComparableComparer<T>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T?}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T?}"/> instance</returns>
        public static IPaginator<T?> Create<T>(IEnumerable<T?> items)
            where T : struct, IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T?[] itemArray = items.ToArray();
            return CreateImpl(itemArray, 0, itemArray.Length, default(NullableComparableComparer<T>));
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
            return CreateImpl(itemArray, 0, itemArray.Length, new ComparerComparer2<T, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            TKey[] keyArray = keys.ToArray();
            int length = keyArray.Length;
            TValue[] valueArray = values.ToArray(length);
            if (default(TKey) is null)
                return CreateImpl(keyArray, valueArray, 0, length, default(NullCheckedComparableComparer<TKey>));
            else
                return CreateImpl(keyArray, valueArray, 0, length, default(NullUncheckedComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue>(IEnumerable<TKey?> keys, IEnumerable<TValue> values)
            where TKey : struct, IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            TKey?[] keyArray = keys.ToArray();
            int length = keyArray.Length;
            TValue[] valueArray = values.ToArray(length);
            return CreateImpl(keyArray, valueArray, 0, length, default(NullableComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
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
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue, TComparer>(IEnumerable<TKey> keys, IEnumerable<TValue> values, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            TKey[] keyArray = keys.ToArray();
            int length = keyArray.Length;
            TValue[] valueArray = values.ToArray(length);
            return CreateImpl(keyArray, valueArray, 0, length, new ComparerComparer2<TKey, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            TValue[] valueArray = values.ToArray();
            if (default(TKey) is null)
                return CreateImpl(valueArray, keySelector, default(NullCheckedComparableComparer<TKey>));
            else
                return CreateImpl(valueArray, keySelector, default(NullUncheckedComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey?> keySelector)
            where TKey : struct, IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            TValue[] valueArray = values.ToArray();
            return CreateImpl(valueArray, keySelector, default(NullableComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
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
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> Create<TKey, TValue, TComparer>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return CreateImpl(values.ToArray(), keySelector, new ComparerComparer2<TKey, TComparer>(comparer));
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
            return CreateImpl(itemArray, 0, itemArray.Length, default(NullUncheckedComparableComparer<T>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// Elements in <paramref name="keys"/> will be assumed to be non-null.
        /// Prefer calling <see cref="Create{TKey, TValue}(IEnumerable{TKey}, IEnumerable{TValue})"/>
        /// if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateNoNulls<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            TKey[] keyArray = keys.ToArray();
            TValue[] valueArray = values.ToArray();
            int length = keyArray.Length;
            if (length != valueArray.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return CreateImpl(keyArray, valueArray, 0, length, default(NullUncheckedComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
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
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateNoNulls<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            return CreateImpl(values.ToArray(), keySelector, default(NullUncheckedComparableComparer<TKey>));
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
            Indexed<T>[] keys = values.ToIndexedArray();
            if (default(T) is null)
                return CreateImpl(keys, values, 0, length, default(NullCheckedComparableStableComparer<T>));
            else
                return CreateImpl(keys, values, 0, length, default(NullUncheckedComparableStableComparer<T>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T?}"/> instance which operates over the elements in <paramref name="items"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T?}"/> instance</returns>
        public static IPaginator<T?> CreateStable<T>(IEnumerable<T?> items)
            where T : struct, IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T?[] values = items.ToArray();
            return CreateImpl(values.ToIndexedArray(), values, 0, values.Length, default(NullableComparableStableComparer<T>));
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
            return CreateImpl(values.ToIndexedArray(), values, 0, values.Length, new StableComparer<T, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            Indexed<TKey>[] indexedKeys = keys.ToIndexedArray();
            int length = indexedKeys.Length;
            TValue[] valueArray = values.ToArray(length);
            if (default(TKey) is null)
                return CreateImpl(indexedKeys, valueArray, 0, length, default(NullCheckedComparableStableComparer<TKey>));
            else
                return CreateImpl(indexedKeys, valueArray, 0, length, default(NullUncheckedComparableStableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue>(IEnumerable<TKey?> keys, IEnumerable<TValue> values)
            where TKey : struct, IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            Indexed<TKey?>[] indexedKeys = keys.ToIndexedArray();
            int length = indexedKeys.Length;
            return CreateImpl(indexedKeys, values.ToArray(length), 0, length, default(NullableComparableStableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
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
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue, TComparer>(IEnumerable<TKey> keys, IEnumerable<TValue> values, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            Indexed<TKey>[] indexedKeys = keys.ToIndexedArray();
            int length = indexedKeys.Length;
            return CreateImpl(indexedKeys, values.ToArray(length), 0, length, new StableComparer<TKey, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            TValue[] valueArray = values.ToArray();
            if (default(TKey) is null)
                return CreateStableImpl(valueArray, keySelector, default(NullCheckedComparableStableComparer<TKey>));
            else
                return CreateStableImpl(valueArray, keySelector, default(NullUncheckedComparableStableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey?> keySelector)
            where TKey : struct, IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            TValue[] valueArray = values.ToArray();
            return CreateStableImpl(valueArray, keySelector, default(NullableComparableStableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
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
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStable<TKey, TValue, TComparer>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            TValue[] valueArray = values.ToArray();
            return CreateImpl(valueArray.MapIndexed(keySelector), valueArray, 0, valueArray.Length, new StableComparer<TKey, TComparer>(comparer));
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
            return CreateImpl(itemArray.ToIndexedArray(), itemArray, 0, itemArray.Length, default(NullUncheckedComparableStableComparer<T>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// Elements in <paramref name="keys"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateStable{TKey, TValue}(IEnumerable{TKey}, IEnumerable{TValue})"/>
        /// if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="keys">Keys on which to sort</param>
        /// <param name="values">Values associated with the keys</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStableNoNulls<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
            where TKey : IComparable<TKey>
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (values is null) throw new ArgumentNullException(nameof(values));
            Indexed<TKey>[] indexedKeys = keys.ToIndexedArray();
            int length = indexedKeys.Length;
            return CreateImpl(indexedKeys, values.ToArray(length), 0, length, default(NullUncheckedComparableStableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{TValue}"/> instance which operates over the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>
        /// Sorting will be done in the order defined by the <see cref="IComparable{TKey}"/> implementation of <typeparamref name="TKey"/>.
        /// Keys returned by <paramref name="keySelector"/> will be assumed to be non-null.
        /// Prefer calling <see cref="CreateStable{TKey, TValue}(IEnumerable{TValue}, Func{TValue, TKey})"/>
        /// if <typeparamref name="TKey"/> is known to be a non-nullable type.
        /// </remarks>
        /// <typeparam name="TKey">Type of keys on which to sort</typeparam>
        /// <typeparam name="TValue">Type of values associated with the keys</typeparam>
        /// <param name="values">Keys on which to sort</param>
        /// <param name="keySelector">Function which selects keys from values</param>
        /// <returns>The <see cref="IPaginator{TValue}"/> instance</returns>
        public static IPaginator<TValue> CreateStableNoNulls<TKey, TValue>(IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            if (values is null) throw new ArgumentNullException(nameof(values));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            return CreateStableImpl(values.ToArray(), keySelector, default(NullUncheckedComparableStableComparer<TKey>));
        }
        #endregion

        #region Implementation
        private static Paginator<T, TComparer> CreateImpl<T, TComparer>(T[] items, int offset, int length, in TComparer comparer)
            where TComparer : IComparer2<T>
            => new Paginator<T, TComparer>(items, offset, length, comparer);

        private static Paginator<TKey, TValue, TComparer> CreateImpl<TKey, TValue, TComparer>(TKey[] keys, TValue[] values, int offset, int length, in TComparer comparer)
            where TComparer : IComparer2<TKey>
            => new Paginator<TKey, TValue, TComparer>(keys, values, offset, length, comparer);

        private static IPaginator<TValue> CreateImpl<TKey, TValue, TComparer>(TValue[] values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer2<TKey>
            => new Paginator<TKey, TValue, TComparer>(values.Map(keySelector), values, 0, values.Length, comparer);

        private static IPaginator<TValue> CreateStableImpl<TKey, TValue, TComparer>(TValue[] values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer2<Indexed<TKey>>
            => new Paginator<Indexed<TKey>, TValue, TComparer>(values.MapIndexed(keySelector), values, 0, values.Length, comparer);
        #endregion
    }
}
