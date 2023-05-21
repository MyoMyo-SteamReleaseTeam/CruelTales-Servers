using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Networks
{
	public static class GlobalNetwork
	{
#if DEBUG
		public const int DisconnectTimeout = 50000;//180000;
#else
		public const int DisconnectTimeout = 5000;
#endif

		public const int MTU = 1400;
	}
}
