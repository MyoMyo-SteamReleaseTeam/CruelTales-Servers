using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CT.Tool.GetOpt
{
	/// <summary>옵션 이벤트입니다. -f의 경우 타입은 "f" 레벨은 하이픈의 개수로서 1 입니다.</summary>
	public class OptionEvent
	{
		/// <summary>옵션의 타입을 나타냅니다.</summary>
		public string Name { get; set; } = string.Empty;
		/// <summary>옵션의 레벨을 나타냅니다. 하이픈의 개수입니다.</summary>
		public int Level { get; set; }
		/// <summary>해당 옵션이 입력되면 호출될 콜백입니다.</summary>
		public Action<IList<string>>? OnOptionCallback;
	}

	public class OptionParser
	{
		private readonly List<OptionEvent> _optionEvents;

		public OptionParser()
		{
			_optionEvents = new List<OptionEvent>();
		}

		public void RegisterEvent(string name, int level, Action<IList<string>>? onOptionCallback)
		{
			if (onOptionCallback == null)
				return;

			_optionEvents.Add(new OptionEvent()
			{
				Name = name,
				Level = level,
				OnOptionCallback = onOptionCallback
			});
		}

		public void OnArguments(string arg)
		{
			OnArguments(new string[] { arg });
		}

		public void OnArguments(string[] args)
		{
			var programOption = parse(args);
		
			foreach (var e in _optionEvents)
			{
				var options = programOption.Where((p) => p.Name ==  e.Name && p.Level == e.Level);
				foreach (var op in options)
				{
					e.OnOptionCallback?.Invoke(op.Parameters);
				}
			}
		}

		public static string GetSignature(string name, int level)
		{
			StringBuilder sb = new StringBuilder(10);

			for (int i = 0; i < level; i++)
			{
				sb.Append('-');
			}
			sb.Append(name);

			return sb.ToString();
		}

		private static ICollection<ProgramOption> parse(string argument)
		{
			return parse(new string[] { argument });
		}

		private static ICollection<ProgramOption> parse(string[] inputArgs)
		{
			List<ValueTuple<string, int>> args = new();
			foreach (var segment in inputArgs)
			{
				args.AddRange(parseSegmentToArgs(segment));
			}

			List<ProgramOption> options = new();

			ProgramOption? opt = null;
			for (int i = 0; i < args.Count; i++)
			{
				var value = args[i];

				// Option parameter
				if (value.Item2 == 0)
				{
					opt?.AddParameter(value.Item1);
					continue;
				}

				if (opt != null)
				{
					options.Add(opt);
				}

				opt = new ProgramOption();

				// Option with level
				opt.SetOption(value.Item1, value.Item2);
			}

			if (opt != null)
			{
				options.Add(opt);
			}

			return options;
		}

		private static ICollection<ValueTuple<string, int>> parseSegmentToArgs(string segment)
		{
			List<ValueTuple<string, int>> result = new();

			StringBuilder sb = new StringBuilder(16);
			int level = 0;
			for (int i = 0; i < segment.Length; i++)
			{
				char c = segment[i];
				if (c == '-')
				{
					if (sb.Length != 0)
					{
						result.Add(new ValueTuple<string, int>(sb.ToString(), level));
						sb.Clear();
						level = 0;
					}
					level++;
				}
				else if (c == ' ')
				{
					if (sb.Length != 0)
					{
						result.Add(new ValueTuple<string, int>(sb.ToString(), level));
						sb.Clear();
						level = 0;
					}
				}
				else
				{
					sb.Append(c);
				}
			}
			if (sb.Length != 0)
			{
				result.Add(new ValueTuple<string, int>(sb.ToString(), level));
			}

			return result;
		}
	}
}