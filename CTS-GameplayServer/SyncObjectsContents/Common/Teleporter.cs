using System.Numerics;
using CT.Common.Gameplay;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class Teleporter : Interactor
	{
		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public Vector2 Destination;
		public byte SectionTo;

		public override void Initialize(InteractorInfo info)
		{
			base.Initialize(info);
			Destination = info.Destination;
			SectionTo = info.SectionTo;
			TeleporterShape = info.TeleporterShape;
		}

		public override void OnInteracted(NetworkPlayer player,
										  PlayerCharacter playerCharacter)
		{
			player.CameraController?.Server_MoveTo(Destination);
			playerCharacter.RigidBody.MoveTo(Destination);
			playerCharacter.Section = SectionTo;
		}
	}
}