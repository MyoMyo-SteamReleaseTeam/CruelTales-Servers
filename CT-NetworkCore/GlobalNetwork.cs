namespace CT.Networks
{
	public static class GlobalNetwork
	{
#if DEBUG
		public const int DisconnectTimeout = 50000;//180000;
#else
		public const int DisconnectTimeout = 50000;
#endif

		public const int MTU = 1400;
		public const int CLIENT_NETWORK_TICK_PER_SEC = 30;
		public const float CLIENT_NETWORK_TICK_INTERVAL = 1.0f / CLIENT_NETWORK_TICK_PER_SEC;

		public const int SYSTEM_MAX_USER = 7;
		public const int SYSTEM_MIN_USER = 1;

		public const float DEFAULT_PHYSICS_STEP_TIME = 0.03f;

		/// <summary>System Area Info의 Index 한계입니다.</summary>
		public const int SYSTEM_AREA_INDEX_LIMIT = 30;

		/// <summary>System Pivot Info의 Index 한계입니다.</summary>
		public const int SYSTEM_PIVOT_INDEX_LIMIT = 30;
	}
}
