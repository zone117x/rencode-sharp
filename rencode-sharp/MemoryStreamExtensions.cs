using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace rencodesharp
{
    public static class MemoryStreamExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, byte[] arr) => stream.Write(arr, 0, arr.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, byte b) => stream.WriteByte(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, sbyte b) => stream.WriteByte((byte)b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, char c) => stream.WriteByte((byte)c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, string x) => stream.Write(Util.GetBytes(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, short x) => stream.Write(BStruct.Pack(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, int x) => stream.Write(BStruct.Pack(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, long x) => stream.Write(BStruct.Pack(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, float x) => stream.Write(BStruct.Pack(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this MemoryStream stream, double x) => stream.Write(BStruct.Pack(x));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString(this MemoryStream stream, int stringLength)
        {
            var res = Encoding.UTF8.GetString(stream.GetBuffer(), (int)stream.Position, stringLength);
            stream.Position += stringLength;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDouble(this MemoryStream stream)
        {
            var res = BStruct.ToDouble(stream.GetBuffer(), (int)stream.Position);
            stream.Position += 8;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadSingle(this MemoryStream stream)
        {
            var res = BStruct.ToFloat(stream.GetBuffer(), (int)stream.Position);
            stream.Position += 4;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadSByte(this MemoryStream stream)
        {
            var res = BStruct.ToByte(stream.GetBuffer(), (int)stream.Position);
            stream.Position += 1;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16(this MemoryStream stream)
        {
            var res = BStruct.ToInt16(stream.GetBuffer(), (int)stream.Position);
            stream.Position += 2;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32(this MemoryStream stream)
        {
            var res = BStruct.ToInt32(stream.GetBuffer(), (int)stream.Position);
            stream.Position += 4;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64(this MemoryStream stream)
        {
            var res = BStruct.ToInt64(stream.GetBuffer(), (int)stream.Position);
            stream.Position += 8;
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this MemoryStream stream, byte item)
        {
            return Array.IndexOf(stream.GetBuffer(), item, (int)stream.Position);
        }


    }
}
