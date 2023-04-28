namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments
{
	public class EnumArgument : BaseArgument
	{
		private string _sizeTypeName = string.Empty;
		private string _clrSizeTypeName = string.Empty;

		public EnumArgument(string typeName, string parameterName, string sizeTypeName, string clrSizeTypeName)
			: base(typeName, parameterName)
		{
			_typeName = typeName;
			_parameterName = parameterName;
			_sizeTypeName = sizeTypeName;
			_clrSizeTypeName = clrSizeTypeName;
		}

		public override string GetParameterDeclaration() => $"{_typeName} {_parameterName}";
		public override string GetParameterName() => _parameterName;
		public override string GetTempReadParameter()
		{
			return string.Format(FuncMemberFormat.TempReadEnum,
								 _typeName, _parameterName, _clrSizeTypeName);
		}
		public override string GetWriteParameter()
		{
			return string.Format(MemberFormat.WriteEnum, _sizeTypeName, _parameterName);
		}

		public override string GetWriteParameterByName(string name)
		{
			return string.Format(MemberFormat.WriteEnum, _sizeTypeName, FuncMemberFormat.TempArgumentName);
		}
	}
}