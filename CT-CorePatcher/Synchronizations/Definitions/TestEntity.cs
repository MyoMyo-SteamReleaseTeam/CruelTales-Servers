using CT.Common.DataType;

namespace CT.CorePatcher.Synchronizations.Definitions
{
	[SyncDefinition]
	public partial class TestEntity
	{
		[SyncVar]
		private NetTransform _transform;

		[SyncVar]
		private int _abc = 0;

		[SyncRpc]
		public void Server_Some(int value1, float value2) { }
	}

	[SyncDefinition]
	public partial class TestUserEntity
	{
		[SyncVar(SyncType.Reliable)]
		private UserId _userId;

		[SyncVar(SyncType.Unreliable)]
		private UserSessionState _sessionState;

		[SyncVar]
		private UserToken _userToken;

		[SyncVar]
		private int _test;

		[SyncVar]
		private int _test1;

		[SyncVar]
		private int _test2;

		[SyncVar]
		private UserSessionState _test3;

		[SyncVar]
		private int _test4;

		[SyncVar]
		private int _test5;

		[SyncVar]
		private int _test9;

		[SyncVar]
		private int _test11;

		[SyncVar]
		private int _test23;

		[SyncVar]
		private int _test243;

		[SyncVar]
		private int _test2223;

		[SyncVar]
		private int _test21143;

		[SyncVar]
		private int _test242423;

		[SyncVar]
		private int _test24246623;

		[SyncVar]
		private int _test2424243;

		[SyncVar]
		private int _test24242543;

		[SyncVar]
		private int _test224242423;

		[SyncVar]
		private int _test14223;

		[SyncVar]
		private int _test142152523;

		[SyncVar]
		private int _test142151241423;

		[SyncVar]
		private int _test14242423;

		[SyncVar]
		private int _test14225223;

		[SyncVar]
		private int _test14221123;

		[SyncVar]
		private int _test14222323;

		[SyncVar]
		private int _test14244423;

		[SyncRpc]
		public void Server_Some(UserSessionState state, InteractId interactId) { }

		[SyncRpc(SyncType.Unreliable)]
		public void Server_Something() { }
	}
}
