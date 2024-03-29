﻿using CT.CorePatcher.Helper;

namespace CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine.FunctionArguments
{
	public class PrimitiveArgument : BaseArgument
	{
		private string _clrTypeName = string.Empty;

		public PrimitiveArgument(string typeName, string parameterName, string clrTypeName)
			: base(typeName, parameterName)
		{
			_clrTypeName = clrTypeName;
		}

		public override string GetParameterDeclaration() => $"{_typeName} {_parameterName}";
		public override string GetParameterName() => _parameterName;
		public override string GetTempReadParameter()
		{
			return string.Format(FuncMemberFormat.TempReadPrimitiveTypeProperty,
								 _typeName, _parameterName, _clrTypeName);
		}
		public override string GetWriteParameter(string paramName = "")
		{
			string name = string.IsNullOrWhiteSpace(paramName) ? _parameterName : paramName;
			return string.Format(MemberFormat.WritePut, name);
		}

		public override string GetWriteParameterInTuple(string name)
		{
			return string.Format(MemberFormat.WritePut, GetArgTuplePropertyName());
		}

		public override string GetIgnoreRead()
		{
			return string.Format(MemberFormat.IgnorePrimitive,
								 ReflectionHelper.GetByteSizeByTypeName(_typeName));
		}
	}
}