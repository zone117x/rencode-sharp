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
		    return string.Concat(r.ToArray());
		}

		/// <summary>
		/// Converts string to byte array.
		/// </summary>
		public static byte[] StringBytes(string s)
		{
		    return s.Select(Convert.ToByte).ToArray();
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

