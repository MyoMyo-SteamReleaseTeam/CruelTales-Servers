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
		private int _selectedEntity = 1;
		private bool _isRun;

		// Loop Timer
		private Stopwatch _physicsCalcTimer = new Stopwatch();
		private long _elapsed;
		private float _deltaTime;

		// Physics Step Timer
		private Stopwatch _sampleTimer = new Stopwatch();
		private double _stepElapsed;
		private double _stepElapsedPerCount;
		private double _currentFps;

		// Inputs
		private Action? OnProcessUpdate;
		private Action? OnPressSpaceBar;
		private Action? OnPressEnter;
		private Action<Vector2>? OnPressLeftMouseClick;
		private Action<Vector2>? OnPressRightMouseClick;

		public KaNetPhysicsRuntime(MainForm mainForm, InputManager inputManager, Vector2 screenSize)
			: base(mainForm, inputManager)
		{
			// Set camera to world center
			Vector2 screenHalfSize = screenSize * 0.5f;
			_renderer.ScreenSize = screenSize;
			int initialZoomAmount = 6;
			_renderer.Zoom = 1 + 3 * initialZoomAmount;
			_renderer.ScreenCameraPosition = -screenHalfSize.FlipY();

			// View diameter for initial setup
			Vector2 viewLB = _renderer.ViewLeftBottom;
			Vector2 viewRT = _renderer.ViewRightTop;
			Vector2 viewHalfSize = screenHalfSize.FlipY() / _renderer.Zoom;

			// Setup
			this.OnPressSpaceBar = () =>
			{
				foreach (KaEntity entity in _entityManager.Entities)
				{
					//if (!entity.Body.IsStatic)
					{
						_entityManager.RemoveEntity(entity);
					}
				}
			};

			// Bind Inputs
			inputManager.GetInputData(GameKey.ShiftKey).OnPressed += () => _isRun = true;
			inputManager.GetInputData(GameKey.ShiftKey).OnReleased += () => _isRun = false;

			inputManager.GetInputData(GameKey.F1).OnPressed += () => setupStaticGameWorld(viewLB, viewRT, viewHalfSize);
			inputManager.GetInputData(GameKey.F2).OnPressed += () => setupForCircleTest(viewLB, viewRT, viewHalfSize);

			inputManager.GetInputData(GameKey.F2).ForceInvokePressed();

			inputManager.GetInputData(GameKey.Num0).OnPressed += () => selectEntity(0);
			inputManager.GetInputData(GameKey.Num1).OnPressed += () => selectEntity(1);
			inputManager.GetInputData(GameKey.Num2).OnPressed += () => selectEntity(2);
			inputManager.GetInputData(GameKey.Num3).OnPressed += () => selectEntity(3);
			inputManager.GetInputData(GameKey.Num4).OnPressed += () => selectEntity(4);
			inputManager.GetInputData(GameKey.Num5).OnPressed += () => selectEntity(5);
			inputManager.GetInputData(GameKey.Num6).OnPressed += () => selectEntity(6);
			inputManager.GetInputData(GameKey.Num7).OnPressed += () => selectEntity(7);
			inputManager.GetInputData(GameKey.Num8).OnPressed += () => selectEntity(8);
			inputManager.GetInputData(GameKey.Num9).OnPressed += () => selectEntity(9);

			// Start Timers
			_sampleTimer.Start();
			_physicsCalcTimer.Start();
		}

		private void setupStaticGameWorld(Vector2 viewLB, Vector2 viewRT, Vector2 viewHalfSize)
		{
			_entityManager.Clear();

			int dynamicCount = 20;
			int staticCount = 10;
			float radiusMin = 1.0f;
			float radiusMax = 2.0f;

			// Bind inputs
			OnPressLeftMouseClick += (worldPos) =>
			{
				float width = RandomHelper.NextSingle(radiusMin, radiusMax);
				float height = RandomHelper.NextSingle(radiusMin, radiusMax);
				_entityManager.AddEntity(new KaEntity(_world, width, height, isStatic: false, worldPos));
			};

			// Create world
			OnPressRightMouseClick += (worldPos) =>
			{
				float radius = RandomHelper.NextSingle(radiusMin, radiusMax);
				_entityManager.AddEntity(new KaEntity(_world, radius, isStatic: false, worldPos));
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
			_entityManager.Clear();

			int dynamicCount = 20;
			int staticCount = 10;
			float radiusMin = 1.0f;
			float radiusMax = 2.0f;

			// Bind Events
			OnProcessUpdate += () =>
			{
				if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
				{
					_renderer.CameraWorldPosition = entity.Body.Position;
				}
			};

			// Bind inputs
			OnPressLeftMouseClick += (worldPos) =>
			{
			};

			OnPressRightMouseClick += (worldPos) =>
			{
				if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
				{
					RigidBody body = entity.Body;
					Vector2 direction = Vector2.Normalize(worldPos - body.Position);
					body.LinearVelocity =  direction * 60.0f;
				}
			};

			// Create world
			for (int i = 0; i < dynamicCount; i++)
			{
				float radius = RandomHelper.NextSingle(radiusMin, radiusMax);
				Vector2 randPos = RandomHelper.NextVectorBetween(viewRT, viewLB);
				KaEntity entity = new KaEntity(_world, radius, isStatic: false, randPos);
				_entityManager.AddEntity(entity);
			}

			for (int i = 0; i < staticCount; i++)
			{
				float radius = RandomHelper.NextSingle(radiusMin, radiusMax);
				Vector2 randPos = RandomHelper.NextVectorBetween(viewRT, viewLB);
				KaEntity entity = new KaEntity(_world, radius, isStatic: true, randPos);
				entity.Color = Color.FromArgb(40, 40, 40);
				_entityManager.AddEntity(entity);
			}
		}

		public override void OnUpdate(float deltaTime)
		{
			// Update entity manager
			_entityManager.Update();

			// Bind delta time
			_deltaTime = deltaTime;

			processCameraInput(deltaTime);
			processEntityInput(deltaTime);

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

			OnProcessUpdate?.Invoke();

			//WarpScreen();
			//RemoveObjectOutOfView();
		}

		private void processCameraInput(float deltaTime)
		{
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
		}

		private bool _isMoved = false;
		private void processEntityInput(float deltaTime)
		{
			if (!_entityManager.TryGetEntity(_selectedEntity, out var entity))
				return;

			RigidBody controlBody = entity.Body;

			// Process movement direction
			float forceMagnitude = 10f;
			forceMagnitude *= _isRun ? 2 : 1;

			Vector2 forceDirection = new();
			if (_inputManager.IsPressed(GameKey.MoveUp))
				forceDirection += new Vector2(0, 1);
			if (_inputManager.IsPressed(GameKey.MoveDown))
				forceDirection += new Vector2(0, -1);
			if (_inputManager.IsPressed(GameKey.MoveLeft))
				forceDirection += new Vector2(-1, 0);
			if (_inputManager.IsPressed(GameKey.MoveRight))
				forceDirection += new Vector2(1, 0);

			if (forceDirection.Length() != 0)
			{
				forceDirection = Vector2.Normalize(forceDirection);
				Vector2 force = forceDirection * forceMagnitude;
				controlBody.LinearVelocity = force;
				_isMoved = true;
			}
			else if (_isMoved)
			{
				controlBody.LinearVelocity = Vector2.Zero;
				_isMoved = false;
			}

			// Process rotation
			float rotateDirection = 0f;
			if (_inputManager.IsPressed(GameKey.RotateLeft))
				rotateDirection += -1;
			if (_inputManager.IsPressed(GameKey.RotateRight))
				rotateDirection += 1;

			if (controlBody != null || rotateDirection != 0)
			{
				controlBody?.Rotate(MathF.PI / 2f * rotateDirection * deltaTime);
			}
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
			_renderer.DrawTextGUI($"Selected Entity : {_selectedEntity}", new Vector2(10, 90), Color.White);
		}

		protected override void onMouseLeftClick(Vector2 worldPos)
		{
			this.OnPressLeftMouseClick?.Invoke(worldPos);
		}

		protected override void onMouseRightClick(Vector2 worldPos)
		{
			this.OnPressRightMouseClick?.Invoke(worldPos);
		}

		private void selectEntity(int entityId)
		{
			_selectedEntity = entityId;
		}

		private void removeObjectOutOfView()
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

		private void warpScreen()
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
