using System;
using System.Collections;
using System.Numerics;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class TestCube : MasterNetworkObject
	{
		public override VisibilityType Visibility => VisibilityType.View;

		public override VisibilityAuthority VisibilityAuthority => VisibilityAuthority.All;

		private static Random _random = new Random();
		private float _radiusFactor = 0.0f;

		private float _showTime;
		private float _lifeTime;

		public override void OnCreated()
		{
			//Console.WriteLine($"{Identity}:{Transform} OnCREATED");

			_radiusFactor = (float)_random.NextDouble();
			_lifeTime = (float)(25 + _random.NextDouble() * 3);

			R = (float)_random.NextDouble();
			G = (float)_random.NextDouble();
			B = (float)_random.NextDouble();
		}

		public override void OnUpdate(float deltaTime)
		{
			_animationTime += deltaTime;

			float x = MathF.Cos(_radiusFactor);
			float z = MathF.Sin(_radiusFactor);
			Transform.SetPosition(Transform.Position + new Vector3(x, 0, z));
			_radiusFactor += 0.128f * 1f;
			//Console.WriteLine(Identity.ToString() + Transform.ToString());

			_lifeTime -= deltaTime;
			if (_lifeTime < 0)
			{
				this.Destroy();
			}

			_showTime -= deltaTime;
			if (_showTime < 0)
			{
				_showTime += 0.5f;
				TestRPC(119);
			}
		}

		public override void OnDestroyed()
		{
			_pool?.Remove(this);
			_pool = null;
		}

		private IList? _pool;

		public void BindPool(IList pool)
		{
			_pool = pool;
		}
	}
}