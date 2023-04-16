namespace CT.CorePatcher.FilePatch
{
	internal static class FilePatcherFormat
	{
		/// <summary>
		/// {0} Code file name
		/// {1} Content
		/// </summary>
		public static readonly string CopyMetadata =
@"/*
 * File : {0}
 * 
 * This file has been copied from the network library of Cruel Tales.
 * Do not modify the code arbitrarily. If you need to add new feature,
 * You should write the code at origin library project.
 */

/* Enable nullable option for unsupported platform */

#nullable enable

{1}";
	}
}
