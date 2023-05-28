using System.Text;
using BenchmarkDotNet.Attributes;
using log4net;

namespace CT.Benchmark
{
	[MemoryDiagnoser]
	public class LogBenchmark
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(LogBenchmark));
		private volatile string _str0 = "123414214124fafawfawfaw";
		private volatile string _str1 = "253151353151fvevevevsfefd32f2vr324vr3rv34";
		private volatile string _str2 = "656235315151secvwvf2vd2dv321vd23";
		private volatile string _str3 = "992412414211v12ev1ev21ve12ve12v41241v51356b365246v";

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

		[Benchmark]
		public void StringFormatLogAtConsole()
		{
			_log.InfoFormat(string.Format("This is {0} avev {1} avev {2} avev {3} avev {4}", nameof(LogBenchmark), _str0, _str1, _str2, _str3));
		}

		[Benchmark]
		public void LogFormatLogAtConsole()
		{
			_log.InfoFormat("This is {0} avev {1} avev {2} avev {3} avev {4}", nameof(LogBenchmark), _str0, _str1, _str2, _str3);
		}
	}
}
