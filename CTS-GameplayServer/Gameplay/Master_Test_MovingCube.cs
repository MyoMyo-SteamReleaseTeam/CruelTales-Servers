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
		private float _destY = 10;
		private float _pivot = 2;

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
			Y = Y + ((_destY - Y) * deltaTime * Speed);

			_moveDelay += deltaTime;
			if (_moveDelay > 5.0f)
			{
				_moveDelay = 0;
				_pivot = -_pivot;
				MoveTo(_destY + _pivot);
			}
		}
	}
}
