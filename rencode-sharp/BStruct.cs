using System;
using System.Linq;
using MiscUtil.Conversion;
using System.IO;

namespace rencodesharp
{
	public static class BStruct
	{
		/// <summary>
		/// Pack the object 'x' into (network order byte format).
		/// </summary>
		public static byte[] Pack(object x, int n)
		{
            byte[] bytes = EndianBitConverter.Big.GetBytes(x);

            if (n != 0 && bytes.Length != n)
            {
                var slice = new byte[n];
                Buffer.BlockCopy(bytes, bytes.Length - n, slice, 0, n);
                bytes = slice;
            }

            return bytes;
		}

        public static byte[] Pack(short x) => EndianBitConverter.Big.GetBytes(x);
        public static byte[] Pack(int x) => EndianBitConverter.Big.GetBytes(x);
        public static byte[] Pack(long x) => EndianBitConverter.Big.GetBytes(x);
        public static byte[] Pack(float x) => EndianBitConverter.Big.GetBytes(x);
        public static byte[] Pack(double x) => EndianBitConverter.Big.GetBytes(x);
        public static byte[] Pack(decimal x) => EndianBitConverter.Big.GetBytes(x);

        /// <summary>
        /// Unpack the string 'x' (network order byte format) into object.
        /// </summary>
        public static object Unpack(byte[] x, int n)
		{
            if (n != 0 && x.Length != n)
            {
                var padded = new byte[x.Length + n];
                Buffer.BlockCopy(x, 0, padded, n, x.Length);
                x = padded;
            }

		    switch (x.Length)
		    {
                case 1: return ToByte(x, 0);
                case 2: return ToInt16(x, 0);
                case 4: return ToInt32(x, 0);
                case 8: return ToInt64(x, 0);
            }

			return null;
		}

		/// <summary>
		/// Converts byte array to INT1 (8 bit integer)
		/// </summary>
		public static sbyte ToByte(byte[] value, int startIndex)
        {
		    return (sbyte)value[startIndex];
        }

		/// <summary>
		/// Converts byte array to INT2 (16 bit integer)
		/// </summary>
		public static short ToInt16(byte[] value, int startIndex)
		{
		    return EndianBitConverter.Big.ToInt16(value, startIndex);
		}

	    /// <summary>
		/// Converts byte array to INT4 (32 bit integer)
		/// </summary>
		public static int ToInt32(byte[] value, int startIndex)
	    {
            return EndianBitConverter.Big.ToInt32(value, startIndex);
	    }

	    /// <summary>
		/// Converts byte array to INT8 (64 bit integer)
		/// </summary>
		public static long ToInt64(byte[] value, int startIndex)
	    {
            return EndianBitConverter.Big.ToInt64(value, startIndex);
	    }

	    /// <summary>
		/// Converts byte array to Float (32 bit float)
		/// </summary>
		public static float ToFloat(byte[] value, int startIndex)
	    {
            return EndianBitConverter.Big.ToSingle(value, startIndex);
	    }

	    /// <summary>
		/// Converts byte array to Double (64 bit float)
		/// </summary>
		public static double ToDouble(byte[] value, int startIndex)
	    {
	        return EndianBitConverter.Big.ToDouble(value, startIndex);
	    }
	}
}

