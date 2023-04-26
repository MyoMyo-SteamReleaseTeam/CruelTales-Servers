using System.Collections.Generic;
using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector
{
	public class SerializeSyncGroup
	{
		private List<DirtyGroup> _dirtyGroups;
		private SyncType _syncType;
		private string _modifier;

		public SerializeSyncGroup(List<ISynchronizeMember> memberTokens, SyncType syncType, string modifier)
		{
			_syncType = syncType;
			_modifier = modifier;

			_dirtyGroups = new();

			int m = 0;
			while (m < memberTokens.Count)
			{
				List<ISynchronizeMember> members = new();
				for (int i = 0; i < 8; i++)
				{
					if (m >= memberTokens.Count)
						break;

					members.Add(memberTokens[m]);
					m++;
				}
				_dirtyGroups.Add(new DirtyGroup(members, _syncType, m / 8));
			}
		}

		public string Master_BitmaskDeclarations()
		{
			StringBuilder sb = new();
			foreach (var g in _dirtyGroups)
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeclaration, g.GetName()));
			return sb.ToString();
		}

		public string Master_GetterSetter()
		{
			StringBuilder sb = new();
			foreach (var g in _dirtyGroups)
				sb.AppendLine(g.Master_MemberSetterSetters());
			return sb.ToString();
		}

		public string Master_DirtyProperty()
		{
			StringBuilder sb = new();
			foreach (var g in _dirtyGroups)
				sb.AppendLine(g.Master_MemberCheckDirtys());

			string content = sb.ToString();
			CodeFormat.AddIndent(ref content, 2);
			return string.Format(SyncGroupFormat.IsDirty, _modifier, _syncType, content);
		}

		public string Master_SerializeSync()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.SerializeFunctionDeclaration, _modifier, _syncType));

			if (_dirtyGroups.Count == 0 )
				return sb.AppendLine(" { }").ToString();

			// TODO : 개수에 따라서 다르게 분배
			var content = CodeFormat.AddIndent(master_SerializeSyncMultipleDirtyGroup());

			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string master_SerializeSyncOneDirtyGroup()
		{
			return "";
		}

		private string master_SerializeSyncTwoDirtyGroup()
		{
			return "";
		}

		private string master_SerializeSyncMultipleDirtyGroup()
		{
			StringBuilder headers = new();
			StringBuilder contents = new();
			for (int i = 0; i < _dirtyGroups.Count; i++)
			{
				var dirtyGroup = _dirtyGroups[i];
				headers.AppendLine(string.Format(SyncGroupFormat.MasterDirtyAnyTrue, i, dirtyGroup.GetName()));
				string c = CodeFormat.AddIndent(dirtyGroup.Master_MemberSerializeIfDirtys());
				string content = string.Format(CommonFormat.IfDirty, SyncGroupFormat.MasterDirtyBitName, i, c);
				contents.AppendLine(content);
			}
			headers.AppendLine(SyncGroupFormat.MasterDirtySerialize);
			headers.AppendLine(contents.ToString());
			return headers.ToString();
		}

		public string Master_ClearDirty()
		{
			StringBuilder sb = new();
			foreach (var d in _dirtyGroups)
				sb.AppendLine(d.Master_ClearDirtys());
			CodeFormat.AddIndent(sb);
			return sb.ToString();
		}
	}

	public class DeserializeSyncGroup
	{
		private List<DirtyGroup> _dirtyGroups;
		private SyncType _syncType;
		private string _modifier;

		public DeserializeSyncGroup(List<ISynchronizeMember> memberTokens, SyncType syncType, string modifier)
		{
			_syncType = syncType;
			_modifier = modifier;

			_dirtyGroups = new();

			int m = 0;
			while (m < memberTokens.Count)
			{
				List<ISynchronizeMember> members = new();
				for (int i = 0; i < 8; i++)
				{
					if (m >= memberTokens.Count)
						break;

					members.Add(memberTokens[m]);
					m++;
				}
				_dirtyGroups.Add(new DirtyGroup(members, _syncType, m / 8));
			}
		}

		public string Remote_DeserializeSync()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.DeserializeFunctionDeclaration, _modifier, _syncType));

			if (_dirtyGroups.Count == 0)
				return sb.AppendLine(" { }").ToString();

			// TODO : 개수에 따라서 다르게 분배
			var content = remote_DeserializeSyncMultipleDirtyGroup();
			CodeFormat.AddIndent(ref content);

			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string remote_DeserializeSyncMultipleDirtyGroup()
		{
			StringBuilder headers = new();
			headers.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize,
											 SyncGroupFormat.MasterDirtyBitName));

			StringBuilder contents = new();
			for (int i = 0; i < _dirtyGroups.Count; i++)
			{

				var dirtyGroup = _dirtyGroups[i];
				var c = dirtyGroup.Remote_MemberDeserializeIfDirtys();
				CodeFormat.AddIndent(ref c);
				contents.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, dirtyGroup.GetName()));
				contents.AppendLine(string.Format(CommonFormat.IfDirty, dirtyGroup, i, c));
			}
			headers.AppendLine(contents.ToString());
			return headers.ToString();
		}
	}
}
