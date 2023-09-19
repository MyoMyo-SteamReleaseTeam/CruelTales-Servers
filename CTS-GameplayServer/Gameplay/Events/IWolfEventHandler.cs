using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay.Events
{
	public interface IWolfEventHandler
	{
		public void OnWolfCatch(WolfCharacter wolf, RedHoodCharacter target);
	}
}
