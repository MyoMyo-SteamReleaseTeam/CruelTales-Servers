using System;
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

		private Vector3 _originPosition;

		public override void OnCreated()
		{
			_originPosition = Transform.Position;
			_radiusFactor = (float)_random.NextDouble();
		}

		private float _showTime;

		public override void OnUpdate(float deltaTime)
		{
			float x = MathF.Cos(_radiusFactor);
			float y = MathF.Sin(_radiusFactor);
			Transform.SetPosition(_originPosition + new Vector3(x, 0, y) * 7);

			_radiusFactor += 0.128f;
			if (_radiusFactor > MathF.PI * 2)
			{
				_radiusFactor = 0;
			}

			_showTime -= deltaTime;
			if (_showTime < 0)
			{
				_showTime += 0.1f;
				//Console.WriteLine(Transform);
				TestRPC($"I'm {Identity}");
			}
		}
	}
}