using System;
using System.Numerics;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject
	{
		public float Speed = 5.0f;

		public NetworkPlayer? NetworkPlayer { get; private set; }

		public override VisibilityType Visibility => VisibilityType.View;

		public override VisibilityAuthority VisibilityAuthority => VisibilityAuthority.All;

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			this.UserId = player.UserId;
			this.Username = player.Username;
			NetworkPlayer = player;
			NetworkPlayer.BindViewTarget(this.Transform);
		}

		public partial void Client_Input(NetworkPlayer player, float x, float z)
		{
			if (_userId != player.UserId)
				return;

			Transform.Move(Transform.Position, new Vector3(x, 0, z) * Speed);
		}
	}
}
