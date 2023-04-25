using CT.Common.DataType;
using CTC.Networks.Synchronizations;
using log4net;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	public partial class Test_MovingCube : RemoteNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(Test_MovingCube));

		public float R => _r;
		public float G => _r;
		public float B => _r;

		public partial void Server_MoveTo(NetVec2 _destination)
		{
			//_log.Info(_destination);
		}
	}
}