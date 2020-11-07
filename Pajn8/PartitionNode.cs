using System.Diagnostics;

namespace Pajn8
{
    internal class PartitionNode
    {
        public int StartIndex { get; }
        public int EndIndex { get; }
        public bool IsSorted { get; set; }
        public PartitionNode LeftNode { get; private set; }
        public PartitionNode RightNode { get; private set; }
        public int Depth { get; }

        public int Count => EndIndex - StartIndex;

        private int splitIndex = -1;

        public PartitionNode(int startIndex, int endIndex, int depth)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
            Depth = depth;
        }

        public PartitionNode Find(int index)
        {
            PartitionNode x = this;
            while (x.splitIndex >= 0)
            {
                if (index < x.splitIndex)
                    x = x.LeftNode;
                else
                    x = x.RightNode;
            }
            return x;
        }

        public void Split(int splitIndex)
        {
            Debug.Assert(!IsSorted);
            Debug.Assert(splitIndex > StartIndex && splitIndex < EndIndex);
            Debug.Assert(this.splitIndex < 0 && LeftNode == null && RightNode == null);

            this.splitIndex = splitIndex;
            LeftNode = new PartitionNode(StartIndex, splitIndex, Depth + 1);
            RightNode = new PartitionNode(splitIndex, EndIndex, Depth + 1);
        }
    }
}
