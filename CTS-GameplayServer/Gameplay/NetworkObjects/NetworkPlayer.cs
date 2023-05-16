using CT.Common.DataType;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.SyncObjects
{
	public partial class NetworkPlayer : MasterNetworkObject
	{
		private readonly static ILog _log = LogManager.GetLogger(typeof(NetworkPlayer));

		public void BindUser(UserSession userSession)
		{
			this.UserId = userSession.UserId;
			this.Username = userSession.Username;
			this.Costume = 119;
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
