using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CTS.Instance.Gameplay;
using CTS.Instance.SyncObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Gameplays
{
	[TestClass]
	public class Test_MapVoteController
	{
		public Span<GameSceneIdentity> GetTestMaps()
		{
			GameSceneIdentity[] testMaps = new GameSceneIdentity[4];
			testMaps[0] = new GameSceneIdentity()
			{
				Map = GameMapType.RedHood_0,
				Mode = GameModeType.RedHood,
				Theme = GameMapThemeType.EastAsia_Korea
			};
			testMaps[1] = new GameSceneIdentity()
			{
				Map = GameMapType.RedHood_1,
				Mode = GameModeType.RedHood,
				Theme = GameMapThemeType.Europe_France
			};
			testMaps[2] = new GameSceneIdentity()
			{
				Map = GameMapType.RedHood_2,
				Mode = GameModeType.Dueoksini,
				Theme = GameMapThemeType.EastAsia_Korea
			};
			testMaps[3] = new GameSceneIdentity()
			{
				Map = GameMapType.Dueoksini_0,
				Mode = GameModeType.Dueoksini,
				Theme = GameMapThemeType.EastAsia_Korea
			};

			return testMaps;
		}

		[TestMethod]
		public void SameVotesTest()
		{
			MiniGameControllerBase miniGame = new();
			miniGame.GameplayState = GameplayState.VoteMap;
			MokupDirtyable owner = new();
			MapVoteController voteController = new(owner);
			voteController.Initialize(miniGame);

			List<NetworkPlayer> players = new List<NetworkPlayer>();
			for (ulong i = 0; i < 6; i++)
			{
				players.Add(new NetworkPlayer(new UserId(i)));
			}

			var maps = GetTestMaps();

			voteController.SetNextMapVotes(maps);
			voteController.Client_VoteMap(players[0], 0);
			voteController.Client_VoteMap(players[1], 0);
			voteController.Client_VoteMap(players[2], 1);
			voteController.Client_VoteMap(players[3], 1);
			voteController.Client_VoteMap(players[4], 2);
			voteController.Client_VoteMap(players[5], 3);
			voteController.OnMapVoteEnd();

			var selected = voteController.NextMap;
			Assert.IsTrue(selected == maps[0] || selected == maps[1]);
		}

		[TestMethod]
		public void SameVotesTest2()
		{
			MiniGameControllerBase miniGame = new();
			miniGame.GameplayState = GameplayState.VoteMap;
			MokupDirtyable owner = new();
			MapVoteController voteController = new(owner);
			voteController.Initialize(miniGame);

			List<NetworkPlayer> players = new List<NetworkPlayer>();
			for (ulong i = 0; i < 6; i++)
			{
				players.Add(new NetworkPlayer(new UserId(i)));
			}

			var maps = GetTestMaps();

			voteController.SetNextMapVotes(maps);
			voteController.Client_VoteMap(players[0], 2);
			voteController.Client_VoteMap(players[1], 0);
			voteController.Client_VoteMap(players[2], 1);
			voteController.Client_VoteMap(players[3], 3);
			voteController.Client_VoteMap(players[4], 2);
			voteController.Client_VoteMap(players[5], 3);
			voteController.OnMapVoteEnd();

			var selected = voteController.NextMap;
			Assert.IsTrue(selected == maps[2] || selected == maps[3]);
		}

		[TestMethod]
		public void MostVotesTest()
		{
			MiniGameControllerBase miniGame = new();
			miniGame.GameplayState = GameplayState.VoteMap;
			MokupDirtyable owner = new();
			MapVoteController voteController = new(owner);
			voteController.Initialize(miniGame);

			List<NetworkPlayer> players = new List<NetworkPlayer>();
			for (ulong i = 0; i < 6; i++)
			{
				players.Add(new NetworkPlayer(new UserId(i)));
			}

			var maps = GetTestMaps();

			voteController.SetNextMapVotes(maps);
			voteController.Client_VoteMap(players[0], 2);
			voteController.Client_VoteMap(players[1], 0);
			voteController.Client_VoteMap(players[2], 3);
			voteController.Client_VoteMap(players[3], 3);
			voteController.Client_VoteMap(players[4], 2);
			voteController.Client_VoteMap(players[5], 3);
			voteController.OnMapVoteEnd();

			var selected = voteController.NextMap;
			Assert.IsTrue(selected == maps[3]);
		}
	}
}
