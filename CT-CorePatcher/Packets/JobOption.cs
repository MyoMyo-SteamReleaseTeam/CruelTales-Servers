using System.IO;

namespace CT.CorePatcher.Packets
{
	public class JobOption
	{
		public string XmlSourcePath = string.Empty;
		public string OutputServerPath = string.Empty;
		public string OutputClientPath = string.Empty;

		public string GetFileNameWithExtension()
		{
			return Path.GetFileNameWithoutExtension(XmlSourcePath) + ".cs";
		}

		public string GetServerTargetPath()
		{
			return Path.Combine(OutputServerPath, GetFileNameWithExtension());
		}

		public string GetClientTargetPath()
		{
			return Path.Combine(OutputClientPath, GetFileNameWithExtension());
		}
	}
}