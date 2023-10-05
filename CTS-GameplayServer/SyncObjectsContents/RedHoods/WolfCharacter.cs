using CTS.Instance.Data;
using CTS.Instance.Gameplay;
using CTS.Instance.Gameplay.Events;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class WolfCharacter : PlayerCharacter
	{
		public override void OnCreated()
		{
			base.OnCreated();
			if (RoomSessionManager.PlayerStateTable.TryGetValue(UserId, out var state))
			{
				var curSkin = state.SelectedSkin;
				curSkin.OverrideSet(SkinSetDataDB.WOLF_SKIN_SET);
				state.CurrentSkin = curSkin;
			}
		}

		public override void OnActionCollide(MasterNetworkObject netObj, out bool isBreak)
		{
			if (netObj is not PlayerCharacter other ||
				other is NormalCharacter)
			{
				isBreak = false;
				return;
			}

			if (other is RedHoodCharacter redHood)
			{
				var curScene = GameplayManager.GameplayController.SceneController;
				var eventHandler = curScene as IWolfEventHandler;
				eventHandler?.OnWolfCatch(this, redHood);
				isBreak = true;
			}
			else
			{
				PlayerPushActionBehaviour.OnPushAction(this, other);
				isBreak = true;
			}
		}

		public override void LoadDefaultPlayerSkin()
		{
			BroadcastOrderTest((int)UserId.Id, 1);
		}
	}
}