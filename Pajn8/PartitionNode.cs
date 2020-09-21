using System.Diagnostics;

namespace Pajn8
{
    internal class PartitionNode
    {
        public int StartIndex { get; }
        public int EndIndex { get; }
        public bool IsSorted { get; set; }

        public int Count => EndIndex - StartIndex;

        private int splitIndex = -1;
        private PartitionNode leftNode;
        private PartitionNode rightNode;

        public PartitionNode(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public PartitionNode Find(int index)
        {
            PartitionNode x = this;
            while (x.splitIndex >= 0)
            {
                if (index < x.splitIndex)
                    x = x.leftNode;
                else
                    x = x.rightNode;
            }
            return x;
        }

        public void Split(int splitIndex)
        {
            Debug.Assert(!IsSorted);
            Debug.Assert(splitIndex > StartIndex && splitIndex < EndIndex);
            Debug.Assert(this.splitIndex < 0 && leftNode == null && rightNode == null);

            this.splitIndex = splitIndex;

            leftNode = new PartitionNode(StartIndex, splitIndex);

            rightNode = new PartitionNode(splitIndex, EndIndex);
        }
    }
}
