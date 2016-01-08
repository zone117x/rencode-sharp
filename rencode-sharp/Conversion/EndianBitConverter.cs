// http://www.yoda.arachsys.com/csharp/miscutil/
//
// Modifications: Added GetBytes(object)

using System;
using System.Collections.Generic;

namespace MiscUtil.Conversion
{
	/// <summary>
	/// Equivalent of System.BitConverter, but with either endianness.
	/// </summary>
	public class EndianBitConverter
	{
	    public EndianBitConverter(Endianness endianness)
	    {
	        Endianness = endianness;
	    }

		#region Endianness of this converter

	    /// <summary>
	    /// Indicates the byte order ("endianess") in which data is converted using this class.
	    /// </summary>
	    /// <remarks>
	    /// Different computer architectures store data using different byte orders. "Big-endian"
	    /// means the most significant byte is on the left end of a word. "Little-endian" means the 
	    /// most significant byte is on the right end of a word.
	    /// </remarks>
	    /// <value>true if this converter is little-endian, false otherwise.</value>
	    public bool IsLittleEndian => Endianness == Endianness.LittleEndian;

	    /// <summary>
		/// Indicates the byte order ("endianess") in which data is converted using this class.
		/// </summary>
		public Endianness Endianness { get; }
		#endregion

		#region Factory properties

	    /// <summary>
		/// Returns a little-endian bit converter instance. The same instance is
		/// always returned.
		/// </summary>
		public static EndianBitConverter Little { get; } = new EndianBitConverter(Endianness.LittleEndian);

	    /// <summary>
		/// Returns a big-endian bit converter instance. The same instance is
		/// always returned.
		/// </summary>
		public static EndianBitConverter Big { get; } = new EndianBitConverter(Endianness.BigEndian);

	    #endregion

