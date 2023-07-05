using System.Numerics;

public static class VectorExtension
{
	#region Vector2

	public static Vector2 FlipY(this Vector2 v) => new Vector2(v.X, -v.Y);
	public static Vector2 FlipX(this Vector2 v) => new Vector2(-v.X, v.Y);

	/// <summary>시계방향으로 90도 회전합니다.</summary>
	public static Vector2 RotateRight(this Vector2 v) => new Vector2(v.Y, -v.X);

	/// <summary>반시계방향으로 90도 회전합니다.</summary>
	public static Vector2 RotateLeft(this Vector2 v) => new Vector2(-v.Y, v.X);

	#endregion

	/// <summary>벡터의 X를 min으로 Y를 max로 하는 범위에서 렌덤한 값을 반환합니다.</summary>
	/// <returns>렌덤한 값</returns>
	public static float GetRandomFromMinMax(this Vector2 v)
	{
		return RandomHelper.NextSingle(v.X, v.Y);
	}
}
