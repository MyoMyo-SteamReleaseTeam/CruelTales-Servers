using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments
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
		public override string GetWriteParameter(string paramName = "")
		{
			string name = string.IsNullOrWhiteSpace(paramName) ? _parameterName : paramName;
			return string.Format(MemberFormat.WriteSerialize, name);
		}

		public override string GetWriteParameterInTuple(string name)
		{
			return string.Format(MemberFormat.WriteSerialize, GetArgTuplePropertyName());
		}

		public override string GetIgnoreRead()
		{
			return string.Format(MemberFormat.IgnoreValueType, _typeName);
		}
	}
}