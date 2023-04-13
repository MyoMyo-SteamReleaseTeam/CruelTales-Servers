using System;

namespace CT.Network.Runtimes
{
	public class ClientInputJobQueue : JobQueue<ClientInputJob>
	{
		public ClientInputJobQueue(TickTimer tickTimer,
								   Action<ClientInputJob> onJobExecute)
			: base(tickTimer, onJobExecute)
		{
		}
	}
}
