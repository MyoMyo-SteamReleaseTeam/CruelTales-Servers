namespace CTS.Instance
{
	public class ServerOption
	{
		public int Port { get; set; } = 60128;
		public int FramePerMs = 66;
		public int AlarmTickMs = 40;
		public int GameCount = 1;
		public float PhysicsStepTime = 1.0f / 60.0f;

		public ServerOption() {}
	}
}
