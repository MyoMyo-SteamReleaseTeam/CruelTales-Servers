using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class Test_MovingCube : MasterNetworkObject
	{
		private float Pivot = 11;
		private float Move = 2;
		private float _destY = 10;

		public void MoveTo(float y)
		{
			this.Server_MoveTo(y);
		}

		private float _moveDelay;

		public void SetMoveTime(float moveTime)
		{
			_moveDelay = moveTime;
		}

		public void Update(float deltaTime)
		{
			Y = Y + ((Dest - Y) * deltaTime * Speed);

			_moveDelay += deltaTime;
			if (_moveDelay > 3f)
			{
				_moveDelay -= 3f;
				Move = -Move;
				Dest = Pivot + Move;
			}
		}
	}
}
