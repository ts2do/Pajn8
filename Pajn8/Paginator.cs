using System;
using System.Collections.Generic;
using System.Linq;

namespace Pajn8
{
    public static class Paginator
    {
        #region CreateDirect
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="items"/> without cloning it.
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateDirect<T>(T[] items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            return new ComparablePaginator<T>(items);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="items"/> without cloning it.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </summary>
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
            return new ComparablePaginator<TKey, TValue>(keys, values);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="keys"/> and <paramref name="values"/> without cloning them.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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

        #region Create
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> Create<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            return new ComparablePaginator<T>(items.ToArray());
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </summary>
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
            return new ComparablePaginator<TKey, TValue>(keyArray, valueArray);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </summary>
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
            return CreateImpl(values.ToArray(), keySelector, default(ComparableComparer<TKey>));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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

        #region CreateStable
        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="items"/>.
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of items to sort</typeparam>
        /// <param name="items">Items to sort</param>
        /// <returns>The <see cref="IPaginator{T}"/> instance</returns>
        public static IPaginator<T> CreateStable<T>(IEnumerable<T> items)
            where T : IComparable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));
            T[] values = items.ToArray();
            int length = values.Length;
            var keys = ArrayUtils.AllocateArray<StableComparable<T>>(length);
            for (int i = 0; i < length; ++i)
            {
                ref var key = ref keys[i];
                key.Value = values[i];
                key.Index = i;
            }
            return new ComparablePaginator<StableComparable<T>, T>(keys, values);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance directly on <paramref name="items"/> without cloning it.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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
            var keys = ArrayUtils.AllocateArray<Indexed<T>>(length);
            for (int i = 0; i < length; ++i)
            {
                ref var key = ref keys[i];
                key.Value = values[i];
                key.Index = i;
            }
            return new Paginator<Indexed<T>, T, StableComparer<T, TComparer>>(keys, values, new StableComparer<T, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </summary>
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
            StableComparable<TKey>[] indexedKeys = keys.ToStableComparableArray(valueArray.Length);
            return new ComparablePaginator<StableComparable<TKey>, TValue>(indexedKeys, valueArray);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="keys"/> and <paramref name="values"/>.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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
            Indexed<TKey>[] indexedKeys = keys.ToIndexedArray(valueArray.Length);
            return new Paginator<Indexed<TKey>, TValue, StableComparer<TKey, TComparer>>(indexedKeys, valueArray, new StableComparer<TKey, TComparer>(comparer));
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// Sorting will be done in the order defined by the <see cref="IComparable{T}"/> implementation of <typeparamref name="TKey"/>.
        /// </summary>
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
            return CreateImpl(values.ToArray(), keySelector);
        }

        /// <summary>
        /// Creates an <see cref="IPaginator{T}"/> instance for the elements in <paramref name="values"/>,
        /// generating keys on which to sort using <paramref name="keySelector"/>.
        /// Sorting will be done in the order defined by <paramref name="comparer"/>.
        /// </summary>
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
            return CreateStableImpl(values.ToArray(), keySelector, comparer);
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

        private static IPaginator<TValue> CreateImpl<TKey, TValue>(TValue[] values, Func<TValue, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            int length = values.Length;
            var keys = ArrayUtils.AllocateArray<StableComparable<TKey>>(length);
            for (int i = 0; i < length; ++i)
            {
                ref StableComparable<TKey> key = ref keys[i];
                key.Value = keySelector(values[i]);
                key.Index = i;
            }
            return new Paginator<StableComparable<TKey>, TValue, ComparableComparer<StableComparable<TKey>>>(keys, values, default);
        }

        private static IPaginator<TValue> CreateStableImpl<TKey, TValue, TComparer>(TValue[] values, Func<TValue, TKey> keySelector, TComparer comparer)
            where TComparer : IComparer<TKey>
        {
            int length = values.Length;
            var keys = ArrayUtils.AllocateArray<Indexed<TKey>>(length);
            for (int i = 0; i < length; ++i)
            {
                ref Indexed<TKey> key = ref keys[i];
                key.Value = keySelector(values[i]);
                key.Index = i;
            }
            return new Paginator<Indexed<TKey>, TValue, StableComparer<TKey, TComparer>>(keys, values, new StableComparer<TKey, TComparer>(comparer));
        }
        #endregion
    }
}
