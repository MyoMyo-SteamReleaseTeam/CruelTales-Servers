using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CT.Tools.GetOpt
{
	public class StringArgument
	{
		public string Argument = string.Empty;

		public bool HasArgument(ref bool checker)
		{
			bool result = !string.IsNullOrEmpty(Argument);
			checker &= result;
			return result;
		}
	}

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
			var programOption = Parse(args);
		
			foreach (var e in _optionEvents)
			{
				var options = programOption.Where((p) => p.Name ==  e.Name && p.Level == e.Level);
				foreach (var op in options)
				{
					e.OnOptionCallback?.Invoke(op.Parameters);
				}
			}
		}

		public static void BindArgument(OptionParser parser, string argumentName,
								 int level, StringArgument value)
		{
			parser.RegisterEvent(argumentName, level, (options) =>
			{
				try
				{
					value.Argument = options[0];
				}
				catch
				{
					throw new NoProcessArgumentsException(argumentName);
				}
			});
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

		public static ICollection<ProgramOption> Parse(string argument)
		{
			return Parse(new string[] { argument });
		}

		public static ICollection<ProgramOption> Parse(string[] inputArgs)
		{
			List<ProgramOption> result = new();
			ProgramOption? op = null;

			List<string> arguments = new List<string>();
			foreach (var s in inputArgs)
			{
				arguments.AddRange(s.Split(' '));
			}

			for (int i = 0; i < arguments.Count; i++)
			{
				string arg = arguments[i].Trim();
				if (string.IsNullOrEmpty(arg))
				{
					continue;
				}

				if (arg[0] == '-')
				{
					if (op != null)
					{
						result.Add(op);
					}

					int level = 0;
					foreach (char c in arg)
					{
						if (c == '-')
							level++;
						else
							break;
					}

					string name = arg.Substring(level);
					op = new ProgramOption();
					op.SetOption(name, level);
					continue;
				}

				if (op != null)
				{
					op.AddParameter(arg);
				}
			}

			if (op != null)
			{
				result.Add(op);
			}
			
			return result;
		}
	}
}