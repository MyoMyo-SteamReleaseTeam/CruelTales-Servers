public static class Numerics
{
	public static class Numeric
	{
		// CS 기준
		public const int KiB = 1024;
		public const int MiB = 1048_576;
		public const int Gib = 1073_741_824;

		// SI 기준
		public const int kB = 1000;
		public const int MB = 1000_000;
		public const int GB = 1000_000_000;

		public const ulong INT8_MAX = 255;
		public const ulong INT16_MAX = 65535;
		public const ulong INT32_MAX = 4294967295;

		public static string PrettyBytes(long bytes)
		{
			// bytes
			if (bytes < 1024)
			{
				return $"{bytes} B";
			}
			// kilobytes
			else if (bytes < 1024L * 1024L)
			{
				return $"{bytes / 1024f:F2} KB";
			}
			// megabytes
			else if (bytes < 1024 * 1024L * 1024L)
			{
				return $"{bytes / (1024f * 1024f):F2} MB";
			}
			// gigabytes
			return $"{bytes / (1024f * 1024f * 1024f):F2} GB";
		}
	}
}
