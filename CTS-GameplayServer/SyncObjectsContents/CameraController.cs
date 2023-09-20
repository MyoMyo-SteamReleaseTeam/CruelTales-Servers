using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class CameraController : MasterNetworkObject
	{
		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.Owner;

		[AllowNull] public NetworkPlayer NetworkPlayer { get; private set; }
		public PlayerCharacter? TargetPlayerCharacter { get; private set; }

		public override void OnCreated()
		{
			IsPersistent = true;
		}

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			Owner = player.UserId;
			NetworkPlayer = player;
		}

		public void BindPlayerCharacter(PlayerCharacter player)
		{
			TargetPlayerCharacter = player;
			Target = player.Identity;
		}

		public void ReleasePlayerCharacter(PlayerCharacter player)
		{
			if (TargetPlayerCharacter == player)
			{
				TargetPlayerCharacter = null;
				Target = new NetworkIdentity(0);
			}
		}

		public partial void Client_CannotFindBindTarget(NetworkPlayer player)
		{
			if (TargetPlayerCharacter != null)
			{
				Server_MoveTo(TargetPlayerCharacter.Position);
			}
		}
	}
}