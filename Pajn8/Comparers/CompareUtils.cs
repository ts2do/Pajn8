using System;
using System.Runtime.CompilerServices;

namespace Pajn8.Utils
{
    internal static class CompareUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanNullUnchecked<T>(ref T x, ref T y)
            where T : IComparable<T>
        {
            if (typeof(T) == typeof(byte)) return (byte)(object)x < (byte)(object)y ? true : false;
            if (typeof(T) == typeof(sbyte)) return (sbyte)(object)x < (sbyte)(object)y ? true : false;
            if (typeof(T) == typeof(ushort)) return (ushort)(object)x < (ushort)(object)y ? true : false;
            if (typeof(T) == typeof(short)) return (short)(object)x < (short)(object)y ? true : false;
            if (typeof(T) == typeof(uint)) return (uint)(object)x < (uint)(object)y ? true : false;
            if (typeof(T) == typeof(int)) return (int)(object)x < (int)(object)y ? true : false;
            if (typeof(T) == typeof(ulong)) return (ulong)(object)x < (ulong)(object)y ? true : false;
            if (typeof(T) == typeof(long)) return (long)(object)x < (long)(object)y ? true : false;
            if (typeof(T) == typeof(float)) return (float)(object)x < (float)(object)y ? true : false;
            if (typeof(T) == typeof(double)) return (double)(object)x < (double)(object)y ? true : false;
            return x.CompareTo(y) < 0 ? true : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanNullUnchecked<T>(ref T x, ref T y)
            where T : IComparable<T>
        {
            if (typeof(T) == typeof(byte)) return (byte)(object)x > (byte)(object)y ? true : false;
            if (typeof(T) == typeof(sbyte)) return (sbyte)(object)x > (sbyte)(object)y ? true : false;
            if (typeof(T) == typeof(ushort)) return (ushort)(object)x > (ushort)(object)y ? true : false;
            if (typeof(T) == typeof(short)) return (short)(object)x > (short)(object)y ? true : false;
            if (typeof(T) == typeof(uint)) return (uint)(object)x > (uint)(object)y ? true : false;
            if (typeof(T) == typeof(int)) return (int)(object)x > (int)(object)y ? true : false;
            if (typeof(T) == typeof(ulong)) return (ulong)(object)x > (ulong)(object)y ? true : false;
            if (typeof(T) == typeof(long)) return (long)(object)x > (long)(object)y ? true : false;
            if (typeof(T) == typeof(float)) return (float)(object)x > (float)(object)y ? true : false;
            if (typeof(T) == typeof(double)) return (double)(object)x > (double)(object)y ? true : false;
            return x.CompareTo(y) > 0 ? true : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Compare<T>(ref T x, ref T y)
            where T : IComparable<T>
        {
            if (typeof(T) == typeof(byte)) return (byte)(object)x - (byte)(object)y;
            if (typeof(T) == typeof(sbyte)) return (sbyte)(object)x - (sbyte)(object)y;
            if (typeof(T) == typeof(ushort)) return (ushort)(object)x - (ushort)(object)y;
            if (typeof(T) == typeof(short)) return (short)(object)x - (short)(object)y;
            if (typeof(T) == typeof(uint)) return Math.Sign((long)(uint)(object)x - (uint)(object)y);
            if (typeof(T) == typeof(int)) return Math.Sign((long)(int)(object)x - (int)(object)y);
            return x.CompareTo(y);
        }
    }
}
