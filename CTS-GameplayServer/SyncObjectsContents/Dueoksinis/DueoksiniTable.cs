using CT.Common.DataType.Primitives;
using CT.Common.Gameplay;
using CT.Common.Gameplay.MiniGames;
using CTS.Instance.Coroutines;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class DueoksiniTable : Interactor
	{
		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public void InitializeAs(Faction faction)
		{
			Faction = faction;
		}

		public override void OnCreated()
		{
			Interactable = true;
		}

		public override void OnInteracted(NetworkPlayer player,
										  PlayerCharacter playerCharacter)
		{
			base.OnInteracted(player, playerCharacter);
			if (player.Faction != Faction)
			{
				return;
			}
			else
			{
				FieldItemType itemType = playerCharacter.FieldItem;

				if (!DueoksiniHelper.TryGetItemInfo(itemType, out var itemInfo))
					return;

				NetInt32 typeKey = (int)itemType;
				if (!ItemCountByType.ContainsKey(typeKey))
				{
					ItemCountByType.Add(typeKey, 0);
				}

				int currentCount = ItemCountByType[typeKey];
				if (currentCount >= itemInfo.TableCount)
				{
					Server_InteractResult(InteractResultType.Failed_ItemLimit);
					return;
				}

				ItemCountByType[typeKey]++;
				playerCharacter.FieldItem = FieldItemType.None;
			}
		}
	}
}