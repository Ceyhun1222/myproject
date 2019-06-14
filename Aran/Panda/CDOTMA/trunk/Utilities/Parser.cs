using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Globalization;

namespace CDOTMA.Utilities
{
	#region Tokens definitions

	// common tokens
	public static class BaseToken
	{
		public const int
		// Special
		EOF = -1,
		NONE = 0,

		// Base token
		NUMBER = 20,			// numeric Value
		QUOTED_WORD = 21,		// string Value

		UNQUOTED_WORD = 22,		// Keywords & variables

		//utilitary
		ARRAYOF = 23,
		ERROR = 24;
	}

	// projection parser tokens
	public static class PrjToken
	{
		public const int
			// Structural keyword
		PROJECTION = 1,
		UNIT = 2,
		PARAMETER = 3,
		PRIMEM = 4,
		SPHEROID = 5,
		DATUM = 6,
		GEOGCS = 7,
		PROJCS = 8,

		// Symbols
		COMMA = 9,				// ,
		LBRA = 10,				// [
		RBRA = 11;				// ]
	}

	// tokens for config file parser 
	public static class CfgToken
	{
		public const int
			// Structural keyword
		VersionNumber = 1,
		Language = 2,
		DistanceUnit = 3,
		DistancePrecision = 4,
		HeightUnit = 5,
		HeightPrecision = 6,
		SpeedUnit = 7,
		SpeedPrecision = 8,
		DSpeedPrecision = 9,
		AnglePrecision = 10,
		GradientPrecision = 11,

		// Symbols
		EQUAL = 12,				// =
		LBRA = 13,				// [	//for section coding
		RBRA = 14;				// ]	//for section coding
	}

	//public enum PrjToken
	//{
	//    // Special
	//    EOF = -1,
	//    NONE = 0,

	//    // Structural keyword
	//    PROJECTION = 1,
	//    UNIT = 2,
	//    PARAMETER = 3,
	//    PRIMEM = 4,
	//    SPHEROID = 5,
	//    DATUM = 6,
	//    GEOGCS = 7,
	//    PROJCS = 8,

	//    // Symbols
	//    COMMA = 9,				// ,
	//    LBRA = 10,				// [
	//    RBRA = 11,				// ]

	//    // Base token
	//    NUMBER = 20,			// Value
	//    QUOTED_WORD = 21,		// Name
	//    UNQUOTED_WORD = 22,		// Keyword

	//    //utilitary
	//    ARRAYOF = 23,
	//    ERROR = 24
	//}

	//public enum CfgToken
	//{
	//    // Special
	//    EOF = -1,
	//    NONE = 0,

	//    // Structural keyword

	//    VersionNumber = 1,
	//    Language = 2,
	//    DistanceUnit = 3,
	//    DistancePrecision = 4,
	//    HeightUnit = 5,
	//    HeightPrecision = 6,
	//    SpeedUnit = 7,
	//    SpeedPrecision = 8,
	//    DSpeedPrecision = 9,
	//    AnglePrecision = 10,
	//    GradientPrecision = 11,

	//    // Symbols
	//    EQU = 12,				// =

	//    // Base token
	//    NUMBER = 20,			// Value
	//    QUOTED_WORD = 21,		// Name
	//    UNQUOTED_WORD = 22,		// Keyword

	//    //utilitary
	//    ARRAYOF = 23,
	//    ERROR = 24
	//}

	public enum ValueType
	{
		NONE,
		Int,
		Double,
		String
	}

	public static class PrjReserved
	{
		public const string
			NONE = "NONE",
			PROJECTION = "PROJECTION",
			UNIT = "UNIT",
			PARAMETER = "PARAMETER",
			PRIMEM = "PRIMEM",
			SPHEROID = "SPHEROID",
			DATUM = "DATUM",
			GEOGCS = "GEOGCS",
			PROJCS = "PROJCS";

		public readonly static string[] reserved = new string[]
		{
			NONE, PROJECTION, UNIT, PARAMETER, PRIMEM,
			SPHEROID, DATUM, GEOGCS, PROJCS, ",", "[", "]",
			NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE,
			"Number", "String", "Keyword", "Array Of", "ERROR"
		};
	}

	public static class CfgReserved
	{
		public const string
			NONE = "NONE",
			VersionNumber = "VERSIONNUMBER",
			Language = "LANGUAGE",
			DistanceUnit = "DISTANCEUNIT",
			DistancePrecision = "DISTANCEPRECISION",
			HeightUnit = "HEIGHTUNIT",
			HeightPrecision = "HEIGHTPRECISION",
			SpeedUnit = "SPEEDUNIT",
			SpeedPrecision = "SPEEDPRECISION",
			DSpeedPrecision = "DSPEEDPRECISION",
			AnglePrecision = "ANGLEPRECISION",
			GradientPrecision = "GRADIENTPRECISION";

