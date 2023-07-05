using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using KaNet.Physics.RigidBodies;

namespace KaNet.Physics
{
	public class PhysicsWorld
	{
		private List<RigidBody> _rigidBodies = new();
		public int BodyCount => _rigidBodies.Count;

		public void AddRigidBody(RigidBody rigidBody)
		{
			_rigidBodies.Add(rigidBody);
		}

		public void RemoveRigidBody(RigidBody rigidBody)
		{
			_rigidBodies.Remove(rigidBody);
		}

		public RigidBody CreateCircle(float radius, bool isStatic)
		{
			var rigid = new CircleRigidBody(radius, isStatic);
			this.AddRigidBody(rigid);
			return rigid;
		}

		public RigidBody CreateBoxAABB(float width, float height, bool isStatic)
		{
			return new BoxAABBRigidBody(width, height, isStatic);
		}

		public RigidBody CreateBoxOBB(float width, float height, float angle, bool isStatic)
		{
			var rigid = new BoxOBBRigidBody(width, height, isStatic);
			rigid.RotateTo(angle);
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
				RigidBody bodyA = _rigidBodies[a];

				for (int b = a + 1; b < bodyCount; b++)
				{
					RigidBody bodyB = _rigidBodies[b];

					if (!bodyA.IsCollideWith(bodyB, out Vector2 normal, out float depth))
					{
						continue;
					}

					solve(bodyA, bodyB, normal, depth);
				}
			}
		}

		private void solve(RigidBody bodyA, RigidBody bodyB, Vector2 normal, float depth)
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
				bodyA.ForceVelocity = Vector2.Reflect(bodyA.ForceVelocity, normal);
			}
		}

		public bool TryGetBody(int i, [NotNullWhen(true)] out RigidBody? body)
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
