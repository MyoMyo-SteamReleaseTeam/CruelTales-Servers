using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CT.Common.Tools.Collections
{
	/// <summary>
	/// 두 값을 양방향으로 짝 지은 Map 입니다.
	/// </summary>
	public class BidirectionalMap<T1, T2> : IEnumerable
		where T1 : notnull
		where T2 : notnull
	{
		private readonly Dictionary<T1, T2> _forwardMap;
		private readonly Dictionary<T2, T1> _reverseMap;

		public Dictionary<T1, T2>.ValueCollection ForwardValues => _forwardMap.Values;
		public Dictionary<T2, T1>.ValueCollection ReverseValues => _reverseMap.Values;
		public Dictionary<T1, T2>.KeyCollection ForwardKeys => _forwardMap.Keys;
		public Dictionary<T2, T1>.KeyCollection ReverseKeys => _reverseMap.Keys;

		public IEnumerator GetEnumerator() => _forwardMap.GetEnumerator();
		public int Count => _forwardMap.Count;

		public BidirectionalMap()
		{
			_forwardMap = new Dictionary<T1, T2>();
			_reverseMap = new Dictionary<T2, T1>();
		}

		public BidirectionalMap(int capacity)
		{
			_forwardMap = new Dictionary<T1, T2>(capacity);
			_reverseMap = new Dictionary<T2, T1>(capacity);
		}

		public T2 this[T1 forwardKey]
		{
			get => _forwardMap[forwardKey];
			set => _forwardMap[forwardKey] = value;
		}

		public T1 this[T2 reverseKey]
		{
			get => _reverseMap[reverseKey];
			set => _reverseMap[reverseKey] = value;
		}

		/// <summary>Map의 모든 요소를 삭제합니다.</summary>
		public void Clear()
		{
			_forwardMap.Clear();
			_reverseMap.Clear();
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
			if (_forwardMap.ContainsKey(key) || _reverseMap.ContainsKey(value))
			{
				return false;
			}

			_forwardMap[key] = value;
			_reverseMap[value] = key;
			return true;
		}

		/// <summary>두 번째 맵에 요소를 추가합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryAddReverse(in T2 key, in T1 value)
		{
			if (_forwardMap.ContainsKey(value) || _reverseMap.ContainsKey(key))
			{
				return false;
			}

			_forwardMap[value] = key;
			_reverseMap[key] = value;
			return true;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		public T2 GetForward(in T1 key)
		{
			return _forwardMap[key];
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		public T1 GetReverse(in T2 key)
		{
			return _reverseMap[key];
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetForward(in T1 key, out T2? value)
		{
			return _forwardMap.TryGetValue(key, out value);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetReverse(in T2 key, out T1? value)
		{
			return _reverseMap.TryGetValue(key, out value);
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemoveForward(in T1 key)
		{
			if (_forwardMap.TryGetValue(key, out var value))
			{
				_forwardMap.Remove(key);
				_reverseMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemoveReverse(in T2 key)
		{
			if (_reverseMap.TryGetValue(key, out var value))
			{
				_reverseMap.Remove(key);
				_forwardMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool ContainsForward(in T1 key)
		{
			return _forwardMap.ContainsKey(key);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool ContainsReverse(in T2 key)
		{
			return _reverseMap.ContainsKey(key);
		}

		#endregion

		// 두 타입이 다를 때에만 사용합니다.
		#region Generic Operation

		/// <summary>첫 번째 맵에 요소를 추가합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryAdd(in T1 key, in T2 value)
		{
			if (_forwardMap.ContainsKey(key) || _reverseMap.ContainsKey(value))
			{
				return false;
			}

			_forwardMap[key] = value;
			_reverseMap[value] = key;
			return true;
		}

		/// <summary>두 번째 맵에 요소를 추가합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryAdd(in T2 key, in T1 value)
		{
			if (_forwardMap.ContainsKey(value) || _reverseMap.ContainsKey(key))
			{
				return false;
			}

			_forwardMap[value] = key;
			_reverseMap[key] = value;
			return true;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		public T2 GetValue(in T1 key)
		{
			return _forwardMap[key];
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		public T1 GetValue(in T2 key)
		{
			return _reverseMap[key];
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetValue(in T1 key, [MaybeNullWhen(false)] out T2 value)
		{
			return _forwardMap.TryGetValue(key, out value);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 찾습니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryGetValue(in T2 key, [MaybeNullWhen(false)] out T1 value)
		{
			return _reverseMap.TryGetValue(key, out value);
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemove(in T1 key)
		{
			if (_forwardMap.TryGetValue(key, out var value))
			{
				_forwardMap.Remove(key);
				_reverseMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>두 번째 맵의 키를 기준으로 값을 제거합니다.</summary>
		/// <returns>성공 여부입니다.</returns>
		public bool TryRemove(in T2 key)
		{
			if (_reverseMap.TryGetValue(key, out var value))
			{
				_reverseMap.Remove(key);
				_forwardMap.Remove(value);
				return true;
			}

			return false;
		}

		/// <summary>첫 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool Contains(in T1 key)
		{
			return _forwardMap.ContainsKey(key);
		}

		/// <summary>두 번째 맵의 키를 기준으로 값의 존재 유무를 판단합니다.</summary>
		/// <returns>값이 존재하면 True를 반환합니다.</returns>
		public bool Contains(in T2 key)
		{
			return _reverseMap.ContainsKey(key);
		}

		#endregion
	}
}
