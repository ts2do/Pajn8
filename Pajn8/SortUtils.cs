using System.Numerics;

namespace Pajn8
{
    internal static class SortUtils
    {
        public const int InsertionSortThreshold = 16;
        public const int TukeyNintherThreshold = 40;
        public const int PartitionMinimumPlay = 10;
        public const int PartitionMaximumPlay = 40;

        public static int DepthLimit(int length) => 64 - 2 * BitOperations.LeadingZeroCount((uint)length);
    }
}
