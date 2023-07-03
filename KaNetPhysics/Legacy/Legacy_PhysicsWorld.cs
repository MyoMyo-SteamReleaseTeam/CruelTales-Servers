using System.Numerics;

namespace KaNet.Physics.Legacy
{
	public sealed class Legacy_PhysicsWorld
	{
		public static readonly float MinBodySize = 0.01f * 0.01f;
		public static readonly float MaxBodySize = 64f * 64f;

		public static readonly float MinDensity = 0.5f; // 밀도 g/cm^3 물이 1
		public static readonly float MaxDensity = 21.4f; // 백금

		public static readonly int MinIterations = 1;
		public static readonly int MaxIterations = 128;

		private Vector2 _gravity;
		private List<Legacy_RigidBody> _bodyList;
		private List<(int, int)> _contactPairs = new();

		public List<Vector2> ContactPointsList;

		public int BodyCount
		{
			get { return _bodyList.Count; }
		}

		public Legacy_PhysicsWorld()
		{
			_gravity = new Vector2(0f, 9.81f).FlipY();
			_bodyList = new List<Legacy_RigidBody>();
			ContactPointsList = new List<Vector2>();
		}

		public void AddBody(Legacy_RigidBody body)
		{
			_bodyList.Add(body);
		}

		public bool RemoveBody(Legacy_RigidBody body)
		{
			return _bodyList.Remove(body);
		}

		public bool GetBody(int index, out Legacy_RigidBody body)
		{
			body = null;
			if (index < 0 || index >= _bodyList.Count)
			{
				return false;
			}

			body = _bodyList[index];
			return true;
		}

		public void Step(float stepTime, int totalIterations)
		{
			totalIterations = KaMath.Clamp(totalIterations, MinIterations, MaxIterations);

			ContactPointsList.Clear();

			for (int currentIteration = 0; currentIteration < totalIterations; currentIteration++)
			{
				_contactPairs.Clear();
				stepBodies(stepTime, totalIterations);
				broadPhase();
				narrowPhase(currentIteration == totalIterations - 1);
			}
		}

		private void broadPhase()
		{
			// Collision step
			for (int i = 0; i < _bodyList.Count - 1; i++)
			{
				Legacy_RigidBody bodyA = _bodyList[i];
				Legacy_AabbBoundary bodyA_aabb = bodyA.GetAABB();

				for (int j = i + 1; j < _bodyList.Count; j++)
				{
					Legacy_RigidBody bodyB = _bodyList[j];
					Legacy_AabbBoundary bodyB_aabb = bodyB.GetAABB();

					if (bodyA.IsStatic && bodyB.IsStatic)
					{
						continue;
					}

					// 미리 AABB로 빠르게 판정
					if (!Legacy_Collisions.IntersectAABBs(bodyA_aabb, bodyB_aabb))
					{
						continue;
					}

					_contactPairs.Add((i, j));
				}
			}
		}

		private void narrowPhase(bool isLastIteration)
		{
			for (int i = 0; i < _contactPairs.Count; i++)
			{
				(int, int) pair = _contactPairs[i];
				Legacy_RigidBody bodyA = _bodyList[pair.Item1];
				Legacy_RigidBody bodyB = _bodyList[pair.Item2];

				if (Legacy_Collisions.Collide(bodyA, bodyB, out Vector2 normal, out float depth))
				{
					separateBodies(bodyA, bodyB, normal * depth);
					Legacy_Collisions.FindContactPoints(bodyA, bodyB, out var contact1, out var contact2, out int contactCount);
					Legacy_Manifold contact = new Legacy_Manifold(bodyA, bodyB, normal, depth, contact1, contact2, contactCount);
					//this.resolveCollisionBasic(contact);
					//this.resolveCollisionWithRotation(contact);
					resolveCollisionWithRotationAndFriction(contact);

					if (isLastIteration)
					{
						if (!ContactPointsList.Contains(contact.Contact1))
						{
							ContactPointsList.Add(contact.Contact1);
						}

						if (contact.ContactCount > 1)
						{
							if (!ContactPointsList.Contains(contact.Contact2))
							{
								ContactPointsList.Add(contact.Contact2);
							}
						}
					}
				}
			}
		}

		private void stepBodies(float stepTime, int totalIterations)
		{
			// Movement step
			for (int i = 0; i < _bodyList.Count; i++)
			{
				_bodyList[i].Step(stepTime, _gravity, totalIterations);
			}
		}

		// mtv = normal times depth = minimum translation vector = 최소 변위 벡터
		private void separateBodies(Legacy_RigidBody bodyA, Legacy_RigidBody bodyB, Vector2 mtv)
		{
			if (bodyA.IsStatic)
			{
				bodyB.Move(mtv);
			}
			else if (bodyB.IsStatic)
			{
				bodyA.Move(-mtv);
			}
			else
			{
				bodyA.Move(-mtv / 2f);
				bodyB.Move(mtv / 2f);
			}
		}

