using System;
using System.Text;
using rencodesharp;
using MiscUtil.Conversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rencodesharp_tests
{
	[TestClass]
	public class PackTest
	{
		[TestMethod]
		public void BasicPack()
		{
			Assert.AreEqual(120, BStruct.Pack(120, 1)[0]);
			Assert.AreEqual(246, BStruct.Pack(-10, 1)[0]);
			Assert.AreEqual(226, BStruct.Pack(-30, 1)[0]);

			Assert.AreEqual(125, BStruct.Pack(32000, 2)[0]);
			Assert.AreEqual(131, BStruct.Pack(-32000, 2)[0]);

			Assert.AreEqual("\x00\x01\x86\xa0", Util.GetString(BStruct.Pack(100000, 4)));
			Assert.AreEqual("\xff\xfey`", Util.GetString(BStruct.Pack(-100000, 4)));
		}

		[TestMethod]
		public void BasicUnpack()
		{
			//Assert.AreEqual(246, BStruct.ToBytes(-10)[0]);

			Assert.AreEqual((sbyte)120, BStruct.Unpack(BStruct.Pack(120, 1), 1));

			var a = BStruct.Pack(-50, 1);
			Assert.AreEqual(206, a[0]);
			Assert.AreEqual((sbyte)-50, BStruct.Unpack(a, 1));

			Assert.AreEqual((short)32000, BStruct.Unpack(BStruct.Pack(32000, 2), 2));
		}

		[TestMethod]
		public void Pad()
		{
			Assert.AreEqual("\x00" + "Hello", Util.StringPad("Hello", 6));
		}
	}
}

