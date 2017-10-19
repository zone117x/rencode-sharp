using System;
using System.Collections;
using System.Collections.Generic;
using rencodesharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace rencodesharp_tests
{
	[TestClass]
	public class RencodeTest
	{
		[TestMethod]
		public void String()
		{
			// ENCODE STRING
			Assert.AreEqual("\x85Hello", Rencode.EncodeToString("Hello"));
			Assert.AreEqual("78:abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", Rencode.EncodeToString("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz"));

			// DECODE STRING
			Assert.AreEqual("Hello", Rencode.Decode("\x85Hello"));
			Assert.AreEqual("abcdefghij", Rencode.Decode("10:abcdefghij"));
		}

        [TestMethod]
        public void StringUnicode()
        {
            Assert.AreEqual("fööbar", Rencode.Decode(Rencode.Encode("fööbar")));
        }

        [TestMethod]
        public void StringLarge()
        {
            var largeStr = new string(Enumerable.Repeat('f', 9000).ToArray());
            var encoded = Rencode.Encode(largeStr);
            var decoded = Rencode.Decode(encoded);

            Assert.AreEqual(largeStr, decoded);
        }

        [TestMethod]
		public void Integer()
		{
			// ENCODE INT1
			Assert.AreEqual((char)RencodeConst.CHR_INT1 + "\x78", Rencode.EncodeToString(120));
			Assert.AreEqual((char)RencodeConst.CHR_INT1 + "\x88", Rencode.EncodeToString(-120));

			// ENCODE INT2
			Assert.AreEqual((char)RencodeConst.CHR_INT2 + "\x06\x04", Rencode.EncodeToString(1540));
			Assert.AreEqual((char)RencodeConst.CHR_INT2 + "\xF9\xFC", Rencode.EncodeToString(-1540));

			// ENCODE INT4
			Assert.AreEqual((char)RencodeConst.CHR_INT4 + "\x7F\xff\xff\xd0", Rencode.EncodeToString(2147483600));
			Assert.AreEqual((char)RencodeConst.CHR_INT4 + "\x80\x00\x00\x30", Rencode.EncodeToString(-2147483600));

			// ENCODE INT8
			Assert.AreEqual(65,  Rencode.Encode(9223372036854700000L)[0]);
			Assert.AreEqual(127, Rencode.Encode(9223372036854700000L)[1]);
			Assert.AreEqual(255, Rencode.Encode(9223372036854700000L)[2]);
			Assert.AreEqual(255, Rencode.Encode(9223372036854700000L)[3]);
			Assert.AreEqual((char)RencodeConst.CHR_INT8 + "\x7F\xFF\xFF\xFF\xFF\xFE\xD7\xE0", Rencode.EncodeToString(9223372036854700000L));
			Assert.AreEqual((char)RencodeConst.CHR_INT8 + "\x80\x00\x00\x00\x00\x01( ", Rencode.EncodeToString(-9223372036854700000L));

			// DECODE INT
			Assert.AreEqual((short)1000, Rencode.Decode((char)RencodeConst.CHR_INT + "1000" + (char)RencodeConst.CHR_TERM));

			// DECODE INT1
			Assert.AreEqual((sbyte)120, Rencode.Decode((char)RencodeConst.CHR_INT1 + "\x78"));
			Assert.AreEqual((sbyte)-120, Rencode.Decode((char)RencodeConst.CHR_INT1 + "\x88"));

			// DECODE INT2
			Assert.AreEqual((short)1540, Rencode.Decode((char)RencodeConst.CHR_INT2 + "\x06\x04"));
			Assert.AreEqual((short)-1540, Rencode.Decode((char)RencodeConst.CHR_INT2 + "\xF9\xFC"));

			// DECODE INT4
			Assert.AreEqual(2147483600, Rencode.Decode((char)RencodeConst.CHR_INT4 + "\x7f\xff\xff\xd0"));
			Assert.AreEqual(-2147483600, Rencode.Decode((char)RencodeConst.CHR_INT4 + "\x80\x00\x00\x30"));

			// DECODE INT8
			Assert.AreEqual(9223372036854700000L, Rencode.Decode((char)RencodeConst.CHR_INT8 + "\x7F\xFF\xFF\xFF\xFF\xFE\xD7\xE0"));
			Assert.AreEqual(-9223372036854700000L, Rencode.Decode((char)RencodeConst.CHR_INT8 + "\x80\x00\x00\x00\x00\x01( "));
		}

		[TestMethod]
		public void List()
		{
			CollectionAssert.AreEqual(new object[] { "one", "two", "three" },
				(ICollection)Rencode.Decode(
					Rencode.Encode(new object[] { "one", "two", "three" })
				)
			);

            CollectionAssert.AreEqual(new object[] { 1, 2, 3 },
                (ICollection)Rencode.Decode(
					Rencode.Encode(new object[] { 1, 2, 3 })
				)
			);

            var decodedIntList = (ICollection)Rencode.Decode(Rencode.Encode(new object[] { -1, -2, -3 }));
            CollectionAssert.AreEqual(new object[] { -1, -2, -3 }, decodedIntList);


            var multiDimStrArr = new object[] {
                    new object[] { "one", "two", "three" },
                    new object[] { "four", "five", "six" }
                };

            var multiDimStrArrRoundTrip = ((ICollection)Rencode.Decode(
                    Rencode.Encode(new object[] {
                        new object[] { "one", "two", "three" },
                        new object[] { "four", "five", "six" }
                    })
                )).Cast<object>().Select(s => (ICollection)s).ToArray();

            CollectionAssert.AreEqual((ICollection)multiDimStrArr[0], multiDimStrArrRoundTrip[0]);
            CollectionAssert.AreEqual((ICollection)multiDimStrArr[1], multiDimStrArrRoundTrip[1]);


            var multiDimIntArr = new object[] {
                    new object[] { 1, 2, 3 },
                    new object[] { 4, 5, 6 }
                };

            var multiDimIntArrRoundTrip = ((ICollection)Rencode.Decode(
                    Rencode.Encode(new object[] {
                        new object[] { 1, 2, 3 },
                        new object[] { 4, 5, 6 }
                    })
                )).Cast<object>().Select(s => (ICollection)s).ToArray();

            CollectionAssert.AreEqual((ICollection)multiDimIntArr[0], multiDimIntArrRoundTrip[0]);
            CollectionAssert.AreEqual((ICollection)multiDimIntArr[1], multiDimIntArrRoundTrip[1]);

		}

        [TestMethod]
        public void ListNonFixed()
        {
            object[] non_fixed_list_test = new object[100];
            Random rand = new Random();
            for (int i = 0; i < non_fixed_list_test.Length; i++)
            {
                non_fixed_list_test[i] = rand.Next();
            }
            var dump = Rencode.Encode(non_fixed_list_test);
            Assert.AreEqual(RencodeConst.CHR_LIST, dump[0]);
            Assert.AreEqual(RencodeConst.CHR_TERM, dump[dump.Length - 1]);
            CollectionAssert.AreEqual(non_fixed_list_test, (ICollection)Rencode.Decode(dump));
        }

		[TestMethod]
		public void Dict()
		{
			Dictionary<object, object> dOne = new Dictionary<object, object> {
				{"Hello", 12},
				{"Blah", 15}
			};

			// Test Encode
			var dump = Rencode.Encode(dOne);
			Assert.AreEqual(((char)(RencodeConst.DICT_FIXED_START + 2)).ToString() +
			                ((char)(RencodeConst.STR_FIXED_START + 5)).ToString() +
			                "Hello" +
			                ((char)12) + 
			                ((char)(RencodeConst.STR_FIXED_START + 4)).ToString() +
			                "Blah" +
			                ((char)15), Util.GetString(dump));

            CollectionAssert.AreEqual(dOne, (ICollection)Rencode.Decode(dump));


			Dictionary<object, object> dTwo = new Dictionary<object, object>();
			for(int i = 0; i < 35; i++)
			{
				dTwo.Add(i.ToString(), i);
			}

            CollectionAssert.AreEqual(dTwo, (ICollection)Rencode.Decode(Rencode.Encode(dTwo)));
		}

		[TestMethod]
		public void Bool()
		{
			Assert.AreEqual(RencodeConst.CHR_TRUE, Rencode.Encode(true).Single());
			Assert.AreEqual(RencodeConst.CHR_FALSE, Rencode.Encode(false).Single());

			Assert.AreEqual(true, Rencode.Decode(Rencode.Encode(true)));
			Assert.AreEqual(false, Rencode.Decode(Rencode.Encode(false)));
		}

		[TestMethod]
		public void Null()
		{
			Assert.AreEqual(((char)RencodeConst.CHR_NONE).ToString(), Rencode.EncodeToString(null));

			Assert.AreEqual(null, Rencode.Decode(Rencode.Encode(null)));
		}

		[TestMethod]
		public void Float()
		{
			Assert.AreEqual(0.005353f, Rencode.Decode(Rencode.Encode(0.005353f)));
			Assert.AreEqual(-0.005353f, Rencode.Decode(Rencode.Encode(-0.005353f)));
		}

		[TestMethod]
		public void Double()
		{
			Assert.AreEqual(0.005353d, Rencode.Decode(Rencode.Encode(0.005353d)));
			Assert.AreEqual(-0.005353d, Rencode.Decode(Rencode.Encode(-0.005353d)));
		}

		[TestMethod]
		public void Bytes()
		{
			byte[] data = new byte[] { 193, 196, 0, 140, 100, 97, 101, 109, 111, 110, 46, 108, 111, 103, 105, 110, 194, 136, 117, 115, 101, 114, 110, 97, 109, 101, 136, 112, 97, 115, 115, 119, 111, 114, 100, 102 };

			// Decode string
			var result = (object[])Rencode.Decode(data);
			var parts = (object[])result[0];

			var credentials = (object[])parts[2];
			var options = (Dictionary<object, object>)parts[3];

			// Validate result
			Assert.AreEqual(0, parts[0]);
			Assert.AreEqual("daemon.login", parts[1]);

			Assert.AreEqual("username", credentials[0]);
			Assert.AreEqual("password", credentials[1]);

			Assert.AreEqual(0, options.Count);
		}
	}
}

