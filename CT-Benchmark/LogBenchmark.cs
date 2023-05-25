using System.Text;
using BenchmarkDotNet.Attributes;
using log4net;

namespace CT.Benchmark
{
	[MemoryDiagnoser]
	public class LogBenchmark
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(LogBenchmark));
		private string _str0 = "123414214124";
		private string _str1 = "253151353151";
		private string _str2 = "656235315151";
		private string _str3 = "992412414211";

		[Benchmark]
		public void StaticLogAtConsole()
		{
			_log.Info("This is log. This is log. This is log. This is log. This is log. This is log. This is log. This is log. ");
		}

		StringBuilder sb = new StringBuilder(1024);

		[Benchmark]
		public void StringBuilderLogAtConsole()
		{
			sb.Clear();
			sb.Append("This is ");
			sb.Append(nameof(LogBenchmark));
			sb.Append(" avev ");
			sb.Append(_str0);
			sb.Append(" avev ");
			sb.Append(_str1);
			sb.Append(" avev ");
			sb.Append(_str2);
			sb.Append(" avev ");
			sb.Append(_str3);
			_log.Info(sb);
		}

		[Benchmark]
		public void InterpolateLogAtConsole()
		{
			_log.Info($"This is {nameof(LogBenchmark)} avev {_str0} avev {_str1} avev {_str2} avev {_str3}");
		}
	}
}
