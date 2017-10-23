using System;
using rencodesharp;
using MiscUtil.Conversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rencodesharp_tests
{
	[TestClass]
	public class BStructTest
	{
		[TestMethod]
		public void Int1()
		{
			int a;

			a = BStruct.ToByte(new byte[] { 50 }, 0);
			Assert.AreEqual(50, a);

			a = BStruct.ToByte(new byte[] { 206 }, 0);
			Assert.AreEqual(-50, a);
		}

		[TestMethod]
		public void Int2()
		{
			int a;

			a = BStruct.ToInt16(new byte[] { 6, 4 }, 0);
			Assert.AreEqual(1540, a);

			a = BStruct.ToInt16(new byte[] { 1, 44 }, 0);
			Assert.AreEqual(300, a);

			a = BStruct.ToInt16(new byte[] { 254, 212 }, 0);
			Assert.AreEqual(-300, a);
		}

		[TestMethod]
		public void Int4()
		{
			var se = BStruct.Pack(1009025546);

			Assert.AreEqual(60, se[0]);
			Assert.AreEqual(36,	se[1]);
			Assert.AreEqual(130, se[2]);
			Assert.AreEqual(10, se[3]);

			int a;

			a = BStruct.ToInt32(new byte[] { 0, 0, 195, 80 }, 0);
			Assert.AreEqual(50000, a);

			a = BStruct.ToInt32(new byte[] { 255, 255, 60, 176 }, 0);
			Assert.AreEqual(-50000, a);
		}

		[TestMethod]
		public void Int8()
		{
			long a;

			a = BStruct.ToInt64(new byte[] { 0, 0, 0, 4, 168, 23, 200, 0 }, 0);
			Assert.AreEqual(20000000000, a);

			a = BStruct.ToInt64(new byte[] { 255, 255, 255, 251, 87, 232, 56, 0 }, 0);
			Assert.AreEqual(-20000000000, a);
		}
	}
}

