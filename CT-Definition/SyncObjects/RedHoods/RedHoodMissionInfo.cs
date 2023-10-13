using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(1, true)]
	public class RedHoodMissionInfo
	{
		[SyncObject]
		public SyncDictionary<NetByte, NetBool> MissionTable = new();
	}
}