		public readonly static string[] reserved = new string[]
		{
			NONE, VersionNumber, Language, DistanceUnit, DistancePrecision,
			HeightUnit, HeightPrecision, SpeedUnit, SpeedPrecision, 
			DSpeedPrecision, AnglePrecision, GradientPrecision,
			"=", "[", "]", NONE, NONE, NONE, NONE, NONE,
			"Number", "String", "Keyword", "Array Of", "ERROR"
		};
	}
	#endregion

	public static class Messages
	{
		public const string
		FatalLexicalErrorMsg = "Fatal Lexical Error";
	}

	public class Parser
	{
		// Tabsize used
		const int tabsize = 4;

		#region private
		//
		private int _parserType;

		// Usual separators
		private string _separators;

		// Name of file
		public string RefName;

		private Stream _stream;
		private StreamReader _reader;

		// Current encoding for stream
		private Encoding _encoding;

		// Current line number
		private int _line;
		// Current col number
		private int _col;

		// Current token
		private int _currentToken;

		// Keywords list
		private Hashtable _keywords;

		// Putbacked character
		private int _putBackChar;
		// Character history
		private int _currentChar;
		private int _previousChar;
		private int _previousPreviousChar;

		// Values for the associated token returned
		private int _intVal;
		private double _doubleVal;
		private string _strVal;

		#endregion

		#region public properties
		// Current line
		public int Line { get { return _line; } }
		// Current col
		public int Col { get { return _col; } }

		// Get all defined keywords
		public ICollection DefinedKeywords { get { return _keywords.Keys; } }

		#region Current token value
		public ValueType valueType { get; set; }
		public int IntValue { get { return _intVal; } }
		public double DoubleValue { get { return _doubleVal; } }
		public string StrValue { get { return _strVal; } }

		#endregion
		#endregion

		#region Current token value
		/// <returns>token value</returns>
		public double GetIntValue() { return _intVal; }
		public double GetDoubleValue() { return _doubleVal; }
		public string GetStrValue() { return _strVal; }

		#endregion

		/// Get the next char in stream
		/// <returns>char read</returns>
		int GetChar()
		{
			_previousPreviousChar = _previousChar;
			_previousChar = _currentChar;

			if (_putBackChar != -1)
			{
				_currentChar = _putBackChar;
				_putBackChar = -1;

				return _currentChar;
			}

			_currentChar = _reader.Read();

			if (_currentChar == '\n')
			{
				_line++;
				_col = 1;
			}
			else if (_currentChar != '\r')
			{
				if (_currentChar == '\t')
					_col += tabsize;
				else
					_col++;
			}

			return _currentChar;
		}

		/// Lookup the next char in stream
		/// <returns>char expected</returns>
		int PeekChar()
		{
			if (_putBackChar != -1)
				return _putBackChar;

			return _reader.Peek();
		}

		/// Put back a char in the stream (will be the next char)
		/// <param name="c">char to put back</param>
		void PutBack(int c)
		{
			if (_putBackChar != -1)
				throw new Exception(Messages.FatalLexicalErrorMsg);

			_putBackChar = c;
			_currentChar = _previousChar;
			_previousChar = _previousPreviousChar;
		}

		/// Test if current value is a one of '[', ']', ',' or '=' symbols
		/// <param name="c">current char</param>
		/// <returns>true if current input is a special symbol</returns>
		int IsSymbol(int c)
		{
			if (c == -1)
			{
				_strVal = null;
				return BaseToken.ERROR;
			}

			_strVal = ((char)c).ToString();

			//strVal = Convert.ToChar(c).ToString();

			if (_parserType == 0)
				switch (c)
				{
					case '[':
						return PrjToken.LBRA;
					case ']':
						return PrjToken.RBRA;
					case ',':
						return PrjToken.COMMA;
				}
			else
				switch (c)
				{
					case '[':
						return CfgToken.LBRA;
					case ']':
						return CfgToken.RBRA;
					case '=':
						return CfgToken.EQUAL;
				}

			_strVal = null;
			return BaseToken.ERROR;
		}

		/// Append digits in string.
		/// <param name="c">current char</param>
		/// <param name="numberBuilder">current value for number</param>
		/// <returns></returns>
		private bool DecimalDigits(int c, StringBuilder numberBuilder)
		{
			bool seenDigits = false;

			if (c != -1)
				numberBuilder.Append((char)c);

			while ((c = PeekChar()) != -1)
			{
				if (char.IsDigit((char)c))
				{
					numberBuilder.Append((char)c);
					GetChar();
					seenDigits = true;
				}
				else
					break;
			}

			return seenDigits;
		}

