using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class SoundController
	{
		[SyncRpc]
		public void Play() { }
	}
}