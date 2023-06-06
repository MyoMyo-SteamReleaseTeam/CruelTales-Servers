using System;
using CT.Common.DataType;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject
	{
		public NetworkPlayer? NetworkPlayer { get; private set; }

		public override VisibilityType Visibility => VisibilityType.View;

		public override VisibilityAuthority VisibilityAuthority => VisibilityAuthority.All;

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			this.UserId = player.UserId;
			this.Username = player.Username;
			NetworkPlayer = player;
			NetworkPlayer.OnViewTargetChanged(this.Transform);
		}

		public partial void Client_Input(NetworkPlayer player, float x, float z)
		{
			Console.WriteLine($"Client input from {player}: ({x}, {z})");
		}
	}
}
