using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace KaNet.Physics
{
	public static class Physics
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ComputeTransform(in Vector2[] vertices, in Vector2[] transformed,
											in Vector2 position, in float angle)
		{
			float cos = MathF.Cos(angle);
			float sin = MathF.Sin(angle);
			Matrix3x2 r = new Matrix3x2(cos, sin, -sin, cos, 0, 0);
			int length = vertices.Length;
			for (int i = 0; i < length; i++)
			{
				transformed[i] = Vector2.Transform(vertices[i], r) + position;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideAABBs(BoxAABBRigidBody bodyA, BoxAABBRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = 0;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircleAABB(CircleRigidBody bodyA, BoxAABBRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{

			normal = Vector2.Zero;
			depth = 0;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircles(CircleRigidBody bodyA, CircleRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{
			Vector2 centerA = bodyA.Position;
			Vector2 centerB = bodyB.Position;

			float distance = Vector2.Distance(centerA, centerB);
			float radii = bodyA.Radius + bodyB.Radius;

			if (distance < radii)
			{
				normal = Vector2.Normalize(centerB - centerA);
				depth = radii - distance;
				return true;
			}

			normal = Vector2.Zero;
			depth = 0f;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideOBBs(BoxOBBRigidBody bodyA, BoxOBBRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = 0;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircleOBB(CircleRigidBody bodyA, BoxOBBRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = 0;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideAABBOBB(BoxAABBRigidBody bodyA, BoxOBBRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = 0;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IntersectCircles(Vector2 centerA, float radiusA,
											Vector2 centerB, float radiusB)
		{
			float radii = radiusA + radiusB;
			return Vector2.DistanceSquared(centerA, centerB) < radii * radii;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IntersectCircles(Vector2 centerA, float radiusA,
											Vector2 centerB, float radiusB,
											out Vector2 normal, out float depth)
		{
			float distance = Vector2.Distance(centerA, centerB);
			float radii = radiusA + radiusB;

			if (distance < radii)
			{
				normal = Vector2.Normalize(centerB - centerA);
				depth = radii - distance;
				return true;
			}

			normal = Vector2.Zero;
			depth = 0f;
			return false;
		}
	}
}
