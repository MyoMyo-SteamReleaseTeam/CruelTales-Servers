using System.Diagnostics;
using System.Numerics;
using KaNet.Physics;
using PhysicsTester.FlatPhysics;

namespace PhysicsTester
{
	public partial class MainForm : Form
	{
		// Mouse Cursor
		public Point InitializeMouseCursorPosition;
		public Point MouseMovedAmount;

		// Game
		private InputManager _inputManager;
		private PhysicsRuntime _physicsRuntime;
		private Stopwatch _stopwatch = new Stopwatch();
		private long _lastTick;

		public MainForm()
		{
			InitializeComponent();

			// Initialize
			_inputManager = new InputManager();

			Vector2 screenSize = new Vector2(MainCanvas.Width, MainCanvas.Height);
			//_physicsRuntime = new FlatPhysicsRuntime(this, _inputManager, screenSize);
			_physicsRuntime = new KaNetPhysicsRuntime(this, _inputManager, screenSize);

			// Start tick timer
			_stopwatch.Start();

			// Initialize Mouse Position
			this.InitializeMouseCursorPosition = new Point(0, 0);
			this.MouseMovedAmount = new Point(0, 0);
			this.InitializeMouseCursorPosition.X = this.Location.X + this.ClientSize.Width / 2;
			this.InitializeMouseCursorPosition.Y = this.Location.Y + this.ClientSize.Width / 2;

			MainCanvas.MouseWheel += mainCanvas_MouseWheel;
		}

		private void mainCanvas_MouseWheel(object? sender, MouseEventArgs e)
		{
			int delta = Math.Sign(e.Delta) * 3;
			_physicsRuntime.Zoom(delta);
		}

		private void Timer_Tick_Tick(object sender, EventArgs e)
		{
			Vector2 screenSize = new Vector2(MainCanvas.Width, MainCanvas.Height);
			_physicsRuntime.SetScreenSize(screenSize);

			// Calcualte delta time
			long currentTick = _stopwatch.ElapsedTicks;
			double deltaTime = currentTick - _lastTick;
			deltaTime = deltaTime / Stopwatch.Frequency;

			// Update game loop
			_physicsRuntime.OnUpdate((float)deltaTime);

			// Upate drawing
			MainCanvas.Invalidate();

			// Set last tick
			_lastTick = currentTick;

			// Process mouse input
			bool IsVirtualMouseMode = false;
			if (this.Focused)
			{
				if (IsVirtualMouseMode)
				{
					this.MouseMovedAmount.X = Cursor.Position.X - this.InitializeMouseCursorPosition.X;
					this.MouseMovedAmount.Y = Cursor.Position.Y - this.InitializeMouseCursorPosition.Y;
					Cursor.Position = this.InitializeMouseCursorPosition;
				}
				else
				{
					this.MouseMovedAmount.X = 0;
					this.MouseMovedAmount.Y = 0;
				}
			}
		}

		private void MainCanvas_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.FromArgb(20, 20, 20));
			_physicsRuntime.OnInvalidate(e.Graphics);
			_physicsRuntime.OnDraw(e.Graphics);

		}

		#region Set Key Input

		private void Main_KeyDown(object sender, KeyEventArgs e)
		{
			_inputManager.SetInput(e.KeyCode, true);
		}

		private void Main_KeyUp(object sender, KeyEventArgs e)
		{
			_inputManager.SetInput(e.KeyCode, false);
		}

		#endregion

		#region Set Mouse Input

		private bool _isDragging = false;
		private Vector2 _lastMousePos = new Vector2();
		private void Main_MouseDown(object sender, MouseEventArgs e)
		{
			var clickPos = new Vector2(e.X, e.Y);

			if (e.Button == MouseButtons.Left)
			{
				_physicsRuntime.OnMouseLeftClick(clickPos);
			}
			else if (e.Button == MouseButtons.Right)
			{
				_physicsRuntime.OnMouseRightClick(clickPos);
			}
			else if (e.Button == MouseButtons.Middle)
			{
				_isDragging = true;
				_lastMousePos = new Vector2(e.X, e.Y);
			}
		}

		private void Main_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				_isDragging = false;
			}
		}

		private void Main_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isDragging)
			{
				Vector2 curMousePos = new Vector2(e.X, e.Y);
				Vector2 delta = _lastMousePos - curMousePos;
				_physicsRuntime.Drag(delta);
				_lastMousePos = curMousePos;
			}
		}

		#endregion

		public void ResetMousePosition()
		{
			Cursor.Position = this.InitializeMouseCursorPosition;
		}

		private void FormSizeOrLocationChanged(object sender, EventArgs e)
		{
			this.InitializeMouseCursorPosition.X = this.Location.X + this.ClientSize.Width / 2;
			this.InitializeMouseCursorPosition.Y = this.Location.Y + this.ClientSize.Height / 2;
		}

		public void SetText(string text)
		{
			this.Text = text;
		}
	}
}