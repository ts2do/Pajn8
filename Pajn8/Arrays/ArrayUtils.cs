using System;
using System.Collections.Generic;

namespace Pajn8.Arrays
{
    internal static class ArrayUtils
    {
        public static Exception KeysAndValuesLengthMismatch() => new ArgumentException(string.Format(Strings.Arg_MismatchedLength, "keys", "values"));

        public static T[] AllocateArray<T>(int length) => new T[length]; // TODO: Replace with inlined calls to GC.AllocateUninitializedArray

        public static Indexed<T>[] ToIndexedArray<T>(this IEnumerable<T> items)
        {
            return items switch
            {
                T[] x => x.ToIndexedArray(),
                List<T> x => CreateFromList(x),
                _ => CreateFromEnumerable(items)
            };

            static Indexed<T>[] CreateFromList(List<T> list)
            {
                using List<T>.Enumerator enumerator = list.GetEnumerator();
                int length = list.Count;
                var indexedKeys = AllocateArray<Indexed<T>>(length);
                for (int i = 0; i < length; ++i)
                {
                    enumerator.MoveNext(); // Enforced by comodification checks
                    indexedKeys[i].Set(enumerator.Current, i);
                }
                return indexedKeys;
            }

            static Indexed<T>[] CreateFromEnumerable(IEnumerable<T> enumerable)
            {
                var buffer = new ArrayBuffer<Indexed<T>>(16);
                foreach (var key in enumerable)
                    buffer.Add().Set(key, buffer.Count);
                return buffer.ToArray();
            }
        }

        public static Indexed<T>[] ToIndexedArray<T>(this T[] items)
        {
            int length = items.Length;
            var indexedKeys = AllocateArray<Indexed<T>>(length);
            for (int i = 0; i < length; ++i)
                indexedKeys[i].Set(items[i], i);
            return indexedKeys;
        }

        public static Indexed<TResult>[] MapIndexed<TValue, TResult>(this TValue[] items, Func<TValue, TResult> keySelector)
        {
            int length = items.Length;
            var indexedKeys = AllocateArray<Indexed<TResult>>(length);
            for (int i = 0; i < length; ++i)
                indexedKeys[i].Set(keySelector(items[i]), i);
            return indexedKeys;
        }

        public static T[] ToArray<T>(this IEnumerable<T> items, int expectedLength)
        {
            return items switch
            {
                T[] x => ArrayToArray(x, expectedLength),
                List<T> x => ListToArray(x, expectedLength),
                ICollection<T> x => ICollectionToArray(x, expectedLength),
                _ => EnumerableToArray(items, expectedLength)
            };
            static T[] ArrayToArray(T[] items, int expectedLength)
            {
                if (items.Length != expectedLength)
                    throw KeysAndValuesLengthMismatch();
                return (T[])items.Clone();
            }
            static T[] ListToArray(List<T> items, int expectedLength)
            {
                if (items.Count != expectedLength)
                    throw KeysAndValuesLengthMismatch();
                var array = items.ToArray();
                if (array.Length != expectedLength)
                    throw KeysAndValuesLengthMismatch();
                return array;
            }
            static T[] ICollectionToArray(ICollection<T> items, int expectedLength)
            {
                if (items.Count != expectedLength)
                    throw KeysAndValuesLengthMismatch();
                return EnumerableToArray(items, expectedLength);
            }
            static T[] EnumerableToArray(IEnumerable<T> items, int expectedLength)
            {
                var array = new T[expectedLength];
                using IEnumerator<T> enumerator = items.GetEnumerator();
                for (int i = 0; i < expectedLength; ++i)
                {
                    if (!enumerator.MoveNext())
                        throw KeysAndValuesLengthMismatch();
                    array[i] = enumerator.Current;
                }
                if (enumerator.MoveNext())
                    throw KeysAndValuesLengthMismatch();
                return array;
            }
        }

        public static TResult[] Map<TValue, TResult>(this TValue[] items, Func<TValue, TResult> keySelector)
        {
            int length = items.Length;
            var keys = AllocateArray<TResult>(length);
            for (int i = 0; i < length; ++i)
                keys[i] = keySelector(items[i]);
            return keys;
        }
    }
}
