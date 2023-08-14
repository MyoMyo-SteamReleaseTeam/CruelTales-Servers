namespace CT.CorePatcher.SynchronizationsCodeGen
{
	/// <summary>동기화 오브젝트 타입입니다.</summary>
	public enum SyncObjectType
	{
		None,

		/// <summary>
		/// 네트워크 객체 타입입니다.
		/// </summary>
		NetworkObject,

		/// <summary>
		/// 단순 동기화 객체 타입입니다.
		/// 네트워크 객체 내부나 Collection 안에 할당됩니다.
		/// </summary>
		SyncObject,
	}
}