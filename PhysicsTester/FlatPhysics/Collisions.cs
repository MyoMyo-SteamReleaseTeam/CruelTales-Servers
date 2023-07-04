using System.Numerics;

namespace FlatPhysics
{
	public static class Collisions
	{
		public static void PointSegmentDistance(Vector2 p, Vector2 a, Vector2 b,
												out float distanceSquared, out Vector2 cp)
		{
			Vector2 ab = b - a;
			Vector2 ap = p - a;

			float proj = Vector2.Dot(ap, ab);
			float abLenSq = ab.LengthSquared();
			float d = proj / abLenSq;

			if (d <= 0f)
				cp = a;
			else if (d >= 1f)
				cp = b;
			else
				cp = a + ab * d;

			distanceSquared = Vector2.DistanceSquared(p, cp);
		}

		public static bool IntersectAABBs(FlatAABB a, FlatAABB b)
		{
			return !(a.Max.X <= b.Min.X || b.Max.X <= a.Min.X ||
				a.Max.Y <= b.Min.Y || b.Max.Y <= a.Min.Y);
		}

		public static void FindContactPoints(FlatBody bodyA, FlatBody bodyB,
											 out Vector2 contact1, out Vector2 contact2,
											 out int contactCount)
		{
			contact1 = Vector2.Zero;
			contact2 = Vector2.Zero;
			contactCount = 0;

			ShapeType shapeTypeA = bodyA.ShapeType;
			ShapeType shapeTypeB = bodyB.ShapeType;

			if (shapeTypeA is ShapeType.Box)
			{
				if (shapeTypeB is ShapeType.Box)
				{
					findPolygonsContactPoints(bodyA.GetTransformedVertices(),
											  bodyB.GetTransformedVertices(),
											  out contact1, out contact2, out contactCount);
				}
				else if (shapeTypeB is ShapeType.Circle)
				{
					findCirclePolygonContactPoint(bodyB.Position, bodyB.Radius, bodyA.Position,
												  bodyA.GetTransformedVertices(), out contact1);
					contactCount = 1;
				}
			}
			else if (shapeTypeA is ShapeType.Circle)
			{
				if (shapeTypeB is ShapeType.Box)
				{
					findCirclePolygonContactPoint(bodyA.Position, bodyA.Radius, bodyB.Position,
												  bodyB.GetTransformedVertices(), out contact1);
					contactCount = 1;
				}
				else if (shapeTypeB is ShapeType.Circle)
				{
					findCirclesContactPoint(bodyA.Position, bodyA.Radius, bodyB.Position, out contact1);
					contactCount = 1;
				}
			}
		}

		private static void findPolygonsContactPoints(Vector2[] verticesA, Vector2[] verticesB,
													  out Vector2 contact1, out Vector2 contact2,
													  out int contactCount)
		{
			contact1 = Vector2.Zero;
			contact2 = Vector2.Zero;
			contactCount = 0;

			float minDistSq = float.MaxValue;

			for (int i = 0; i < verticesA.Length; i++)
			{
				Vector2 p = verticesA[i];

				for (int j = 0; j < verticesB.Length; j++)
				{
					Vector2 va = verticesB[j];
					Vector2 vb = verticesB[(j + 1) % verticesB.Length];

					PointSegmentDistance(p, va, vb, out float distSq, out Vector2 cp);

					if (KaMath.NearlyEqual(distSq, minDistSq))
					{
						if (!KaMath.NearlyEqual(cp, contact1))
						{
							contact2 = cp;
							contactCount = 2;
						}
					}
					else if (distSq < minDistSq)
					{
						minDistSq = distSq;
						contactCount = 1;
						contact1 = cp;
					}
				}
			}

			for (int i = 0; i < verticesB.Length; i++)
			{
				Vector2 p = verticesB[i];

				for (int j = 0; j < verticesA.Length; j++)
				{
					Vector2 va = verticesA[j];
					Vector2 vb = verticesA[(j + 1) % verticesA.Length];

					PointSegmentDistance(p, va, vb, out float distSq, out Vector2 cp);

					if (KaMath.NearlyEqual(distSq, minDistSq))
					{
						if (!KaMath.NearlyEqual(cp, contact1))
						{
							contact2 = cp;
							contactCount = 2;
						}
					}
					else if (distSq < minDistSq)
					{
						minDistSq = distSq;
						contactCount = 1;
						contact1 = cp;
					}
				}
			}
		}

