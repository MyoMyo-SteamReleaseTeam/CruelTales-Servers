using System;
using System.Collections;
using System.Collections.Generic;

namespace CT.Tool
{
	public readonly struct EnumPair<EnumType, BaseEnumType>
		where EnumType : Enum
		where BaseEnumType : Enum
	{
		public readonly EnumType Type;
		public readonly BaseEnumType BaseType;

		public EnumPair(EnumType type, BaseEnumType baseType)
		{
			Type = type;
			BaseType = baseType;
		}
	}

	public class EnumTableByte<EnumType, BaseEnumType>
		: IEnumerable<EnumPair<EnumType, BaseEnumType>>
		where EnumType : Enum
		where BaseEnumType : Enum
	{
		private List<EnumPair<EnumType, BaseEnumType>> _baseEnumTable = new();

		public void Add(EnumType enumType, BaseEnumType baseEnum)
		{
			_baseEnumTable.Add(new EnumPair<EnumType, BaseEnumType>(enumType, baseEnum));
		}

		/// <summary>기본 타입을 반환합니다.</summary>
		/// <returns>기본 타입</returns>
		public BaseEnumType GetBaseType(EnumType type)
		{
			for (int i = _baseEnumTable.Count - 1; i >= 0; i--)
			{
				var table = _baseEnumTable[i];

				if ((byte)(object)type >= (byte)(object)table.Type)
				{
					return table.BaseType;
				}
			}

			return (BaseEnumType)(object)0;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}

		IEnumerator<EnumPair<EnumType, BaseEnumType>> IEnumerable<EnumPair<EnumType, BaseEnumType>>.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}
	}

	public class EnumTableUInt16<EnumType, BaseEnumType>
		: IEnumerable<EnumPair<EnumType, BaseEnumType>>
		where EnumType : Enum
		where BaseEnumType : Enum
	{
		public void Add(EnumType enumType, BaseEnumType baseEnum)
		{
			_baseEnumTable.Add(new EnumPair<EnumType, BaseEnumType>(enumType, baseEnum));
		}

		private List<EnumPair<EnumType, BaseEnumType>> _baseEnumTable = new();

		/// <summary>기본 타입을 반환합니다.</summary>
		/// <returns>기본 타입</returns>
		public BaseEnumType GetBaseType(EnumType type)
		{
			for (int i = _baseEnumTable.Count - 1; i >= 0; i--)
			{
				var table = _baseEnumTable[i];

				if ((ushort)(object)type >= (ushort)(object)table.Type)
				{
					return table.BaseType;
				}
			}

			return (BaseEnumType)(object)0;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}

		IEnumerator<EnumPair<EnumType, BaseEnumType>> IEnumerable<EnumPair<EnumType, BaseEnumType>>.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}
	}

	public class EnumTableUInt32<EnumType, BaseEnumType>
		: IEnumerable<EnumPair<EnumType, BaseEnumType>>
		where EnumType : Enum
		where BaseEnumType : Enum
	{
		public void Add(EnumType enumType, BaseEnumType baseEnum)
		{
			_baseEnumTable.Add(new EnumPair<EnumType, BaseEnumType>(enumType, baseEnum));
		}

		private List<EnumPair<EnumType, BaseEnumType>> _baseEnumTable = new();

		/// <summary>기본 타입을 반환합니다.</summary>
		/// <returns>기본 타입</returns>
		public BaseEnumType GetBaseType(EnumType type)
		{
			for (int i = _baseEnumTable.Count - 1; i >= 0; i--)
			{
				var table = _baseEnumTable[i];

				if ((uint)(object)type >= (uint)(object)table.Type)
				{
					return table.BaseType;
				}
			}

			return (BaseEnumType)(object)0;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}

		IEnumerator<EnumPair<EnumType, BaseEnumType>> IEnumerable<EnumPair<EnumType, BaseEnumType>>.GetEnumerator()
		{
			return _baseEnumTable.GetEnumerator();
		}
	}
}
