using System;
using System.Collections;
using System.Numerics;
using CTS.Instance.Gameplay.MiniGames;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class TestCube : MasterNetworkObject
	{
		public override VisibilityType Visibility => VisibilityType.View;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		private static Random _random = new Random();
		private float _radiusFactor = 0.0f;

		private float _showTime;
		private float _lifeTime;

		public override void Constructor() {}

		public override void OnCreated()
		{
			//Console.WriteLine($"{Identity}:{Transform} OnCREATED");

			_radiusFactor = (float)_random.NextDouble();
			_lifeTime = (float)(3 + _random.NextDouble() * 3);

			R = (float)_random.NextDouble();
			G = (float)_random.NextDouble();
			B = (float)_random.NextDouble();
		}

		public override void OnUpdate(float deltaTime)
		{
			_animationTime += deltaTime;

			float x = MathF.Cos(_radiusFactor);
			float y = MathF.Sin(_radiusFactor);

			Vector2 velocity = Vector2.Normalize(new Vector2(x, y)) * 10;
			//Vector2 velocity = Vector2.Normalize(new Vector2(-1, 0));
			RigidBody.ChangeVelocity(velocity);

			//Console.WriteLine(RigidBody.Position);

			//RigidBody.MoveTo(RigidBody.Position + new Vector2(x, y));
			_radiusFactor += 0.128f * 1f;

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

		// Test
		private MiniGameController? _controller;

		// Test
		public void BindMiniGame(MiniGameController controller)
		{
			_controller = controller;
		}

		public override void OnDestroyed()
		{
			_pool?.Remove(this);
			_pool = null;

			// Test
			_controller?.OnTestCubeDestroyed(this);
		}

		private IList? _pool;

		public void BindPool(IList pool)
		{
			_pool = pool;
		}
	}
}