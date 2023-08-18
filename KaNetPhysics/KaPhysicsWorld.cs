using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using KaNet.Physics.RigidBodies;

namespace KaNet.Physics
{
	public class KaPhysicsWorld
	{
		private List<KaRigidBody> _rigidBodies = new(128);
		private List<KaRigidBody>? _staticRigidBodies;
		public int BodyCount => _rigidBodies.Count;
		private List<int> _raycastHitList = new(128);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddRigidBody(KaRigidBody rigidBody)
		{
			_rigidBodies.Add(rigidBody);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RemoveRigidBody(KaRigidBody rigidBody)
		{
			_rigidBodies.Remove(rigidBody);
		}

		public void SetStaticRigidBodies(List<KaRigidBody> staticRigidBodies)
		{
			_staticRigidBodies = staticRigidBodies;
		}

		public void ReleaseStaticRigidBodies()
		{
			_staticRigidBodies = null;
		}

		public KaRigidBody CreateCircle(float radius, bool isStatic, PhysicsLayerMask layerMask)
		{
			var rigid = new CircleRigidBody(radius, isStatic, layerMask);
			this.AddRigidBody(rigid);
			return rigid;
		}

		public KaRigidBody CreateBoxAABB(float width, float height,
										 bool isStatic, PhysicsLayerMask layerMask)
		{
			var rigid = new BoxAABBRigidBody(width, height, isStatic, layerMask);
			this.AddRigidBody(rigid);
			return rigid;
		}

		public KaRigidBody CreateBoxOBB(float width, float height, float rotation,
										bool isStatic, PhysicsLayerMask layerMask)
		{
			var rigid = new BoxOBBRigidBody(width, height, rotation, isStatic, layerMask);
			this.AddRigidBody(rigid);
			return rigid;
		}

		public void Step(float stepTime)
		{
			// Step objects
			int bodyCount = _rigidBodies.Count;

			for (int i = 0; i < bodyCount; i++)
			{
				_rigidBodies[i].Step(stepTime);
			}

			// Solve with static objects
			if (_staticRigidBodies != null)
			{
				int staticBodyCount = _staticRigidBodies.Count;
				int rigidBodyCount = _rigidBodies.Count;

				for (int s = 0; s < staticBodyCount; s++)
				{
					KaRigidBody bodyS = _staticRigidBodies[s];

					for (int r = 0; r < rigidBodyCount; r++)
					{
						KaRigidBody bodyR = _rigidBodies[r];

						if ((bodyS.LayerMask.Mask & bodyR.LayerMask.Flags) != bodyS.LayerMask.Mask)
							continue;

						if (!bodyS.IsCollideWith(bodyR, out Vector2 normal, out float depth))
							continue;

						solveIfCollideWithStatic(bodyR, normal, depth);
					}
				}
			}

			// Solve in physics world
			for (int a = 0; a < bodyCount - 1; a++)
			{
				KaRigidBody bodyA = _rigidBodies[a];

				for (int b = a + 1; b < bodyCount; b++)
				{
					KaRigidBody bodyB = _rigidBodies[b];

					if ((bodyA.LayerMask.Mask & bodyB.LayerMask.Flags) != bodyA.LayerMask.Mask)
						continue;

					if (!bodyA.IsCollideWith(bodyB, out Vector2 normal, out float depth))
						continue;

					if (bodyA.IsStatic && bodyB.IsStatic)
						continue;

					if (!bodyA.IsStatic && !bodyB.IsStatic)
					{
						bodyA.OnCollided(bodyB.ID);
						bodyB.OnCollided(bodyA.ID);
					}

					solve(bodyA, bodyB, normal, depth);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void solve(KaRigidBody bodyA, KaRigidBody bodyB, Vector2 normal, float depth)
		{
			if (!bodyA.IsStatic)
			{
				float depthAmount = bodyB.IsStatic ? depth : depth * 0.5f;
				bodyA.Move(-normal, depthAmount);
				bodyA.ForceVelocity = Vector2.Reflect(bodyA.ForceVelocity, normal);
			}

			if (!bodyB.IsStatic)
			{
				float depthAmount = bodyA.IsStatic ? depth : depth * 0.5f;
				bodyB.Move(normal, depthAmount);
				bodyB.ForceVelocity = Vector2.Reflect(bodyB.ForceVelocity, normal);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void solveIfCollideWithStatic(KaRigidBody collideByStatic, Vector2 normal, float depth)
		{
			if (!collideByStatic.IsStatic)
			{
				collideByStatic.Move(normal, depth * 0.5f);
				collideByStatic.ForceVelocity = Vector2.Reflect(collideByStatic.ForceVelocity, normal);
			}
		}

		public bool TryGetBody(int id, [NotNullWhen(true)] out KaRigidBody? body)
		{
			if (id < 0 || id >= BodyCount)
			{
				body = null;
				return false;
			}

			body = _rigidBodies[id];
			return true;
		}

		public bool Raycast(Vector2 center, float radius,
							out List<int> raycastHitList,
							PhysicsLayerMask mask = LayerMaskHelper.ALL_MASK)
		{
			_raycastHitList.Clear();

			foreach (var rigid in  _rigidBodies)
			{
				if (rigid.IsStatic)
					continue;

				if ((mask ^ rigid.LayerMask.Mask) != 0)
					continue;

				bool isCollide = false;

				switch (rigid.ShapeType)
				{
					case KaPhysicsShapeType.Box_AABB:
						var boxRigid = (BoxAABBRigidBody)rigid;
						isCollide |= KaPhysics
							.IsIntersectCircleAABB(center, radius, boxRigid.Position,
												   boxRigid.Width, boxRigid.Height);
						break;

					case KaPhysicsShapeType.Box_OBB:
						var obbRigid = (BoxOBBRigidBody)rigid;
						isCollide |= KaPhysics
							.IsIntersectCircleOBB(center, radius, obbRigid.Position,
												  obbRigid.GetTransformedVertices(),
												  obbRigid.Width, obbRigid.Height,
												  obbRigid.BoundaryRadius);
						break;

					case KaPhysicsShapeType.Circle:
						var circleRigid = (CircleRigidBody)rigid;
						isCollide |= KaPhysics
							.IsIntersectCircles(center, radius,
												circleRigid.Position, circleRigid.Radius);
						break;
				}

				if (isCollide)
				{
					_raycastHitList.Add(rigid.ID);
				}
			}

			raycastHitList = _raycastHitList;
			return raycastHitList.Count != 0;
		}

		public bool Raycast(Vector2 center, float width, float height,
							out List<int> raycastHitList,
							PhysicsLayerMask mask = LayerMaskHelper.ALL_MASK)
		{
			_raycastHitList.Clear();

			foreach (var rigid in _rigidBodies)
			{
				if (rigid.IsStatic)
					continue;

				if ((mask ^ rigid.LayerMask.Mask) != 0)
					continue;

				bool isCollide = false;

				switch (rigid.ShapeType)
				{
					case KaPhysicsShapeType.Box_AABB:
						var boxRigid = (BoxAABBRigidBody)rigid;
						isCollide |= KaPhysics
							.IsIntersectAABBs(new BoundingBox(center, width, height),
											  boxRigid.GetBoundingBox());
						break;

					case KaPhysicsShapeType.Box_OBB:
						var obbRigid = (BoxOBBRigidBody)rigid;
						isCollide |= KaPhysics
							.IsIntersectAABBOBB(new BoundingBox(center, width, height),
												obbRigid.GetTransformedVertices(),
												obbRigid.GetBoundingBox());
						break;

					case KaPhysicsShapeType.Circle:
						var circleRigid = (CircleRigidBody)rigid;
						isCollide |= KaPhysics
							.IsIntersectCircleAABB(circleRigid.Position, circleRigid.Radius,
												   center, width, height);
						break;
				}

				if (isCollide)
				{
					_raycastHitList.Add(rigid.ID);
				}
			}

			raycastHitList = _raycastHitList;
			return raycastHitList.Count != 0;
		}
	}
}
