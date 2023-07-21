using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using KaNet.Physics.RigidBodies;

namespace KaNet.Physics
{
	public class KaPhysicsWorld
	{
		private List<KaRigidBody> _rigidBodies = new(128);
		public int BodyCount => _rigidBodies.Count;

		public void AddRigidBody(KaRigidBody rigidBody)
		{
			_rigidBodies.Add(rigidBody);
		}

		public void RemoveRigidBody(KaRigidBody rigidBody)
		{
			_rigidBodies.Remove(rigidBody);
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

		public void Step(float interval)
		{
			int bodyCount = _rigidBodies.Count;

			for (int i = 0; i < bodyCount; i++)
			{
				_rigidBodies[i].Step(interval);
			}

			for (int a = 0; a < bodyCount - 1; a++)
			{
				KaRigidBody bodyA = _rigidBodies[a];

				for (int b = a + 1; b < bodyCount; b++)
				{
					KaRigidBody bodyB = _rigidBodies[b];

					if ((bodyA.LayerMask.Mask & bodyB.LayerMask.Flags) != bodyA.LayerMask.Mask)
					{
						continue;
					}

					if (!bodyA.IsCollideWith(bodyB, out Vector2 normal, out float depth))
					{
						continue;
					}

					solve(bodyA, bodyB, normal, depth);
				}
			}
		}

		private void solve(KaRigidBody bodyA, KaRigidBody bodyB, Vector2 normal, float depth)
		{
			if (!bodyA.IsStatic)
			{
				float depthAmount = bodyB.IsStatic ? depth : depth * 0.5f;
				bodyA.Move(-normal, depthAmount);
				bodyA.Impulse(Vector2.Reflect(bodyA.ForceVelocity, normal));
			}

			if (!bodyB.IsStatic)
			{
				float depthAmount = bodyA.IsStatic ? depth : depth * 0.5f;
				bodyB.Move(normal, depthAmount);
				bodyB.Impulse(Vector2.Reflect(bodyA.ForceVelocity, normal));
			}
		}

		public bool TryGetBody(int i, [NotNullWhen(true)] out KaRigidBody? body)
		{
			if (i < 0 || i >= BodyCount)
			{
				body = null;
				return false;
			}

			body = _rigidBodies[i];
			return true;
		}
	}
}
