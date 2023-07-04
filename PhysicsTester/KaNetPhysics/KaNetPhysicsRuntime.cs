﻿using System.Diagnostics;
using System.Numerics;
using PhysicsTester;

namespace KaNet.Physics
{
	public class KaNetPhysicsRuntime : PhysicsRuntime
	{
		private PhysicsWorld _world = new();
		private List<KaEntity> _entityList = new();
		private List<KaEntity> _entityRemovalList = new();

		public KaNetPhysicsRuntime(MainForm mainForm, InputManager inputManager, Vector2 screenSize)
			: base(mainForm, inputManager)
		{
			Vector2 screenHalfSize = screenSize * 0.5f;
			_renderer.Zoom = 1 + 3 * 7;
			_renderer.ScreenCameraPosition = -screenHalfSize.FlipY();

			Vector2 viewLB = _renderer.ViewLeftBottom;
			Vector2 viewRT = _renderer.ViewRightTop;

			const float padding = 2f;
			Vector2 worldHalfSize = screenHalfSize.FlipY() / _renderer.Zoom - new Vector2(padding, padding);

			//var groundEntity = new KaEntity(_world,
			//								width: worldHalfSize.X * 2f * 0.9f,
			//								height: 3f,
			//								isStatic: true,
			//								position: new Vector2(0, -12));
			//groundEntity.Color = Color.DarkGreen;
			//_entityList.Add(groundEntity);

			//var ledgeBody1 = new KaEntity(_world,
			//							  width: 20.0f,
			//							  height: 2.0f,
			//							  isStatic: true,
			//							  position: new Vector2(-10, 1),
			//							  rotation: -MathF.PI * 2 / 20f);
			//ledgeBody1.Color = Color.DarkGray;
			//_entityList.Add(ledgeBody1);

			//var ledgeBody2 = new KaEntity(_world,
			//							  width: 20.0f,
			//							  height: 2.0f,
			//							  isStatic: true,
			//							  position: new Vector2(10, 10),
			//							  rotation: MathF.PI * 2 / 20f);
			//ledgeBody2.Color = Color.DarkGray;
			//_entityList.Add(ledgeBody2);

			// Timer;
			_sampleTimer.Start();
			_physicsCalcTimer.Start();
		}

		private Stopwatch _physicsCalcTimer = new Stopwatch();
		private long _elapsed;
		private float _deltaTime;

		public override void OnUpdate(float deltaTime)
		{
			_deltaTime = deltaTime;

			// Process Input
			_inputManager.Update();

			// Camera move direction
			Vector2 cameraMoveDirection = new();
			if (_inputManager.IsPressed(GameKey.CameraMoveUp))
				cameraMoveDirection += new Vector2(0, 1);
			if (_inputManager.IsPressed(GameKey.CameraMoveDown))
				cameraMoveDirection += new Vector2(0, -1);
			if (_inputManager.IsPressed(GameKey.CameraMoveLeft))
				cameraMoveDirection += new Vector2(-1, 0);
			if (_inputManager.IsPressed(GameKey.CameraMoveRight))
				cameraMoveDirection += new Vector2(1, 0);

			if (cameraMoveDirection.Length() != 0)
			{
				cameraMoveDirection = Vector2.Normalize(cameraMoveDirection);
				_renderer.CameraWorldPosition += cameraMoveDirection / _renderer.Zoom * deltaTime * 500.0f;
			}

			// Remove dynamic objects
			if (_inputManager.IsPressed(GameKey.Space))
			{
				for (int i = 0; i < _entityList.Count; i++)
				{
					KaEntity entity = _entityList[i];
					if (!entity.Body.IsStatic)
					{
						_entityRemovalList.Add(entity);
					}
				}
			}

#if false
			// Process movement direction
			float forceMagnitude = 48f;

			FlatVector forceDirection = new();
			if (InputManager.IsPressed(GameKey.MoveUp))
				forceDirection += new FlatVector(0, 1);
			if (InputManager.IsPressed(GameKey.MoveDown))
				forceDirection += new FlatVector(0, -1);
			if (InputManager.IsPressed(GameKey.MoveLeft))
				forceDirection += new FlatVector(-1, 0);
			if (InputManager.IsPressed(GameKey.MoveRight))
				forceDirection += new FlatVector(1, 0);

			if (FlatMath.Length(forceDirection) != 0)
			{
				forceDirection = FlatMath.Normalize(forceDirection);
				FlatVector force = forceDirection * forceMagnitude;
				body.AddForce(force);
			}

			// Process rotation
			float rotateDirection = 0f;
			if (InputManager.IsPressed(GameKey.RotateLeft))
				rotateDirection += -1;
			if (InputManager.IsPressed(GameKey.RotateRight))
				rotateDirection += 1;

			if (rotateDirection != 0)
			{
				body.Rotate(MathF.PI / 2f * rotateDirection * deltaTime);
			}
#endif
			_physicsCalcTimer.Restart();
			//this.world.Step(0.01f, 1);
			//this.world.Step(deltaTime, 1);
			float interval = 0.0016f;
			while (deltaTime > interval)
			{
				deltaTime -= interval;
				_world.Step(interval, 1);
			}
			_elapsed = _physicsCalcTimer.ElapsedTicks;

			//WarpScreen();
			//RemoveObjectOutOfView();

			for (int i = 0; i < _entityRemovalList.Count; ++i)
			{
				KaEntity entity = _entityRemovalList[i];
				_entityList.Remove(entity);
				_world.RemoveRigidBody(entity.Body);
			}
		}

