using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Networks.Synchronizations;

namespace CT.CorePatcher.Synchronizations.Definitions
{
	public class ABC : NetworkObject
	{
		[SyncObject]
		SyncDataTest _data = new();

		public override NetworkObjectType Type => throw new System.NotImplementedException();

		public override void SerializeEveryProperty(PacketWriter writer)
		{
			_data.SerializeEveryProperty(writer);
		}

		public override void SerializeSyncReliable(PacketWriter writer)
		{
		}

		public override void SerializeSyncUnreliable(PacketWriter writer)
		{
		}
	}

	public partial class SyncDataTest : IMasterSynchronizable
	{
		public bool IsDirty => throw new System.NotImplementedException();

		public void ClearDirtyReliable()
		{
			throw new System.NotImplementedException();
		}

		public void ClearDirtyUnreliable()
		{
			throw new System.NotImplementedException();
		}

		public void SerializeEveryProperty(PacketWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public void SerializeSyncReliable(PacketWriter writer)
		{
			throw new System.NotImplementedException();
		}

		public void SerializeSyncUnreliable(PacketWriter writer)
		{
			throw new System.NotImplementedException();
		}
	}

	[SyncNetworkObjectDefinition]
	public partial class TestUserEntityWithObject
	{
		[SyncVar]
		private UserToken _userToken;

		[SyncObject]
		private TestEntity _TEST_ENTITY = new();

		[SyncObject]
		private TestEntity _TEST_ENTITY2 = new();

		[SyncObject]
		private TestEntity _TEST_ENTITY3 = new();

		[SyncObject(SyncType.Unreliable)]
		private TestEntity _TEST_ENTITY4 = new();

		[SyncObject]
		private TestEntity _TEST_ENTITY5 = new();

		[SyncObject]
		private TestEntity _TEST_ENTITY6 = new();

		[SyncObject(SyncType.Unreliable)]
		private TestEntity _TEST_ENTITY7 = new();

		[SyncObject]
		private TestEntity _TEST_ENTITY8 = new();

		[SyncObject]
		private TestEntity _TEST_ENTITY9 = new();
	}

	[SyncObjectDefinition]
	public partial class TestEntity
	{
		[SyncVar]
		private NetTransform _transform;

		[SyncVar]
		private int _abc = 0;

		[SyncRpc]
		public void Server_Some(int value1, float value2) { }

		public void SerializeEveryProperty(PacketWriter writer)
		{
			_transform.Serialize(writer);
			writer.Put(_abc);
		}

		public void DeserializeEveryProperty(PacketReader reader)
		{
			_transform.Deserialize(reader);
			_abc = reader.ReadInt32();
		}
	}

	//[SyncNetworkObjectDefinition]
	//public partial class TestUserEntity
	//{
	//	[SyncVar(SyncType.Reliable)]
	//	private UserId _userId;

	//	[SyncVar(SyncType.Unreliable)]
	//	private UserSessionState _sessionState;

	//	[SyncVar]
	//	private UserToken _userToken;

	//	[SyncObject]
	//	private TestEntity _TEST_ENTITY = new();

	//	[SyncVar]
	//	private int _test1;

	//	[SyncVar]
	//	private UserSessionState _test3;

	//	[SyncVar]
	//	private int _test23;

	//	[SyncVar(SyncType.Unreliable)]
	//	private int _test243;

	//	[SyncRpc]
	//	public void Server_Some(UserSessionState state, InteractId interactId) { }

	//	[SyncRpc(SyncType.Unreliable)]
	//	public void Server_Something() { }
	//}
}
