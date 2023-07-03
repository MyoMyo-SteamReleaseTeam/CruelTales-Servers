using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KaNet.Physics
{
	public class KaPhysicsWorld
	{
		private List<KaRigidBody> _bodies = new(16);
		public int BodyCount => _bodies.Count;

		public void AddRigidBody(KaRigidBody rigidBody)
		{
			_bodies.Add(rigidBody);
		}

		public void RemoveRigidBody(KaRigidBody rigidBody)
		{
			_bodies.Remove(rigidBody);
		}

		public static KaRigidBody CreateCircle(float radius, bool isStatic)
		{
			return new KaRigidBody(PhysicsShapeType.Circle, radius,
								   width: 0, height: 0, angle: 0, null, isStatic);
		}

		public static KaRigidBody CreateBox(float width, float height, bool isStatic)
		{
			Vector2[] vertices = new Vector2[4];
			float hw = width * 0.5f;
			float hh = height * 0.5f;
			vertices[0] = new Vector2(-hw, hh);
			vertices[1] = new Vector2(hw, hh);
			vertices[2] = new Vector2(hw, -hh);
			vertices[3] = new Vector2(-hw, -hh);
			return new KaRigidBody(PhysicsShapeType.Box_AABB, radius: 0,
								   width, height, angle: 0, vertices, isStatic);
		}

		public static KaRigidBody CreateBoxOBB(float width, float height, float angle, bool isStatic)
		{
			Vector2[] vertices = new Vector2[4];
			float hw = width * 0.5f;
			float hh = height * 0.5f;
			vertices[0] = new Vector2(-hw, hh);
			vertices[1] = new Vector2(hw, hh);
			vertices[2] = new Vector2(hw, -hh);
			vertices[3] = new Vector2(-hw, -hh);
			return new KaRigidBody(PhysicsShapeType.Box_OBB, radius: 0,
								   width, height, angle, vertices, isStatic);
		}

		public void Step(float interval, int iterateCount)
		{
		}

		public bool TryGetBody(int i, [NotNullWhen(true)] out KaRigidBody? body)
		{
			if (i < 0 || i >= _bodies.Count)
			{
				body = null;
				return false;
			}
			body = _bodies[i];

			return true;
		}
	}
}
