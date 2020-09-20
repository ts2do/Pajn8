namespace Pajn8
{
    internal struct Part
    {
        public int Start { get; set; }
        public int End { get; set; }
        public bool IsSorted { get; set; }

        public int Midpoint => (End - Start) / 2;
        public int Count => End - Start;
    }
}
