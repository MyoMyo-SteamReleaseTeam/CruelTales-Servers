using System.IO;

namespace CT.CorePatcher.Packets
{
	public class JobOption
	{
		public string XmlSourcePath = string.Empty;
		public string OutputServerPath = string.Empty;

		public string GetFileNameWithExtension()
		{
			return Path.GetFileNameWithoutExtension(XmlSourcePath) + ".cs";
		}

		public string GetTargetPath()
		{
			return Path.Combine(OutputServerPath, GetFileNameWithExtension());
		}
	}

	public class CodeGenOperation
	{
		public string GeneratedCode = string.Empty;
		public string TargetPath = string.Empty;
	}
}