		#region Double/primitive conversions
		/// <summary>
		/// Converts the specified double-precision floating point number to a 
		/// 64-bit signed integer. Note: the endianness of this converter does not
		/// affect the returned value.
		/// </summary>
		/// <param name="value">The number to convert. </param>
		/// <returns>A 64-bit signed integer whose value is equivalent to value.</returns>
		public long DoubleToInt64Bits(double value)
		{
			return BitConverter.DoubleToInt64Bits(value);
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to a double-precision 
		/// floating point number. Note: the endianness of this converter does not
		/// affect the returned value.
		/// </summary>
		/// <param name="value">The number to convert. </param>
		/// <returns>A double-precision floating point number whose value is equivalent to value.</returns>
		public double Int64BitsToDouble(long value)
		{
			return BitConverter.Int64BitsToDouble(value);
		}

		/// <summary>
		/// Converts the specified single-precision floating point number to a 
		/// 32-bit signed integer. Note: the endianness of this converter does not
		/// affect the returned value.
		/// </summary>
		/// <param name="value">The number to convert. </param>
		/// <returns>A 32-bit signed integer whose value is equivalent to value.</returns>
		public int SingleToInt32Bits(float value)
		{
		    return (int)value;
		}

		/// <summary>
		/// Converts the specified 32-bit signed integer to a single-precision floating point 
		/// number. Note: the endianness of this converter does not
		/// affect the returned value.
		/// </summary>
		/// <param name="value">The number to convert. </param>
		/// <returns>A single-precision floating point number whose value is equivalent to value.</returns>
		public float Int32BitsToSingle (int value)
		{
			return (float)value;
		}

		#endregion

		#region To(PrimitiveType) conversions
		/// <summary>
		/// Returns a Boolean value converted from one byte at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>true if the byte at startIndex in value is nonzero; otherwise, false.</returns>
		public bool ToBoolean (byte[] value, int startIndex)
		{
			CheckByteArgument(value, startIndex, 1);
			return BitConverter.ToBoolean(value, startIndex);
		}

		/// <summary>
		/// Returns a Unicode character converted from two bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A character formed by two bytes beginning at startIndex.</returns>
		public char ToChar (byte[] value, int startIndex)
        {
            byte[] bytes = GetCorrectedBytes(value, startIndex, 8);
            return BitConverter.ToChar(bytes, 0);
        }

		/// <summary>
		/// Returns a double-precision floating point number converted from eight bytes 
		/// at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A double precision floating point number formed by eight bytes beginning at startIndex.</returns>
		public double ToDouble (byte[] value, int startIndex)
        {
            byte[] bytes = GetCorrectedBytes(value, startIndex, 8);
            return BitConverter.ToDouble(bytes, 0);
        }

		/// <summary>
		/// Returns a single-precision floating point number converted from four bytes 
		/// at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A single precision floating point number formed by four bytes beginning at startIndex.</returns>
		public float ToSingle (byte[] value, int startIndex)
		{
		    byte[] corrected = GetCorrectedBytes(value, startIndex, 4);
            return BitConverter.ToSingle(corrected, 0);
        }

	    /// <summary>
		/// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A 16-bit signed integer formed by two bytes beginning at startIndex.</returns>
		public short ToInt16 (byte[] value, int startIndex)
        {
            byte[] bytes = GetCorrectedBytes(value, startIndex, 8);
            return BitConverter.ToInt16(bytes, 0);
        }

		/// <summary>
		/// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A 32-bit signed integer formed by four bytes beginning at startIndex.</returns>
		public int ToInt32 (byte[] value, int startIndex)
        {
            byte[] bytes = GetCorrectedBytes(value, startIndex, 8);
            return BitConverter.ToInt32(bytes, 0);
        }

		/// <summary>
		/// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A 64-bit signed integer formed by eight bytes beginning at startIndex.</returns>
		public long ToInt64 (byte[] value, int startIndex)
        {
            byte[] bytes = GetCorrectedBytes(value, startIndex, 8);
            return BitConverter.ToInt64(bytes, 0);
		}

		/// <summary>
		/// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A 16-bit unsigned integer formed by two bytes beginning at startIndex.</returns>
		public ushort ToUInt16 (byte[] value, int startIndex)
        {
            byte[] bytes = GetCorrectedBytes(value, startIndex, 2);
            return BitConverter.ToUInt16(bytes, 0);
        }

		/// <summary>
		/// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A 32-bit unsigned integer formed by four bytes beginning at startIndex.</returns>
		public uint ToUInt32 (byte[] value, int startIndex)
        {
            byte[] bytes = GetCorrectedBytes(value, startIndex, 4);
            return BitConverter.ToUInt32(bytes, 0);
        }

		/// <summary>
		/// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A 64-bit unsigned integer formed by eight bytes beginning at startIndex.</returns>
		public ulong ToUInt64 (byte[] value, int startIndex)
		{
		    byte[] bytes = GetCorrectedBytes(value, startIndex, 8);
		    return BitConverter.ToUInt64(bytes, 0);
		}

		/// <summary>
		/// Checks the given argument for validity.
		/// </summary>
		/// <param name="value">The byte array passed in</param>
		/// <param name="startIndex">The start index passed in</param>
		/// <param name="bytesRequired">The number of bytes required</param>
		/// <exception cref="ArgumentNullException">value is a null reference</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// startIndex is less than zero or greater than the length of value minus bytesRequired.
		/// </exception>
		private static void CheckByteArgument(byte[] value, int startIndex, int bytesRequired)
		{
		    if (value == null)
		        throw new ArgumentNullException(nameof(value));
            if (bytesRequired <= 0)
                throw new ArgumentOutOfRangeException(nameof(bytesRequired));
            if (startIndex < 0 || startIndex > value.Length - bytesRequired)
		        throw new ArgumentOutOfRangeException(nameof(startIndex));
		}

		#endregion

		#region ToString conversions
		/// <summary>
		/// Returns a String converted from the elements of a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <remarks>All the elements of value are converted.</remarks>
		/// <returns>
		/// A String of hexadecimal pairs separated by hyphens, where each pair 
		/// represents the corresponding element in value; for example, "7F-2C-4A".
		/// </returns>
		public static string ToString(byte[] value)
		{
			return BitConverter.ToString(value);
		}

		/// <summary>
		/// Returns a String converted from the elements of a byte array starting at a specified array position.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <remarks>The elements from array position startIndex to the end of the array are converted.</remarks>
		/// <returns>
		/// A String of hexadecimal pairs separated by hyphens, where each pair 
		/// represents the corresponding element in value; for example, "7F-2C-4A".
		/// </returns>
		public static string ToString(byte[] value, int startIndex)
		{
			return BitConverter.ToString(value, startIndex);
		}

		/// <summary>
		/// Returns a String converted from a specified number of bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <param name="length">The number of bytes to convert.</param>
		/// <remarks>The length elements from array position startIndex are converted.</remarks>
		/// <returns>
		/// A String of hexadecimal pairs separated by hyphens, where each pair 
		/// represents the corresponding element in value; for example, "7F-2C-4A".
		/// </returns>
		public static string ToString(byte[] value, int startIndex, int length)
		{
			return BitConverter.ToString(value, startIndex, length);
		}
		#endregion

		#region	Decimal conversions
		/// <summary>
		/// Returns a decimal value converted from sixteen bytes 
		/// at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within value.</param>
		/// <returns>A decimal  formed by sixteen bytes beginning at startIndex.</returns>
		public decimal ToDecimal (byte[] value, int startIndex)
		{
			// HACK: This always assumes four parts, each in their own endianness,
			// starting with the first part at the start of the byte array.
			// On the other hand, there's no real format specified...
			var bits = new int[4];

            for (int i = 0; i < 16; i+=4)
		        bits[i] = ToInt32(value, startIndex + i);

            return new decimal(bits);
		}

		/// <summary>
		/// Returns the specified decimal value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 16.</returns>
		public byte[] GetBytes(decimal value)
		{
			int[] bits = decimal.GetBits(value);

		    var bytes = new List<byte>();
            for (int i = 0; i < bits.Length && bytes.Count < 16; i++)
                bytes.AddRange(GetBytes(bits[i]));

		    return bytes.ToArray();
		}

		#endregion

		#region GetBytes conversions

	    public byte[] GetBytes(object value)
	    {
	        Type type = value.GetType();
            
	        if (type == typeof(decimal)) return GetBytes((decimal)value);
	        if (type == typeof(long))    return GetBytes((long)value);
	        if (type == typeof(bool))    return GetBytes((bool)value);
	        if (type == typeof(char))    return GetBytes((char)value);
	        if (type == typeof(double))  return GetBytes((double)value);
	        if (type == typeof(short))   return GetBytes((short)value);
	        if (type == typeof(int))     return GetBytes((int)value);
	        if (type == typeof(long))    return GetBytes((long)value);
	        if (type == typeof(float))   return GetBytes((float)value);
	        if (type == typeof(ushort))  return GetBytes((ushort)value);
	        if (type == typeof(uint))    return GetBytes((uint)value);
	        if (type == typeof(ulong))   return GetBytes((ulong)value);
	        if (type == typeof(byte))    return new[] {(byte)value};

	        throw new NotImplementedException();
	    }

		/// <summary>
		/// Returns the specified Boolean value as an array of bytes.
		/// </summary>
		/// <param name="value">A Boolean value.</param>
		/// <returns>An array of bytes with length 1.</returns>
		public byte[] GetBytes(bool value)
		{
		    byte[] bytes = BitConverter.GetBytes(value);
		    FixEndianess(bytes);
		    return bytes;
		}

		/// <summary>
		/// Returns the specified Unicode character value as an array of bytes.
		/// </summary>
		/// <param name="value">A character to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		public byte[] GetBytes(char value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }

		/// <summary>
		/// Returns the specified double-precision floating point value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		public byte[] GetBytes(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }
		
		/// <summary>
		/// Returns the specified 16-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		public byte[] GetBytes(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }

		/// <summary>
		/// Returns the specified 32-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		public byte[] GetBytes(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }

		/// <summary>
		/// Returns the specified 64-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		public byte[] GetBytes(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }

		/// <summary>
		/// Returns the specified single-precision floating point value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		public byte[] GetBytes(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }

		/// <summary>
		/// Returns the specified 16-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		public byte[] GetBytes(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }

		/// <summary>
		/// Returns the specified 32-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		public byte[] GetBytes(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            FixEndianess(bytes);
            return bytes;
        }

		/// <summary>
		/// Returns the specified 64-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		public byte[] GetBytes(ulong value)
		{
		    byte[] bytes = BitConverter.GetBytes(value);
		    FixEndianess(bytes);
		    return bytes;
        }

        private byte[] GetCorrectedBytes(byte[] bytes, int startIndex, int count)
        {
            CheckByteArgument(bytes, startIndex, count);

            var copy = new byte[count];
            Array.Copy(bytes, startIndex, copy, 0, copy.Length);
            FixEndianess(copy);
            return copy;
        }

        private void FixEndianess(byte[] bytes)
	    {
            if (BitConverter.IsLittleEndian != IsLittleEndian)
                Array.Reverse(bytes);
        }

	    #endregion
    }
}
