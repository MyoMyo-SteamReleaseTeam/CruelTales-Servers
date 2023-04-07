using CT.Network.DataType;
using LiteNetLib;

namespace CT.Network.Extensions
{
	public static class LiteNetLibExtension
	{
		public static DisconnectReasonType ConvertEnum(DisconnectReason disconnectReason)
		{
			return (DisconnectReasonType)((int)disconnectReason + 1);
		}
	}
}