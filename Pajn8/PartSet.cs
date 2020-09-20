using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Pajn8
{
    internal struct PartSet
    {
        private const int BlockSize = 8192;
        private List<Part[]> partBlocks;
        private Part[] lastPartBlock;
        private int nextId;
        private int[] partIds;

        public void Init(int count)
        {
            partBlocks = new List<Part[]>();
            partIds = new int[count];
            Add(out _).End = count;
        }

        public ref Part this[int index]
        {
            get
            {
                int id = partIds[index];
                return ref partBlocks[id / BlockSize][id % BlockSize];
            }
        }

        private ref Part Add(out int index)
        {
            if (nextId % BlockSize == 0)
            {
                // TODO: Replace with call to GC.AllocateUninitializedArray
                partBlocks.Add(lastPartBlock = new Part[BlockSize]);
            }

            index = nextId++;
            return ref lastPartBlock[index % BlockSize];
        }

        public void Split(ref Part p, int splitIndex)
        {
            Debug.Assert(!p.IsSorted);
            Debug.Assert(splitIndex > p.Start && splitIndex < p.End);

            int end = p.End;
            p.End = splitIndex;

            ref Part p2 = ref Add(out int id);
            p2.Start = splitIndex;
            p2.End = end;
            p2.IsSorted = false;

            ref int partIdsHead = ref partIds[0];

            for (int i = splitIndex; i < end; ++i)
                Unsafe.Add(ref partIdsHead, i) = id;
        }
    }
}
