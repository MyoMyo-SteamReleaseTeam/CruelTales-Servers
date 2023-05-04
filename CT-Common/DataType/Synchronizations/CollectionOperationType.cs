using CT.Common.Synchronizations;

namespace CT.Common.DataType.Synchronizations
{
	public enum CollectionOperationType : byte
	{
		None = 0,
		Clear,
		Add,
		Remove,
		Change,
		Insert,
	}
}
