﻿namespace CT.Networks
{
	public static class GlobalNetwork
	{
#if DEBUG
		public const int DisconnectTimeout = 50000;//180000;
#else
		public const int DisconnectTimeout = 5000;
#endif

		public const int MTU = 1400;
		public const int CLIENT_NETWORK_TICK_PER_SEC = 30;
		public const float CLIENT_NETWORK_TICK_INTERVAL = 1.0f / CLIENT_NETWORK_TICK_PER_SEC;

		public const int SYSTEM_MAX_USER = 7;
		public const int SYSTEM_MIN_USER = 2;

		public const float DEFAULT_PHYSICS_STEP_TIME = 0.03f;
	}
}
