namespace CT.Common.Gameplay
{
	/// <summary>상호작용 객체와 상호작용한 타입입니다.</summary>
	public enum Input_InteractType : byte
	{
		None = 0,

		/// <summary>한 번 눌렀습니다.</summary>
		Switch,
		
		/// <summary>누르고 있습니다.</summary>
		Progress,
	}
}
