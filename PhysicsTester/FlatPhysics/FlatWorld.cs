using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		private FlatVector gravity;
		private List<FlatBody> bodyList;
		private List<(int, int)> contactPairs = new();

		public List<FlatVector> ContactPointsList;

		public int BodyCount
		{
			get { return bodyList.Count; }
		}

		public FlatWorld()
		{
			this.gravity = new FlatVector(0f, 9.81f).FlipY();
			this.bodyList = new List<FlatBody>();
			this.ContactPointsList = new List<FlatVector>();
		}

		public void AddBody(FlatBody body)
		{
			this.bodyList.Add(body);
		}

		public bool RemoveBody(FlatBody body)
		{
			return this.bodyList.Remove(body);
		}

		public bool GetBody(int index, out FlatBody body)
		{
			body = null;
			if (index < 0 || index >= bodyList.Count)
			{
				return false;
			}

			body = this.bodyList[index];
			return true;
		}

		public void Step(float time, int totalIterations)
		{
			totalIterations = FlatMath.Clamp(totalIterations, MinIterations, MaxIterations);

			this.ContactPointsList.Clear();

			for (int currentIteration = 0; currentIteration < totalIterations; currentIteration++)
			{
				this.contactPairs.Clear();
				this.StepBodies(time, totalIterations);
				this.BroadPhase();
				this.NarrowPhase(currentIteration == totalIterations - 1);
			}
		}

		private void BroadPhase()
		{
			// Collision step
			for (int i = 0; i < this.bodyList.Count - 1; i++)
			{
				FlatBody bodyA = this.bodyList[i];
				FlatAABB bodyA_aabb = bodyA.GetAABB();

				for (int j = i + 1; j < this.bodyList.Count; j++)
				{
					FlatBody bodyB = this.bodyList[j];
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

					this.contactPairs.Add((i, j));
				}
			}
		}

		private void NarrowPhase(bool isLastIteration)
		{
			for (int i = 0; i < this.contactPairs.Count; i++)
			{
				(int, int) pair = this.contactPairs[i];
				FlatBody bodyA = this.bodyList[pair.Item1];
				FlatBody bodyB = this.bodyList[pair.Item2];

				if (Collisions.Collide(bodyA, bodyB, out FlatVector normal, out float depth))
				{
					this.SeparateBodies(bodyA, bodyB, normal * depth);
					Collisions.FindContactPoints(bodyA, bodyB, out var contact1, out var contact2, out int contactCount);
					FlatManifold contact = new FlatManifold(bodyA, bodyB, normal, depth, contact1, contact2, contactCount);
					//this.ResolveCollisionBasic(contact);
					//this.ResolveCollisionWithRotation(contact);
					this.ResolveCollisionWithRotationAndFriction(contact);

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

		public void StepBodies(float time, int totalIterations)
		{
			// Movement step
			for (int i = 0; i < this.bodyList.Count; i++)
			{
				this.bodyList[i].Step(time, gravity, totalIterations);
			}
		}

		// mtv = normal times depth = minimum translation vector = 최소 변위 벡터
		private void SeparateBodies(FlatBody bodyA, FlatBody bodyB, FlatVector mtv)
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

		public void ResolveCollisionBasic(in FlatManifold contact)
		{
			FlatBody bodyA = contact.BodyA;
			FlatBody bodyB = contact.BodyB;
			FlatVector normal = contact.Normal;
			float depth = contact.Depth;

			FlatVector relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

			if (FlatMath.Dot(relativeVelocity, normal) > 0f)
			{
				// 이미 떨어져서 멀어지고 있기 때문에 계산을 무시
				return;
			}

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);
			float j = -(1f + e) * FlatMath.Dot(relativeVelocity, normal);
			j /= bodyA.InvMass + bodyB.InvMass;

			FlatVector impulse = j * normal;

			// 수식
			bodyA.LinearVelocity += -impulse * bodyA.InvMass;
			bodyB.LinearVelocity += impulse * bodyB.InvMass;
		}

		private FlatVector[] contactList = new FlatVector[2];
		private FlatVector[] impulseList = new FlatVector[2];
		private FlatVector[] frictionImpulseList = new FlatVector[2];
		private FlatVector[] raList = new FlatVector[2];
		private FlatVector[] rbList = new FlatVector[2];
		private float[] jList = new float[2];
		public void ResolveCollisionWithRotation(in FlatManifold contact)
		{
			FlatBody bodyA = contact.BodyA;
			FlatBody bodyB = contact.BodyB;
			FlatVector normal = contact.Normal;
			FlatVector contact1 = contact.Contact1;
			FlatVector contact2 = contact.Contact2;
			int contactCount = contact.ContactCount;

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

			this.contactList[0] = contact1;
			this.contactList[1] = contact2;

			for (int i = 0; i < contactCount; i++)
			{
				this.impulseList[i] = FlatVector.Zero;
				this.raList[i] = FlatVector.Zero;
				this.rbList[i] = FlatVector.Zero;
			}

			for (int i = 0; i < contactCount; i++)
			{
				// 중심점에서 접촉 지점 까지의 벡터
				FlatVector ra = contactList[i] - bodyA.Position;
				FlatVector rb = contactList[i] - bodyB.Position;

				raList[i] = ra;
				rbList[i] = rb;

				// 위 벡터의 수직 벡터
				FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
				FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

				FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
				FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

				FlatVector relativeVelocity =
					(bodyB.LinearVelocity + angularLinearVelocityB) - 
					(bodyA.LinearVelocity + angularLinearVelocityA);

				float contactVelocityMag = FlatMath.Dot(relativeVelocity, normal);
				if (contactVelocityMag > 0f)
				{
					// 이미 떨어져서 멀어지고 있기 때문에 계산을 무시
					continue;
				}

				float raPerpDotN = FlatMath.Dot(raPerp, normal);
				float rbPerpDotN = FlatMath.Dot(rbPerp, normal);

				float denominator = bodyA.InvMass + bodyB.InvMass +
					(raPerpDotN * raPerpDotN) * bodyA.InvInertia +
					(rbPerpDotN * rbPerpDotN) * bodyB.InvInertia;

				float j = -(1f + e) * contactVelocityMag;
				j /= denominator;
				j /= (float)contactCount; // 접촉점 마다 힘이 있기 때문

				FlatVector impulse = j * normal;
				impulseList[i] = impulse;
			}

			// 각 접점에서 가속도를 적용해서 변경되어 버리는것을 방지하기 위해 별도의 반복문으로 분리
			for (int i = 0; i < contactCount; i++)
			{
				FlatVector impulse = impulseList[i];
				FlatVector ra = raList[i];
				FlatVector rb = rbList[i];

				bodyA.LinearVelocity += -impulse * bodyA.InvMass;
				bodyA.AngularVelocity += -FlatMath.Cross(ra, impulse) * bodyA.InvInertia; // 회전하는 힘
				bodyB.LinearVelocity += impulse * bodyB.InvMass;
				bodyB.AngularVelocity += FlatMath.Cross(rb, impulse) * bodyB.InvInertia;
			}
		}

		public void ResolveCollisionWithRotationAndFriction(in FlatManifold contact)
		{
			FlatBody bodyA = contact.BodyA;
			FlatBody bodyB = contact.BodyB;
			FlatVector normal = contact.Normal;
			FlatVector contact1 = contact.Contact1;
			FlatVector contact2 = contact.Contact2;
			int contactCount = contact.ContactCount;

			float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

			float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
			float df = (bodyA.DynamicFriction + bodyA.DynamicFriction) * 0.5f;

			this.contactList[0] = contact1;
			this.contactList[1] = contact2;

			for (int i = 0; i < contactCount; i++)
			{
				this.impulseList[i] = FlatVector.Zero;
				this.raList[i] = FlatVector.Zero;
				this.rbList[i] = FlatVector.Zero;
				this.frictionImpulseList[i] = FlatVector.Zero;
				this.jList[i] = 0;
			}

			for (int i = 0; i < contactCount; i++)
			{
				// 중심점에서 접촉 지점 까지의 벡터
				FlatVector ra = contactList[i] - bodyA.Position;
				FlatVector rb = contactList[i] - bodyB.Position;

				raList[i] = ra;
				rbList[i] = rb;

				// 위 벡터의 수직 벡터
				FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
				FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

				FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
				FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

				FlatVector relativeVelocity =
					(bodyB.LinearVelocity + angularLinearVelocityB) -
					(bodyA.LinearVelocity + angularLinearVelocityA);

				float contactVelocityMag = FlatMath.Dot(relativeVelocity, normal);
				if (contactVelocityMag > 0f)
				{
					// 이미 떨어져서 멀어지고 있기 때문에 계산을 무시
					continue;
				}

				float raPerpDotN = FlatMath.Dot(raPerp, normal);
				float rbPerpDotN = FlatMath.Dot(rbPerp, normal);

				float denominator = bodyA.InvMass + bodyB.InvMass +
					(raPerpDotN * raPerpDotN) * bodyA.InvInertia +
					(rbPerpDotN * rbPerpDotN) * bodyB.InvInertia;

				float j = -(1f + e) * contactVelocityMag;
				j /= denominator;
				j /= (float)contactCount; // 접촉점 마다 힘이 있기 때문

				jList[i] = j;
				FlatVector impulse = j * normal;
				impulseList[i] = impulse;
			}

			// 각 접점에서 가속도를 적용해서 변경되어 버리는것을 방지하기 위해 별도의 반복문으로 분리
			for (int i = 0; i < contactCount; i++)
			{
				FlatVector impulse = impulseList[i];
				FlatVector ra = raList[i];
				FlatVector rb = rbList[i];

				bodyA.LinearVelocity += -impulse * bodyA.InvMass;
				bodyA.AngularVelocity += -FlatMath.Cross(ra, impulse) * bodyA.InvInertia; // 회전하는 힘
				bodyB.LinearVelocity += impulse * bodyB.InvMass;
				bodyB.AngularVelocity += FlatMath.Cross(rb, impulse) * bodyB.InvInertia;
			}

			// Calculate friction
			for (int i = 0; i < contactCount; i++)
			{
				// 중심점에서 접촉 지점 까지의 벡터
				FlatVector ra = contactList[i] - bodyA.Position;
				FlatVector rb = contactList[i] - bodyB.Position;

				raList[i] = ra;
				rbList[i] = rb;

				// 위 벡터의 수직 벡터
				FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
				FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

				FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
				FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

				FlatVector relativeVelocity =
					(bodyB.LinearVelocity + angularLinearVelocityB) -
					(bodyA.LinearVelocity + angularLinearVelocityA);

				FlatVector tangent = relativeVelocity - FlatMath.Dot(relativeVelocity, normal) * normal;

				if (FlatMath.NearlyEqual(tangent, FlatVector.Zero))
				{
					continue;
				}
				else
				{
					tangent = FlatMath.Normalize(tangent);
				}

				float raPerpDotT = FlatMath.Dot(raPerp, tangent);
				float rbPerpDotT = FlatMath.Dot(rbPerp, tangent);

				float denominator = bodyA.InvMass + bodyB.InvMass +
					(raPerpDotT * raPerpDotT) * bodyA.InvInertia +
					(rbPerpDotT * rbPerpDotT) * bodyB.InvInertia;

				// Restitution 같은 연산을 하지 않기 때문에 e 연산 없음
				float jt = -FlatMath.Dot(relativeVelocity, tangent);
				jt /= denominator;
				jt /= (float)contactCount; // 접촉점 마다 힘이 있기 때문

				FlatVector frictionImpluse;
				float j = jList[i];

				if (MathF.Abs(jt) < j * sf)
				{
					frictionImpluse = jt * tangent;
				}
				else
				{
					frictionImpluse = -j * tangent * df;
				}

				this.frictionImpulseList[i] = frictionImpluse;
			}

			// 각 접점에서 가속도를 적용해서 변경되어 버리는것을 방지하기 위해 별도의 반복문으로 분리
			for (int i = 0; i < contactCount; i++)
			{
				FlatVector frictionImpluse = frictionImpulseList[i];
				FlatVector ra = raList[i];
				FlatVector rb = rbList[i];

				bodyA.LinearVelocity += -frictionImpluse * bodyA.InvMass;
				bodyA.AngularVelocity += -FlatMath.Cross(ra, frictionImpluse) * bodyA.InvInertia; // 회전하는 힘
				bodyB.LinearVelocity += frictionImpluse * bodyB.InvMass;
				bodyB.AngularVelocity += FlatMath.Cross(rb, frictionImpluse) * bodyB.InvInertia;
			}
		}
	}
}