		public override void OnInvalidate(Graphics g)
		{
			_renderer.BindGraphics(g);
		}

		private void RemoveObjectOutOfView()
		{
			for (int i = 0; i < _entityList.Count; i++)
			{
				KaEntity entity = _entityList[i];
				RigidBody body = entity.Body;

				if (body.IsStatic)
					continue;

				if (body.Position.Y < -12)
				{
					_entityRemovalList.Add(entity);
				}
			}
		}

		private void WarpScreen()
		{
			var vs = _renderer.ViewSize * 1.2f;
			var offset = _renderer.ViewSize * 0.1f;
			var lb = _renderer.ViewLeftBottom - offset;
			var rt = _renderer.ViewRightTop + offset;

			for (int i = 0; i < _world.BodyCount; i++)
			{
				if (!_world.TryGetBody(i, out RigidBody? body))
				{
					continue;
				}
				//FlatVector rt = new FlatVector(16, 8);
				//FlatVector lb = -rt;
				//FlatVector vs = rt * 2;

				if (body.Position.X < lb.X) { body.MoveTo(body.Position + new Vector2(vs.X, 0f)); }
				if (body.Position.X > rt.X) { body.MoveTo(body.Position - new Vector2(vs.X, 0f)); }
				if (body.Position.Y < lb.Y) { body.MoveTo(body.Position + new Vector2(0f, vs.Y)); }
				if (body.Position.Y > rt.Y) { body.MoveTo(body.Position - new Vector2(0f, vs.Y)); }
			}
		}

		private Stopwatch _sampleTimer = new Stopwatch();
		private double _stepElapsed;
		private double _stepElapsedPerCount;
		private double _currentFps;
		public override void OnDraw(Graphics g)
		{
			for (int i = 0; i < _entityList.Count; i++)
			{
				_entityList[i].Draw(_renderer);
			}

			// Draw camera
			var lb = _renderer.ViewLeftBottom;
			var rt = _renderer.ViewRightTop;
			var vs = _renderer.ViewSize;
			var margin = 8 / _renderer.Zoom;
			var cameraPos = _renderer.CameraWorldPosition;
			var corssHairSize = 32 / _renderer.Zoom;

			Vector2 marginVec = new Vector2(margin / 2, margin / 2);
			_renderer.DrawCircleGUI(rt - marginVec, 20f, Color.Yellow);
			_renderer.DrawCircleGUI(lb + marginVec, 20f, Color.Yellow);
			_renderer.DrawCircleGUI(cameraPos, 10f, Color.Yellow);
			_renderer.DrawLine(cameraPos - new Vector2(corssHairSize, 0), cameraPos + new Vector2(corssHairSize, 0), Color.Yellow);
			_renderer.DrawLine(cameraPos - new Vector2(0, corssHairSize), cameraPos + new Vector2(0, corssHairSize), Color.Yellow);
			_renderer.DrawBox((lb + rt) / 2, vs.X - margin, vs.Y - margin, Color.Yellow);

			// Draw contact points
			//var contactPoints = _world.ContactPointsList;
			//for (int i = 0; i < contactPoints.Count; i++)
			//{
			//	var cp = contactPoints[i];
			//	_renderer.DrawBoxGUI(cp, 10.0f, 10.0f, Color.Orange, 2);
			//}

			// Draw debug
			if (_sampleTimer.Elapsed.TotalSeconds > 0.5d)
			{
				_stepElapsed = (double)_elapsed * 1000 / Stopwatch.Frequency;
				_stepElapsedPerCount = (double)_elapsed * 1000 / Stopwatch.Frequency / _world.BodyCount;
				_sampleTimer.Restart();
				_currentFps = 1 / _deltaTime;
			}

			_renderer.DrawTextGUI($"Body count : {_world.BodyCount}", new Vector2(10, 10), Color.White);
			_renderer.DrawTextGUI($"Elapsed : {_stepElapsed:F3} ms", new Vector2(10, 30), Color.White);
			_renderer.DrawTextGUI($"Elapsed per count : {_stepElapsedPerCount:F3} ms", new Vector2(10, 50), Color.White);

			_renderer.DrawTextGUI($"Current FPS : {_currentFps:F0} fps", new Vector2(10, 70), Color.White);
		}

		protected override void onMouseLeftClick(Vector2 clickPos)
		{
			float width = RandomHelper.RandomSingle(1f, 2f);
			float height = RandomHelper.RandomSingle(1f, 2f);
			_entityList.Add(new KaEntity(_world, width, height, isStatic: false, clickPos));
		}

		protected override void onMouseRightClick(Vector2 clickPos)
		{
			float radius = RandomHelper.RandomSingle(0.75f, 1f);
			_entityList.Add(new KaEntity(_world, radius, isStatic: false, clickPos));
		}
	}
}
