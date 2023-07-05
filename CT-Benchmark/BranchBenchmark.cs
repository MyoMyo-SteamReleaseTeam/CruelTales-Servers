using System.Numerics;
using BenchmarkDotNet.Attributes;
using KaNet.Physics;

namespace CT.Benchmark
{
	public class BranchBenchmark
	{
		private const int TEST_COUNT = 1000000;
		private BoundingBox _boundingBox = new BoundingBox(new Vector2(10, 10), 10, 5);
		private List<(Vector2 Position, float Radius)> _testCases = new(TEST_COUNT);

		public BranchBenchmark()
		{
			for (int i = 0; i < TEST_COUNT; i++)
			{
				_testCases.Add((RandomHelper.NextVector2() * 20, RandomHelper.NextSingle(3, 5)));
			}
		}

		[Benchmark]
		public void PhysicsEarlyReturn()
		{
			for (int i = 0; i < TEST_COUNT; i++)
			{
				var testCase = _testCases[i];
				IntersectCircleAABBIf(testCase.Position, testCase.Radius, _boundingBox);
			}
		}

		[Benchmark]
		public void PhysicsReturn()
		{
			for (int i = 0; i < TEST_COUNT; i++)
			{
				var testCase = _testCases[i];
				IntersectCircleAABB(testCase.Position, testCase.Radius, _boundingBox);
			}
		}

		[Benchmark]
		public void PhysicsReturnShort()
		{
			for (int i = 0; i < TEST_COUNT; i++)
			{
				var testCase = _testCases[i];
				IntersectCircleAABBShort(testCase.Position, testCase.Radius, _boundingBox);
			}
		}

		public static bool IntersectCircleAABBIf(Vector2 centerA, float radiusA, BoundingBox boundingB)
		{
			float closestX = KaMath.Clamp(centerA.X, boundingB.Min.X, boundingB.Max.X);
			float closestY = KaMath.Clamp(centerA.Y, boundingB.Min.Y, boundingB.Max.Y);
			Vector2 closestPoint = new Vector2(closestX, closestY);

			if (closestPoint == centerA)
				return true;

			Vector2 pointToCircle = centerA - closestPoint;
			return radiusA * radiusA > pointToCircle.LengthSquared();
		}

		public static bool IntersectCircleAABB(Vector2 centerA, float radiusA, BoundingBox boundingB)
		{
			float closestX = KaMath.Clamp(centerA.X, boundingB.Min.X, boundingB.Max.X);
			float closestY = KaMath.Clamp(centerA.Y, boundingB.Min.Y, boundingB.Max.Y);
			Vector2 closestPoint = new Vector2(closestX, closestY);
			Vector2 pointToCircle = centerA - closestPoint;
			return radiusA * radiusA > pointToCircle.LengthSquared();
		}

		public static bool IntersectCircleAABBShort(Vector2 centerA, float radiusA, BoundingBox boundingB)
		{
			return radiusA * radiusA > (centerA - new Vector2(KaMath.Clamp(centerA.X, boundingB.Min.X, boundingB.Max.X),
															  KaMath.Clamp(centerA.Y, boundingB.Min.Y, boundingB.Max.Y)))
															  .LengthSquared();
		}
	}
}
