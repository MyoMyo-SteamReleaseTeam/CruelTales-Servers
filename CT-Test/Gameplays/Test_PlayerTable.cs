using CT.Common.DataType;
using CTS.Instance.Gameplay;
using CTS.Instance.SyncObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Gameplays
{
	[TestClass]
	public class Test_PlayerTable
	{
		[TestMethod]
		public void PlayerTableTest()
		{
			NetworkPlayer p0 = new NetworkPlayer(new UserId(0));
			NetworkPlayer p1 = new NetworkPlayer(new UserId(1));
			NetworkPlayer p2 = new NetworkPlayer(new UserId(2));

			PlayerCharacterTable table = new PlayerCharacterTable(7);

			Assert.AreEqual(0, table.Count);

			table.AddPlayerByType(p0, NetworkObjectType.WolfCharacter);
			Assert.AreEqual(1, table.Count);
			Assert.AreEqual(1, table.GetCountBy(NetworkObjectType.WolfCharacter));
			Assert.AreEqual(0, table.GetCountBy(NetworkObjectType.PlayerCharacter));

			table.AddPlayerByType(p1, NetworkObjectType.PlayerCharacter);
			Assert.AreEqual(2, table.Count);
			Assert.AreEqual(1, table.GetCountBy(NetworkObjectType.WolfCharacter));
			Assert.AreEqual(1, table.GetCountBy(NetworkObjectType.PlayerCharacter));

			table.AddPlayerByType(p2, NetworkObjectType.PlayerCharacter);
			Assert.AreEqual(3, table.Count);
			Assert.AreEqual(1, table.GetCountBy(NetworkObjectType.WolfCharacter));
			Assert.AreEqual(2, table.GetCountBy(NetworkObjectType.PlayerCharacter));

			table.DeletePlayer(p2);
			Assert.AreEqual(2, table.Count);
			Assert.AreEqual(1, table.GetCountBy(NetworkObjectType.WolfCharacter));
			Assert.AreEqual(1, table.GetCountBy(NetworkObjectType.PlayerCharacter));

			table.AddPlayerByType(p0, NetworkObjectType.PlayerCharacter);
			Assert.AreEqual(2, table.Count);
			Assert.AreEqual(0, table.GetCountBy(NetworkObjectType.WolfCharacter));
			Assert.AreEqual(2, table.GetCountBy(NetworkObjectType.PlayerCharacter));

			// 멱등성 테스트
			table.AddPlayerByType(p0, NetworkObjectType.PlayerCharacter);
			Assert.AreEqual(2, table.Count);
			Assert.AreEqual(0, table.GetCountBy(NetworkObjectType.WolfCharacter));
			Assert.AreEqual(2, table.GetCountBy(NetworkObjectType.PlayerCharacter));
		}
	}
}
