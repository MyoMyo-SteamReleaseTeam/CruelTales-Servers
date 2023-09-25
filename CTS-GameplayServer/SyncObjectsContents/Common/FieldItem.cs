using CT.Common.Gameplay;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public partial class FieldItem : Interactor
	{
		public void InitializeAs(FieldItemType itemType)
		{
			ItemType = itemType;
			Interactable = true;
		}

		public override void OnInteracted(NetworkPlayer player, PlayerCharacter playerCharacter)
		{
			if (!IsAlive)
				return;

			if (playerCharacter.FieldItem == FieldItemType.None)
			{
				playerCharacter.FieldItem = ItemType;
				Destroy();
			}
			else
			{
				Server_InteractResult(InteractResultType.Failed_YouAlreadyHaveItem);
			}
		}
	}
}