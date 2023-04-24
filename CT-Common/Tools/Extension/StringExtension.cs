using System;

public static class StringExtension
{
	/// <summary>해당 문자열이 알파벳과 숫자로만 이루어져있는지 판단합니다.</summary>
	/// <param name="data">검사할 문자열</param>
	/// <returns>해당 문자열이 알파벳과 숫자로만 이루어져있다면 true를 반환합니다.</returns>
	public static bool IsOnlyAlphabetAndNumber(this string data)
	{
		foreach (char c in data)
		{
			if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')))
			{
				return false;
			}
		}

		return true;
	}

	public static bool IsLowerCase(this char c)
	{
		return (c >= 'a' && c <= 'z');
	}

	public static bool IsUpperCase(this char c)
	{
		return (c >= 'A' && c <= 'Z');
	}

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
}
