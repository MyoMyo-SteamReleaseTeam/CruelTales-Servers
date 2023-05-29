#if !DEBUG
using CT.Common.Tools.Data;
#endif

namespace CTS.Instance
{
	public struct ServerOption
	{
		public int Port { get; set; } = 60128;
		public int FramePerMs = 66;
		//public int FramePerMs = 1000;
		public int AlarmTickMs = 40;
		public int GameCount = 1;

		public ServerOption() {}
	}
}
