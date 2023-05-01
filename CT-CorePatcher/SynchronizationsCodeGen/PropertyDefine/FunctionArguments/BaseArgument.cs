namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments
{
	public abstract class BaseArgument
	{
		protected string _typeName = string.Empty;
		protected string _parameterName = string.Empty;
		public string TypeName => _typeName;

		public BaseArgument(string typeName, string parameterName)
		{
			_typeName = typeName;
			_parameterName = parameterName;
		}

		/// <summary>TypeName valueName</summary>
		public abstract string GetParameterDeclaration();

		/// <summary>valueName</summary>
		public abstract string GetParameterName();

		public abstract string GetTempReadParameter();
		public abstract string GetWriteParameter(string paramName);
		public abstract string GetWriteParameterInTuple(string tupleArgName);

		/// <summary>arg.valueName</summary>
		public string GetArgTuplePropertyName()
		{
			return $"{FuncMemberFormat.TempArgumentName}.{_parameterName}";
		}
	}
}