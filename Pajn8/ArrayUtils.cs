using System;
using System.Collections.Generic;

namespace Pajn8
{
    internal static class ArrayUtils
    {
        public static Exception KeysAndValuesLengthMismatch() => new ArgumentException(string.Format(Strings.Argument_MismatchedLength, "keys", "values"));

        public static T[] AllocateArray<T>(int length) => new T[length]; // TODO: Replace with inlined calls to GC.AllocateUninitializedArray

        public static TIndexed[] ToIndexedArray<T, TIndexed>(this IEnumerable<T> items, int expectedLength)
            where TIndexed : IIndexed<T>
        {
            return items switch
            {
                T[] x => x.ToIndexedArray<T, TIndexed>(expectedLength),
                List<T> x => CreateFromList(x, expectedLength),
                _ => CreateFromEnumerable(items, expectedLength)
            };

            static TIndexed[] CreateFromList(List<T> list, int expectedLength)
            {
                using List<T>.Enumerator enumerator = list.GetEnumerator();
                int length = list.Count;
                if (length != expectedLength) throw KeysAndValuesLengthMismatch();
                var indexedKeys = AllocateArray<TIndexed>(length);
                for (int i = 0; i < length; ++i)
                {
                    enumerator.MoveNext(); // Enforced by comodification checks
                    indexedKeys[i].Set(enumerator.Current, i);
                }
                return indexedKeys;
            }

            static TIndexed[] CreateFromEnumerable(IEnumerable<T> enumerable, int expectedLength)
            {
                var buffer = new ArrayBuffer<TIndexed>(16);
                foreach (var key in enumerable)
                    buffer.Add().Set(key, buffer.Count);
                if (buffer.Count != expectedLength) throw KeysAndValuesLengthMismatch();
                return buffer.ToArray();
            }
        }

        public static TIndexed[] ToIndexedArray<T, TIndexed>(this T[] items, int expectedLength)
            where TIndexed : IIndexed<T>
        {
            int length = items.Length;
            if (length != expectedLength) throw KeysAndValuesLengthMismatch();
            var indexedKeys = AllocateArray<TIndexed>(length);
            for (int i = 0; i < length; ++i)
                indexedKeys[i].Set(items[i], i);
            return indexedKeys;
        }
    }
}
