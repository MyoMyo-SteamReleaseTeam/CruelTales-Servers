using System.Collections.Generic;
using System.Reflection;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector.Previous
{
	public class SyncFunctionToken
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		/// <summary>함수의 이름입니다.</summary>
		public string FunctionName { get; private set; } = string.Empty;

		/// <summary>인자 목록입니다.</summary>
		public List<SyncPropertyToken> Parameters { get; private set; } = new();

		public SyncFunctionToken(SynchronizerGenerator generator,
								 MethodInfo methodInfo)
		{
			FunctionName = methodInfo.Name;

			var paramInfo = methodInfo.GetParameters();
			foreach (var param in paramInfo)
			{
				SyncPropertyToken syncParam = new(generator,
												  param.Name ?? string.Empty,
												  param.ParameterType,
												  isPublic: true);
				Parameters.Add(syncParam);
			}
		}

		public string GetPartialDeclaraction(SyncType syncType, SyncDirection syncDirection)
		{
			return string.Format(FunctionFormat.Declaration,
								 SyncFormat.GetSyncRpcAttribute(syncType, syncDirection),
								 FunctionName,
								 GetParameterContent());
		}

		public string GetFunctionCallWithStack(string dirtyBitsName, int funcIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(FunctionFormat.CallWithStackVoid,
									 FunctionName,
									 GetParameterContent(),
									 dirtyBitsName,
									 funcIndex);
			}
			else
			{
				string parameterContent;
				string callStackContent;
				string genericType;

				if (Parameters.Count > 1)
				{
					parameterContent = GetParameterContent();
					callStackContent = $"({GetCallStackTupleContent()})";
					genericType = $"({parameterContent})";
				}
				else
				{
					parameterContent = GetParameterContent();
					callStackContent = GetCallStackTupleContent();
					genericType = Parameters[0].TypeName;
				}

				return string.Format(FunctionFormat.CallWithStack,
									 FunctionName,
									 parameterContent,
									 callStackContent,
									 dirtyBitsName,
									 funcIndex,
									 genericType);
			}
		}

		public string GetSerializeIfDirty(string dirtyBitName, int curFuncIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(FunctionFormat.SerializeIfDirtyVoid,
									 dirtyBitName, curFuncIndex,
									 FunctionName);
			}
			else if (Parameters.Count == 1)
			{
				var funcSerializeContent = Parameters[0].GetWriterSerializeByName("arg");
				CodeFormat.AddIndent(ref funcSerializeContent, 2);
				return string.Format(FunctionFormat.SerializeIfDirtyOneArg,
									 dirtyBitName, curFuncIndex,
									 FunctionName,
									 funcSerializeContent);
			}
			else
			{
				var funcSerializeContent = GetCallstackSerializeContent();
				CodeFormat.AddIndent(ref funcSerializeContent, 2);
				return string.Format(FunctionFormat.SerializeIfDirty,
									 dirtyBitName, curFuncIndex,
									 FunctionName,
									 funcSerializeContent);
			}
		}

		public string GetFunctionDeserializeIfDirty(string dirtyBitName, int curFuncIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(FunctionFormat.DeserializeIfDirtyVoid,
									 dirtyBitName, curFuncIndex,
									 FunctionName);
			}
			else
			{
				var funcDeserializeContent = GetCallstackDeserializeContent();
				CodeFormat.AddIndent(ref funcDeserializeContent, 2);
				return string.Format(FunctionFormat.DeserializeIfDirty,
									 dirtyBitName, curFuncIndex,
									 FunctionName,
									 funcDeserializeContent,
									 GetCallStackTupleContent());
			}
		}

		public string GetParameterContent()
		{
			string paramContent = string.Empty;
			for (int i = 0; i < Parameters.Count; i++)
			{
				var param = Parameters[i];
				paramContent += param.GetParameter();

				if (i < Parameters.Count - 1)
					paramContent += ", ";
			}
			return paramContent;
		}

		public string GetCallStackTupleContent()
		{
			if (Parameters.Count == 1)
			{
				return Parameters[0].PrivateName;
			}

			string paramContent = string.Empty;
			for (int i = 0; i < Parameters.Count; i++)
			{
				var param = Parameters[i];
				paramContent += param.PrivateName;

				if (i < Parameters.Count - 1)
					paramContent += ", ";
			}
			return paramContent;
		}

		public string GetCallstackSerializeContent()
		{
			string content = string.Empty;
			foreach (var param in Parameters)
				content += param.GetWriterSerializeWithPrefix("args.");

			return content;
		}

		public string GetCallstackDeserializeContent()
		{
			string content = string.Empty;
			foreach (var param in Parameters)
				content += param.GetTempReadDeserialize() + NewLine;

			return content;
		}
	}
}