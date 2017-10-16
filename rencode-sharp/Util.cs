using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rencodesharp
{
	public class Util
    {
        public static readonly Encoding ExtendedAsciiEncoding = Encoding.GetEncoding("iso-8859-1");

        /// <summary>
        /// Converts string to byte array.
        /// </summary>
        public static byte[] GetBytes(string s)
        {
            return ExtendedAsciiEncoding.GetBytes(s);
        }

        public static string GetString(byte[] bytes)
        {
            return ExtendedAsciiEncoding.GetString(bytes);
        }

        public static string GetString(byte[] bytes, int index, int count)
        {
            return ExtendedAsciiEncoding.GetString(bytes, index, count);
        }

        /// <summary>
        /// Pads the front of a string with NUL bytes.
        /// </summary>
        public static string StringPad(string x, int n)
		{
		    return x.PadLeft(n, '\x00');
        }
	}
}

