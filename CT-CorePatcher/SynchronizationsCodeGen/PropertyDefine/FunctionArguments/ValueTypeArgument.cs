using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments
{
	public class ValueTypeArgument : BaseArgument
	{
		public bool IsNativeStruct { get; private set; } = false;

		public ValueTypeArgument(string typeName, string parameterName)
			: base(typeName, parameterName)
		{
			IsNativeStruct = ReflectionHelper.IsNativeStruct(_typeName);
		}

		public override string GetParameterDeclaration() => $"{_typeName} {_parameterName}";
		public override string GetParameterName() => _parameterName;
		public override string GetTempReadParameter()
		{
			string format = IsNativeStruct ? 
				FuncMemberFormat.TempReadByDeserializerNativeStruct :
				FuncMemberFormat.TempReadByDeserializerStruct;

			return string.Format(format, _typeName, _parameterName);
		}
		public override string GetWriteParameter(string paramName = "")
		{
			string name = string.IsNullOrWhiteSpace(paramName) ? _parameterName : paramName;
			return string.Format(MemberFormat.WriteSerialize, name);
		}

		public override string GetWriteParameterInTuple(string name)
		{
			string format = IsNativeStruct ? MemberFormat.WritePut : MemberFormat.WriteSerialize;
			return string.Format(format, GetArgTuplePropertyName());
		}

		public override string GetIgnoreRead()
		{
			string dataTypeName = IsNativeStruct ? _typeName + "Extension" : _typeName;
			return string.Format(MemberFormat.IgnoreValueType, dataTypeName);
		}
	}
}