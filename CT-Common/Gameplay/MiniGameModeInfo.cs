using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Common.Gameplay
{
	public class MiniGameModeInfo
	{
		public MiniGameModeType MiniGameMode { get; set; }
		public MiniGameMapType[] MapTypes { get; set; }
		public int MaxPlayer { get; set; }
		public int MinPlayer { get; set; }
		public int MiniGameTime { get; set; }
		public int PortalCoolTime { get; set; }
		public int MaxFeverTime { get; set; }
		public int MinFeverTime { get; set; }
	}
}
