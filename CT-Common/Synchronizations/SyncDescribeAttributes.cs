﻿using System;

namespace CT.Common.Synchronizations
{
	/// <summary>
	/// 생성과 소멸이 동기화되는 네트워크 객체의 선언 속성입니다.
	/// 객체 동기화 코드 자동생성을 위해 필요합니다.
	/// </summary>
	public class SyncNetworkObjectDefinitionAttribute : Attribute
	{
		public bool MultiplyByMaxUser;
		public int Capacity;
		public bool IsDebugOnly;

		public SyncNetworkObjectDefinitionAttribute(int capacity = 16, bool multiplyByMaxUser = false, bool isDebugOnly = false)
		{
			Capacity = capacity;
			MultiplyByMaxUser = multiplyByMaxUser;
			IsDebugOnly = isDebugOnly;
		}
	}

	/// <summary>
	/// 동기화 가능한 객체의 선언 속성입니다.
	/// 객체 동기화 코드 자동생성을 위해 필요합니다.
	/// </summary>
	public class SyncObjectDefinitionAttribute : Attribute
	{
		/// <summary>
		/// 사용자가 임의로 정의했는지 여부입니다.
		/// 사용자가 임의로 정의한 경우 코드를 자동생성하지 않습니다.
		/// </summary>
		public bool IsCustom { get; private set; }
		public bool IsDebugOnly;

		public SyncObjectDefinitionAttribute(bool isCustom = false, bool isDebugOnly = false)
		{
			IsCustom = isCustom;
			IsDebugOnly = isDebugOnly;
		}
	}

	/// <summary>
	/// 멤버 변수가 동기화 객체임을 나타내는 속성입니다.
	/// 동기화 객체가 비신뢰성 속성만을 포함하는 경우 SyncType을 Unreliable로 설정하세요.
	/// 동기화 객체가 비신뢰성 속성과 신뢰성 속성을 모두 포함하는 경우 SyncType을 RelibaleOrUnreliable로 설정하세요.
	/// </summary>
	public class SyncObjectAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }
		public SyncDirection SyncDirection { get; private set; }
		public string ConstructorContent { get; private set; }

		/// <param name="cc">생성자 인자 문자열입니다.</param>
		public SyncObjectAttribute(SyncType sync = SyncType.Reliable,
								   SyncDirection dir = SyncDirection.FromMaster,
								   string cc = "")
		{
			SyncType = sync;
			SyncDirection = dir;
			ConstructorContent = cc;
		}
	}

	/// <summary>
	/// 멤버 변수가 동기화 변수임을 나타내는 속성입니다.
	/// 원시 타입 및 구조체를 수식합니다.
	/// </summary>
	public class SyncVarAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }
		public SyncDirection SyncDirection { get; private set; }

		public SyncVarAttribute(SyncType sync = SyncType.Reliable,
								SyncDirection dir = SyncDirection.FromMaster)
		{
			SyncType = sync;
			SyncDirection = dir;
		}
	}

	/// <summary>
	/// 멤버 함수가 원격 호출될 수 있음을 나타내는 속성입니다.
	/// </summary>
	public class SyncRpcAttribute : Attribute
	{
		public SyncType SyncType { get; private set; }
		public SyncDirection SyncDirection { get; private set; }

		public SyncRpcAttribute(SyncType sync = SyncType.Reliable,
								SyncDirection dir = SyncDirection.FromMaster)
		{
			SyncType = sync;
			SyncDirection = dir;
		}
	}
}
