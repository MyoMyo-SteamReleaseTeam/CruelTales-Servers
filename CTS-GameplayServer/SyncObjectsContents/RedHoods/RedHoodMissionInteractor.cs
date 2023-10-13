using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class RedHoodMissionInteractor : Interactor
	{
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.Users;

		public override void OnInteracted(NetworkPlayer player, PlayerCharacter playerCharacter)
		{
			base.OnInteracted(player, playerCharacter);
			RemoveVisibleUser(player.UserId);
		}

		public override void OnCreated()
		{
			base.OnCreated();
			Interactable = true;
		}
	}
}