using System;
using System.Linq;
using MiscUtil.Conversion;

namespace rencodesharp
{
	public static class BStruct
	{
		/// <summary>
		/// Pack the object 'x' into (network order byte format).
		/// </summary>
		public static string Pack(object x, int n)
		{
			byte[] bytes = EndianBitConverter.Big.GetBytes(x);

			string output = string.Concat(bytes.Select(Convert.ToChar).Cast<object>().ToArray());

		    return output.Substring(output.Length - n, n);
		}

		/// <summary>
		/// Unpack the string 'x' (network order byte format) into object.
		/// </summary>
		public static object Unpack(string x, int n)
		{
			x = Util.StringPad(x, n);

		    byte[] bytes = x.Select(Convert.ToByte).ToArray();

		    switch (bytes.Length)
		    {
                case 1: return ToByte(bytes, 0);
                case 2: return ToInt16(bytes, 0);
                case 4: return ToInt32(bytes, 0);
                case 8: return ToInt64(bytes, 0);
            }

			return null;
		}

		/// <summary>
		/// Converts byte array to INT1 (8 bit integer)
		/// </summary>
		public static byte ToByte(byte[] value, int startIndex)
        {
            if (value.Length != 1)
                throw new ArgumentException("\"value\" doesn't have 1 byte.");
		    return value[0];
        }

		/// <summary>
		/// Converts byte array to INT2 (16 bit integer)
		/// </summary>
		public static short ToInt16(byte[] value, int startIndex)
		{
		    if (value.Length != 2)
		        throw new ArgumentException("\"value\" doesn't have 2 bytes.");

		    return EndianBitConverter.Big.ToInt16(value, startIndex);
		}

	    /// <summary>
		/// Converts byte array to INT4 (32 bit integer)
		/// </summary>
		public static int ToInt32(byte[] value, int startIndex)
	    {
	        if (value.Length != 4)
	            throw new ArgumentException("\"value\" doesn't have 4 bytes.");

            return EndianBitConverter.Big.ToInt32(value, startIndex);
	    }

	    /// <summary>
		/// Converts byte array to INT8 (64 bit integer)
		/// </summary>
		public static long ToInt64(byte[] value, int startIndex)
	    {
	        if (value.Length != 8)
	            throw new ArgumentException("\"value\" doesn't have 8 bytes.");

            return EndianBitConverter.Big.ToInt64(value, startIndex);
	    }

	    /// <summary>
		/// Converts byte array to Float (32 bit float)
		/// </summary>
		public static float ToFloat(byte[] value, int startIndex)
	    {
	        if (value.Length != 4)
	            throw new ArgumentException("\"value\" doesn't have 4 bytes.");

            return EndianBitConverter.Big.ToSingle(value, startIndex);
	    }

	    /// <summary>
		/// Converts byte array to Double (64 bit float)
		/// </summary>
		public static double ToDouble(byte[] value, int startIndex)
	    {
	        if (value.Length != 8)
	            throw new ArgumentException("\"value\" doesn't have 8 bytes.");

	        return EndianBitConverter.Big.ToDouble(value, startIndex);
	    }
	}
}

