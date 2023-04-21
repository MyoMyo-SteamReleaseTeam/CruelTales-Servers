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
}
