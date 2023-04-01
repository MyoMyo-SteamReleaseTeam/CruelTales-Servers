using System;

namespace CT.Tools.GetOpt
{
	public class NoProcessArgumentsException : Exception
	{
		public NoProcessArgumentsException(string argumentsName)
			: base($"There is no process arguments. Needed arguments : {argumentsName}") { }
	}
}
