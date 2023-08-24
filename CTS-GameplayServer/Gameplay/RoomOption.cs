using CT.Common.DataType;

namespace CTS.Instance.Gameplay
{
	/// <summary>게임 방의 옵션입니다.</summary>
	public class RoomOption
	{
		private int _password;
		public int Password
		{
			get => _password;
			set => _password = (value < 10000 && value >= 0) ? value : -1;
		}
		public NetStringShort Name;
		public NetStringShort Discription;
		public int MaxUser;

		public bool HasPassword => Password >= 0;

		public void Reset(InstanceInitializeOption option)
		{
			_password = -1;
			Name = "CruelTales game room!";
			Discription = "This game is good for mental health!";
			MaxUser = option.SystemMaxUser;
		}
	}
}
