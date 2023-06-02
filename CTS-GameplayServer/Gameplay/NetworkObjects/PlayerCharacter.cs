using System;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject
	{
		public override VisibilityType Visibility => VisibilityType.View;

		public override VisibilityAuthority VisibilityAuthority => VisibilityAuthority.All;

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			player.OnViewTargetChanged(this.Transform);
		}

		public partial void Client_Input(float x, float z)
		{
			Console.WriteLine($"Client input ({x}, {z})");
		}
	}
}
