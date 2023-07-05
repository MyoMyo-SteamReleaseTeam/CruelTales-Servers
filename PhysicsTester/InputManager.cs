using System.CodeDom;

namespace PhysicsTester
{
	public enum GameKey
	{
		None,

		MoveLeft,
		MoveRight,
		MoveUp,
		MoveDown,
		RotateLeft,
		RotateRight,

		CameraMoveLeft,
		CameraMoveRight,
		CameraMoveUp,
		CameraMoveDown,

		Num0,
		Num1,
		Num2,
		Num3,
		Num4,
		Num5,
		Num6,
		Num7,
		Num8,
		Num9,

		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,

		Space,
		LControlKey,
		ShiftKey,
	}

	public class InputData
	{
		public event Action? OnPressed;
		public event Action? OnReleased;
		public bool IsPressed;

		public void OnInputPressed()
		{
			if (IsPressed)
				return;

			OnPressed?.Invoke();
			IsPressed = true;
		}

		public void OnInputReleased()
		{
			if (!IsPressed)
				return;

			OnReleased?.Invoke();
			IsPressed = false;
		}

		public void ForceInvokePressed()
		{
			OnPressed?.Invoke();
		}

		public void ForceInvokeReleased()
		{
			OnReleased?.Invoke();
		}
	}

	public class InputManager
	{
		private Dictionary<GameKey, Keys> _inputByGameKey = new();
		private Dictionary<Keys, InputData> _inputTable = new();

		public InputManager()
		{
			// Moving Camera
			addInput(GameKey.CameraMoveLeft, Keys.Left);
			addInput(GameKey.CameraMoveRight, Keys.Right);
			addInput(GameKey.CameraMoveUp, Keys.Up);
			addInput(GameKey.CameraMoveDown, Keys.Down);

			// Moving
			addInput(GameKey.MoveLeft, Keys.A);
			addInput(GameKey.MoveRight, Keys.D);
			addInput(GameKey.MoveUp, Keys.W);
			addInput(GameKey.MoveDown, Keys.S);
			addInput(GameKey.RotateLeft, Keys.Q);
			addInput(GameKey.RotateRight, Keys.E);

			// Number
			addInput(GameKey.Num0, Keys.D0);
			addInput(GameKey.Num1, Keys.D1);
			addInput(GameKey.Num2, Keys.D2);
			addInput(GameKey.Num3, Keys.D3);
			addInput(GameKey.Num4, Keys.D4);
			addInput(GameKey.Num5, Keys.D5);
			addInput(GameKey.Num6, Keys.D6);
			addInput(GameKey.Num7, Keys.D7);
			addInput(GameKey.Num8, Keys.D8);
			addInput(GameKey.Num9, Keys.D9);

			// F Number
			addInput(GameKey.F1, Keys.F1);
			addInput(GameKey.F2, Keys.F2);
			addInput(GameKey.F3, Keys.F3);
			addInput(GameKey.F4, Keys.F4);
			addInput(GameKey.F5, Keys.F5);
			addInput(GameKey.F6, Keys.F6);
			addInput(GameKey.F7, Keys.F7);
			addInput(GameKey.F8, Keys.F8);
			addInput(GameKey.F9, Keys.F9);
			addInput(GameKey.F10, Keys.F10);
			addInput(GameKey.F11, Keys.F11);
			addInput(GameKey.F12, Keys.F12);

			// Others
			addInput(GameKey.Space, Keys.Space);
			addInput(GameKey.LControlKey, Keys.LControlKey);
			addInput(GameKey.ShiftKey, Keys.ShiftKey);

			void addInput(GameKey key, Keys value)
			{
				_inputByGameKey.Add(key, value);
				_inputTable.Add(value, new InputData());
			}
		}

		public InputData GetInputData(GameKey gameKey)
		{
			return _inputTable[_inputByGameKey[gameKey]];
		}

		public void SetInput(Keys key, bool isPressed)
		{
			if (!_inputTable.ContainsKey(key))
				return;

			if (isPressed)
				_inputTable[key].OnInputPressed();
			else
				_inputTable[key].OnInputReleased();
		}

		public bool IsPressed(GameKey gameKey)
		{
			return _inputTable[_inputByGameKey[gameKey]].IsPressed;
		}
	}
}
