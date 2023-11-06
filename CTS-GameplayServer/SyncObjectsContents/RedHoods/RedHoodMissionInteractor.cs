using CT.Common.DataType.Primitives;
using CTS.Instance.Gameplay;
using CTS.Instance.Gameplay.Events;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class RedHoodMissionInteractor : Interactor
	{
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.Users;
		private IMissionTableHandler _missionTableHandler;

		public override void OnInteracted(NetworkPlayer player, PlayerCharacter playerCharacter)
		{
			base.OnInteracted(player, playerCharacter);
			RemoveVisibleUser(player.UserId);
			_missionTableHandler.OnInteracted(player, playerCharacter, this);
		}

		public override void OnCreated()
		{
			base.OnCreated();
			Interactable = true;
		}

		public void BindHandler(IMissionTableHandler handler)
		{
			_missionTableHandler = handler;
		}

		public void OnCharacterCreated(NetworkPlayer player, PlayerCharacter character)
		{
			if (!_missionTableHandler.TryGetMissionTable(player, out var missionTable))
				return;

			NetByte missionKey = (byte)Mission;
			if (!missionTable.ContainsKey(missionKey))
				return;

			bool isClear = missionTable[missionKey];
			if (!isClear)
				return;

			switch (character)
			{
				case WolfCharacter wolf:
					RemoveVisibleUser(player.UserId);
					break;

				case RedHoodCharacter redHood:
				case NormalCharacter normal:
					AddVisibleUser(player.UserId);
					break;
			}
		}

		public override bool CanInteract(NetworkPlayer player, PlayerCharacter playerCharacter)
		{
			return playerCharacter is not WolfCharacter;
		}
	}
}