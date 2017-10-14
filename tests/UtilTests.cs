using System;
using rencodesharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rencodesharp_tests
{
	[TestClass]
	public class UtilTests
	{
		[TestMethod]
		public void StringBytes()
		{
			Assert.AreEqual(16, (int)Util.StringBytes("\x10")[0]);
			Assert.AreEqual(127, (int)Util.StringBytes("\x7F")[0]);
			Assert.AreEqual(100, (int)Util.StringBytes("\x64")[0]);
		}
	}
}

