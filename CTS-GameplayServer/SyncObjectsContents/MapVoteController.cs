using System;
using CT.Common.DataType;
using CT.Common.DataType.Primitives;
using CT.Common.Gameplay;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public struct Vote : IComparable<Vote>
	{
		public int Index;
		public int Votes;

		public Vote(int index, int votes)
		{
			Index = index;
			Votes = votes;
		}

		public int CompareTo(Vote other)
		{
			return Votes.CompareTo(other.Votes);
		}
	}

	public partial class MapVoteController
	{
		private MiniGameControllerBase _miniGameController;

		public void Initialize(MiniGameControllerBase miniGameControllerBase)
		{
			_miniGameController = miniGameControllerBase;
		}

		public void OnPlayerLeave(NetworkPlayer player)
		{
			UserId userId = player.UserId;
			if (MapIndexByUserId.ContainsKey(userId))
			{
				MapIndexByUserId.Remove(userId);
			}
		}

		public partial void Client_VoteMap(NetworkPlayer player, int mapIndex)
		{
			if (_miniGameController.GameplayState != GameplayState.VoteMap)
			{
				return;
			}

			UserId userId = player.UserId;

			if (MapIndexByUserId.ContainsKey(userId))
			{
				MapIndexByUserId[userId] = mapIndex;
			}
			else
			{
				MapIndexByUserId.Add(userId, mapIndex);
			}
		}

		public void SetNextMapVotes(Span<GameSceneIdentity> nextMaps)
		{
			NextMapVoteList.Clear();
			MapIndexByUserId.Clear();
			for (int i = 0; i < nextMaps.Length; i++)
			{
				var nextMap = nextMaps[i];
				NextMapVoteList.Add(nextMap);
			}
		}

		public void OnMapVoteEnd()
		{
			if (MapIndexByUserId.Count == 0)
			{
				selectRandomIndex();
				return;
			}

			int mapCount = NextMapVoteList.Count;
			Span<Vote> votes = stackalloc Vote[mapCount];

			for (int i = 0; i < mapCount; i++)
			{
				votes[i] = new Vote(i, 0);
			}

			foreach (NetInt32 voteIndex in MapIndexByUserId.Values)
			{
				votes[voteIndex.Value].Votes++;
			}

			votes.Sort();

			int mostVotes = votes[mapCount - 1].Votes;
			if (mostVotes == 0)
			{
				selectRandomIndex();
				return;
			}

			Span<Vote> sameVotes = stackalloc Vote[mapCount];
			int sameVoteCount = 0;
			for (int i = votes.Length - 1; i >= 0; i--)
			{
				if (mostVotes == votes[i].Votes)
				{
					sameVotes[sameVoteCount] = votes[i];
					sameVoteCount++;
					continue;
				}

				break;
			}

			int nextMapIndex = RandomHelper.NextInt(sameVoteCount);
			Vote nextMap = sameVotes[nextMapIndex];
			NextMap = NextMapVoteList[nextMap.Index];

			void selectRandomIndex()
			{
				int nextMap = RandomHelper.NextInt(NextMapVoteList.Count);
				NextMap = NextMapVoteList[nextMap];
			}
		}
	}
}