		private static void findCirclePolygonContactPoint(Vector2 circleCenter, float circleRadius,
														  Vector2 polygonCenter, Vector2[] polygonVertices,
														  out Vector2 cp)
		{
			cp = Vector2.Zero;

			float minDistSq = float.MaxValue;

			for (int i = 0; i < polygonVertices.Length; i++)
			{
				Vector2 va = polygonVertices[i];
				Vector2 vb = polygonVertices[(i + 1) % polygonVertices.Length];

				PointSegmentDistance(circleCenter, va, vb, out float distSq, out Vector2 contact);

				if (distSq < minDistSq)
				{
					minDistSq = distSq;
					cp = contact;
				}
			}
		}

		private static void findCirclesContactPoint(Vector2 centerA, float radiusA,
													Vector2 centerB, out Vector2 cp)
		{
			Vector2 ab = centerB - centerA;
			Vector2 dir = Vector2.Normalize(ab);
			cp = centerA + dir * radiusA;
		}

		public static bool Collide(FlatBody bodyA, FlatBody bodyB,
								   out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = 0f;

			ShapeType shapeTypeA = bodyA.ShapeType;
			ShapeType shapeTypeB = bodyB.ShapeType;

			if (shapeTypeA is ShapeType.Box)
			{
				if (shapeTypeB is ShapeType.Box)
				{
					return IntersectPolygons(bodyA.Position, bodyA.GetTransformedVertices(),
											 bodyB.Position, bodyB.GetTransformedVertices(),
											 out normal, out depth);
				}
				else if (shapeTypeB is ShapeType.Circle)
				{
					bool result = IntersectCirclePolygon(bodyB.Position, bodyB.Radius,
														 bodyA.Position, bodyA.GetTransformedVertices(),
														 out normal, out depth);
					normal = -normal;
					return result;
				}
			}
			else if (shapeTypeA is ShapeType.Circle)
			{
				if (shapeTypeB is ShapeType.Box)
				{
					return IntersectCirclePolygon(bodyA.Position, bodyA.Radius,
												  bodyB.Position, bodyB.GetTransformedVertices(),
												  out normal, out depth);
				}
				else if (shapeTypeB is ShapeType.Circle)
				{
					return IntersectCircles(bodyA.Position, bodyA.Radius,
											bodyB.Position, bodyB.Radius,
											out normal, out depth);
				}
			}

			return false;
		}

		public static bool IntersectCirclePolygon(Vector2 circleCenter, float circleRadius,
												  Vector2 polygonCenter, Vector2[] vertices,
												  out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = float.MaxValue;

			Vector2 axis = Vector2.Zero;
			float axisDepth = 0f;
			float minA, maxA, minB, maxB;

			for (int i = 0; i < vertices.Length; i++)
			{
				Vector2 va = vertices[i];
				Vector2 vb = vertices[(i + 1) % vertices.Length];

				Vector2 edge = vb - va;
				axis = new Vector2(-edge.Y, edge.X);
				axis = Vector2.Normalize(axis);

				projectVertices(vertices, axis, out minA, out maxA);
				projectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

				if (minA >= maxB || minB >= maxA)
				{
					return false;
				}

				axisDepth = MathF.Min(maxB - minA, maxA - minB);

				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = axis;
				}
			}

			int cpIndex = findClosestPointOnPolygon(circleCenter, vertices);
			Vector2 cp = vertices[cpIndex];

			axis = cp - circleCenter;
			axis = Vector2.Normalize(axis);

			projectVertices(vertices, axis, out minA, out maxA);
			projectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

