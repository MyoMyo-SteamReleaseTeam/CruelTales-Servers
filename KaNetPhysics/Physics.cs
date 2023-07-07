//#define NO_OPTIMIZE

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using KaNet.Physics.RigidBodies;

namespace KaNet.Physics
{
	/*
	 * 반환되는 Normal 벡터는 A가 충돌한 지점의 수직 벡터입니다.
	 */

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
		public static void ComputeTransform(in Vector2[] vertices, in Vector2[] transformed,
											in Vector2 position)
		{
			int length = vertices.Length;
			for (int i = 0; i < length; i++)
			{
				transformed[i] = vertices[i] + position;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideAABBs(BoxAABBRigidBody bodyA, BoxAABBRigidBody bodyB,
										  out Vector2 normal, out float depth)
		{
			return IsCollideAABBs(bodyA.GetBoundingBox(),
								  bodyB.GetBoundingBox(),
								  out normal, out depth);
		}

		/// <summary>
		/// 사각형 자체와는 AABB 연산을 수행하고,
		/// 각 꼭지점과는 민코프스키 합으로 계산합니다.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircleAABB(CircleRigidBody bodyA, BoxAABBRigidBody bodyB,
											   out Vector2 normal, out float depth)
		{
			depth = float.MaxValue;
			normal = Vector2.Zero;

			Vector2 aCircleCenter = bodyA.Position;
			Vector2 bBoxCenter = bodyB.Position;
			BoundingBox bBoundIn = bodyB.GetBoundingBox();
			BoundingBox boundOut = new(bBoxCenter,
									   bodyB.Width + bodyA.Diameter,
									   bodyB.Height + bodyA.Diameter);

			if (!IsIntersectPointAABB(aCircleCenter, boundOut))
				return false;

			float closestCornerX = float.MaxValue;
			float closestCornerY = float.MaxValue;

			if (aCircleCenter.X <= bBoundIn.Min.X)
			{
				closestCornerX = bBoundIn.Min.X;
			}
			else if (aCircleCenter.X >= bBoundIn.Max.X)
			{
				closestCornerX = bBoundIn.Max.X;
			}
			else
			{
				if (aCircleCenter.Y < bBoxCenter.Y)
				{
					float d = aCircleCenter.Y - boundOut.Min.Y;
					if (d < depth)
					{
						depth = d;
						normal = new Vector2(0, 1);
					}
				}
				else
				{
					float d = boundOut.Max.Y - aCircleCenter.Y;
					if (d < depth)
					{
						depth = d;
						normal = new Vector2(0, -1);
					}
				}
			}

			if (aCircleCenter.Y <= bBoundIn.Min.Y)
			{
				closestCornerY = bBoundIn.Min.Y;
			}
			else if (aCircleCenter.Y >= bBoundIn.Max.Y)
			{
				closestCornerY = bBoundIn.Max.Y;
			}
			else
			{
				if (aCircleCenter.X < bBoxCenter.X)
				{
					float d = aCircleCenter.X - boundOut.Min.X;
					if (d < depth)
					{
						depth = d;
						normal = new Vector2(1, 0);
					}
				}
				else
				{
					float d = boundOut.Max.X - aCircleCenter.X;
					if (d < depth)
					{
						depth = d;
						normal = new Vector2(-1, 0);
					}
				}
			}

			if (depth < float.MaxValue)
				return true;

			Vector2 closestCorner = new Vector2(closestCornerX, closestCornerY);
			return IsIntersectPointCircle(closestCorner, aCircleCenter, bodyA.Radius,
										  out normal, out depth);
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
			return IsIntersectCircles(bodyA.Position, bodyA.Radius,
									  bodyB.Position, bodyB.Radius,
									  out normal, out depth);
		}

#if NO_OPTIMIZE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideOBBs(BoxOBBRigidBody bodyA, BoxOBBRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = float.MaxValue;

			Vector2 centerA = bodyA.Position;
			Vector2 centerB = bodyB.Position;

			if (!IsIntersectCircles(centerA, bodyA.BoundaryRadius,
									centerB, bodyB.BoundaryRadius))
			{
				return false;
			}

			Vector2[] aVertices = bodyA.GetTransformedVertices();
			Vector2[] bVertices = bodyB.GetTransformedVertices();

			for (int a = 0; a < 4; a++)
			{
				Vector2 aEdge1 = aVertices[a];
				Vector2 aEdge2 = aVertices[(a + 1) % 4];
				Vector2 aNormal = Vector2.Normalize(aEdge2 - aEdge1).RotateLeft();

				float minA = float.MaxValue;
				float minB = float.MaxValue;
				float maxA = float.MinValue;
				float maxB = float.MinValue;

				for (int i = 0; i < 4; i++)
				{
					Vector2 va = aVertices[i];
					float projection = Vector2.Dot(va, aNormal);
					if (projection < minA) { minA = projection; }
					if (projection > maxA) { maxA = projection; }
				}

				for (int i = 0; i < 4; i++)
				{
					Vector2 vb = bVertices[i];
					float projection = Vector2.Dot(vb, aNormal);
					if (projection < minB) { minB = projection; }
					if (projection > maxB) { maxB = projection; }
				}

				if (maxA <= minB || maxB <= minA)
				{
					return false;
				}

				float axisDepth = MathF.Min(maxA - minB, maxB - minA);
				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = aNormal;
				}
			}

