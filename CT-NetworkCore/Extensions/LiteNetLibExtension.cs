using CT.Common.DataType;
using LiteNetLib;

namespace CT.Networks.Extensions
{
	public static class LiteNetLibExtension
	{
		public static DisconnectReasonType ConvertEnum(DisconnectReason disconnectReason)
		{
			return (DisconnectReasonType)((int)disconnectReason + 1);
		}
	}
}