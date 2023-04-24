using System;

namespace CT.Common.Tools.GetOpt
{
	public class NoProcessArgumentsException : Exception
	{
		public NoProcessArgumentsException(string argumentsName)
			: base($"There is no process arguments. Needed arguments : {argumentsName}") { }
	}
}
