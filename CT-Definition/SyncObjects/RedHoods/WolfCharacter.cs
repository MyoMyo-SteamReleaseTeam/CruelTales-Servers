using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(1, true)]
	public class WolfCharacter : PlayerCharacter
	{
		[SyncVar]
		public int WolfSpeed;
	}
}