using System.Numerics;

namespace KaNet.Physics.RigidBodies
{
	/// <summary>형체가 없는 강체입니다. 연산되지 않습니다.</summary>
	public class NullRigidBody : KaRigidBody
	{
		public NullRigidBody() : base(KaPhysicsShapeType.None, true)
		{
		}

		public override BoundingBox GetBoundingBox()
		{
			return new BoundingBox(Vector2.Zero, 0, 0);
		}

		public override bool IsCollideWith(KaRigidBody otherBody, out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = 0;
			return false;
		}
	}
}
