using System;

namespace rencodesharp
{
	public class RencodeConst
	{
		// The bencode 'typecodes' such as i, d, etc have been extended and
		// relocated on the base-256 character set.
		public const byte CHR_LIST		= 59;
		public const byte CHR_DICT		= 60;
		public const byte CHR_INT		= 61;
		public const byte CHR_INT1		= 62;
		public const byte CHR_INT2		= 63;
		public const byte CHR_INT4		= 64;
		public const byte CHR_INT8		= 65;
		public const byte CHR_FLOAT32	= 66;
		public const byte CHR_FLOAT64	= 44;
		public const byte CHR_TRUE		= 67;
		public const byte CHR_FALSE		= 68;
		public const byte CHR_NONE		= 69;
		public const byte CHR_TERM		= 127;

		// Maximum length of integer when written as base 10 string.
		public const int MAX_INT_LENGTH = 64;

		// Positive integers with value embedded in typecode.
		public const int INT_POS_FIXED_START = 0;
		public const int INT_POS_FIXED_COUNT = 44;

		// Negative integers with valuve embedded in typecode.
		public const int INT_NEG_FIXED_START = 70;
		public const int INT_NEG_FIXED_COUNT = 32;

		// Strings with length embedded in typecode.
		public const int STR_FIXED_START = 128;
		public const int STR_FIXED_COUNT = 64;

		// Lists with length embedded in typecode.
		public const int LIST_FIXED_START = STR_FIXED_START+STR_FIXED_COUNT;
		public const int LIST_FIXED_COUNT = 64;

		// Dictionaries with length embedded in typecode.
		public const int DICT_FIXED_START = 102;
		public const int DICT_FIXED_COUNT = 25;
	}
}

