using System.Numerics;

namespace CT.Common.Gameplay.PlayerCharacterStates
{
	public interface IPlayerBehaviour
	{
		public void UpdateRigidStop();
		public void UpdateRigid(Vector2 moveDirection, bool isWalk);
	}
}
