using System.Numerics;

namespace PhysicsTester
{
	public abstract class PhysicsRuntime
	{
		protected MainForm _mainForm;
		protected InputManager _inputManager;
		protected Renderer _renderer = new();
		public Vector2 MouseWorldPosition { get; private set; }

		// Inputs
		public Action<Vector2>? OnLeftMouseClick;
		public Action<Vector2>? OnRightMouseClick;
		public Action<Vector2>? OnLeftMousePress;
		public Action<Vector2>? OnRightMousePress;

		public InputData InputSpaceBar => _inputManager.GetInputData(GameKey.Space);
		public InputData InputEnter => _inputManager.GetInputData(GameKey.Enter);

		public PhysicsRuntime(MainForm mainForm, InputManager inputManager)
		{
			_mainForm = mainForm;
			_inputManager = inputManager;
		}

		public abstract void OnUpdate(float deltaTime);
		public virtual void OnInvalidate(Graphics g)
		{
			_renderer.BindGraphics(g);
			_renderer.OnBeforeDraw();
		}
		public abstract void OnDraw(Graphics g);

		public void Zoom(int delta)
		{
			_renderer.Zoom += delta;
		}

		public void Drag(Vector2 delta)
		{
			_renderer.CameraWorldPosition += delta.FlipY() / _renderer.Zoom * 1.0f;
		}

		public void SetScreenSize(Vector2 screenSize)
		{
			_renderer.ScreenSize = screenSize;
		}

		public void OnMouseLeftClick(Vector2 mousePos)
		{
			Vector2 worldPos = _renderer.GetMousePosition(mousePos);
			OnLeftMouseClick?.Invoke(worldPos);
		}

		public void OnMouseRightClick(Vector2 mousePos)
		{
			Vector2 worldPos = _renderer.GetMousePosition(mousePos);
			OnRightMouseClick?.Invoke(worldPos);
		}

		public void OnMouseLeftPress(Vector2 mousePos)
		{
			Vector2 worldPos = _renderer.GetMousePosition(mousePos);
			OnLeftMousePress?.Invoke(worldPos);
		}

		public void OnMouseRightPress(Vector2 mousePos)
		{
			Vector2 worldPos = _renderer.GetMousePosition(mousePos);
			OnRightMousePress?.Invoke(worldPos);
		}

		public void OnMouseMove(Vector2 mousePos)
		{
			MouseWorldPosition = _renderer.GetMousePosition(mousePos);
		}
	}
}
