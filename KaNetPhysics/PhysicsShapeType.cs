namespace KaNet.Physics
{
	/// <summary>물리 형체 타입</summary>
	public enum PhysicsShapeType
	{
		None = 0,

		/// <summary>축 정렬된 직사각형</summary>
		Box_AABB = 1,

		/// <summary>회전된 직사각형</summary>
		Box_OBB = 2,
		
		/// <summary>원</summary>
		Circle = 3,
	}
}
