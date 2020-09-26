using System;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct ArrayBuffer<T>
    {
        private T[] buf;
        private int bufLength;
        private readonly Node head;
        private Node tail;
        private int offset;

        public int Count { get; private set; }

        private sealed class Node
        {
            public readonly T[] Array;
            public Node Next;

            public Node(T[] array)
            {
                Array = array;
            }
        }

        public ArrayBuffer(int initLength)
        {
            var array = new T[initLength];
            buf = array;
            bufLength = initLength;
            head = tail = new Node(array);
            offset = 0;
            Count = 0;
        }

        public void Add(in T value)
        {
            if (Count - offset < bufLength)
            {
                buf[Count++ - offset] = value;
                return;
            }

            AddWithResize(value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AddWithResize(in T value)
        {
            buf = new T[bufLength *= 2];
            offset = Count;
            tail = tail.Next = new Node(buf);
            buf[Count++ - offset] = value;
        }

        public ref T Add()
        {
            if (Count - offset == bufLength)
            {
                buf = new T[bufLength *= 2];
                offset = Count;
                tail = tail.Next = new Node(buf);
            }

            return ref buf[Count++ - offset];
        }

        public T[] ToArray()
        {
            var array = new T[Count];
            int index = 0;
            for (Node node = head, tail = this.tail; node != tail; node = node.Next)
            {
                node.Array.CopyTo(array, index);
                index += node.Array.Length;
            }
            Array.Copy(buf, 0, array, index, Count - index);
            return array;
        }
    }
}
