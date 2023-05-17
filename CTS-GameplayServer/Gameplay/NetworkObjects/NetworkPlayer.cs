using CT.Common.DataType;
using CTS.Instance.Gameplay;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class NetworkPlayer : MasterNetworkObject
	{
		private readonly static ILog _log = LogManager.GetLogger(typeof(NetworkPlayer));
		public PlayerVisibleTable? VisibleTable { get; private set; }

		public void BindUserSession(UserSession userSession, PlayerVisibleTable visibleTable)
		{
			this.UserId = userSession.UserId;
			this.Username = userSession.Username;
			this.Costume = 119;
			this.VisibleTable = visibleTable;
		}

		public void RemoveUserSession()
		{
			this.UserId = new UserId(0);
			this.Username = string.Empty;
			this.Costume = 0;
			this.VisibleTable = null;
		}

		public override void OnCreated()
		{
			_log.Debug($"Player {Username} network object created!");
		}

		public override void OnDestroyed()
		{
			_log.Debug($"Player {Username} network object destroyed!");
		}

		public override void OnUpdate(float deltaTime)
		{

		}
	}
}