			if (minA >= maxB || minB >= maxA)
			{
				return false;
			}

			axisDepth = MathF.Min(maxB - minA, maxA - minB);

			if (axisDepth < depth)
			{
				depth = axisDepth;
				normal = axis;
			}

			Vector2 direction = polygonCenter - circleCenter;

			if (Vector2.Dot(direction, normal) < 0f)
			{
				normal = -normal;
			}

			return true;
		}

		private static int findClosestPointOnPolygon(Vector2 circleCenter, Vector2[] vertices)
		{
			int result = -1;
			float minDistance = float.MaxValue;

			for (int i = 0; i < vertices.Length; i++)
			{
				Vector2 v = vertices[i];
				float distance = Vector2.Distance(v, circleCenter);

				if (distance < minDistance)
				{
					minDistance = distance;
					result = i;
				}
			}

			return result;
		}

		private static void projectCircle(Vector2 center, float radius, Vector2 axis,
										  out float min, out float max)
		{
			Vector2 direction = Vector2.Normalize(axis);
			Vector2 directionAndRadius = direction * radius;

			Vector2 p1 = center + directionAndRadius;
			Vector2 p2 = center - directionAndRadius;

			min = Vector2.Dot(p1, axis);
			max = Vector2.Dot(p2, axis);

			if (min > max)
			{
				// swap the min and max values.
				float t = min;
				min = max;
				max = t;
			}
		}

		public static bool IntersectPolygons(Vector2 centerA, Vector2[] verticesA,
											 Vector2 centerB, Vector2[] verticesB,
											 out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = float.MaxValue;

			for (int i = 0; i < verticesA.Length; i++)
			{
				Vector2 va = verticesA[i];
				Vector2 vb = verticesA[(i + 1) % verticesA.Length];

				Vector2 edge = vb - va;
				Vector2 axis = new Vector2(-edge.Y, edge.X);
				axis = Vector2.Normalize(axis);

				projectVertices(verticesA, axis, out float minA, out float maxA);
				projectVertices(verticesB, axis, out float minB, out float maxB);

				if (minA >= maxB || minB >= maxA)
				{
					return false;
				}

				float axisDepth = MathF.Min(maxB - minA, maxA - minB);

				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = axis;
				}
			}

			for (int i = 0; i < verticesB.Length; i++)
			{
				Vector2 va = verticesB[i];
				Vector2 vb = verticesB[(i + 1) % verticesB.Length];

				Vector2 edge = vb - va;
				Vector2 axis = new Vector2(-edge.Y, edge.X);
				axis = Vector2.Normalize(axis);

				projectVertices(verticesA, axis, out float minA, out float maxA);
				projectVertices(verticesB, axis, out float minB, out float maxB);

				if (minA >= maxB || minB >= maxA)
				{
					return false;
				}

				float axisDepth = MathF.Min(maxB - minA, maxA - minB);

				if (axisDepth < depth)
				{
					depth = axisDepth;
					normal = axis;
				}
			}

			Vector2 direction = centerB - centerA;

			if (Vector2.Dot(direction, normal) < 0f)
			{
				normal = -normal;
			}

			return true;
		}

		private static void projectVertices(Vector2[] vertices, Vector2 axis,
											out float min, out float max)
		{
			min = float.MaxValue;
			max = float.MinValue;

			for (int i = 0; i < vertices.Length; i++)
			{
				Vector2 v = vertices[i];
				float proj = Vector2.Dot(v, axis);

				if (proj < min) { min = proj; }
				if (proj > max) { max = proj; }
			}
		}

		public static bool IntersectCircles(Vector2 centerA, float radiusA,
											Vector2 centerB, float radiusB,
											out Vector2 normal, out float depth)
		{
			normal = Vector2.Zero;
			depth = 0f;

			float distance = Vector2.Distance(centerA, centerB);
			float radii = radiusA + radiusB;

			if (distance >= radii)
				return false;

			normal = Vector2.Normalize(centerB - centerA);
			depth = radii - distance;

			return true;
		}
	}
}
