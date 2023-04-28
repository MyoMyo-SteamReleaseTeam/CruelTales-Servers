using CT.Common.DataType;
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	public partial class TestRemoteNetworkObject : RemoteNetworkObject
	{
		[SyncVar]
		public UserToken UserToken => _userToken;
		[SyncVar]
		public int IntValue => _intValue;

		public float FloatParam;
		public string TextParam = string.Empty;

		public partial void Server_SendValue(float floatParam)
		{
			FloatParam = floatParam;
		}

		public partial void Server_SendMessage(NetStringShort textParam)
		{
			TextParam = textParam;
		}
	}
}
