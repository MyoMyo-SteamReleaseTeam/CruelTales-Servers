namespace CT.Common.Synchronizations
{
	/// <summary>
	/// 동기화 흐름 방향입니다.
	/// </summary>
	public enum SyncDirection
	{
		None = 0,

		/// <summary>
		/// Master에서 Remote로 전송합니다.
		/// Server에서 Client로 동기화합니다.
		/// </summary>
		FromMaster,

		/// <summary>
		/// Remote에서 Master로 전송합니다.
		/// Client에서 Server로 동기화합니다.
		/// </summary>
		FromRemote,

		/// <summary>
		/// 양방향으로 동기화합니다.
		/// 네트워크 객체인 경우만 가능합니다.
		/// </summary>
		Bidirection,
	}

	public static class SyncDirectionExtension
	{
		public static string GetDeclarationComment(this SyncDirection value)
		{
			switch (value)
			{
				case SyncDirection.FromMaster: return $"/// DECLARE MASTER SIDE SYNC ELEMETS ///";
				case SyncDirection.FromRemote: return $"/// DECLARE REMOTE SIDE SYNC ELEMETS ///";
			}

			return string.Empty;
		}

		public static string GetRegionContent(this SyncDirection value) => $"#region {value}";

		public static SyncDirection Reverse(this SyncDirection value)
		{
			if (value == SyncDirection.None)
				return SyncDirection.None;

			return (value == SyncDirection.FromMaster) ?
				SyncDirection.FromRemote : SyncDirection.FromMaster;
		}
	}
}
