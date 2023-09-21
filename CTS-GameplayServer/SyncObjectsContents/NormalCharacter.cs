using CTS.Instance.Data;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using System.Numerics;

namespace CTS.Instance.SyncObjects
{
	public partial class NormalCharacter : PlayerCharacter
	{
		public override void OnCreated()
		{
			base.OnCreated();
			if (RoomSessionManager.PlayerStateTable.TryGetValue(UserId, out var state))
			{
				state.CurrentSkin = SkinSetDataDB.DEFAULT_SKIN_SET;
			}
		}

		public override void OnActionCollide(MasterNetworkObject netObj, out bool isBreak)
		{
			if (netObj is not PlayerCharacter other)
			{
				isBreak = false;
				return;
			}

			PlayerPushActionBehaviour.OnPushAction(this, other);
			isBreak = true;
		}
	}
}