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
				bodyB.ForceVelocity = Vector2.Reflect(bodyA.ForceVelocity, normal);
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
	}
}
