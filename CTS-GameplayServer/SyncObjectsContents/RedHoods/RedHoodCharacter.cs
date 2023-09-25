using CTS.Instance.Data;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class RedHoodCharacter : PlayerCharacter
	{
		public override void OnCreated()
		{
			base.OnCreated();
			if (RoomSessionManager.PlayerStateTable.TryGetValue(UserId, out var state))
			{
				var curSkin = state.SelectedSkin;
				curSkin.OverrideSet(SkinSetDataDB.REDHOOD_SKIN_SET);
				state.CurrentSkin = curSkin;
			}
		}

		public override void OnActionCollide(MasterNetworkObject netObj, out bool isBreak)
		{
			if (netObj is not PlayerCharacter other)
			{
				isBreak = false;
				return;
			}

			if (other is NormalCharacter ||
				other is WolfCharacter)
			{
				isBreak = false;
			}
			else
			{
				PlayerPushActionBehaviour.OnPushAction(this, other);
				isBreak = true;
			}
		}
	}
}