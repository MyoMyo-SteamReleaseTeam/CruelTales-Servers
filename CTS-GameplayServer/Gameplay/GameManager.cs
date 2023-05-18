using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Tools.Collections;
using CTS.Instance.Networks;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameManager : IUpdatable
	{
		// Log
		private readonly static ILog _log = LogManager.GetLogger(typeof(GameManager));

		// Reference
		private GameplayInstance _gameplayInstance;
		private WorldManager _worldManager;

		private BidirectionalMap<UserId, NetworkPlayer> _networkPlayerByUserId;

		public GameManager(GameplayInstance gameplayInstance,
						   InstanceInitializeOption option)
		{
			_gameplayInstance = gameplayInstance;
			_worldManager = gameplayInstance.WorldManager;
			_networkPlayerByUserId = new(option.SystemMaxUser);
		}

		public void Update(float deltaTime)
		{
		}

		public void OnUserEnterGame(UserSession userSession)
		{
			var player = _worldManager.CreatePlayerVisibleTable(userSession);
			_networkPlayerByUserId.Add(userSession.UserId, player);
		}

		public void OnUserLeaveGame(UserSession userSession)
		{
			if (!_networkPlayerByUserId.TryRemove(userSession.UserId))
			{
				_log.Error($"[{_gameplayInstance}] There is no {userSession}'s network player!");
				return;
			}
			_worldManager.DestroyNetworkPlayer(userSession);
			_log.Info($"[{_gameplayInstance}] Session {userSession} leave the game");
		}
	}
}