			for (int b = 0; b < 4; b++)
			{
				Vector2 bEdge1 = bVertices[b];
				Vector2 bEdge2 = bVertices[(b + 1) % 4];
				Vector2 bNormal = Vector2.Normalize(bEdge2 - bEdge1).RotateLeft();

				float minB = float.MaxValue;
				float minA = float.MaxValue;
				float maxB = float.MinValue;
				float maxA = float.MinValue;

				for (int i = 0; i < 4; i++)
				{
					Vector2 vb = bVertices[i];
					float projection = Vector2.Dot(vb, bNormal);
					if (projection < minB) { minB = projection; }
					if (projection > maxB) { maxB = projection; }
				}

				for (int i = 0; i < 4; i++)
				{
					Vector2 va = aVertices[i];
					float projection = Vector2.Dot(va, bNormal);
					if (projection < minA) { minA = projection; }
					if (projection > maxA) { maxA = projection; }
				}

				if (maxA <= minB || maxB <= minA)
				{
					return false;
				}

				float axisDepth = MathF.Min(maxA - minB, maxB - minA);
				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = bNormal;
				}
			}

			Vector2 aTob = centerB - centerA;
			if (Vector2.Dot(aTob, normal) < 0)
			{
				normal = -normal;
			}

			return true;
		}
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideOBBs(BoxOBBRigidBody bodyA, BoxOBBRigidBody bodyB,
										 out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = float.MaxValue;

			Vector2 centerA = bodyA.Position;
			Vector2 centerB = bodyB.Position;

			if (!IsIntersectCircles(centerA, bodyA.BoundaryRadius,
									centerB, bodyB.BoundaryRadius))
			{
				return false;
			}

			Vector2[] aVertices = bodyA.GetTransformedVertices();
			Vector2[] bVertices = bodyB.GetTransformedVertices();

			for (int i = 0; i < 2; i++)
			{
				Vector2 normalA = Vector2.Normalize(aVertices[i + 1] - aVertices[i]).RotateLeft();
				float minA = Vector2.Dot(aVertices[i + 2], normalA);
				float maxA = Vector2.Dot(aVertices[i], normalA);

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				float projection;

				for (int b = 0; b < 4; b++)
				{
					projection = Vector2.Dot(bVertices[b], normalA);
					if (projection > maxB) { maxB = projection; }
					if (projection < minB) { minB = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;

				float axisDepth = MathF.Min(maxA - minB, maxB - minA);
				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = normalA;
				}
			}

			for (int i = 0; i < 2; i++)
			{
				Vector2 normalB = Vector2.Normalize(bVertices[i + 1] - bVertices[i]).RotateLeft();
				float minB = Vector2.Dot(bVertices[i + 2], normalB);
				float maxB = Vector2.Dot(bVertices[i], normalB);

				float minA = float.MaxValue;
				float maxA = float.MinValue;

				float projection;

				for (int a = 0; a < 4; a++)
				{
					projection = Vector2.Dot(aVertices[a], normalB);
					if (projection < minA) { minA = projection; }
					if (projection > maxA) { maxA = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;

				float axisDepth = MathF.Min(maxA - minB, maxB - minA);
				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = normalB;
				}
			}

			if (Vector2.Dot(centerB - centerA, normal) < 0)
			{
				normal = -normal;
			}

			return true;
		}
#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircleOBB(CircleRigidBody bodyA, BoxOBBRigidBody bodyB,
											  out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = float.MaxValue;

			if (!IsIntersectAABBs(bodyA.GetBoundingBox(), bodyB.GetBoundingBox()))
			{
				return false;
			}

			Vector2 centerA = bodyA.Position;
			Vector2[] verticesB = bodyB.GetTransformedVertices();
			float radiusA = bodyA.Radius;
			bool hasContacted = false;

			for (int i = 0; i < 4; i++)
			{
				Vector2 b1 = verticesB[i];
				Vector2 b2 = verticesB[(i + 1) % 4];

				ComputePointSegmentDistance(centerA, b1, b2, out float d, out Vector2 cp);

				if (d < radiusA && d < depth)
				{
					depth = d;
					normal = Vector2.Normalize(centerA - cp);
					hasContacted = true;
				}
			}

			depth -= radiusA;
			return hasContacted;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideAABBOBB(BoxAABBRigidBody bodyA, BoxOBBRigidBody bodyB,
											out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = float.MaxValue;

			Vector2 centerA = bodyA.Position;
			Vector2 centerB = bodyB.Position;

			BoundingBox boundBoxA = bodyA.GetBoundingBox();
			BoundingBox boundBoxB = bodyB.GetBoundingBox();

			if (!IsIntersectAABBs(boundBoxA, boundBoxB))
				return false;

			Vector2[] bVertices = bodyB.GetTransformedVertices();

			{
				Vector2 normalA = new Vector2(0, 1);
				float minA = boundBoxA.Min.Y;
				float maxA = boundBoxA.Max.Y;

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				float projection;

				for (int b = 0; b < 4; b++)
				{
					projection = bVertices[b].Y;;
					if (projection < minB) { minB = projection; }
					if (projection > maxB) { maxB = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;

				float axisDepth = MathF.Min(maxA - minB, maxB - minA);
				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = normalA;
				}
			}

			{
				Vector2 normalA = new Vector2(1, 0);
				float minA = boundBoxA.Min.X;
				float maxA = boundBoxA.Max.X;

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				float projection;

				for (int b = 0; b < 4; b++)
				{
					projection = bVertices[b].X;
					if (projection < minB) { minB = projection; }
					if (projection > maxB) { maxB = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;

				float axisDepth = MathF.Min(maxA - minB, maxB - minA);
				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = normalA;
				}
			}

			Vector2[] aVertices = bodyA.GetTransformedVertices();

			for (int i = 0; i < 2; i++)
			{
				Vector2 normalB = Vector2.Normalize(bVertices[i + 1] - bVertices[i]).RotateLeft();
				float minB = Vector2.Dot(bVertices[i + 2], normalB);
				float maxB = Vector2.Dot(bVertices[i], normalB);

				float minA = float.MaxValue;
				float maxA = float.MinValue;

				float projection;
				Vector2 va;

				for (int a = 0; a < 4; a++)
				{
					va = aVertices[a];
					projection = Vector2.Dot(va, normalB);
					if (projection < minA) { minA = projection; }
					if (projection > maxA) { maxA = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;

				float axisDepth = MathF.Min(maxA - minB, maxB - minA);
				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = normalB;
				}
			}

			if (Vector2.Dot(centerB - centerA, normal) < 0)
			{
				normal = -normal;
			}

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideAABBs(in BoundingBox boundA, in BoundingBox boundB,
										  out Vector2 normal, out float depth)
		{
			depth = float.MaxValue;
			normal = Vector2.Zero;

			float atobX = boundA.Max.X - boundB.Min.X;
			if (atobX <= 0) return false;
			if (atobX < depth)
			{
				normal = new Vector2(1, 0);
				depth = atobX;
			}

			float atobY = boundA.Max.Y - boundB.Min.Y;
			if (atobY <= 0) return false;
			if (atobY < depth)
			{
				normal = new Vector2(0, 1);
				depth = atobY;
			}

			float btoaX = boundB.Max.X - boundA.Min.X;
			if (btoaX <= 0) return false;
			if (btoaX < depth)
			{
				normal = new Vector2(-1, 0);
				depth = btoaX;
			}

			float btoaY = boundB.Max.Y - boundA.Min.Y;
			if (btoaY <= 0) return false;
			if (btoaY < depth)
			{
				normal = new Vector2(0, -1);
				depth = btoaY;
			}

			return true;
		}

		public static bool IsCollidePointAABB(in Vector2 point, in BoundingBox bound,
											  out Vector2 normal, out float depth)
		{
			depth = float.MaxValue;
			normal = Vector2.Zero;

			float atobX = point.X - bound.Min.X;
			if (atobX <= 0) return false;
			if (atobX < depth)
			{
				normal = new Vector2(1, 0);
				depth = atobX;
			}

			float atobY = point.Y - bound.Min.Y;
			if (atobY <= 0) return false;
			if (atobY < depth)
			{
				normal = new Vector2(0, 1);
				depth = atobY;
			}

			float btoaX = bound.Max.X - point.X;
			if (btoaX <= 0) return false;
			if (btoaX < depth)
			{
				normal = new Vector2(-1, 0);
				depth = btoaX;
			}

			float btoaY = bound.Max.Y - point.Y;
			if (btoaY <= 0) return false;
			if (btoaY < depth)
			{
				normal = new Vector2(0, -1);
				depth = btoaY;
			}

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectCircles(Vector2 centerA, float radiusA,
											  Vector2 centerB, float radiusB)
		{
			float radii = radiusA + radiusB;
			return Vector2.DistanceSquared(centerA, centerB) < radii * radii;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectCircles(Vector2 centerA, float radiusA,
											  Vector2 centerB, float radiusB,
											  out Vector2 normal, out float depth)
		{
			if (centerA == centerB)
			{
				normal = new Vector2(1, 0);
				depth = radiusA;
				return true;
			}

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
		public static bool IsIntersectPointCircle(Vector2 point, Vector2 circleCenter, float radius)
		{
			return Vector2.Distance(circleCenter, point) < radius;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectPointCircle(Vector2 point, Vector2 circleCenter, float radius,
												  out Vector2 normal, out float depth)
		{
			if (point == circleCenter)
			{
				normal = new Vector2(1, 0);
				depth = radius;
				return true;
			}

			float distance = Vector2.Distance(point, circleCenter);
			if (distance < radius)
			{
				normal = Vector2.Normalize(point - circleCenter);
				depth = radius - distance;
				return true;
			}

			normal = Vector2.Zero;
			depth = 0f;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectAABBs(in BoundingBox boundA, in BoundingBox boundB)
		{
			return !(boundA.Max.X <= boundB.Min.X || boundA.Max.Y <= boundB.Min.Y ||
					 boundB.Max.X <= boundA.Min.X || boundB.Max.Y <= boundA.Min.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectPointAABB(in Vector2 point, in BoundingBox bound)
		{
			return !(point.X <= bound.Min.X || point.Y <= bound.Min.Y ||
					 bound.Max.X <= point.X || bound.Max.Y <= point.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectCircleAABB(Vector2 centerA, float radiusA,
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectOBBs(Vector2 centerA, float boundaryRadiusA, Vector2[] aVertices, 
										   Vector2 centerB, float boundaryRadiusB, Vector2[] bVertices)
		{
			if (!IsIntersectCircles(centerA, boundaryRadiusA,
									centerB, boundaryRadiusB))
			{
				return false;
			}

			for (int i = 0; i < 2; i++)
			{
				Vector2 normalA = Vector2.Normalize(aVertices[i + 1] - aVertices[i]).RotateLeft();
				float maxA = Vector2.Dot(aVertices[i], normalA);
				float minA = Vector2.Dot(aVertices[i + 2], normalA);

				float maxB = float.MinValue;
				float minB = float.MaxValue;

				float projection;
				Vector2 vb;

				for (int b = 0; b < 4; b++)
				{
					vb = bVertices[b];
					projection = Vector2.Dot(vb, normalA);
					if (projection < minB) { minB = projection; }
					if (projection > maxB) { maxB = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;
			}

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IsIntersectCircleSegment(Vector2 p, Vector2 a, Vector2 b,
													out float distance, out Vector2 nearestPoint)
		{
			// TODO
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ComputePointSegmentDistance(Vector2 p, Vector2 a, Vector2 b,
													   out float distance, out Vector2 nearestPoint)
		{
			Vector2 ab = b - a;
			Vector2 ap = p - a;

			float projection = Vector2.Dot(ap, ab);
			float abLengthSq = ab.LengthSquared();
			float d = projection / abLengthSq;

			if (d <= 0f)
			{
				nearestPoint = a;
			}
			else if (d >= 1f)
			{
				nearestPoint = b;
			}
			else
			{
				nearestPoint = a + ab * d;
			}

			distance = Vector2.Distance(p, nearestPoint);
		}
	}
}
