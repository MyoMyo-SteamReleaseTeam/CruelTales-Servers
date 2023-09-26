using System.Numerics;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class EffectController
	{
		[SyncRpc]
		public void Play(EffectType effect, Vector2 position, float duration) { }

		[SyncRpc]
		public void Play3D(EffectType effect, Vector3 position, float duration) { }
	}
}