		private void resolveCollisionBasic(in Legacy_Manifold contact)
		{
			Legacy_RigidBody bodyA = contact.BodyA;
			Legacy_RigidBody bodyB = contact.BodyB;
			Vector2 normal = contact.Normal;
			float depth = contact.Depth;

			Vector2 relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

			if (Vector2.Dot(relativeVelocity, normal) > 0f)
			{
				// 이미 떨어져서 멀어지고 있기 때문에 계산을 무시
				return;
			}

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);
			float j = -(1f + e) * Vector2.Dot(relativeVelocity, normal);
			j /= bodyA.InvMass + bodyB.InvMass;

			Vector2 impulse = j * normal;

			// 수식
			bodyA.LinearVelocity += -impulse * bodyA.InvMass;
			bodyB.LinearVelocity += impulse * bodyB.InvMass;
		}

		private Vector2[] _contactList = new Vector2[2];
		private Vector2[] _impulseList = new Vector2[2];
		private Vector2[] _frictionImpulseList = new Vector2[2];
		private Vector2[] _raList = new Vector2[2];
		private Vector2[] _rbList = new Vector2[2];
		private float[] _jList = new float[2];
		private void resolveCollisionWithRotation(in Legacy_Manifold contact)
		{
			Legacy_RigidBody bodyA = contact.BodyA;
			Legacy_RigidBody bodyB = contact.BodyB;
			Vector2 normal = contact.Normal;
			Vector2 contact1 = contact.Contact1;
			Vector2 contact2 = contact.Contact2;
			int contactCount = contact.ContactCount;

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

			_contactList[0] = contact1;
			_contactList[1] = contact2;

			for (int i = 0; i < contactCount; i++)
			{
				_impulseList[i] = Vector2.Zero;
				_raList[i] = Vector2.Zero;
				_rbList[i] = Vector2.Zero;
			}

			for (int i = 0; i < contactCount; i++)
			{
				// 중심점에서 접촉 지점 까지의 벡터
				Vector2 ra = _contactList[i] - bodyA.Position;
				Vector2 rb = _contactList[i] - bodyB.Position;

				_raList[i] = ra;
				_rbList[i] = rb;

				// 위 벡터의 수직 벡터
				Vector2 raPerp = new Vector2(-ra.Y, ra.X);
				Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

				Vector2 angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
				Vector2 angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

				Vector2 relativeVelocity =
					bodyB.LinearVelocity + angularLinearVelocityB -
					(bodyA.LinearVelocity + angularLinearVelocityA);

				float contactVelocityMag = Vector2.Dot(relativeVelocity, normal);
				if (contactVelocityMag > 0f)
				{
					// 이미 떨어져서 멀어지고 있기 때문에 계산을 무시
					continue;
				}

				float raPerpDotN = Vector2.Dot(raPerp, normal);
				float rbPerpDotN = Vector2.Dot(rbPerp, normal);

				float denominator = bodyA.InvMass + bodyB.InvMass +
					raPerpDotN * raPerpDotN * bodyA.InvInertia +
					rbPerpDotN * rbPerpDotN * bodyB.InvInertia;

				float j = -(1f + e) * contactVelocityMag;
				j /= denominator;
				j /= contactCount; // 접촉점 마다 힘이 있기 때문

				Vector2 impulse = j * normal;
				_impulseList[i] = impulse;
			}

			// 각 접점에서 가속도를 적용해서 변경되어 버리는것을 방지하기 위해 별도의 반복문으로 분리
			for (int i = 0; i < contactCount; i++)
			{
				Vector2 impulse = _impulseList[i];
				Vector2 ra = _raList[i];
				Vector2 rb = _rbList[i];

				bodyA.LinearVelocity += -impulse * bodyA.InvMass;
				bodyA.AngularVelocity += -KaMath.Cross(ra, impulse) * bodyA.InvInertia; // 회전하는 힘
				bodyB.LinearVelocity += impulse * bodyB.InvMass;
				bodyB.AngularVelocity += KaMath.Cross(rb, impulse) * bodyB.InvInertia;
			}
		}

