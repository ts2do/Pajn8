using System;
using System.Collections.Generic;

namespace Pajn8
{
    internal static class ArrayUtils
    {
        public static Exception KeysAndValuesLengthMismatch() => new ArgumentException(string.Format(Strings.Arg_MismatchedLength, "keys", "values"));

        public static T[] AllocateArray<T>(int length) => new T[length]; // TODO: Replace with inlined calls to GC.AllocateUninitializedArray

        public static Indexed<T>[] ToIndexedArray<T>(this IEnumerable<T> items, int expectedLength)
        {
            return items switch
            {
                T[] x => x.ToIndexedArray(expectedLength),
                List<T> x => CreateFromList(x, expectedLength),
                _ => CreateFromEnumerable(items, expectedLength)
            };

            static Indexed<T>[] CreateFromList(List<T> list, int expectedLength)
            {
                using List<T>.Enumerator enumerator = list.GetEnumerator();
                int length = list.Count;
                if (length != expectedLength) throw KeysAndValuesLengthMismatch();
                var indexedKeys = AllocateArray<Indexed<T>>(length);
                for (int i = 0; i < length; ++i)
                {
                    enumerator.MoveNext(); // Enforced by comodification checks
                    indexedKeys[i].Set(enumerator.Current, i);
                }
                return indexedKeys;
            }

            static Indexed<T>[] CreateFromEnumerable(IEnumerable<T> enumerable, int expectedLength)
            {
                var buffer = new ArrayBuffer<Indexed<T>>(16);
                foreach (var key in enumerable)
                    buffer.Add().Set(key, buffer.Count);
                if (buffer.Count != expectedLength) throw KeysAndValuesLengthMismatch();
                return buffer.ToArray();
            }
        }

        public static Indexed<T>[] ToIndexedArray<T>(this T[] items, int expectedLength)
        {
            int length = items.Length;
            if (length != expectedLength) throw KeysAndValuesLengthMismatch();
            var indexedKeys = AllocateArray<Indexed<T>>(length);
            for (int i = 0; i < length; ++i)
                indexedKeys[i].Set(items[i], i);
            return indexedKeys;
        }
    }
}
