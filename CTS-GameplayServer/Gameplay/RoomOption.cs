using CT.Common.DataType;

namespace CTS.Instance.Gameplay
{
	/// <summary>게임 방의 옵션입니다.</summary>
	public class RoomOption
	{
		private InstanceInitializeOption _option;

		private int _password;
		public int Password
		{
			get => _password;
			set => _password = (value < 10000 && value >= 0) ? value : -1;
		}
		public NetStringShort Name;
		public NetStringShort Discription;
		public int MaxUser;
		public int MinUser;

		public bool HasPassword => Password >= 0;

		public RoomOption(InstanceInitializeOption option)
		{
			_option = option;
		}

		public void Reset()
		{
			_password = -1;
			Name = "CruelTales game room!";
			Discription = "This game is good for mental health!";
			MaxUser = _option.SystemMaxUser;
			MinUser = _option.SystemMinUser;
		}
	}
}
