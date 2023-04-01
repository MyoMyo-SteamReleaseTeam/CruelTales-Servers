using System.Collections;
using System.Collections.Generic;

namespace CT.Tools.Collections
{
	/// <summary>
	/// 두 값을 양방향으로 짝 지은 Map 입니다.
	/// </summary>
	public class BidirectionalMap<T1, T2> : IEnumerable
		where T1 : notnull
		where T2 : notnull
	{
		private readonly Dictionary<T1, T2> mForwardMap = new Dictionary<T1, T2>();
		private readonly Dictionary<T2, T1> mReverseMap = new Dictionary<T2, T1>();

		public Dictionary<T1, T2>.ValueCollection ForwardValues => mForwardMap.Values;
		public Dictionary<T2, T1>.ValueCollection ReverseValues => mReverseMap.Values;
		public Dictionary<T1, T2>.KeyCollection ForwardKeys => mForwardMap.Keys;
		public Dictionary<T2, T1>.KeyCollection ReverseKeys => mReverseMap.Keys;

		public IEnumerator GetEnumerator() => mForwardMap.GetEnumerator();
		public int Count => mForwardMap.Count;

		/// <summary>Map의 모든 요소를 삭제합니다.</summary>
		public void Clear()
		{
			mForwardMap.Clear();
			mReverseMap.Clear();
		}

		public void Add(T1 fValue, T2 rValue)
		{
			TryAddForward(fValue, rValue);
		}

		// 두 타입이 같을 때에만 사용합니다.
		#region Type Safe Operation

		/// <summary>첫 번째 맵에 요소를 추가합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryAddForward(in T1 key, in T2 value)
		{
			if (mForwardMap.ContainsKey(key) || mReverseMap.ContainsKey(value))
			{
				return false;
			}

			mForwardMap[key] = value;
			mReverseMap[value] = key;
			return true;
		}

		/// <summary>두 번째 맵에 요소를 추가합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryAddReverse(in T2 key, in T1 value)
		{
			if (mForwardMap.ContainsKey(value) || mReverseMap.ContainsKey(key))
			{
				return false;
			}

			mForwardMap[value] = key;
			mReverseMap[key] = value;
			return true;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetForward(in T1 key, out T2? value)
		{
			return mForwardMap.TryGetValue(key, out value);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetReverse(in T2 key, out T1? value)
		{
			return mReverseMap.TryGetValue(key, out value);
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemoveForward(in T1 key)
		{
			if (mForwardMap.TryGetValue(key, out var value))
			{
				mForwardMap.Remove(key);
				mReverseMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemoveReverse(in T2 key)
		{
			if (mReverseMap.TryGetValue(key, out var value))
			{
				mReverseMap.Remove(key);
				mForwardMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool ContainsForward(in T1 key)
		{
			return mForwardMap.ContainsKey(key);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool ContainsReverse(in T2 key)
		{
			return mReverseMap.ContainsKey(key);
		}

		#endregion

		// 두 타입이 다를 때에만 사용합니다.
		#region Generic Operation

		/// <summary>첫 번째 맵에 요소를 추가합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryAdd(in T1 key, in T2 value)
		{
			if (mForwardMap.ContainsKey(key) || mReverseMap.ContainsKey(value))
			{
				return false;
			}

			mForwardMap[key] = value;
			mReverseMap[value] = key;
			return true;
		}

		/// <summary>두 번째 맵에 요소를 추가합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryAdd(in T2 key, in T1 value)
		{
			if (mForwardMap.ContainsKey(value) || mReverseMap.ContainsKey(key))
			{
				return false;
			}

			mForwardMap[value] = key;
			mReverseMap[key] = value;
			return true;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetValue(in T1 key, out T2? value)
		{
			return mForwardMap.TryGetValue(key, out value);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetValue(in T2 key, out T1? value)
		{
			return mReverseMap.TryGetValue(key, out value);
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemove(in T1 key)
		{
			if (mForwardMap.TryGetValue(key, out var value))
			{
				mForwardMap.Remove(key);
				mReverseMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemove(in T2 key)
		{
			if (mReverseMap.TryGetValue(key, out var value))
			{
				mReverseMap.Remove(key);
				mForwardMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool Contains(in T1 key)
		{
			return mForwardMap.ContainsKey(key);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool Contains(in T2 key)
		{
			return mReverseMap.ContainsKey(key);
		}

		#endregion
	}
}
