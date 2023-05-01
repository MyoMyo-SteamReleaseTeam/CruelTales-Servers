namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments
{
	public class PrimitiveArgument : BaseArgument
	{
		private string _clrTypeName = string.Empty;

		public PrimitiveArgument(string typeName, string parameterName, string clrTypeName)
			: base(typeName, parameterName)
		{
			_typeName = typeName;
			_parameterName = parameterName;
			_clrTypeName = clrTypeName;
		}

		public override string GetParameterDeclaration() => $"{_typeName} {_parameterName}";
		public override string GetParameterName() => _parameterName;
		public override string GetTempReadParameter()
		{
			return string.Format(FuncMemberFormat.TempReadPrimitiveTypeProperty,
								 _typeName, _parameterName, _clrTypeName);
		}
		public override string GetWriteParameter()
		{
			return string.Format(MemberFormat.WritePut, _parameterName);
		}

		public override string GetWriteParameterByName(string name)
		{
			return string.Format(MemberFormat.WritePut, GetArgTuplePropertyName());
		}
	}
}