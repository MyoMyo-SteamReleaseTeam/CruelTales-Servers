namespace CTS.Instance.Synchronizations
{
	/// <summary>
	/// 네트워크 객체가 보일 조건 특성입니다.
	/// </summary>
	public enum VisibilityType
	{
		/// <summary>보이지 않습니다.</summary>
		None = 0,

		/// <summary>거리와 상관 없이 보입니다.</summary>
		Global,

		/// <summary>시야 내에 있을 때 보입니다.</summary>
		View,
	}

	/// <summary>
	/// 네트워크 객체가 보일 대상을 결정합니다.
	/// 예) Owner = 소유자에게는 무조건 보이고
	/// 다른 사람에게는 가까운 거리에서만 보입니다.
	/// 예) All | Global = 모든 사람에게 거리 상관없이 보입니다.
	/// 예) Team | Global = 같은 팀에게 거리 상관없이 보입니다.
	/// </summary>
	public enum VisibilityAuthority
	{
		/// <summary>보이지 않습니다.</summary>
		None,

		/// <summary>모든 대상에게 보입니다.</summary>
		All,

		/// <summary>소유자에게만 보입니다.</summary>
		Owner,

		/// <summary>같은 소속일 때 보입니다.</summary>
		Faction,

		/// <summary>해당되는 유저에게만 보입니다.</summary>
		Users,
	}

	public enum Faction : byte
	{
		/// <summary>시스템입니다.</summary>
		System,

		/// <summary>중립입니다.</summary>
		Neutral,

		/// <summary>관전자입니다.</summary>
		Speculator,

		Red,
		Bule,
		Green,

		// RedHood
		RedHood_RedHood,
		RedHood_Wolf,
		RedHood_Freeman,

		// Dueoksini
		// Horus
	}
}
