using CT.Common.Synchronizations;

namespace CT.Common.DataType.Synchronizations
{
	public enum CollectionSyncType : byte
	{
		None = 0,
		Clear,
		Add,
		Remove,
		Change,
		Insert,
	}
}
