using System.Collections.Generic;
using CT.CorePatcher.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncNetworkObjectInfo
	{
		public string ObjectName { get; private set; } = string.Empty;

		public List<SyncPropertyToken> Properties { get; private set; } = new();
		public List<SyncFunctionToken> Functions { get; private set; } = new();

		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;

		public SyncNetworkObjectInfo(string objectName)
		{
			ObjectName = objectName;
		}

		public string GenerateServerDeclaration()
		{
			string content = string.Empty;

			// Add properties
			foreach (var prop in Properties)
			{
				content += prop.GetPraivteDeclaration() + NewLine + NewLine;
			}

			// Add functions
			foreach (var func in Functions)
			{
				content += func.GetPartialDeclaraction();
			}

			CodeFormat.AddIndent(ref content);

			return string.Format(SyncFormat.ServerDeclaration, ObjectName, nameof(NetworkObject), content);
		}
	}
}
