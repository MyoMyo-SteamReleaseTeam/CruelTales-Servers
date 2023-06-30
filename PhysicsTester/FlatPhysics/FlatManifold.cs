using System.Numerics;

namespace FlatPhysics
{
	public readonly struct FlatManifold
	{
		public readonly FlatBody BodyA;
		public readonly FlatBody BodyB;

		public readonly Vector2 Normal;
		public readonly float Depth;

		public readonly Vector2 Contact1;
		public readonly Vector2 Contact2;
		public readonly int ContactCount;

		public FlatManifold(FlatBody bodyA,
							FlatBody bodyB, 
							Vector2 normal, 
							float depth, 
							Vector2 contact1, 
							Vector2 contact2, 
							int contactCount)
		{
			BodyA = bodyA;
			BodyB = bodyB;
			Normal = normal;
			Depth = depth;
			Contact1 = contact1;
			Contact2 = contact2;
			ContactCount = contactCount;
		}
	}
}
