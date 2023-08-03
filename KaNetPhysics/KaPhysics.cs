using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using KaNet.Physics.RigidBodies;

namespace KaNet.Physics
{
	/*
	 * 반환되는 Normal 벡터는 A가 충돌한 지점의 수직 벡터입니다.
	 */

	public static class KaPhysics
	{
		public const float MIN_COLLIDER_SIZE = 0.2f;
		public const float FLOAT_EPSILON = 0.001f;

		// Comparison

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NearlyEqual(float a, float b)
		{
			return MathF.Abs(a - b) < FLOAT_EPSILON;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NearlyEqual(Vector2 a, Vector2 b)
		{
			return NearlyEqual(a.X, b.X) && NearlyEqual(a.Y, b.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NearlyNotEqual(float a, float b)
		{
			return MathF.Abs(a - b) >= FLOAT_EPSILON;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NearlyNotEqual(Vector2 a, Vector2 b)
		{
			return NearlyNotEqual(a.X, b.X) || NearlyNotEqual(a.Y, b.Y);
		}

		// Transform

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ComputeTransform(in Vector2[] vertices, in Vector2[] transformed,
											in Vector2 position, in float rotation)
		{
			// Right-handed coordinate system
			//float cos = MathF.Cos(rotation);
			//float sin = MathF.Sin(rotation);

			// Left-handed coordinate system
			float cos = MathF.Cos(-rotation);
			float sin = MathF.Sin(-rotation);

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

		// Compute

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

		// Collision

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

			Vector2 centerA = bodyA.Position;
			Vector2 centerB = bodyB.Position;
			BoundingBox boundInB = bodyB.GetBoundingBox();
			BoundingBox boundOutB = new(centerB,
										bodyB.Width + bodyA.Diameter,
										bodyB.Height + bodyA.Diameter);

			if (!IsIntersectPointAABB(centerA, boundOutB))
				return false;

			float closestVertexX = float.MaxValue;
			float closestVertexY = float.MaxValue;

			if (centerA.X <= boundInB.Min.X)
			{
				closestVertexX = boundInB.Min.X;
			}
			else if (centerA.X >= boundInB.Max.X)
			{
				closestVertexX = boundInB.Max.X;
			}
			else
			{
				if (centerA.Y < centerB.Y)
				{
					depth = centerA.Y - boundOutB.Min.Y;
					normal = new Vector2(0, 1);
				}
				else
				{
					depth = boundOutB.Max.Y - centerA.Y;
					normal = new Vector2(0, -1);
				}
			}

			if (centerA.Y <= boundInB.Min.Y)
			{
				closestVertexY = boundInB.Min.Y;
			}
			else if (centerA.Y >= boundInB.Max.Y)
			{
				closestVertexY = boundInB.Max.Y;
			}
			else
			{
				if (centerA.X < centerB.X)
				{
					float d = centerA.X - boundOutB.Min.X;
					if (d < depth)
					{
						depth = d;
						normal = new Vector2(1, 0);
					}
				}
				else
				{
					float d = boundOutB.Max.X - centerA.X;
					if (d < depth)
					{
						depth = d;
						normal = new Vector2(-1, 0);
					}
				}
			}

			if (depth < float.MaxValue)
				return true;

			Vector2 closestCorner = new Vector2(closestVertexX, closestVertexY);
			return IsIntersectPointCircle(closestCorner, centerA, bodyA.Radius,
										  out normal, out depth);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCollideCircles(CircleRigidBody bodyA, CircleRigidBody bodyB,
											out Vector2 normal, out float depth)
		{
			return IsIntersectCircles(bodyA.Position, bodyA.Radius,
									  bodyB.Position, bodyB.Radius,
									  out normal, out depth);
		}

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

			Vector2[] verticesA = bodyA.GetTransformedVertices();
			Vector2[] verticesB = bodyB.GetTransformedVertices();

			for (int i = 0; i < 2; i++)
			{
				Vector2 normalA = Vector2.Normalize(verticesA[i + 1] - verticesA[i]).RotateLeft();
				float minA = Vector2.Dot(verticesA[i + 2], normalA);
				float maxA = Vector2.Dot(verticesA[i], normalA);

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				float projection;

				for (int b = 0; b < 4; b++)
				{
					projection = Vector2.Dot(verticesB[b], normalA);
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
				Vector2 normalB = Vector2.Normalize(verticesB[i + 1] - verticesB[i]).RotateLeft();
				float minB = Vector2.Dot(verticesB[i + 2], normalB);
				float maxB = Vector2.Dot(verticesB[i], normalB);

				float minA = float.MaxValue;
				float maxA = float.MinValue;

				float projection;

				for (int a = 0; a < 4; a++)
				{
					projection = Vector2.Dot(verticesA[a], normalB);
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
		public static bool IsCollideCircleOBB(CircleRigidBody bodyA, BoxOBBRigidBody bodyB,
											  out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = float.MaxValue;

			Vector2 centerA = bodyA.Position;
			float radiusA = bodyA.Radius;
			Vector2 centerB = bodyB.Position;

			if (!IsIntersectCircles(centerA, radiusA, centerB, bodyB.BoundaryRadius))
				return false;

			Vector2[] verticesB = bodyB.GetTransformedVertices();
			float widthB = bodyB.Width;
			float heightB = bodyB.Height;

			float closestVertexRefX = float.MaxValue;
			float closestVertexRefY = float.MaxValue;

			Vector2 b0 = verticesB[0];
			Vector2 b1 = verticesB[1];
			Vector2 b3 = verticesB[3];

			Vector2 normalRight = Vector2.Normalize(b1 - b0);
			Vector2 normalUp = normalRight.RotateLeft();

			float centerProjXA = Vector2.Dot(normalRight, centerA);
			float centerProjXB = Vector2.Dot(normalRight, centerB);

			float maxInXB = Vector2.Dot(normalRight, b1);
			float minInXB = maxInXB - widthB;
			float maxOutXB = maxInXB + radiusA;
			float minOutXB = minInXB - radiusA;

			float centerProjYA = Vector2.Dot(normalUp, centerA);
			float centerProjYB = Vector2.Dot(normalUp, centerB);

			float maxInYB = Vector2.Dot(normalUp, b1);
			float minInYB = maxInYB - heightB;
			float maxOutYB = maxInYB + radiusA;
			float minOutYB = minInYB - radiusA;

			if (centerProjXA <= minInXB)
			{
				closestVertexRefX = 0;
			}
			else if (centerProjXA >= maxInXB)
			{
				closestVertexRefX = widthB;
			}
			else
			{
				if (centerProjYA < centerProjYB)
				{
					depth = centerProjYA - minOutYB;
					if (depth <= 0)
						return false;
					normal = normalUp;
				}
				else
				{
					depth = maxOutYB - centerProjYA;
					if (depth <= 0)
						return false;
					normal = -normalUp;
				}
			}

			if (centerProjYA <= minInYB)
			{
				closestVertexRefY = 0;
			}
			else if (centerProjYA >= maxInYB)
			{
				closestVertexRefY = heightB;
			}
			else
			{
				if (centerProjXA < centerProjXB)
				{
					float d = centerProjXA - minOutXB;
					if (d <= 0) return false;
					if (d < depth)
					{
						depth = d;
						normal = normalRight;
					}
				}
				else
				{
					float d = maxOutXB - centerProjXA;
					if (d <= 0) return false;
					if (d < depth)
					{
						depth = d;
						normal = -normalRight;
					}
				}
			}

			if (depth < float.MaxValue)
				return true;

			Vector2 closestVertex = b3 + normalRight * closestVertexRefX + normalUp * closestVertexRefY;
			return IsIntersectPointCircle(closestVertex, centerA, bodyA.Radius,
										  out normal, out depth);
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

			Vector2[] verticesB = bodyB.GetTransformedVertices();

			{
				Vector2 normalA = new Vector2(0, 1);
				float minA = boundBoxA.Min.Y;
				float maxA = boundBoxA.Max.Y;

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				float projection;

				for (int b = 0; b < 4; b++)
				{
					projection = verticesB[b].Y; ;
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
					projection = verticesB[b].X;
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

			Vector2[] verticesA = bodyA.GetTransformedVertices();

			for (int i = 0; i < 2; i++)
			{
				Vector2 normalB = Vector2.Normalize(verticesB[i + 1] - verticesB[i]).RotateLeft();
				float minB = Vector2.Dot(verticesB[i + 2], normalB);
				float maxB = Vector2.Dot(verticesB[i], normalB);

				float minA = float.MaxValue;
				float maxA = float.MinValue;

				float projection;
				Vector2 va;

				for (int a = 0; a < 4; a++)
				{
					va = verticesA[a];
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
			normal = new Vector2(1, 0);
			depth = atobX;

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		// Intersection

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
												 Vector2 centerB, float widthB, float heightB)
		{
			float diameterA = radiusA * 2;

			BoundingBox boundInB = new(centerB, widthB, heightB);
			BoundingBox boundOutB = new(centerB, widthB + diameterA, heightB + diameterA);

			if (!IsIntersectPointAABB(centerA, boundOutB))
				return false;

			float closestVertexX;
			float closestVertexY;

			if (centerA.X <= boundInB.Min.X)
			{
				closestVertexX = boundInB.Min.X;
			}
			else if (centerA.X >= boundInB.Max.X)
			{
				closestVertexX = boundInB.Max.X;
			}
			else
			{
				return true;
			}

			if (centerA.Y <= boundInB.Min.Y)
			{
				closestVertexY = boundInB.Min.Y;
			}
			else if (centerA.Y >= boundInB.Max.Y)
			{
				closestVertexY = boundInB.Max.Y;
			}
			else
			{
				return true;
			}

			return IsIntersectPointCircle(new Vector2(closestVertexX, closestVertexY),
										  centerA, radiusA);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectCircleOBB(Vector2 centerA, float radiusA,
												Vector2 centerB, Vector2[] verticesB,
												float widthB, float heightB, float boundaryRadius)
		{
			if (!IsIntersectCircles(centerA, radiusA, centerB, boundaryRadius))
				return false;

			Vector2 b0 = verticesB[0];
			Vector2 b1 = verticesB[1];
			Vector2 b3 = verticesB[3];

			Vector2 normalRight = Vector2.Normalize(b1 - b0);
			Vector2 normalUp = normalRight.RotateLeft();

			float centerProjXA = Vector2.Dot(normalRight, centerA);
			float centerProjXB = Vector2.Dot(normalRight, centerB);

			float maxInXB = Vector2.Dot(normalRight, b1);
			float minInXB = maxInXB - widthB;
			float maxOutXB = maxInXB + radiusA;
			float minOutXB = minInXB - radiusA;

			float centerProjYA = Vector2.Dot(normalUp, centerA);
			float centerProjYB = Vector2.Dot(normalUp, centerB);

			float maxInYB = Vector2.Dot(normalUp, b1);
			float minInYB = maxInYB - heightB;
			float maxOutYB = maxInYB + radiusA;
			float minOutYB = minInYB - radiusA;

			float closestVertexRefX;
			float closestVertexRefY;

			if (centerProjXA <= minInXB)
			{
				closestVertexRefX = 0;
			}
			else if (centerProjXA >= maxInXB)
			{
				closestVertexRefX = widthB;
			}
			else
			{
				if (centerProjYA < centerProjYB)
				{
					return centerProjYA - minOutYB > 0;
				}
				else
				{
					return maxOutYB - centerProjYA > 0;
				}
			}

			if (centerProjYA <= minInYB)
			{
				closestVertexRefY = 0;
			}
			else if (centerProjYA >= maxInYB)
			{
				closestVertexRefY = heightB;
			}
			else
			{
				if (centerProjXA < centerProjXB)
				{
					return centerProjXA - minOutXB > 0;
				}
				else
				{
					return maxOutXB - centerProjXA > 0;
				}
			}

			Vector2 closestVertex = b3 + normalRight * closestVertexRefX + normalUp * closestVertexRefY;
			return IsIntersectPointCircle(closestVertex, centerA, radiusA);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIntersectAABBOBB(BoundingBox boundA,
											  Vector2[] verticesB, BoundingBox boundB)
		{
			if (!IsIntersectAABBs(boundA, boundB))
				return false;

			Span<Vector2> verticesA = stackalloc Vector2[4];

			verticesA[0] = boundA.LeftTop;
			verticesA[1] = boundA.RightTop;
			verticesA[2] = boundA.RightBottom;
			verticesA[3] = boundA.LeftBottom;

			{
				float minA = boundA.Min.Y;
				float maxA = boundA.Max.Y;

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				float projection;

				for (int b = 0; b < 4; b++)
				{
					projection = verticesB[b].Y; ;
					if (projection < minB) { minB = projection; }
					if (projection > maxB) { maxB = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;
			}

			{
				float minA = boundA.Min.X;
				float maxA = boundA.Max.X;

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				float projection;

				for (int b = 0; b < 4; b++)
				{
					projection = verticesB[b].X;
					if (projection < minB) { minB = projection; }
					if (projection > maxB) { maxB = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;
			}

			for (int i = 0; i < 2; i++)
			{
				Vector2 normalB = Vector2.Normalize(verticesB[i + 1] - verticesB[i]).RotateLeft();
				float minB = Vector2.Dot(verticesB[i + 2], normalB);
				float maxB = Vector2.Dot(verticesB[i], normalB);

				float minA = float.MaxValue;
				float maxA = float.MinValue;

				float projection;
				Vector2 va;

				for (int a = 0; a < 4; a++)
				{
					va = verticesA[a];
					projection = Vector2.Dot(va, normalB);
					if (projection < minA) { minA = projection; }
					if (projection > maxA) { maxA = projection; }
				}

				if (maxA <= minB || maxB <= minA)
					return false;
			}

			return true;
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
	}
}
