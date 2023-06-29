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

		Space,
		LControlKey,
		ShiftKey,
	}

	public class InputData
	{
		public event Action? OnPressed;
		public event Action? OnReleased;
		public bool IsPressed;

		public void Update()
		{
			if (IsPressed)
			{
				OnPressed?.Invoke();
			}
		}

		public void OnInputPressed()
		{
			if (!IsPressed)
			{
				OnPressed?.Invoke();
			}

			IsPressed = true;
		}

		public void OnInputReleased()
		{
			OnReleased?.Invoke();
			IsPressed = false;
		}
	}

	public class InputManager
	{
		private Dictionary<GameKey, Keys> _inputByGameKey = new()
		{
			// Moving Camera
			{ GameKey.CameraMoveLeft, Keys.Left },
			{ GameKey.CameraMoveRight, Keys.Right },
			{ GameKey.CameraMoveUp, Keys.Up },
			{ GameKey.CameraMoveDown, Keys.Down },

			// Moving
			{ GameKey.MoveLeft, Keys.A },
			{ GameKey.MoveRight, Keys.D },
			{ GameKey.MoveUp, Keys.W },
			{ GameKey.MoveDown, Keys.S },
			{ GameKey.RotateLeft, Keys.Q },
			{ GameKey.RotateRight, Keys.E },

			{ GameKey.Space, Keys.Space },
			{ GameKey.LControlKey, Keys.LControlKey },
			{ GameKey.ShiftKey, Keys.ShiftKey },
		};

		private Dictionary<Keys, InputData> _inputTable = new()
		{
			// Moving Camera
			{ Keys.Left, new InputData() },
			{ Keys.Right, new InputData() },
			{ Keys.Up, new InputData() },
			{ Keys.Down, new InputData() },

			// Moving
			{ Keys.A, new InputData() },
			{ Keys.D, new InputData() },
			{ Keys.W, new InputData() },
			{ Keys.S, new InputData() },
			{ Keys.Q, new InputData() },
			{ Keys.E, new InputData() },

			{ Keys.Space, new InputData() },
			{ Keys.LControlKey, new InputData() },
			{ Keys.ShiftKey, new InputData() },
		};

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

		public void Update()
		{
			foreach (var inputData in _inputTable.Values)
			{
				inputData.Update();
			}
		}
	}
}
