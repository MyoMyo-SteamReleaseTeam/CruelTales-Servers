using System.Diagnostics;
using CTS.Instance.Services;

namespace CTS.Instance
{
	public class ServerService
	{
		public TickTimer ServerTimer { get; private set; }

		public ServerService()
		{
			ServerTimer = new TickTimer();
		}
	}
}