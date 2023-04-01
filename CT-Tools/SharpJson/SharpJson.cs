/**
 *
 * Copyright (c) 2016 Adriano Tinoco d'Oliveira Rezende
 * 
 * Based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/news/view/14605/14863/how-do-i-write-my-own-parser-(for-json).html
 *
 * Changes made:
 * 
 * 	- Optimized parser speed (deserialize roughly near 3x faster than original)
 *  - Added support to handle lexer/parser error messages with line numbers
 *  - Added more fine grained control over type conversions during the parsing
 *  - Refactory API (Separate Lexer code from Parser code and the Encoder from Decoder)
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CT.Tools.SharpJson
{
	class Lexer
	{
		public enum Token
		{
			None,
			Null,
			True,
			False,
			Colon,
			Comma,
			String,
			Number,
			CurlyOpen,
			CurlyClose,
			SquaredOpen,
			SquaredClose,
		};

		public bool HasError => !mSuccess;
		public int LineNumber { get; private set; }
		public bool ParseNumbersAsFloat { get; set; }

		private char[] mJson;
		private int mIndex = 0;
		private bool mSuccess = true;
		private char[] mStringBuffer = new char[4096];

		public Lexer(string text)
		{
			Reset();

			mJson = text.ToCharArray();
			ParseNumbersAsFloat = false;
		}

		public void Reset()
		{
			mIndex = 0;
			LineNumber = 1;
			mSuccess = true;
		}

		public string? ParseString()
		{
			int idx = 0;
			StringBuilder? builder = null;

			skipWhiteSpaces();

			// "
			char c = mJson[mIndex++];

			bool failed = false;
			bool complete = false;

			while (!complete && !failed)
			{
				if (mIndex == mJson.Length)
					break;

				c = mJson[mIndex++];
				if (c == '"')
				{
					complete = true;
					break;
				}
				else if (c == '\\')
				{
					if (mIndex == mJson.Length)
						break;

					c = mJson[mIndex++];

					switch (c)
					{
						case '"':
							mStringBuffer[idx++] = '"';
							break;
						case '\\':
							mStringBuffer[idx++] = '\\';
							break;
						case '/':
							mStringBuffer[idx++] = '/';
							break;
						case 'b':
							mStringBuffer[idx++] = '\b';
							break;
						case 'f':
							mStringBuffer[idx++] = '\f';
							break;
						case 'n':
							mStringBuffer[idx++] = '\n';
							break;
						case 'r':
							mStringBuffer[idx++] = '\r';
							break;
						case 't':
							mStringBuffer[idx++] = '\t';
							break;
						case 'u':
							int remainingLength = mJson.Length - mIndex;
							if (remainingLength >= 4)
							{
								var hex = new string(mJson, mIndex, 4);

								// XXX: handle UTF
								mStringBuffer[idx++] = (char)Convert.ToInt32(hex, 16);

								// skip 4 chars
								mIndex += 4;
							}
							else
							{
								failed = true;
							}
							break;
					}
				}
				else
				{
					mStringBuffer[idx++] = c;
				}

				if (idx >= mStringBuffer.Length)
				{
					if (builder == null)
						builder = new StringBuilder();

					builder.Append(mStringBuffer, 0, idx);
					idx = 0;
				}
			}

			if (!complete)
			{
				mSuccess = false;
				return null;
			}

			if (builder == null)
			{
				return new string(mStringBuffer, 0, idx);
			}
			else
			{
				if (idx > 0)
					builder.Append(mStringBuffer, 0, idx);

				return builder.ToString();
			}
		}

		private string getNumberString()
		{
			skipWhiteSpaces();

			int lastIndex = getLastIndexOfNumber(mIndex);
			int charLength = lastIndex - mIndex + 1;

			var result = new string(mJson, mIndex, charLength);

			mIndex = lastIndex + 1;

			return result;
		}

		public float ParseFloatNumber()
		{
			float number;
			var str = getNumberString();

			if (!float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out number))
				return 0;

			return number;
		}

		public double ParseDoubleNumber()
		{
			double number;
			var str = getNumberString();

			if (!double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
				return 0;

			return number;
		}

		private int getLastIndexOfNumber(int index)
		{
			int lastIndex;

			for (lastIndex = index; lastIndex < mJson.Length; lastIndex++)
			{
				char ch = mJson[lastIndex];

				if ((ch < '0' || ch > '9') && ch != '+' && ch != '-'
					&& ch != '.' && ch != 'e' && ch != 'E')
					break;
			}

			return lastIndex - 1;
		}

		private void skipWhiteSpaces()
		{
			for (; mIndex < mJson.Length; mIndex++)
			{
				char ch = mJson[mIndex];

				if (ch == '\n')
					LineNumber++;

				if (!char.IsWhiteSpace(mJson[mIndex]))
					break;
			}
		}

		public Token LookAhead()
		{
			skipWhiteSpaces();

			int savedIndex = mIndex;
			return nextToken(mJson, ref savedIndex);
		}

		public Token NextToken()
		{
			skipWhiteSpaces();
			return nextToken(mJson, ref mIndex);
		}

		private static Token nextToken(char[] json, ref int index)
		{
			if (index == json.Length)
				return Token.None;

			char c = json[index++];

			switch (c)
			{
				case '{':
					return Token.CurlyOpen;
				case '}':
					return Token.CurlyClose;
				case '[':
					return Token.SquaredOpen;
				case ']':
					return Token.SquaredClose;
				case ',':
					return Token.Comma;
				case '"':
					return Token.String;
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '-':
					return Token.Number;
				case ':':
					return Token.Colon;
			}

			index--;

			int remainingLength = json.Length - index;

			// false
			if (remainingLength >= 5)
			{
				if (json[index] == 'f' &&
					json[index + 1] == 'a' &&
					json[index + 2] == 'l' &&
					json[index + 3] == 's' &&
					json[index + 4] == 'e')
				{
					index += 5;
					return Token.False;
				}
			}

			// true
			if (remainingLength >= 4)
			{
				if (json[index] == 't' &&
					json[index + 1] == 'r' &&
					json[index + 2] == 'u' &&
					json[index + 3] == 'e')
				{
					index += 4;
					return Token.True;
				}
			}

			// null
			if (remainingLength >= 4)
			{
				if (json[index] == 'n' &&
					json[index + 1] == 'u' &&
					json[index + 2] == 'l' &&
					json[index + 3] == 'l')
				{
					index += 4;
					return Token.Null;
				}
			}

			return Token.None;
		}
	}

	public class JsonDecoder
	{
		public string? ErrorMessage { get; private set; }
		public bool ParseNumbersAsFloat { get; set; }

		private Lexer? _lexer;

		public JsonDecoder()
		{
			ErrorMessage = null;
			ParseNumbersAsFloat = false;
		}

		public object? Decode(string text)
		{
			ErrorMessage = null;

			_lexer = new Lexer(text);
			_lexer.ParseNumbersAsFloat = ParseNumbersAsFloat;

			return parseValue();
		}

		public static object? DecodeText(string text)
		{
			var builder = new JsonDecoder();
			return builder.Decode(text);
		}

		public static Dictionary<string, object>? DecodeAsObject(string text)
		{
			var builder = new JsonDecoder();
			return builder.Decode(text) as Dictionary<string, object>;
		}

		private IDictionary<string, object?>? parseObject()
		{
			if (_lexer == null)
				return null;

			var table = new Dictionary<string, object?>();

			// {
			_lexer.NextToken();

			while (true)
			{
				var token = _lexer.LookAhead();

				switch (token)
				{
					case Lexer.Token.None:
						triggerError("Invalid token");
						return null;
					case Lexer.Token.Comma:
						_lexer.NextToken();
						break;
					case Lexer.Token.CurlyClose:
						_lexer.NextToken();
						return table;
					default:
						// name
						string? name = evalLexer(_lexer.ParseString());

						if (ErrorMessage != null)
							return null;

						// :
						token = _lexer.NextToken();

						if (token != Lexer.Token.Colon)
						{
							triggerError("Invalid token; expected ':'");
							return null;
						}

						// value
						object? value = parseValue();

						if (ErrorMessage != null)
							return null;

						if (name == null)
							break;

						table[name] = value;
						break;
				}
			}
		}

		private IList<object>? parseArray()
		{
			if (_lexer == null)
				return null;

			var array = new List<object>();

			// [
			_lexer.NextToken();

			while (true)
			{
				var token = _lexer.LookAhead();

				switch (token)
				{
					case Lexer.Token.None:
						triggerError("Invalid token");
						return null;
					case Lexer.Token.Comma:
						_lexer.NextToken();
						break;
					case Lexer.Token.SquaredClose:
						_lexer.NextToken();
						return array;
					default:
						object? value = parseValue();

						if (ErrorMessage != null)
							return null;

						if (value == null)
							break;

						array.Add(value);
						break;
				}
			}
		}

		private object? parseValue()
		{
			if (_lexer == null)
				return null;

			switch (_lexer.LookAhead())
			{
				case Lexer.Token.String:
					return evalLexer(_lexer.ParseString());

				case Lexer.Token.Number:
					return ParseNumbersAsFloat ? _lexer.ParseFloatNumber() : _lexer.ParseDoubleNumber();

				case Lexer.Token.CurlyOpen:
					return parseObject();

				case Lexer.Token.SquaredOpen:
					return parseArray();

				case Lexer.Token.True:
					_lexer.NextToken();
					return true;

				case Lexer.Token.False:
					_lexer.NextToken();
					return false;

				case Lexer.Token.Null:
					_lexer.NextToken();
					return null;

				case Lexer.Token.None:
					break;
			}

			triggerError("Unable to parse value");
			return null;
		}

		private void triggerError(string message)
		{
			int lineNumber = _lexer == null ? 0 : _lexer.LineNumber;

			ErrorMessage = string.Format("Error: '{0}' at line {1}",
										 message, lineNumber);
		}

		private T evalLexer<T>(T value)
		{
			if (_lexer == null)
			{
				triggerError("Lexical null error ocurred");
			}
			else
			{
				if (_lexer.HasError)
					triggerError("Lexical error ocurred");
			}

			return value;
		}
	}
}