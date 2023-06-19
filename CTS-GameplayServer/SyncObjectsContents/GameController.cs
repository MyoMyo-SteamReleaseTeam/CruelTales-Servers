using CT.Common.Gameplay;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class GameController : MasterNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameController));

		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public partial void Client_ReadyToSync(NetworkPlayer player)
		{
			_log.Debug($"Client {player} ready to controll");
			Server_LoadGame(player, GameMapType.MiniGame_RedHood_0);
		}

		public partial void Client_OnMapLoaded(NetworkPlayer player)
		{
			player.CanSeeViewObject = true;
		}
	}
}
