using System;
using System.Collections.Generic;
using System.Diagnostics;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class PlayerCharacterTable
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(PlayerCharacterTable));

		private readonly Dictionary<NetworkPlayer, NetworkObjectType> _playerByType;
		private readonly Dictionary<NetworkObjectType, HashSet<NetworkPlayer>> _playerSetTable;

		public int Count { get; private set; }

		public PlayerCharacterTable(int systemMaxUser)
		{
			_playerByType = new(systemMaxUser);
			_playerSetTable = new()
			{
				{ NetworkObjectType.PlayerCharacter, new(systemMaxUser) },
				{ NetworkObjectType.WolfCharacter, new(systemMaxUser) },
				{ NetworkObjectType.NormalCharacter, new(systemMaxUser) },
				{ NetworkObjectType.RedHoodCharacter, new(systemMaxUser) },
			};
		}

		public void Clear()
		{
			_playerByType.Clear();
			foreach (var playerSet in _playerSetTable.Values)
			{
				playerSet.Clear();
			}
			Count = 0;
		}

		public int GetCountBy(NetworkObjectType type)
		{
			if (_playerSetTable.TryGetValue(type, out var set))
			{
				return set.Count;
			}

			return 0;
		}

		public HashSet<NetworkPlayer> GetPlayerSetBy(NetworkObjectType type)
		{
			if (_playerSetTable.TryGetValue(type, out var set))
			{
				return set;
			}

			throw new ArgumentException($"There is no such player set as {type}");
		}

		public void AddPlayerByType(NetworkPlayer player, NetworkObjectType objectType)
		{
			if (_playerByType.TryGetValue(player, out NetworkObjectType type))
			{
				if (type == objectType)
					return;

				if (_playerSetTable.TryGetValue(type, out var removeSet))
				{
					removeSet.Remove(player);
				}
				else
				{
					_log.Fatal($"There is no player set. Type : {type}");
					Debug.Assert(false);
					return;
				}

				_playerByType[player] = objectType;
			}
			else
			{
				_playerByType.Add(player, objectType);
				Count++;
			}

			if (_playerSetTable.TryGetValue(objectType, out var playerSet))
			{
				playerSet.Add(player);
			}
			else
			{
				_log.Fatal($"There is no player set. Type : {objectType}");
				Debug.Assert(false);
				return;
			}
		}

		public void DeletePlayer(NetworkPlayer player)
		{
			if (_playerByType.TryGetValue(player, out NetworkObjectType type))
			{
				if (_playerSetTable.TryGetValue(type, out var playerSet))
				{
					playerSet.Remove(player);
				}
				else
				{
					_log.Fatal($"There is no player set. Type : {type}");
					Debug.Assert(false);
					return;
				}

				_playerByType.Remove(player);
				Count--;
			}
		}
	}
}
