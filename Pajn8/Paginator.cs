using System;
using System.Collections.Generic;
using System.Linq;

namespace Pajn8
{
    public static class Paginator
    {
        #region CreateDirect
        /// <summary>Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="items"/> without cloning it.</summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.</remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirect<T>(T[] items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (default(T) == null)
                return new ComparableNullCheckedPaginator<T>(items);
            else
                return new ComparableNullUncheckedPaginator<T>(items);
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="items"/> without cloning it.</summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
            return new Paginator<T, TComparer>(items, comparer);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="keys"/> and <paramref name="values"/> without cloning them.
        /// </summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.</remarks>
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
            if (keys.Length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (default(TKey) == null)
                return new ComparableNullCheckedPaginator<TKey, TValue>(keys, values);
            else
                return new ComparableNullUncheckedPaginator<TKey, TValue>(keys, values);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="keys"/> and <paramref name="values"/> without cloning them.
        /// </summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
            if (keys.Length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new Paginator<TKey, TValue, TComparer>(keys, values, comparer);
        }
        #endregion

        #region CreateDirectNoNulls
        /// <summary>Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="items"/> without cloning it.</summary>
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
            return new ComparableNullUncheckedPaginator<T>(items);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="keys"/> and <paramref name="values"/> without cloning them.
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
            if (keys.Length != values.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new ComparableNullUncheckedPaginator<TKey, TValue>(keys, values);
        }
        #endregion

        #region Create
        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.</summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.</remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> Create<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            if (default(T) == null)
                return new ComparableNullCheckedPaginator<T>(items.ToArray());
            else
                return new ComparableNullUncheckedPaginator<T>(items.ToArray());
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.</summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
            return new Paginator<T, TComparer>(items.ToArray(), comparer);
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.</summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.</remarks>
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
            if (keyArray.Length != valueArray.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            if (default(TKey) == null)
                return new ComparableNullCheckedPaginator<TKey, TValue>(keyArray, valueArray);
            else
                return new ComparableNullUncheckedPaginator<TKey, TValue>(keyArray, valueArray);
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.</summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
            if (keyArray.Length != valueArray.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new Paginator<TKey, TValue, TComparer>(keyArray, valueArray, comparer);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.</remarks>
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
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.</summary>
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
            return new ComparableNullUncheckedPaginator<T>(items.ToArray());
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.</summary>
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
            if (keyArray.Length != valueArray.Length) throw ArrayUtils.KeysAndValuesLengthMismatch();
            return new ComparableNullUncheckedPaginator<TKey, TValue>(keyArray, valueArray);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
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
        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.</summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.</remarks>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateStable<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T[] values = items.ToArray();
            if (default(T) == null)
                return new ComparableNullUncheckedPaginator<StableNullCheckedComparable<T>, T>(
                    values.ToIndexedArray<T, StableNullCheckedComparable<T>>(values.Length), values);
            else
                return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<T>, T>(
                    values.ToIndexedArray<T, StableNullUncheckedComparable<T>>(values.Length), values);
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="items"/> without cloning it.</summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
            return new Paginator<Indexed<T>, T, StableComparer<T, TComparer>>(
                values.ToIndexedArray<T, Indexed<T>>(values.Length), values, new StableComparer<T, TComparer>(comparer));
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.</summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.</remarks>
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
            if (default(TKey) == null)
                return new ComparableNullUncheckedPaginator<StableNullCheckedComparable<TKey>, TValue>(
                    keys.ToIndexedArray<TKey, StableNullCheckedComparable<TKey>>(valueArray.Length), valueArray);
            else
                return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<TKey>, TValue>(
                    keys.ToIndexedArray<TKey, StableNullUncheckedComparable<TKey>>(valueArray.Length), valueArray);
        }

        /// <summary>Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.</summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
            return new Paginator<Indexed<TKey>, TValue, StableComparer<TKey, TComparer>>(
                keys.ToIndexedArray<TKey, Indexed<TKey>>(valueArray.Length), valueArray, new StableComparer<TKey, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.</remarks>
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
                return CreateImpl<TKey, TValue, StableNullCheckedComparable<TKey>>(valueArray, keySelector, valueArray.Length);
            else
                return CreateImpl<TKey, TValue, StableNullUncheckedComparable<TKey>>(valueArray, keySelector, valueArray.Length);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// </summary>
        /// <remarks>Sorting will be done in the order defined by <paramref name="comparer"/>.</remarks>
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
            return new Paginator<Indexed<TKey>, TValue, StableComparer<TKey, TComparer>>(keys, valueArray, new StableComparer<TKey, TComparer>(comparer));
        }
        #endregion

        #region CreateStableNoNulls
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.
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
            T[] values = items.ToArray();
            return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<T>, T>(
                values.ToIndexedArray<T, StableNullUncheckedComparable<T>>(values.Length), values);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.
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
            return new ComparableNullUncheckedPaginator<StableNullUncheckedComparable<TKey>, TValue>(
                keys.ToIndexedArray<TKey, StableNullUncheckedComparable<TKey>>(valueArray.Length), valueArray);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
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
            return CreateImpl<TKey, TValue, StableNullUncheckedComparable<TKey>>(valueArray, keySelector, valueArray.Length);
        }
        #endregion

        #region Implementation
        private static IPaginator<TValue> CreateImpl<TKey, TValue, TComparer>(TValue[] values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            var keys = ArrayUtils.AllocateArray<TKey>(values.Length);
            for (int i = 0; i < values.Length; ++i)
                keys[i] = keySelector(values[i]);
            return new Paginator<TKey, TValue, TComparer>(keys, values, comparer);
        }

        private static IPaginator<TValue> CreateImpl<TKey, TValue, TStableComparable>(TValue[] values, Func<TValue, TKey> keySelector, int length)
            where TKey : IComparable<TKey>
            where TStableComparable : IIndexed<TKey>, IComparable<TStableComparable>
        {
            var keys = ArrayUtils.AllocateArray<TStableComparable>(length);
            for (int i = 0; i < length; ++i)
                keys[i].Set(keySelector(values[i]), i);
            return new Paginator<TStableComparable, TValue, ComparableNullUncheckedComparer<TStableComparable>>(keys, values, default);
        }
        #endregion
    }
}
