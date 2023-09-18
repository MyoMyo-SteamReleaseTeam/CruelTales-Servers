using CTS.Instance.Gameplay;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class MiniGameControllerBase : SceneControllerBase
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MiniGameControllerBase));

		public override void OnCreated()
		{
			base.OnCreated();

			foreach (NetworkPlayer player in PlayerCharacterByPlayer.ForwardKeys)
			{
				player.IsMapLoaded = false;
				player.IsReady = false;
			}

			Server_TryLoadSceneAll(GameSceneIdentity);

			foreach (NetworkPlayer player in GameplayController.PlayerSet)
			{
				createPlayerBy(player);
			}
		}

		public virtual partial void Client_ReadyGame(NetworkPlayer player, bool isReady)
		{
			player.IsReady = isReady;

			if (!isAllReady())
				return;

			bool isAllReady()
			{
				foreach (var player in RoomSessionManager.PlayerStateTable.Values)
				{
					if (!player.IsReady)
						return false;
				}

				return true;
			}
		}

		public void OnGameEnd()
		{
			foreach (var pc in PlayerCharacterByPlayer.ForwardValues)
			{
				pc.Destroy();
			}
		}

		public override void OnPlayerEnter(NetworkPlayer player)
		{
			checkGameOverCondition();
		}

		public override void OnPlayerLeave(NetworkPlayer player)
		{
			if (PlayerCharacterByPlayer.TryGetValue(player, out var pc))
			{
				pc.Destroy();
				PlayerCharacterByPlayer.TryRemove(player);
			}

			checkGameOverCondition();
		}

		protected virtual void checkGameOverCondition()
		{

		}
	}
}
