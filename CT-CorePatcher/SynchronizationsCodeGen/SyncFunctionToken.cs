using System.Collections.Generic;
using System.Reflection;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncFunctionToken
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		/// <summary>동기화 타입입니다.</summary>
		public SyncType SyncType { get; private set; }

		/// <summary>함수의 이름입니다.</summary>
		public string FunctionName { get; private set; } = string.Empty;

		/// <summary>인자 목록입니다.</summary>
		public List<SyncPropertyToken> Parameters { get; private set; } = new();

		public SyncFunctionToken(SynchronizerGenerator generator, SyncType syncType, MethodInfo methodInfo)
		{
			this.SyncType = syncType;
			this.FunctionName = methodInfo.Name;

			var paramInfo = methodInfo.GetParameters();
			foreach (var param in paramInfo)
			{
				SyncPropertyToken syncParam = new(generator, SyncType.None, param.Name ?? string.Empty, param.ParameterType);
				Parameters.Add(syncParam);
			}
		}

		public string GetPartialDeclaraction()
		{
			return string.Format(SyncFormat.FunctionPartialDeclaration,
								 SyncFormat.GetSyncRpcAttribute(SyncType),
								 FunctionName,
								 GetParameterContent());
		}

		public string GetFunctionCallWithStack(string dirtyBitsName, int funcIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(SyncFormat.FunctionCallWithStackVoid,
									 this.FunctionName,
									 this.GetParameterContent(),
									 dirtyBitsName,
									 funcIndex);
			}
			else
			{
				return string.Format(SyncFormat.FunctionCallWithStack,
									 this.FunctionName,
									 this.GetParameterContent(),
									 this.GetCallStackTupleContent(),
									 dirtyBitsName,
									 funcIndex);
			}
		}

		public string GetSerializeIfDirty(string dirtyBitName, int curFuncIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(SyncFormat.FunctionSerializeIfDirtyVoid,
									 dirtyBitName, curFuncIndex,
									 this.FunctionName);
			}
			else
			{
				var funcSerializeContent = this.GetCallstackSerializeContent();
				CodeFormat.AddIndent(ref funcSerializeContent, 2);
				return string.Format(SyncFormat.FunctionSerializeIfDirty,
									 dirtyBitName, curFuncIndex,
									 this.FunctionName,
									 funcSerializeContent);
			}
		}

		public string GetFunctionDeserializeIfDirty(string dirtyBitName, int curFuncIndex)
		{
			if (Parameters.Count == 0)
			{
				return string.Format(SyncFormat.FunctionDeserializeIfDirtyVoid,
									 dirtyBitName, curFuncIndex,
									 this.FunctionName);
			}
			else
			{
				var funcDeserializeContent = this.GetCallstackDeserializeContent();
				CodeFormat.AddIndent(ref funcDeserializeContent, 2);
				return string.Format(SyncFormat.FunctionDeserializeIfDirty,
									 dirtyBitName, curFuncIndex,
									 this.FunctionName,
									 funcDeserializeContent,
									 this.GetCallStackTupleContent());
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