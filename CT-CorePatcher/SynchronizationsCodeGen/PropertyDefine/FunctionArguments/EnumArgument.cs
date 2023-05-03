using CT.CorePatcher.Helper;

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

		public override string GetWriteParameter(string paramName = "")
		{
			string name = string.IsNullOrWhiteSpace(paramName) ? _parameterName : paramName;
			return string.Format(MemberFormat.WriteEnum, _sizeTypeName, name);
		}

		public override string GetWriteParameterInTuple(string name)
		{
			return string.Format(MemberFormat.WriteEnum, _sizeTypeName, GetArgTuplePropertyName());
		}

		public override string GetIgnoreRead()
		{
			return string.Format(MemberFormat.IgnorePrimitive,
								 ReflectionHelper.GetByteSizeByTypeName(_sizeTypeName));
		}
	}
}