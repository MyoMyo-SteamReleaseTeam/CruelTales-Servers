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
		private KaPhysicsWorld _world = new();

		// Entity
		private KaEntityManager _entityManager = new();
		private int _selectedEntity = 1;
		private bool _isRun;

		// Loop Timer
		private Stopwatch _physicsCalcTimer = new Stopwatch();
		private long _elapsed;
		private long _currentIterateCount;
		private long _iterateCount;
		private float _deltaTime;

		// Physics Step Timer
		private Stopwatch _sampleTimer = new Stopwatch();
		private double _stepElapsed;
		private double _stepElapsedPerCount;
		private double _stepElapsedPerIter;
		private double _currentFps;
		private float _deltaTimeStack = 0;

		// Inputs
		private Action? OnProcessUpdate;
		private Action? OnPressSpaceBar;
		private Action? OnPressEnter;
		private Action<Vector2>? OnPressLeftMouseClick;
		private Action<Vector2>? OnPressRightMouseClick;

		// Color
		private Color _staticObjectColor = Color.FromArgb(20, 20, 20);

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
			inputManager.GetInputData(GameKey.F2).OnPressed += () => setupForCirclesTest(viewLB, viewRT, viewHalfSize);
			inputManager.GetInputData(GameKey.F3).OnPressed += () => setupForAABBsTest(viewLB, viewRT, viewHalfSize);
			inputManager.GetInputData(GameKey.F4).OnPressed += () => setupForOBBsTest(viewLB, viewRT, viewHalfSize);

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
			OnPressLeftMouseClick = (worldPos) =>
			{
				float width = RandomHelper.NextSingle(radiusMin, radiusMax);
				float height = RandomHelper.NextSingle(radiusMin, radiusMax);
				_entityManager.AddEntity(KaEntity.CreateAABBEntity(_world, width, height, isStatic: false, worldPos));
				_entityManager.AddEntity(KaEntity.CreateAABBEntity(_world, width, height, isStatic: false, worldPos));
			};

			// Create world
			OnPressRightMouseClick = (worldPos) =>
			{
				float radius = RandomHelper.NextSingle(radiusMin, radiusMax);
				_entityManager.AddEntity(KaEntity.CreateCircleEntity(_world, radius, isStatic: false, worldPos));
				_entityManager.AddEntity(KaEntity.CreateCircleEntity(_world, radius, isStatic: false, worldPos));
			};

			var groundEntity = KaEntity.CreateAABBEntity(_world,
														 width: viewHalfSize.X * 2f * 0.9f,
														 height: 3f,
														 isStatic: true,
														 position: new Vector2(0, -12));
			groundEntity.Color = Color.DarkGreen;
			_entityManager.AddEntity(groundEntity);

			var ledgeBody1 = KaEntity.CreateOBBEntity(_world,
													  width: 20.0f,
													  height: 2.0f,
													  isStatic: true,
													  position: new Vector2(-10, 1),
													  rotation: -MathF.PI * 2 / 20f);
			ledgeBody1.Color = Color.DarkGray;
			_entityManager.AddEntity(ledgeBody1);

			var ledgeBody2 = KaEntity.CreateOBBEntity(_world,
													  width: 20.0f,
													  height: 2.0f,
													  isStatic: true,
													  position: new Vector2(10, 10),
													  rotation: MathF.PI * 2 / 20f);
			ledgeBody2.Color = Color.DarkGray;
			_entityManager.AddEntity(ledgeBody2);
		}

		private void setupForCirclesTest(Vector2 viewLB, Vector2 viewRT, Vector2 viewHalfSize)
		{
			_entityManager.Clear();

			int dynamicCount = 5;
			int staticCount = 0;
			float sizeMin = 1.0f;
			float sizeMax = 2.0f;

			// Bind Events
			OnProcessUpdate = () =>
			{
				if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
				{
					_renderer.CameraWorldPosition = entity.Body.Position;
				}
			};

			// Bind inputs
			OnPressLeftMouseClick = (worldPos) =>
			{
				float width = RandomHelper.NextSingle(sizeMin, sizeMax);
				float height = RandomHelper.NextSingle(sizeMin, sizeMax);
				_entityManager.AddEntity(KaEntity.CreateAABBEntity(_world, width, height, isStatic: false, worldPos));
			};

			OnPressRightMouseClick = (worldPos) =>
			{
				float width = RandomHelper.NextSingle(sizeMin * 2, sizeMax * 2);
				float height = RandomHelper.NextSingle(sizeMin * 2, sizeMax * 2);
				float rotation = RandomHelper.NextSingle(0, MathF.PI * 2);
				_entityManager.AddEntity(KaEntity.CreateOBBEntity(_world, width, height, isStatic: false, rotation, worldPos));
				//if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
				//{
				//	RigidBody body = entity.Body;
				//	Vector2 direction = Vector2.Normalize(worldPos - body.Position);
				//	body.ForceVelocity =  direction * 60.0f;
				//}
			};

			// Create world
			createRandomWorldBy(dynamicCount, staticCount, sizeMin, sizeMax, viewLB, viewRT, KaPhysicsShapeType2D.Circle);
		}

		private void setupForAABBsTest(Vector2 viewLB, Vector2 viewRT, Vector2 viewHalfSize)
		{
			_entityManager.Clear();

			int dynamicCount = 10;
			int staticCount = 10;
			float sizeMin = 2.0f;
			float sizeMax = 4.0f;

			// Bind Events
			OnProcessUpdate = () =>
			{
				if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
				{
					_renderer.CameraWorldPosition = entity.Body.Position;
				}
			};

			// Bind inputs
			OnPressLeftMouseClick = (worldPos) =>
			{
				for (int i = 0; i < 1; i++)
				{
					float radius = RandomHelper.NextSingle(sizeMin * 0.5f, sizeMax * 0.5f);
					_entityManager.AddEntity(KaEntity.CreateCircleEntity(_world, radius, isStatic: false, worldPos));
				}
			};

			OnPressRightMouseClick = (worldPos) =>
			{
				for (int i = 0; i < 1; i++)
				{
					float width = RandomHelper.NextSingle(sizeMin, sizeMax);
					float height = RandomHelper.NextSingle(sizeMin, sizeMax);
					_entityManager.AddEntity(KaEntity.CreateAABBEntity(_world, width, height, isStatic: false, worldPos));
				}
			};

			// Create world
			createRandomWorld(dynamicCount, staticCount, sizeMin, sizeMax, viewLB, viewRT);
		}

		private void setupForOBBsTest(Vector2 viewLB, Vector2 viewRT, Vector2 viewHalfSize)
		{
			_entityManager.Clear();

			int dynamicCount = 0;
			int staticCount = 1;
			float sizeMin = 2.0f;
			float sizeMax = 4.0f;

			// Bind Events
			OnProcessUpdate = () =>
			{
				if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
				{
					_renderer.CameraWorldPosition = entity.Body.Position;
				}
			};

			// Bind inputs
			OnPressLeftMouseClick = (worldPos) =>
			{
				for (int i = 0; i < 1; i++)
				{
					float rotation = RandomHelper.NextSingle(0, MathF.PI * 2);
					float width = RandomHelper.NextSingle(sizeMin, sizeMax);
					float height = RandomHelper.NextSingle(sizeMin, sizeMax);
					_entityManager.AddEntity(KaEntity.CreateOBBEntity(_world, width, height, isStatic: false, rotation, worldPos));
				}
			};

			OnPressRightMouseClick = (worldPos) =>
			{
				for (int i = 0; i < 1; i++)
				{
					float rotation = RandomHelper.NextSingle(0, MathF.PI * 2);
					float width = RandomHelper.NextSingle(sizeMin, sizeMax);
					float height = RandomHelper.NextSingle(sizeMin, sizeMax);
					_entityManager.AddEntity(KaEntity.CreateAABBEntity(_world, width, height, isStatic: false, worldPos));
				}
			};

			createRandomWorldBy(dynamicObjectCount: 1, 0, sizeMin, sizeMax,
								viewLB, viewRT, KaPhysicsShapeType2D.Box_AABB);

			//createRandomWorldBy(dynamicObjectCount: 1, 0, sizeMin, sizeMax,
			//					viewLB, viewRT, PhysicsShapeType.Circle);

			// Create world
			createRandomWorldBy(dynamicCount, staticCount, sizeMin, sizeMax,
								viewLB, viewRT, KaPhysicsShapeType2D.Box_OBB);
		}

		public override void OnUpdate(float deltaTime)
		{
			// Update entity manager
			_entityManager.Update();

			// Bind delta time
			_deltaTime = deltaTime;
			_deltaTimeStack += deltaTime;

			processCameraInput(deltaTime);
			processEntityInput(deltaTime);

			_physicsCalcTimer.Restart();
			//_world.Step(deltaTime);
			_world.Step(0.01f);
			_currentIterateCount = 1;

			//int iterCount = 0;
			//float interval = 0.01f;
			//while (_deltaTimeStack > interval)
			//{
			//	_deltaTimeStack -= interval;
			//	_world.Step(interval);
			//	if (++iterCount >= 15)
			//		break;
			//}
			//_currentIterateCount = iterCount;

			_elapsed = _physicsCalcTimer.ElapsedTicks;

			OnProcessUpdate?.Invoke();

			if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
			{
				entity.Body.LinearVelocity = Vector2.Zero;
			}

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

			KaRigidBody controlBody = entity.Body;

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
				rotateDirection += 1;
			if (_inputManager.IsPressed(GameKey.RotateRight))
				rotateDirection += -1;

			if (controlBody != null && rotateDirection != 0)
			{
				controlBody?.Rotate(MathF.PI / 2f * rotateDirection * deltaTime);
			}
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
				_iterateCount = _currentIterateCount;
				_stepElapsedPerIter = _iterateCount == 0 ? 0 : (float)(_stepElapsed / _iterateCount);
			}

			_renderer.DrawTextGUI($"Body count : {_world.BodyCount}", new Vector2(10, 10), Color.White);
			_renderer.DrawTextGUI($"Elapsed / iterate: {_stepElapsed:F3} ms / {_iterateCount}", new Vector2(10, 30), Color.White);
			_renderer.DrawTextGUI($"Elapsed per iterate : {_stepElapsedPerIter:F3} ms", new Vector2(10, 50), Color.White);
			_renderer.DrawTextGUI($"Elapsed per count : {_stepElapsedPerCount:F3} ms", new Vector2(10, 70), Color.White);

			_renderer.DrawTextGUI($"Current FPS : {_currentFps:F0} fps", new Vector2(10, 110), Color.White);
			_renderer.DrawTextGUI($"DeltaTime : {_deltaTime:F4} ms", new Vector2(10, 130), Color.White);
			_renderer.DrawTextGUI($"Selected Entity : {_selectedEntity}", new Vector2(10, 150), Color.White);
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
				KaRigidBody body = entity.Body;

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
				if (!_world.TryGetBody(i, out KaRigidBody? body))
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

		private void pushSelectedBody(Vector2 worldPos)
		{
			if (_entityManager.TryGetEntity(_selectedEntity, out var entity))
			{
				KaRigidBody body = entity.Body;
				Vector2 direction = Vector2.Normalize(worldPos - body.Position);
				body.ForceVelocity = direction * 60.0f;
			}
		}

		private void createRandomWorld(int dynamicObjectCount, int staticObjectCount,
									   float sizeMin, float sizeMax,
									   Vector2 viewLB, Vector2 viewRT)
		{
			for (int i = 0; i < dynamicObjectCount + staticObjectCount; i++)
			{
				KaEntity? entity = null;
				bool isStatic = i >= dynamicObjectCount;
				Vector2 randPos = RandomHelper.NextVectorBetween(viewRT, viewLB);
				float radius = RandomHelper.NextSingle(sizeMin, sizeMax);
				float weith = RandomHelper.NextSingle(sizeMin, sizeMax);
				float height = RandomHelper.NextSingle(sizeMin, sizeMax);
				float rotation = RandomHelper.NextSingle(0, MathF.PI * 2);

				switch (RandomHelper.NextInteger(0, 3))
				{
					case 0:
						entity = KaEntity.CreateCircleEntity(_world, radius, isStatic, randPos);
						break;

					case 1:
						entity = KaEntity.CreateAABBEntity(_world, weith, height, isStatic, randPos);
						break;

					case 2:
						entity = KaEntity.CreateOBBEntity(_world, weith, height, isStatic,
														  rotation, randPos);
						break;
				}

				if (entity == null)
					continue;

				entity.Color = isStatic ? _staticObjectColor : entity.Color;
				_entityManager.AddEntity(entity);
			}
		}

		private void createRandomWorldBy(int dynamicObjectCount, int staticObjectCount,
										 float sizeMin, float sizeMax,
										 Vector2 viewLB, Vector2 viewRT,
										 KaPhysicsShapeType2D shapeType)
		{
			for (int i = 0; i < dynamicObjectCount + staticObjectCount; i++)
			{
				KaEntity? entity = null;
				bool isStatic = i >= dynamicObjectCount;
				Vector2 randPos = RandomHelper.NextVectorBetween(viewRT, viewLB);
				float radius = RandomHelper.NextSingle(sizeMin, sizeMax);
				float weith = RandomHelper.NextSingle(sizeMin, sizeMax);
				float height = RandomHelper.NextSingle(sizeMin, sizeMax);
				float rotation = RandomHelper.NextSingle(0, MathF.PI * 2);

				switch (shapeType)
				{
					case KaPhysicsShapeType2D.Circle:
						entity = KaEntity.CreateCircleEntity(_world, radius, isStatic, randPos);
						break;

					case KaPhysicsShapeType2D.Box_AABB:
						entity = KaEntity.CreateAABBEntity(_world, weith, height, isStatic, randPos);
						break;

					case KaPhysicsShapeType2D.Box_OBB:
						entity = KaEntity.CreateOBBEntity(_world, weith, height, isStatic,
														  rotation, randPos);
						break;
				}

				if (entity == null)
					continue;

				entity.Color = isStatic ? _staticObjectColor : entity.Color;
				_entityManager.AddEntity(entity);
			}
		}
	}
}