		/// Test if current value is a number
		/// <param name="c">current char</param>
		/// <returns>true if current input is a number</returns>
		int IsNumber(int c)
		{
			if (!char.IsDigit((char)c))
				return BaseToken.ERROR;

			bool isReal = false;

			StringBuilder numberBuilder = new StringBuilder();
			numberBuilder.Length = 0;
			DecimalDigits(c, numberBuilder);

			c = GetChar();

			if (c == '.')
			{
				if (DecimalDigits('.', numberBuilder) || PeekChar() == 'e' || PeekChar() == 'E')
				{
					isReal = true;
					c = GetChar();
				}
				else
				{
					PutBack('.');
					numberBuilder.Length -= 1;
					_strVal = numberBuilder.ToString();

					_intVal = Convert.ToInt32(_strVal);		//ToInt32
					_doubleVal = _intVal;

					valueType = ValueType.Int;
					return BaseToken.NUMBER;
				}
			}

			if (c == 'e' || c == 'E')
			{
				isReal = true;
				numberBuilder.Append("e");
				c = GetChar();

				if (c == '+' || c == '-')
				{
					numberBuilder.Append((char)c);
					c = GetChar();
				}

				DecimalDigits(c, numberBuilder);
				c = GetChar();
			}

			PutBack(c);
			_strVal = numberBuilder.ToString();

			if (!isReal)
			{
				_intVal = Convert.ToInt32(_strVal);
				_doubleVal = _intVal;

				valueType = ValueType.Int;
				return BaseToken.NUMBER;
			}

			NumberFormatInfo provider = new NumberFormatInfo();
			provider.NumberDecimalSeparator = ".";
			provider.NumberGroupSeparator = "";

			try
			{
				_doubleVal = double.Parse(_strVal, provider);
			}
			catch (OverflowException)
			{
				_doubleVal = double.PositiveInfinity;
			}

			_intVal = Convert.ToInt32(_doubleVal);	//???
			valueType = ValueType.Double;

			return BaseToken.NUMBER;
		}

		/// Test if current value is a word
		/// <param name="c">current char</param>
		/// <returns>true if current input is a word</returns>
		int IsQuotedWord(int c)
		{
			// Start by quote
			if (c != '"')
				return BaseToken.ERROR;

			// Init buffer
			StringBuilder tmp = new StringBuilder();

			// Extract word value
			string separator = "\r\n\"";

			while ((c = PeekChar()) != -1 && separator.IndexOf((char)c) == -1)
			{
				// Append each char to value
				tmp.Append((char)c);
				GetChar();
			}

			if (c != -1)
				GetChar();

			// Return value
			_strVal = tmp.ToString();
			valueType = ValueType.String;

			return BaseToken.QUOTED_WORD;
		}

		/// Process current value as a keyword, an operation or, finally, as an unquoted word
		/// <param name="c">current char</param>
		/// <returns>the token for keyword, unquoted operation</returns>
		int IsKeywordOrUnquotedWord(int c)
		{
			// First char is always good
			StringBuilder tmp = new StringBuilder();
			tmp.Append((char)c);

			// Look for the full word
			while ((c = PeekChar()) != -1 && _separators.IndexOf((char)c) == -1)
			{
				// Append each char to value
				tmp.Append((char)c);
				GetChar();
			}

			_strVal = tmp.ToString();

			// Is a keyword
			string name = _strVal.ToUpper();

			if (!_keywords.Contains(name))
				return BaseToken.UNQUOTED_WORD;

			_strVal = name;

			return (int)_keywords[name];
		}

		/// <summary>
		/// Return next token
		/// </summary>
		/// <returns>next token</returns>
		public int NextToken()
		{
			if (_currentToken != BaseToken.NONE)
			{
				int result = _currentToken;
				_currentToken = BaseToken.NONE;
				return result;
			}

			int c = GetChar();

			// Ignore white spaces
			while (char.IsWhiteSpace((char)c))
				c = GetChar();

			valueType = ValueType.NONE;

			// End of file
			if (c == -1)
				return BaseToken.EOF;

			// Look for known token
			int token;

			if ((token = IsSymbol(c)) != BaseToken.ERROR)
				return token;

			if ((token = IsNumber(c)) != BaseToken.ERROR)
				return token;

			if ((token = IsQuotedWord(c)) != BaseToken.ERROR)
				return token;

			// Finally, look for keyword or process value as an unquoted word
			return IsKeywordOrUnquotedWord(c);
		}

