namespace CT.CorePatcher.SyncRetector.PropertyDefine.FunctionArguments
{
	public class ValueTypeArgument : BaseArgument
	{
		public ValueTypeArgument(string typeName, string parameterName)
			: base(typeName, parameterName)
		{
			_typeName = typeName;
			_parameterName = parameterName;
		}

		public override string GetParameterDeclaration() => $"{_typeName} {_parameterName}";
		public override string GetParameterName() => _parameterName;
		public override string GetTempReadParameter()
		{
			return string.Format(FuncMemberFormat.TempReadByDeserializerStruct,
								 _typeName, _parameterName);
		}
		public override string GetWriteParameter()
		{
			return string.Format(MemberFormat.WriteSerialize, _parameterName);
		}
	}
}