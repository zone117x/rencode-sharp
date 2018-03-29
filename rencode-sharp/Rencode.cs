using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace rencodesharp
{
    public static class Rencode
    {
        private delegate void EncodeDelegate(object x, MemoryStream dest);
        private delegate object DecodeDelegate(MemoryStream x);

        static readonly Dictionary<Type, EncodeDelegate> EncodeFunc = new Dictionary<Type, EncodeDelegate>()
        {
            {typeof(string),                        EncodeString},

            {typeof(sbyte),                         EncodeInt},
            {typeof(short),                         EncodeInt},
            {typeof(int),                           EncodeInt},
            {typeof(long),                          EncodeInt},
            {typeof(float),                         EncodeFloat},
            {typeof(double),                        EncodeDouble},

            {typeof(IEnumerable),                   EncodeList},
            {typeof(object[]),                      EncodeList},
            {typeof(List<object>),                  EncodeList},

            {typeof(IDictionary),                   EncodeDictionary},
            {typeof(Dictionary<object, object>),    EncodeDictionary},

            {typeof(bool),                          EncodeBool},
        };

        static readonly Dictionary<byte, DecodeDelegate> DecodeFunc = new Dictionary<byte, DecodeDelegate>()
        {
            {(byte)'0',         DecodeString},
            {(byte)'1',         DecodeString},
            {(byte)'2',         DecodeString},
            {(byte)'3',         DecodeString},
            {(byte)'4',         DecodeString},
            {(byte)'5',         DecodeString},
            {(byte)'6',         DecodeString},
            {(byte)'7',         DecodeString},
            {(byte)'8',         DecodeString},
            {(byte)'9',         DecodeString},

            {RencodeConst.CHR_INT,      DecodeInt},
            {RencodeConst.CHR_INT1,     DecodeInt1},
            {RencodeConst.CHR_INT2,     DecodeInt2},
            {RencodeConst.CHR_INT4,     DecodeInt4},
            {RencodeConst.CHR_INT8,     DecodeInt8},
            {RencodeConst.CHR_FLOAT32,  DecodeFloat},
            {RencodeConst.CHR_FLOAT64,  DecodeDouble},

            {RencodeConst.CHR_LIST,     DecodeList},
            {RencodeConst.CHR_DICT,     DecodeDictionary},

            {RencodeConst.CHR_TRUE,     DecodeBoolTrue},
            {RencodeConst.CHR_FALSE,    DecodeBoolFalse},
            {RencodeConst.CHR_NONE,     DecodeNull},
        };


        #region Initialization

        static Rencode()
        {
            for (int i = 0; i < RencodeConst.STR_FIXED_COUNT; i++)
            {
                DecodeFunc.Add((byte)(RencodeConst.STR_FIXED_START + i), DecodeFixedString);
            }

            for (int i = 0; i < RencodeConst.LIST_FIXED_COUNT; i++)
            {
                DecodeFunc.Add((byte)(RencodeConst.LIST_FIXED_START + i), DecodeFixedList);
            }

            for (int i = 0; i < RencodeConst.INT_POS_FIXED_COUNT; i++)
            {
                DecodeFunc.Add((byte)(RencodeConst.INT_POS_FIXED_START + i), DecodeFixedPositiveInt);
            }

            for (int i = 0; i < RencodeConst.INT_NEG_FIXED_COUNT; i++)
            {
                DecodeFunc.Add((byte)(RencodeConst.INT_NEG_FIXED_START + i), DecodeFixedNegativeInt);
            }

            for (int i = 0; i < RencodeConst.DICT_FIXED_COUNT; i++)
            {
                DecodeFunc.Add((byte)(RencodeConst.DICT_FIXED_START + i), DecodeFixedDictionary);
            }
        }

        static object DecodeFixedString(MemoryStream x)
        {
            x.Position--;
            return x.ReadString(x.ReadByte() - RencodeConst.STR_FIXED_START);
        }

        static object DecodeFixedPositiveInt(MemoryStream x)
        {
            x.Position--;
            return x.ReadByte() - RencodeConst.INT_POS_FIXED_START;
        }

        static object DecodeFixedNegativeInt(MemoryStream x)
        {
            x.Position--;
            var p1 = x.ReadByte();
            var p2 = RencodeConst.INT_NEG_FIXED_START;
            var r = -1 - (p1 - p2);
            return r;
        }

        static object DecodeFixedList(MemoryStream x)
        {
            x.Position--;
            var result = new List<object>();
            int listCount = x.ReadByte() - RencodeConst.LIST_FIXED_START;

            for (int i = 0; i < listCount; i++)
            {
                object v = DecodeObject(x);
                result.Add(v);
            }

            return result.ToArray();
        }

        static object DecodeFixedDictionary(MemoryStream x)
        {
            x.Position--;
            var result = new Dictionary<object, object>();
            int dictCount = x.ReadByte() - RencodeConst.DICT_FIXED_START;

            for (int i = 0; i < dictCount; i++)
            {
                object k = DecodeObject(x);
                object v = DecodeObject(x);
                result.Add(k, v);
            }

            return result;
        }

        #endregion

        #region Core

        /// <summary>
        /// Encode object 'x' into a rencode byte array.
        /// </summary>
        public static byte[] Encode(object x)
        {
            if (x != null && !EncodeFunc.ContainsKey(x.GetType()))
            {
                throw new Exception("No Encoder Found");
            }

            var ms = new MemoryStream();
            EncodeObject(x, ms);

            return ms.ToArray();
        }

        /// <summary>
        /// Encode object 'x' into a rencode string.
        /// </summary>
        public static string EncodeToString(object x)
        {
            var bytes = Encode(x);
            var str = Util.GetString(bytes);
            return str;
        }

        /// <summary>
        /// Decode rencode string 'x' into object.
        /// </summary>
        public static object Decode(string x)
        {
            var bytes = Util.GetBytes(x);
            return Decode(bytes);
        }

        /// <summary>
        /// Decode rencode bytes 'x' into object.
        /// </summary>
        public static object Decode(byte[] x)
        {
            return Decode(x, 0);
        }

        /// <summary>
        /// Decode rencode bytes 'x' into object.
        /// </summary>
        public static object Decode(byte[] x, int startIndex)
        {
            if (!DecodeFunc.ContainsKey(x[0]))
            {
                throw new Exception("No Decoder Found");
            }

            var memStream = new MemoryStream(x, startIndex, x.Length - startIndex, true, true);
            return DecodeObject(memStream);
        }

        #endregion

        #region Encode

        static void EncodeObject(object x, MemoryStream dest)
        {
            if (x == null)
            {
                EncodeNull(null, dest);
            }
            else
            {
                if (EncodeFunc.TryGetValue(x.GetType(), out var func))
                {
                    func(x, dest);
                }
                else if (x is IDictionary)
                {
                    EncodeFunc[typeof(IDictionary)](x, dest);
                }
                else if (x is IEnumerable)
                {
                    EncodeFunc[typeof(IEnumerable)](x, dest);
                }
                else
                {
                    throw new Exception("Cannot encoded unsupported type: " + x.GetType());
                }

            }
        }

        static void EncodeString(object x, MemoryStream dest)
        {
            var xs = (string)x;
            var utf8 = Encoding.UTF8.GetBytes(xs);

            if (utf8.Length < RencodeConst.STR_FIXED_COUNT)
            {
                dest.Write((byte)(RencodeConst.STR_FIXED_START + utf8.Length));
                dest.Write(utf8);
            }
            else
            {
                dest.Write(utf8.Length.ToString());
                dest.Write(':');
                dest.Write(utf8);
            }
        }

        static void EncodeInt(object x, MemoryStream dest)
        {
            // Check to determine if long type is able
            // to be packed inside an Int32 or is actually
            // an Int64 value.
            bool isLong = x is long xl && (xl > int.MaxValue || xl < int.MinValue);
            long num = x is long l ? l : Convert.ToInt64(x);

            if (!isLong && 0 <= num && num < RencodeConst.INT_POS_FIXED_COUNT)
            {
                dest.Write((byte)(RencodeConst.INT_POS_FIXED_START + num));
            }
            else if (!isLong && -RencodeConst.INT_NEG_FIXED_COUNT <= num && num < 0)
            {
                dest.Write((byte)(RencodeConst.INT_NEG_FIXED_START - 1 - num));
            }
            else if (!isLong && sbyte.MinValue <= num && num <= sbyte.MaxValue)
            {
                dest.Write(RencodeConst.CHR_INT1);
                dest.Write(Convert.ToSByte(num));
            }
            else if (!isLong && short.MinValue <= num && num <= short.MaxValue)
            {
                dest.Write(RencodeConst.CHR_INT2);
                dest.Write(Convert.ToInt16(num));
            }
            else if (int.MinValue <= num && num <= int.MaxValue)
            {
                dest.Write(RencodeConst.CHR_INT4);
                dest.Write(Convert.ToInt32(num));
            }
            else if (long.MinValue < num && num < long.MaxValue)
            {
                dest.Write(RencodeConst.CHR_INT8);
                dest.Write(Convert.ToInt64(num));
            }
            else
            {
                string s = num.ToString(System.Globalization.CultureInfo.InvariantCulture);
                if (s.Length >= RencodeConst.MAX_INT_LENGTH)
                {
                    throw new ArgumentOutOfRangeException();
                }
                dest.Write(RencodeConst.CHR_INT);
                dest.Write(s);
                dest.Write(RencodeConst.CHR_TERM);
            }
        }

        static void EncodeFloat(object x, MemoryStream dest)
        {
            dest.Write(RencodeConst.CHR_FLOAT32);
            dest.Write((float)x);
        }

        static void EncodeDouble(object x, MemoryStream dest)
        {
            dest.Write(RencodeConst.CHR_FLOAT64);
            dest.Write((double)x);
        }

        static void EncodeList(object x, MemoryStream dest)
        {
            var listItems = x as IEnumerable;
            if (listItems == null)
            {
                throw new Exception();
            }

            object[] xl = listItems.Cast<object>().ToArray();

            if (xl.Length < RencodeConst.LIST_FIXED_COUNT)
            {
                dest.Write((byte)(RencodeConst.LIST_FIXED_START + xl.Length));
                foreach (object e in xl)
                {
                    EncodeObject(e, dest);
                }
            }
            else
            {
                dest.Write(RencodeConst.CHR_LIST);
                foreach (object e in xl)
                {
                    EncodeObject(e, dest);
                }
                dest.Write(RencodeConst.CHR_TERM);
            }
        }

        static void EncodeDictionary(object x, MemoryStream dest)
        {
            var xd = x as IDictionary;
            if (xd == null)
            {
                throw new Exception();
            }

            if (xd.Count < RencodeConst.DICT_FIXED_COUNT)
            {
                dest.Write((byte)(RencodeConst.DICT_FIXED_START + xd.Count));
                foreach (DictionaryEntry kv in xd)
                {
                    EncodeObject(kv.Key, dest);
                    EncodeObject(kv.Value, dest);
                }
            }
            else
            {
                dest.Write(RencodeConst.CHR_DICT);
                foreach (DictionaryEntry kv in xd)
                {
                    EncodeObject(kv.Key, dest);
                    EncodeObject(kv.Value, dest);
                }
                dest.Write(RencodeConst.CHR_TERM);
            }
        }

        static void EncodeBool(object x, MemoryStream dest)
        {
            if (x.GetType() != typeof(bool))
            {
                throw new Exception();
            }
            var xb = (bool)x;

            dest.Write(xb ? RencodeConst.CHR_TRUE : RencodeConst.CHR_FALSE);
        }

        static void EncodeNull(object x, MemoryStream dest)
        {
            if (x != null)
            {
                throw new Exception();
            }

            dest.Write(RencodeConst.CHR_NONE);
        }

        #endregion

        #region Decode

        static object DecodeObject(MemoryStream x)
        {
            var type = (byte)x.ReadByte();
            return DecodeFunc[type](x);
        }

        static string DecodeString(MemoryStream x)
        {
            x.Position--;

            const byte colon = (byte)':';
            var indexOfColon = x.IndexOf(colon);

            // TODO: doesn't support long length
            var stringLength = Convert.ToInt32(x.ReadString(indexOfColon - (int)x.Position));

            x.Position++;
            return x.ReadString(stringLength);
        }

        static object DecodeInt(MemoryStream x)
        {
            var buff = x.GetBuffer();
            int chrTermIndex = (int)x.Position;

            for (; chrTermIndex < x.Length; chrTermIndex++)
            {
                if (buff[chrTermIndex] == RencodeConst.CHR_TERM)
                {
                    break;
                }
                if (chrTermIndex - x.Position > RencodeConst.MAX_INT_LENGTH)
                {
                    throw new Exception("Overflow");
                }
            }

            var n = Convert.ToInt32(x.ReadString(chrTermIndex - (int)x.Position));

            object result = n;

            if (n <= sbyte.MaxValue && n >= sbyte.MinValue)
            {
                result = (sbyte)n;
            }
            else if (n <= short.MaxValue && n >= short.MinValue)
            {
                result = (short)n;
            }
            else if (n <= int.MaxValue && n >= int.MaxValue)
            {
                result = (int)n;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeInt1(MemoryStream x) => x.ReadSByte();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeInt2(MemoryStream x) => x.ReadInt16();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeInt4(MemoryStream x) => x.ReadInt32();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeInt8(MemoryStream x) => x.ReadInt64();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeFloat(MemoryStream x) => x.ReadSingle();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeDouble(MemoryStream x) => x.ReadDouble();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeList(MemoryStream x)
        {
            var result = new List<object>();
            while (x.ReadByte() != RencodeConst.CHR_TERM)
            {
                x.Position--;
                object v = DecodeObject(x);
                result.Add(v);
            }
            return result.ToArray();
        }

        static object DecodeDictionary(MemoryStream x)
        {
            var result = new Dictionary<object, object>();
            while (x.ReadByte() != RencodeConst.CHR_TERM)
            {
                x.Position--;
                object k = DecodeObject(x);
                object v = DecodeObject(x);
                result.Add(k, v);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeBoolTrue(MemoryStream x) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeBoolFalse(MemoryStream x) => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static object DecodeNull(MemoryStream x) => null;

        #endregion
    }
}

