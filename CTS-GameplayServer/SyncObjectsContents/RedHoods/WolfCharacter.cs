using CT.Common.Gameplay.Players;
using CTS.Instance.ClientShared;
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
				var curSkin = state.SelectedCostume.GetSkinSet();
				curSkin.OverrideSet(SkinSetDataDB.WOLF_SKIN_SET);
				state.CurrentCostume.SetBy(curSkin);
			}
		}

		public override void OnFeverTime()
		{
			base.OnFeverTime();
			Status.ActionDuration *= 0.5f;
			Status.ActionRadius *= 1.5f;
			Status.MoveSpeed *= 1.5f;
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