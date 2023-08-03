using System.Numerics;

namespace PhysicsTester
{
	public abstract class PhysicsRuntime
	{
		protected MainForm _mainForm;
		protected InputManager _inputManager;
		protected Renderer _renderer = new();
		public Vector2 MouseWorldPosition { get; private set; }

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

		public void OnMouseLeftClick(Vector2 clickPos)
		{
			onMouseLeftClick(_renderer.GetMousePosition(clickPos));
		}

		public void OnMouseRightClick(Vector2 clickPos)
		{
			onMouseRightClick(_renderer.GetMousePosition(clickPos));
		}

		public void OnMouseMove(Vector2 mousePos)
		{
			MouseWorldPosition = _renderer.GetMousePosition(mousePos);
		}

		protected abstract void onMouseLeftClick(Vector2 worldPosition);
		protected abstract void onMouseRightClick(Vector2 worldPosition);
	}
}
