using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType.Primitives;
using CT.Common.Gameplay;
using CT.Common.Gameplay.MiniGames;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class DueoksiniTable : Interactor
	{
		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		[AllowNull] private MiniGameControllerBase? _miniGameController;

		public void InitializeAs(Faction faction)
		{
			Team = faction;
		}

		public void BindController(MiniGameControllerBase controller)
		{
			_miniGameController = controller;
		}

		public override void OnCreated()
		{
			Interactable = true;
		}

		public override void OnInteracted(NetworkPlayer player,
										  PlayerCharacter playerCharacter)
		{
			base.OnInteracted(player, playerCharacter);
			if (player.Faction != Team)
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
				onScoreAdd(itemInfo.Score);
			}
		}

		private void onScoreAdd(int score)
		{
			NetByte factionKey = (byte)Team;
			if (_miniGameController == null)
				return;

			var table = _miniGameController.TeamScoreByFaction;

			if (!table.ContainsKey(factionKey))
			{
				table.Add(factionKey, 0);
			}

			table[factionKey] += (short)score;
		}
	}
}