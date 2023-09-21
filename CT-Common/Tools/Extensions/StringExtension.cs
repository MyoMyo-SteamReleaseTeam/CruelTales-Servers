using System;
using System.Runtime.CompilerServices;
using System.Text;

public static class StringExtension
{
	private const int TO_LOWERCASE_OFFSET = ('a' - 'A');
	private const int TO_UPPERCASE_OFFSET = ('A' - 'a');

	/// <summary>해당 문자열이 알파벳과 숫자로만 이루어져있는지 판단합니다.</summary>
	/// <param name="data">검사할 문자열</param>
	/// <returns>해당 문자열이 알파벳과 숫자로만 이루어져있다면 true를 반환합니다.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsOnlyAlphabetOrNumber(this string data)
	{
		foreach (char c in data)
		{
			if (!IsAlphabetOrNumber(c))
				return false;
		}

		return true;
	}

	/// <summary>해당 문자가 16진수 문자인지 여부를 반환합니다.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsHexChar(this char c)
	{
		return (c >= '0' && c <= '9') || 
			   (c >= 'a' && c <= 'f') || 
			   (c >= 'A' && c <= 'F');
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAlphabetOrNumber(this char c)
	{
		return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAlphabet(this char c)
	{
		return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLowerCase(this char c)
	{
		return (c >= 'a' && c <= 'z');
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsUpperCase(this char c)
	{
		return (c >= 'A' && c <= 'Z');
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static char ToLower(this char c)
	{
		return (char)(c + TO_LOWERCASE_OFFSET);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static char ToUpper(this char c)
	{
		return (char)(c + TO_UPPERCASE_OFFSET);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string TryFormat(string format, object arg)
	{
		try
		{
			return string.Format(format, arg);
		}
		catch (FormatException e)
		{
			return format + e.Message;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string TryFormat(string format, params object[] args)
	{
		try
		{
			return string.Format(format, args);
		}
		catch (FormatException e)
		{
			return format + e.Message;
		}
	}

	public static string ToSnakeCase(this string value)
	{
		StringBuilder sb = new(value.Length * 2);

		int s;

		for (s = 0; s < value.Length; s++)
		{
			char c = value[s];
			if (c.IsAlphabet())
			{
				sb.Append(c.ToLower());
				break;
			}
		}

		s++;

		for (int i = s; i < value.Length; i++)
		{
			char c = value[i];

			if (c.IsUpperCase())
			{
				if (i == value.Length - 1)
				{
					sb.Append(c);
					break;
				}

				sb.Append('_');
				sb.Append(c.ToLower());
			}
			else
			{
				sb.Append(c);
			}
		}

		return sb.ToString();
	}

	public static string ToPascalCase(this string value)
	{
		StringBuilder sb = new(value.Length * 2);

		int s;

		for (s = 0; s < value.Length; s++)
		{
			char c = value[s];
			if (c.IsAlphabet())
			{
				sb.Append(c.ToUpper());
				break;
			}
		}

		s++;

		for (int i = s; i < value.Length; i++)
		{
			char c = value[i];
			if (c == '_')
			{
				i++;

				if (i >= value.Length)
				{
					sb.Append(c);
					break;
				}

				c = value[i];
				if (c.IsAlphabet())
				{
					sb.Append(c.ToUpper());
				}
				else
				{
					sb.Append(c);
				}
			}
			else
			{
				sb.Append(c);
			}
		}

		return sb.ToString();
	}
}
