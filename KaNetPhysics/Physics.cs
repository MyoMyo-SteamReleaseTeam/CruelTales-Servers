using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using KaNet.Physics.RigidBodies;

namespace KaNet.Physics
{
	public static class Physics
	{
		public const float FLOAT_EPSILON = 0.001f;

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
			return IsCollideAABBs(bodyA.GetBoundingBox(), bodyB.GetBoundingBox(),
										 out normal, out depth);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircleAABB(CircleRigidBody bodyA, BoxAABBRigidBody bodyB,
									   out Vector2 normal, out float depth)
		{
			Vector2 centerA = bodyA.Position;
			BoundingBox boundingB = bodyB.GetBoundingBox();

			float closestX = KaMath.Clamp(centerA.X, boundingB.Min.X, boundingB.Max.X);
			float closestY = KaMath.Clamp(centerA.Y, boundingB.Min.Y, boundingB.Max.Y);

			Vector2 closestPoint = new Vector2(closestX, closestY);
			Vector2 pointToCircle = centerA - closestPoint;

			if (pointToCircle == Vector2.Zero)
			{
				normal = new Vector2(1, 0);
				depth = bodyA.Radius;
				return true;
			}

			depth = bodyA.Radius * bodyA.Radius - pointToCircle.LengthSquared();

			if (depth > 0)
			{
				normal = Vector2.Normalize(pointToCircle);
				return true;
			}
			else
			{
				normal = Vector2.Zero;
				return false;
			}
		}

		/*
		 * AABB의 4개 꼭지점을 기준으로 접촉 검사
		 * 원이 사각형 안에 들어온 경우 법선 벡터의 값이 잘못되는 오류가 있음
		 */
		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static bool IsCollideCircleAABB(CircleRigidBody bodyA, BoxAABBRigidBody bodyB,
		//									   out Vector2 normal, out float depth)
		//{
		//	Vector2 centerA = bodyA.Position;
		//	BoundingBox boundingB = bodyB.GetBoundingBox();

		//	float closestX = KaMath.Clamp(centerA.X, boundingB.Min.X, boundingB.Max.X);
		//	float closestY = KaMath.Clamp(centerA.Y, boundingB.Min.Y, boundingB.Max.Y);

		//	Vector2 closestPoint = new Vector2(closestX, closestY);
		//	Vector2 pointToCircle = centerA - closestPoint;

		//	depth = bodyA.Radius * bodyA.Radius - pointToCircle.LengthSquared();
			
		//	if (depth > 0)
		//	{
		//		normal = Vector2.Normalize(pointToCircle);
		//		return true;
		//	}
		//	else
		//	{
		//		normal = Vector2.Zero;
		//		return false;
		//	}
		//}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircles(CircleRigidBody bodyA, CircleRigidBody bodyB,
											out Vector2 normal, out float depth)
		{
			Vector2 centerA = bodyA.Position;
			Vector2 centerB = bodyB.Position;

			if (centerA == centerB)
			{
				normal = new Vector2(1, 0);
				depth = bodyA.Radius;
				return true;
			}

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
		public static bool IsCollideAABBs(in BoundingBox boundA, in BoundingBox boundB,
										  out Vector2 normal, out float depth)
		{
			depth = float.MaxValue;
			normal = Vector2.Zero;

			float atobX = boundA.Max.X - boundB.Min.X;

			if (atobX <= 0)
				return false;

			if (atobX < depth)
			{
				normal = new Vector2(1, 0);
				depth = atobX;
			}

			float atobY = boundA.Max.Y - boundB.Min.Y;

			if (atobY <= 0)
				return false;

			if (atobY < depth)
			{
				normal = new Vector2(0, 1);
				depth = atobY;
			}

			float btoaX = boundB.Max.X - boundA.Min.X;

			if (btoaX <= 0)
				return false;

			if (btoaX < depth)
			{
				normal = new Vector2(-1, 0);
				depth = btoaX;
			}

			float btoaY = boundB.Max.Y - boundA.Min.Y;

			if (btoaY <= 0)
				return false;

			if (btoaY < depth)
			{
				normal = new Vector2(0, -1);
				depth = btoaY;
			}

			return true;
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IntersectAABBs(in BoundingBox boundA, in BoundingBox boundB)
		{
			return !(boundA.Max.X <= boundB.Min.X || boundA.Max.Y <= boundB.Min.Y ||
					 boundB.Max.X <= boundA.Min.X || boundB.Max.Y <= boundA.Min.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IntersectCircleAABB(Vector2 centerA, float radiusA,
											   BoundingBox boundingB)
		{
			float closestX = KaMath.Clamp(centerA.X, boundingB.Min.X, boundingB.Max.X);
			float closestY = KaMath.Clamp(centerA.Y, boundingB.Min.Y, boundingB.Max.Y);
			Vector2 closestPoint = new Vector2(closestX, closestY);

			/*
			 * 접촉검사 적중률이 높으면 Early Return이 더 빠를 수 있다.
			 * 하지만 일반적으로 적중률은 높지 않기 때문에
			 * 함수의 실행 속도 편차가 크지 않도록 분기를 제거함
			 */

			//if (closestPoint == centerA)
			//	return true;

			Vector2 pointToCircle = centerA - closestPoint;
			return radiusA * radiusA > pointToCircle.LengthSquared();
		}
	}
}
