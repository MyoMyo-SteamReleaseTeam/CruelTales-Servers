using CT.Common.Gameplay;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class DueoksiniTable : Interactor
	{
		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public override void OnInteracted(NetworkPlayer player, PlayerCharacter playerCharacter)
		{
			base.OnInteracted(player, playerCharacter);
			Interactable = true;
		}

		public void InitializeAs(Faction faction)
		{
			Faction = faction;
		}
	}
}