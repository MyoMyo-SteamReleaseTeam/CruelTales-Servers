using System.Numerics;
using CT.Common.Gameplay.Infos;
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

		public override void Initialize(in InteractorInfo info)
		{
			base.Initialize(info);
			Destination = info.Destination;
			SectionTo = info.SectionTo;
			TeleporterShape = info.TeleporterShape;
			Interactable = true;
		}

		public override void OnInteracted(NetworkPlayer player,
										  PlayerCharacter playerCharacter)
		{
			playerCharacter.RigidBody.MoveTo(Destination);
			player.CameraController?.MoveToTarget();
			playerCharacter.Section = SectionTo;
		}
	}
}