		/// <summary>
		/// Return current token
		/// </summary>
		/// <returns>current token</returns>
		public int PeekToken()
		{
			if (_currentToken == BaseToken.NONE)
				_currentToken = NextToken();

			return _currentToken;
		}

		#region	Class initializers

		// Initialize keywords, structural primitive
		// TO DO: use CultureInfo for localization of keywords
		private void InitializePrjKeywords(/*CultureInfo newCulture*/)
		{
			_keywords.Clear();

			_keywords.Add(PrjReserved.PROJCS, PrjToken.PROJCS);
			_keywords.Add(PrjReserved.GEOGCS, PrjToken.GEOGCS);
			_keywords.Add(PrjReserved.DATUM, PrjToken.DATUM);
			_keywords.Add(PrjReserved.SPHEROID, PrjToken.SPHEROID);
			_keywords.Add(PrjReserved.PRIMEM, PrjToken.PRIMEM);
			_keywords.Add(PrjReserved.UNIT, PrjToken.UNIT);
			_keywords.Add(PrjReserved.PROJECTION, PrjToken.PROJECTION);
			_keywords.Add(PrjReserved.PARAMETER, PrjToken.PARAMETER);
		}

		private void InitializeCfgKeywords(/*CultureInfo newCulture*/)
		{
			_keywords.Clear();

			_keywords.Add(CfgReserved.VersionNumber, CfgToken.VersionNumber);
			_keywords.Add(CfgReserved.Language, CfgToken.Language);
			_keywords.Add(CfgReserved.DistanceUnit, CfgToken.DistanceUnit);
			_keywords.Add(CfgReserved.DistancePrecision, CfgToken.DistancePrecision);
			_keywords.Add(CfgReserved.HeightUnit, CfgToken.HeightUnit);
			_keywords.Add(CfgReserved.HeightPrecision, CfgToken.HeightPrecision);
			_keywords.Add(CfgReserved.SpeedUnit, CfgToken.SpeedUnit);
			_keywords.Add(CfgReserved.SpeedPrecision, CfgToken.SpeedPrecision);
			_keywords.Add(CfgReserved.DSpeedPrecision, CfgToken.DSpeedPrecision);
			_keywords.Add(CfgReserved.AnglePrecision, CfgToken.AnglePrecision);
			_keywords.Add(CfgReserved.GradientPrecision, CfgToken.GradientPrecision);
		}

		// Culture has changed, reinitialize keywords with local language.
		private void CultureChanged(CultureInfo newCulture)
		{
			if (_parserType == 0)
				InitializePrjKeywords(/*CultureInfo newCulture*/);
			else
				InitializeCfgKeywords(/*CultureInfo newCulture*/);
		}

		/// <summary>
		/// Initialize or reinitialize the tokenizer. By the way, go the the first position in the stream.
		/// </summary>
		public void Init(int parserType)
		{
			_parserType = parserType;

			if (_parserType == 0)
			{
				InitializePrjKeywords();
				_separators = "[], \r\n\t";
			}
			else
			{
				InitializeCfgKeywords();
				_separators = "[]= \r\n\t";
			}

			_stream.Position = 0;
			_reader = new StreamReader(_stream, _encoding);

			_putBackChar = _currentChar = _previousChar = _previousPreviousChar = -1;
			_currentToken = BaseToken.NONE;

			_line = _col = 1;
		}

		/// Constructor with a string
		/// <param name="toTokenize">string to use</param>
		public Parser(string toTokenize, int parserType)
		{
			_encoding = new UTF8Encoding();
			MemoryStream memStream = new MemoryStream(_encoding.GetBytes(toTokenize));
			this.RefName = "";
			_stream = memStream;
			_keywords = new Hashtable();

			Init(parserType);
		}

		/// <summary>
		/// Constructor with a stream. Use default encoding like for a stream file.
		/// </summary>
		/// <param name="input">stream to use</param>
		/// <param name="fname">file name</param>
		public Parser(Stream input, int parserType, string fname = "")
		{
			_encoding = Encoding.Default;
			this.RefName = fname;
			_stream = input;
			_keywords = new Hashtable();

			Init(parserType);
		}

		/// <summary>
		/// Constructor with a stream with an encoding.
		/// </summary>
		/// <param name="input">stream to use</param>
		/// <param name="fname">file name</param>
		/// <param name="initEncoding">encoding to use in reader</param>
		public Parser(Stream input, int parserType, Encoding initEncoding, string fname = "")
		{
			_encoding = initEncoding;
			this.RefName = fname;
			_stream = input;
			_keywords = new Hashtable();

			Init(parserType);
		}
		#endregion
	}

}
