using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal static class ArrayUtils
    {
        internal static Exception KeysAndValuesLengthMismatch() => new ArgumentException(string.Format(Strings.Argument_MismatchedLength, "keys", "values"));

        internal static T[] AllocateArray<T>(int length) => new T[length]; // TODO: Replace with inlined calls to GC.AllocateUninitializedArray

        internal static Indexed<T>[] ToIndexedArray<T>(this IEnumerable<T> items, int expectedLength)
        {
            return items switch
            {
                T[] x => CreateFromArray(x, expectedLength),
                List<T> x => CreateFromList(x, expectedLength),
                _ => CreateFromEnumerable(items, expectedLength)
            };

            static Indexed<T>[] CreateFromArray(T[] array, int expectedLength)
            {
                int length = array.Length;
                if (length != expectedLength) throw KeysAndValuesLengthMismatch();
                var indexedKeys = AllocateArray<Indexed<T>>(length);
                for (int i = 0; i < length; ++i)
                {
                    ref var key = ref indexedKeys[i];
                    key.Value = array[i];
                    key.Index = i;
                }
                return indexedKeys;
            }

            static Indexed<T>[] CreateFromList(List<T> list, int expectedLength)
            {
                using List<T>.Enumerator enumerator = list.GetEnumerator();
                int length = list.Count;
                if (length != expectedLength) throw KeysAndValuesLengthMismatch();
                var indexedKeys = AllocateArray<Indexed<T>>(length);
                for (int i = 0; i < length; ++i)
                {
                    enumerator.MoveNext(); // Enforced by comodification checks
                    ref var key = ref indexedKeys[i];
                    key.Value = enumerator.Current;
                    key.Index = i;
                }
                return indexedKeys;
            }

            static Indexed<T>[] CreateFromEnumerable(IEnumerable<T> enumerable, int expectedLength)
            {
                var buffer = new ArrayBuffer<Indexed<T>>(16);
                foreach (var key in enumerable)
                {
                    ref var indexedKey = ref buffer.Add();
                    indexedKey.Value = key;
                    indexedKey.Index = buffer.Count;
                }
                if (buffer.Count != expectedLength) throw KeysAndValuesLengthMismatch();
                return buffer.ToArray();
            }
        }

        internal static StableComparable<T>[] ToStableComparableArray<T>(this IEnumerable<T> items, int expectedLength)
            where T : IComparable<T>
        {
            return items switch
            {
                T[] x => CreateFromArray(x, expectedLength),
                List<T> x => CreateFromList(x, expectedLength),
                _ => CreateFromEnumerable(items, expectedLength)
            };

            static StableComparable<T>[] CreateFromArray(T[] array, int expectedLength)
            {
                int length = array.Length;
                if (length != expectedLength) throw KeysAndValuesLengthMismatch();
                var indexedKeys = AllocateArray<StableComparable<T>>(length);
                for (int i = 0; i < length; ++i)
                {
                    ref var key = ref indexedKeys[i];
                    key.Value = array[i];
                    key.Index = i;
                }
                return indexedKeys;
            }

            static StableComparable<T>[] CreateFromList(List<T> list, int expectedLength)
            {
                using List<T>.Enumerator enumerator = list.GetEnumerator();
                int length = list.Count;
                if (length != expectedLength) throw KeysAndValuesLengthMismatch();
                var indexedKeys = AllocateArray<StableComparable<T>>(length);
                for (int i = 0; i < length; ++i)
                {
                    enumerator.MoveNext(); // Enforced by comodification checks
                    ref var key = ref indexedKeys[i];
                    key.Value = enumerator.Current;
                    key.Index = i;
                }
                return indexedKeys;
            }

            static StableComparable<T>[] CreateFromEnumerable(IEnumerable<T> enumerable, int expectedLength)
            {
                var buffer = new ArrayBuffer<StableComparable<T>>(16);
                foreach (var key in enumerable)
                {
                    ref var indexedKey = ref buffer.Add();
                    indexedKey.Value = key;
                    indexedKey.Index = buffer.Count;
                }
                if (buffer.Count != expectedLength) throw KeysAndValuesLengthMismatch();
                return buffer.ToArray();
            }
        }
    }
}
