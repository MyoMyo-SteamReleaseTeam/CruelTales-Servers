using System.Collections.Generic;

public static class CollectionExtension
{
	public static bool IsEqual<T>(this IList<T> lhs, IList<T> rhs)
		where T : notnull
	{
		if (lhs.Count != rhs.Count || lhs == null || rhs == null)
			return false;

		int count = lhs.Count;
		for (int i = 0; i < count; i++)
		{
			if (!lhs[i].Equals(rhs[i]))
				return false;
		}

		return true;
	}

	public static bool IsEqualNullable<T>(this IList<T> lhs, IList<T> rhs)
	{
		if (lhs.Count != rhs.Count || lhs == null || rhs == null)
			return false;

		int count = lhs.Count;
		for (int i = 0; i < count; i++)
		{
			var lValue = lhs[i];
			var rValue = rhs[i];

			if (lValue == null)
			{
				if (rValue == null)
					continue;

				return false;
			}

			if (rValue == null)
			{
				if (lValue == null)
				continue;

				return false;
			}

			if (!lValue.Equals(rValue))
			return false;
		}

		return true;
	}

	/// <summary>요소가 포함되어 있지 않다면 추가합니다.</summary>
	/// <typeparam name="T">추가할 요소의 타입</typeparam>
	/// <param name="collection">대상 컬렉션</param>
	/// <param name="value">추가할 요소입니다.</param>
	/// <returns>추가하는데 성공한다면 true를 반환합니다.</returns>
	public static bool TryAddUnique<T>(this ICollection<T> collection, T value)
	{
		if (!collection.Contains(value))
		{
			collection.Add(value);
			return true;
		}

		return false;
	}
}
