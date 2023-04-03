using System.Diagnostics;
using CTS.Instance.Services;

namespace CTS.Instance
{
	public class ServerServices
	{
		public NetworkService NetworkService { get; private set; }
		public TickTimer ServerTimer { get; private set; }

		public ServerServices(ServerOption serverOption)
		{
			ServerTimer = new TickTimer();
			NetworkService = new NetworkService(serverOption);
		}
	}
}