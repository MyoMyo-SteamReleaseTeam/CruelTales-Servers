using System.Numerics;
using PhysicsTester;

namespace FlatPhysics
{
	public sealed class FlatWorld
	{
		public static readonly float MinBodySize = 0.01f * 0.01f;
		public static readonly float MaxBodySize = 64f * 64f;

		public static readonly float MinDensity = 0.5f; // 밀도 g/cm^3 물이 1
		public static readonly float MaxDensity = 21.4f; // 백금

		public static readonly int MinIterations = 1;
		public static readonly int MaxIterations = 128;

		private Vector2 _gravity;
		private List<FlatBody> _bodyList;
		private List<(int, int)> _contactPairs = new();

		public List<Vector2> ContactPointsList;

		public int BodyCount
		{
			get { return _bodyList.Count; }
		}

		public FlatWorld()
		{
			this._gravity = new Vector2(0f, 9.81f).FlipY();
			this._bodyList = new List<FlatBody>();
			this.ContactPointsList = new List<Vector2>();
		}

		public void AddBody(FlatBody body)
		{
			this._bodyList.Add(body);
		}

		public bool RemoveBody(FlatBody body)
		{
			return this._bodyList.Remove(body);
		}

		public bool GetBody(int index, out FlatBody body)
		{
			body = null;
			if (index < 0 || index >= _bodyList.Count)
			{
				return false;
			}

			body = this._bodyList[index];
			return true;
		}

		public void Step(float stepTime, int totalIterations)
		{
			totalIterations = KaMath.Clamp(totalIterations, MinIterations, MaxIterations);

			this.ContactPointsList.Clear();

			for (int currentIteration = 0; currentIteration < totalIterations; currentIteration++)
			{
				this._contactPairs.Clear();
				this.stepBodies(stepTime, totalIterations);
				this.broadPhase();
				this.narrowPhase(currentIteration == totalIterations - 1);
			}
		}

		private void broadPhase()
		{
			// Collision step
			for (int i = 0; i < this._bodyList.Count - 1; i++)
			{
				FlatBody bodyA = this._bodyList[i];
				FlatAABB bodyA_aabb = bodyA.GetAABB();

				for (int j = i + 1; j < this._bodyList.Count; j++)
				{
					FlatBody bodyB = this._bodyList[j];
					FlatAABB bodyB_aabb = bodyB.GetAABB();

					if (bodyA.IsStatic && bodyB.IsStatic)
					{
						continue;
					}

					// 미리 AABB로 빠르게 판정
					if (!Collisions.IntersectAABBs(bodyA_aabb, bodyB_aabb))
					{
						continue;
					}

					this._contactPairs.Add((i, j));
				}
			}
		}

		private void narrowPhase(bool isLastIteration)
		{
			for (int i = 0; i < this._contactPairs.Count; i++)
			{
				(int, int) pair = this._contactPairs[i];
				FlatBody bodyA = this._bodyList[pair.Item1];
				FlatBody bodyB = this._bodyList[pair.Item2];

				if (Collisions.Collide(bodyA, bodyB, out Vector2 normal, out float depth))
				{
					this.separateBodies(bodyA, bodyB, normal * depth);
					Collisions.FindContactPoints(bodyA, bodyB, out var contact1, out var contact2, out int contactCount);
					FlatManifold contact = new FlatManifold(bodyA, bodyB, normal, depth, contact1, contact2, contactCount);
					//this.resolveCollisionBasic(contact);
					//this.resolveCollisionWithRotation(contact);
					this.resolveCollisionWithRotationAndFriction(contact);

					if (isLastIteration)
					{
						if (!this.ContactPointsList.Contains(contact.Contact1))
						{
							this.ContactPointsList.Add(contact.Contact1);
						}

						if (contact.ContactCount > 1)
						{
							if (!this.ContactPointsList.Contains(contact.Contact2))
							{
								this.ContactPointsList.Add(contact.Contact2);
							}
						}
					}
				}
			}
		}

		private void stepBodies(float stepTime, int totalIterations)
		{
			// Movement step
			for (int i = 0; i < this._bodyList.Count; i++)
			{
				this._bodyList[i].Step(stepTime, _gravity, totalIterations);
			}
		}

		// mtv = normal times depth = minimum translation vector = 최소 변위 벡터
		private void separateBodies(FlatBody bodyA, FlatBody bodyB, Vector2 mtv)
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

		private void resolveCollisionBasic(in FlatManifold contact)
		{
			FlatBody bodyA = contact.BodyA;
			FlatBody bodyB = contact.BodyB;
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
		private void resolveCollisionWithRotation(in FlatManifold contact)
		{
			FlatBody bodyA = contact.BodyA;
			FlatBody bodyB = contact.BodyB;
			Vector2 normal = contact.Normal;
			Vector2 contact1 = contact.Contact1;
			Vector2 contact2 = contact.Contact2;
			int contactCount = contact.ContactCount;

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

			this._contactList[0] = contact1;
			this._contactList[1] = contact2;

			for (int i = 0; i < contactCount; i++)
			{
				this._impulseList[i] = Vector2.Zero;
				this._raList[i] = Vector2.Zero;
				this._rbList[i] = Vector2.Zero;
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
					(bodyB.LinearVelocity + angularLinearVelocityB) - 
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
					(raPerpDotN * raPerpDotN) * bodyA.InvInertia +
					(rbPerpDotN * rbPerpDotN) * bodyB.InvInertia;

				float j = -(1f + e) * contactVelocityMag;
				j /= denominator;
				j /= (float)contactCount; // 접촉점 마다 힘이 있기 때문

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

		private void resolveCollisionWithRotationAndFriction(in FlatManifold contact)
		{
			FlatBody bodyA = contact.BodyA;
			FlatBody bodyB = contact.BodyB;
			Vector2 normal = contact.Normal;
			Vector2 contact1 = contact.Contact1;
			Vector2 contact2 = contact.Contact2;
			int contactCount = contact.ContactCount;

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

			float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
			float df = (bodyA.DynamicFriction + bodyA.DynamicFriction) * 0.5f;

			this._contactList[0] = contact1;
			this._contactList[1] = contact2;

			for (int i = 0; i < contactCount; i++)
			{
				this._impulseList[i] = Vector2.Zero;
				this._raList[i] = Vector2.Zero;
				this._rbList[i] = Vector2.Zero;
				this._frictionImpulseList[i] = Vector2.Zero;
				this._jList[i] = 0;
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
					(bodyB.LinearVelocity + angularLinearVelocityB) -
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
					(raPerpDotN * raPerpDotN) * bodyA.InvInertia +
					(rbPerpDotN * rbPerpDotN) * bodyB.InvInertia;

				float j = -(1f + e) * contactVelocityMag;
				j /= denominator;
				j /= (float)contactCount; // 접촉점 마다 힘이 있기 때문

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
					(bodyB.LinearVelocity + angularLinearVelocityB) -
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
					(raPerpDotT * raPerpDotT) * bodyA.InvInertia +
					(rbPerpDotT * rbPerpDotT) * bodyB.InvInertia;

				// Restitution 같은 연산을 하지 않기 때문에 e 연산 없음
				float jt = -Vector2.Dot(relativeVelocity, tangent);
				jt /= denominator;
				jt /= (float)contactCount; // 접촉점 마다 힘이 있기 때문

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

				this._frictionImpulseList[i] = frictionImpluse;
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
