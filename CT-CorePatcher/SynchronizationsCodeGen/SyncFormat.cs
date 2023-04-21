using CT.CorePatcher.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public static class SyncFormat
	{
		/// <summary>
		/// {0} using statements<br/>
		/// {1} namespace<br/>
		/// {2} Content<br/>
		/// </summary>
		public static readonly string FileFormat =
@"{0}
namespace {1}
{{
{2}
}}";

		public static readonly string SyncVarReliable = @"SyncVar";
		public static readonly string SyncVarUnreliable = @"SyncVar(SyncType.Unreliable)";

		public static readonly string SyncRpcReliable = @"SyncRpc";
		public static readonly string SyncRpcUnreliable = @"SyncRpc(SyncType.Unreliable)";

		/// <summary>
		/// {0} Object name<br/>
		/// {1} Inherit type name<br/>
		/// {2} Content<br/>
		/// </summary>
		public static readonly string ServerDeclaration =
@"
[Serializable]
public partial class {0} : {1}
{{
{2}
}}
";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Argument name<br/>
		/// </summary>
		public static readonly string Parameter = @"{0} {1}";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Type name<br/>
		/// {2} Property name<br/>
		/// {3} Initialize<br/>
		/// </summary>
		public static readonly string PrivateDeclaration =
@"[{0}]
private {1} {2}{3};";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Function name<br/>
		/// {2} Arguments content<br/>
		/// </summary>
		public static readonly string FunctionPartialDeclaration =
@"[{0}]
public partial void {1}({2});";

		public static string GetSyncVarAttribute(SyncType syncType)
		{
			if (syncType == SyncType.Reliable)
			{
				return SyncVarReliable;
			}
			else if (syncType == SyncType.Unreliable)
			{
				return SyncVarUnreliable;
			}

			return string.Empty;
		}

		public static string GetSyncRpcAttribute(SyncType syncType)
		{
			if (syncType == SyncType.Reliable)
			{
				return SyncRpcReliable;
			}
			else if (syncType == SyncType.Unreliable)
			{
				return SyncRpcUnreliable;
			}

			return string.Empty;
		}
	}
}
