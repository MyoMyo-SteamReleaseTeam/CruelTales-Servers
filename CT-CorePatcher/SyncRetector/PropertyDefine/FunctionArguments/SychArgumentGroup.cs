using System.Collections.Generic;
using System.Text;

namespace CT.CorePatcher.SyncRetector.PropertyDefine.FunctionArguments
{
	public class SychArgumentGroup
	{
		private List<BaseArgument> _args;

		public int Count => _args.Count;

		public SychArgumentGroup(List<BaseArgument> args)
		{
			_args = args;
		}

		/// <summary>Type value1, Type value2</summary>
		public string GetParameterDeclaration()
		{
			if (Count == 0)
				return string.Empty;

			StringBuilder sb = new();
			for (int i = 0; i < Count; i++)
			{
				sb.Append(_args[i].GetParameterDeclaration());
				if (i < Count - 1)
					sb.Append(", ");
			}
			return sb.ToString();
		}

		/// <summary>(value1, value2)</summary>
		public string GetTupleEnqueueValue()
		{
			if (Count == 1)
				return _args[0].GetParameterName();

			StringBuilder sb = new();
			sb.Append("(");
			for (int i = 0; i < _args.Count; i++)
			{
				sb.Append(_args[i].GetParameterName());
				if (i < _args.Count - 1)
					sb.Append(", ");
			}
			sb.Append(")");
			return sb.ToString();
		}

		/// <summary>(Type value1, Type value2)</summary>
		public string GetTupleDeclaration()
		{
			if (Count == 1)
				return _args[0].TypeName;

			StringBuilder sb = new();
			sb.Append("(");
			for (int i = 0; i < _args.Count; i++)
			{
				sb.Append(_args[i].GetParameterDeclaration());
				if (i < _args.Count - 1)
					sb.Append(", ");
			}
			sb.Append(")");
			return sb.ToString();
		}

		/// <summary>value1, value2</summary>
		public string GetCallParameters()
		{
			if (Count == 1)
				return _args[0].GetParameterName();

			StringBuilder sb = new();
			for (int i = 0; i < _args.Count; i++)
			{
				sb.Append(_args[i].GetParameterName());
				if (i < _args.Count - 1)
					sb.Append(", ");
			}
			return sb.ToString();
		}

		public string GetWriteParameterContent()
		{
			StringBuilder sb = new();
			foreach (var arg in _args)
				sb.AppendLine(arg.GetWriteParameter());
			return sb.ToString();
		}

		public string GetReadParameterContent()
		{
			StringBuilder sb = new();
			foreach (var arg in _args)
				sb.AppendLine(arg.GetTempReadParameter());
			return sb.ToString();
		}
	}
}