using System.Diagnostics;
using System.Numerics;
using KaNet.Physics.RigidBodies;
using PhysicsTester;
using PhysicsTester.KaNetPhysics;

namespace KaNet.Physics
{
	public class KaNetPhysicsRuntime : PhysicsRuntime
	{
		// Physics
		private PhysicsWorld _world = new();

		// Entity
		private KaEntityManager _entityManager = new();

		// Loop Timer
		private Stopwatch _physicsCalcTimer = new Stopwatch();
		private long _elapsed;
		private float _deltaTime;

		// Physics Step Timer
		private Stopwatch _sampleTimer = new Stopwatch();
		private double _stepElapsed;
		private double _stepElapsedPerCount;
		private double _currentFps;

		// Control
		private RigidBody? _controlBody;

		private Action? OnPressSpaceBar;

		public KaNetPhysicsRuntime(MainForm mainForm, InputManager inputManager, Vector2 screenSize)
			: base(mainForm, inputManager)
		{
			// Set camera to world center
			Vector2 screenHalfSize = screenSize * 0.5f;
			_renderer.Zoom = 1 + 3 * 7;
			_renderer.ScreenCameraPosition = -screenHalfSize.FlipY();

			// View diameter for initial setup
			Vector2 viewLB = _renderer.ViewLeftBottom;
			Vector2 viewRT = _renderer.ViewRightTop;
			Vector2 viewHalfSize = screenHalfSize.FlipY() / _renderer.Zoom;

			// Setup
			setupStaticGameWorld(viewLB, viewRT, viewHalfSize);
			//setupForCircleTest(viewLB, viewRT, viewHalfSize);

			// Start Timers
			_sampleTimer.Start();
			_physicsCalcTimer.Start();
		}

		private void setupStaticGameWorld(Vector2 viewLB, Vector2 viewRT, Vector2 viewHalfSize)
		{
			this.OnPressSpaceBar = () =>
			{
				foreach (KaEntity entity in _entityManager.Entities)
				{
					if (!entity.Body.IsStatic)
					{
						_entityManager.RemoveEntity(entity);
					}
				}
			};

			var groundEntity = new KaEntity(_world,
											width: viewHalfSize.X * 2f * 0.9f,
											height: 3f,
											isStatic: true,
											position: new Vector2(0, -12));
			groundEntity.Color = Color.DarkGreen;
			_entityManager.AddEntity(groundEntity);

			var ledgeBody1 = new KaEntity(_world,
										  width: 20.0f,
										  height: 2.0f,
										  isStatic: true,
										  position: new Vector2(-10, 1),
										  rotation: -MathF.PI * 2 / 20f);
			ledgeBody1.Color = Color.DarkGray;
			_entityManager.AddEntity(ledgeBody1);

			var ledgeBody2 = new KaEntity(_world,
										  width: 20.0f,
										  height: 2.0f,
										  isStatic: true,
										  position: new Vector2(10, 10),
										  rotation: MathF.PI * 2 / 20f);
			ledgeBody2.Color = Color.DarkGray;
			_entityManager.AddEntity(ledgeBody2);
		}

		private void setupForCircleTest(Vector2 viewLB, Vector2 viewRT, Vector2 viewHalfSize)
		{

		}

		public override void OnUpdate(float deltaTime)
		{
			// Update entity manager
			_entityManager.Update();

			// Bind delta time
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
				OnPressSpaceBar?.Invoke();
			}

			// Process movement direction
			float forceMagnitude = 48f;

			Vector2 forceDirection = new();
			if (_inputManager.IsPressed(GameKey.MoveUp))
				forceDirection += new Vector2(0, 1);
			if (_inputManager.IsPressed(GameKey.MoveDown))
				forceDirection += new Vector2(0, -1);
			if (_inputManager.IsPressed(GameKey.MoveLeft))
				forceDirection += new Vector2(-1, 0);
			if (_inputManager.IsPressed(GameKey.MoveRight))
				forceDirection += new Vector2(1, 0);

			if (_controlBody != null && forceDirection.Length() != 0)
			{
				forceDirection = Vector2.Normalize(forceDirection);
				Vector2 force = forceDirection * forceMagnitude;
				_controlBody.LinearVelocity = force;
			}

			// Process rotation
			float rotateDirection = 0f;
			if (_inputManager.IsPressed(GameKey.RotateLeft))
				rotateDirection += -1;
			if (_inputManager.IsPressed(GameKey.RotateRight))
				rotateDirection += 1;

			if (_controlBody != null || rotateDirection != 0)
			{
				_controlBody?.Rotate(MathF.PI / 2f * rotateDirection * deltaTime);
			}

			_physicsCalcTimer.Restart();
			//this.world.Step(0.01f, 1);
			//this.world.Step(deltaTime, 1);
			float interval = 0.0016f;
			while (deltaTime > interval)
			{
				deltaTime -= interval;
				_world.Step(interval);
			}
			_elapsed = _physicsCalcTimer.ElapsedTicks;

			//WarpScreen();
			//RemoveObjectOutOfView();
		}

		public override void OnInvalidate(Graphics g)
		{
			_renderer.BindGraphics(g);
		}

		public override void OnDraw(Graphics g)
		{
			foreach (KaEntity entity in _entityManager.Entities)
			{
				entity.Draw(_renderer);
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
			_entityManager.AddEntity(new KaEntity(_world, width, height, isStatic: false, clickPos));
		}

		protected override void onMouseRightClick(Vector2 clickPos)
		{
			float radius = RandomHelper.RandomSingle(0.75f, 1f);
			_entityManager.AddEntity(new KaEntity(_world, radius, isStatic: false, clickPos));
		}

		private void RemoveObjectOutOfView()
		{
			foreach (KaEntity entity in _entityManager.Entities)
			{
				RigidBody body = entity.Body;

				if (body.IsStatic)
					continue;

				if (body.Position.Y < -12)
				{
					_entityManager.RemoveEntity(entity);
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
	}
}
