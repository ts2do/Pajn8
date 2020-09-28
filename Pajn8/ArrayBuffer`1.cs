using System;

namespace Pajn8
{
    internal struct ArrayBuffer<T>
    {
        private const int BufferLimit = 1_000_000;

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

        public ref T Add()
        {
            if (Count - offset == bufLength)
            {
                bufLength = (int)Math.Min((uint)bufLength * 2, BufferLimit);
                buf = ArrayUtils.AllocateArray<T>(bufLength);
                offset = Count;
                tail = tail.Next = new Node(buf);
            }

            return ref buf[checked(Count++) - offset];
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
