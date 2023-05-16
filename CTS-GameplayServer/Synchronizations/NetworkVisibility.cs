using System;

namespace CTS.Instance.Synchronizations
{
	/// <summary>
	/// 네트워크 가시성 특성입니다.
	/// 예) Owner | Distance = 소유자에게는 무조건 보이고
	/// 다른 사람에게는 가까운 거리에서만 보입니다.
	/// </summary>
	[Flags]
	public enum NetworkVisibility : byte
	{
		/// <summary>보이지 않습니다.</summary>
		None = 0b_0000_0000,

		/// <summary>모든 대상에게 보입니다.</summary>
		All = 0b_0000_0001,

		/// <summary>소유자에게만 보입니다.</summary>
		Owner = 0b_0000_0010,

		/// <summary>가까운 거리에서만 보입니다.</summary>
		Distance = 0b_0000_0100,

		/// <summary>같은 팀 일때만 보입니다.</summary>
		Team = 0b_0000_1000,

		/// <summary>해당되는 유저에게만 보입니다.</summary>
		UserTargets = 0b_0001_0000,
	}
}
