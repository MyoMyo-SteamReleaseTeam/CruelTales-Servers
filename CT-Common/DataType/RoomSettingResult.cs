using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Common.DataType
{
	public enum RoomSettingResult : byte
	{
		None = 0,

		/// <summary>시스템 한계보다 더 적은 최소 유저 설정 불가</summary>
		MinimumUsersRequired,

		/// <summary>시스템 한계보다 더 많은 최대 유저 설정 불가</summary>
		MaximumUsersReached,

		/// <summary>현재 접속중인 유저보다 더 적은 최대 유저를 설정 불가</summary>
		CannotSetMaxUserUnderConnections,
	}
}
