using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class NetworkPlayer
	{
		// Log
		private readonly static ILog _log = LogManager.GetLogger(typeof(NetworkPlayer));

		// Referenece
		[AllowNull] public WorldManager WorldManager { get; private set; }
		[AllowNull] public PlayerVisibleTable VisibleTable { get; private set; }

		// Session Info
		public UserId UserId { get; private set; }
		public NetStringShort Username { get; private set; }
		public int Costume { get; private set; }

		// Visibility
		public VisibilityAuthority VisibilityAuthority { get; private set; }

		public void Initialize(WorldManager worldManager,
							   UserSession userSession,
							   PlayerVisibleTable visibleTable)
		{
			WorldManager = worldManager;
			UserId = userSession.UserId;
			Username = userSession.Username;
			Costume = 119;
			VisibleTable = visibleTable;
		}

		public void RemoveUserSession()
		{
			UserId = new UserId(0);
			Username = string.Empty;
			Costume = 0;
			VisibleTable = null;
		}

		public void OnCreated()
		{
			_log.Debug($"Player {Username} network object created!");
		}

		public void OnDestroyed()
		{
			_log.Debug($"Player {Username} network object destroyed!");
		}

		public void OnUpdate(float deltaTime)
		{

		}
	}
}