		private void resolveCollisionWithRotationAndFriction(in Legacy_Manifold contact)
		{
			Legacy_RigidBody bodyA = contact.BodyA;
			Legacy_RigidBody bodyB = contact.BodyB;
			Vector2 normal = contact.Normal;
			Vector2 contact1 = contact.Contact1;
			Vector2 contact2 = contact.Contact2;
			int contactCount = contact.ContactCount;

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

			float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
			float df = (bodyA.DynamicFriction + bodyA.DynamicFriction) * 0.5f;

			_contactList[0] = contact1;
			_contactList[1] = contact2;

			for (int i = 0; i < contactCount; i++)
			{
				_impulseList[i] = Vector2.Zero;
				_raList[i] = Vector2.Zero;
				_rbList[i] = Vector2.Zero;
				_frictionImpulseList[i] = Vector2.Zero;
				_jList[i] = 0;
			}

			for (int i = 0; i < contactCount; i++)
			{
				// 중심점에서 접촉 지점 까지의 벡터
				Vector2 ra = _contactList[i] - bodyA.Position;
				Vector2 rb = _contactList[i] - bodyB.Position;

				_raList[i] = ra;
				_rbList[i] = rb;

				// 위 벡터의 수직 벡터
				Vector2 raPerp = new Vector2(-ra.Y, ra.X);
				Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

				Vector2 angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
				Vector2 angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

				Vector2 relativeVelocity =
					bodyB.LinearVelocity + angularLinearVelocityB -
					(bodyA.LinearVelocity + angularLinearVelocityA);

				float contactVelocityMag = Vector2.Dot(relativeVelocity, normal);
				if (contactVelocityMag > 0f)
				{
					// 이미 떨어져서 멀어지고 있기 때문에 계산을 무시
					continue;
				}

				float raPerpDotN = Vector2.Dot(raPerp, normal);
				float rbPerpDotN = Vector2.Dot(rbPerp, normal);

				float denominator = bodyA.InvMass + bodyB.InvMass +
					raPerpDotN * raPerpDotN * bodyA.InvInertia +
					rbPerpDotN * rbPerpDotN * bodyB.InvInertia;

				float j = -(1f + e) * contactVelocityMag;
				j /= denominator;
				j /= contactCount; // 접촉점 마다 힘이 있기 때문

				_jList[i] = j;
				Vector2 impulse = j * normal;
				_impulseList[i] = impulse;
			}

			// 각 접점에서 가속도를 적용해서 변경되어 버리는것을 방지하기 위해 별도의 반복문으로 분리
			for (int i = 0; i < contactCount; i++)
			{
				Vector2 impulse = _impulseList[i];
				Vector2 ra = _raList[i];
				Vector2 rb = _rbList[i];

				bodyA.LinearVelocity += -impulse * bodyA.InvMass;
				bodyA.AngularVelocity += -KaMath.Cross(ra, impulse) * bodyA.InvInertia; // 회전하는 힘
				bodyB.LinearVelocity += impulse * bodyB.InvMass;
				bodyB.AngularVelocity += KaMath.Cross(rb, impulse) * bodyB.InvInertia;
			}

			// Calculate friction
			for (int i = 0; i < contactCount; i++)
			{
				// 중심점에서 접촉 지점 까지의 벡터
				Vector2 ra = _contactList[i] - bodyA.Position;
				Vector2 rb = _contactList[i] - bodyB.Position;

				_raList[i] = ra;
				_rbList[i] = rb;

				// 위 벡터의 수직 벡터
				Vector2 raPerp = new Vector2(-ra.Y, ra.X);
				Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

				Vector2 angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
				Vector2 angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

				Vector2 relativeVelocity =
					bodyB.LinearVelocity + angularLinearVelocityB -
					(bodyA.LinearVelocity + angularLinearVelocityA);

				Vector2 tangent = relativeVelocity - Vector2.Dot(relativeVelocity, normal) * normal;

				if (KaMath.NearlyEqual(tangent, Vector2.Zero))
				{
					continue;
				}
				else
				{
					tangent = Vector2.Normalize(tangent);
				}

				float raPerpDotT = Vector2.Dot(raPerp, tangent);
				float rbPerpDotT = Vector2.Dot(rbPerp, tangent);

				float denominator = bodyA.InvMass + bodyB.InvMass +
					raPerpDotT * raPerpDotT * bodyA.InvInertia +
					rbPerpDotT * rbPerpDotT * bodyB.InvInertia;

				// Restitution 같은 연산을 하지 않기 때문에 e 연산 없음
				float jt = -Vector2.Dot(relativeVelocity, tangent);
				jt /= denominator;
				jt /= contactCount; // 접촉점 마다 힘이 있기 때문

				Vector2 frictionImpluse;
				float j = _jList[i];

				if (MathF.Abs(jt) < j * sf)
				{
					frictionImpluse = jt * tangent;
				}
				else
				{
					frictionImpluse = -j * tangent * df;
				}

				_frictionImpulseList[i] = frictionImpluse;
			}

			// 각 접점에서 가속도를 적용해서 변경되어 버리는것을 방지하기 위해 별도의 반복문으로 분리
			for (int i = 0; i < contactCount; i++)
			{
				Vector2 frictionImpluse = _frictionImpulseList[i];
				Vector2 ra = _raList[i];
				Vector2 rb = _rbList[i];

				bodyA.LinearVelocity += -frictionImpluse * bodyA.InvMass;
				bodyA.AngularVelocity += -KaMath.Cross(ra, frictionImpluse) * bodyA.InvInertia; // 회전하는 힘
				bodyB.LinearVelocity += frictionImpluse * bodyB.InvMass;
				bodyB.AngularVelocity += KaMath.Cross(rb, frictionImpluse) * bodyB.InvInertia;
			}
		}
	}
}
