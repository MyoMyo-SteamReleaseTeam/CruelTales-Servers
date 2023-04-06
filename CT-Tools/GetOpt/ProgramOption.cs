using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CT.Tools.GetOpt
{
	public class ProgramOption : IEnumerable<string>
	{
		public string Name { get; private set; } = string.Empty;
		public int Level { get; private set; }
		public readonly List<string> Parameters = new();

		public void SetOption(string name, int optionLevel)
		{
			Name = name;
			Level = optionLevel;
		}

		public void AddParameter(string parameter)
		{
			Parameters.Add(parameter);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return Parameters.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Parameters.GetEnumerator();
		}

		public string GetSignature()
		{
			StringBuilder sb = new StringBuilder(10);

			for (int i = 0; i < Level; i++)
			{
				sb.Append('-');
			}
			sb.Append(Name);

			return sb.ToString();
		}

		public override string ToString()
		{
			return OptionParser.GetSignature(Name, Level);
		}

		public string ToString(bool showParameters)
		{
			if (showParameters)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(OptionParser.GetSignature(Name, Level));
				foreach (string parameter in Parameters)
				{
					sb.Append(' ');
					sb.Append(parameter);
				}
				return sb.ToString();
			}
			else
			{
				return this.ToString();
			}
		}
	}
}