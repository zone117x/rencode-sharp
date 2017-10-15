using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rencodesharp
{
	public class Util
	{
		/// <summary>
		/// Join the List of objects by converting each item to a string.
		/// </summary>
		public static string Join(List<object> r)
		{
            return string.Concat(r);
		}

		/// <summary>
		/// Converts string to byte array.
		/// </summary>
		public static byte[] StringBytes(string s)
		{
            var arr = new byte[s.Length];
            for (var i = 0; i < s.Length; i++)
            {
                arr[i] = (byte)s[i];
            }
            return arr;
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

