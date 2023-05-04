using System.Collections.Generic;
using System.Linq;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	/// <summary>
	/// Dirty Group은 1 바이트의 Bitmask로 8개 요소의 변경사항을 감지할 수 있다.
	/// - 8개 이하의 요소는 1 바이트로 Dirty 상태를 표현한다.
	/// - 16개 이하의 요소는 2 바이트로 Dirty 상태를 표현한다.
	/// - 16개를 초과하는 요소는 1 바이트의 Master Dirty Bitmask로 각각 8개의 Bitmask를 표현한다.
	///   이 경우 최대 64개의 요소를 포함할 수 있다.
	///   최소 2 바이트의 Bitmaks가 필요하고 최대 5 바이트의 Bitmask로 상태를 표현한다.
	/// </summary>
	public class SerializeSyncGroup
	{
		private List<DirtyGroup> _dirtyGroups;
		private SyncType _syncType;
		private string _modifier;

		public SerializeSyncGroup(List<BaseMemberToken> memberTokens, SyncType syncType, string modifier)
		{
			_syncType = syncType;
			_modifier = modifier;
			_dirtyGroups = new();

			int m = 0;
			int groupCount = 0;
			while (m < memberTokens.Count)
			{
				List<BaseMemberToken> members = new();
				for (int i = 0; i < 8; i++)
				{
					if (m >= memberTokens.Count)
						break;

					members.Add(memberTokens[m]);
					m++;
				}
				_dirtyGroups.Add(new DirtyGroup(members, _syncType, groupCount++));
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
			if (_dirtyGroups.Count == 0)
				return string.Format(SyncGroupFormat.IsDirtyIfNoElement, _modifier, _syncType);

			StringBuilder sb = new();
			foreach (var g in _dirtyGroups)
			{
				sb.AppendLine(g.Master_MemberCheckDirtys());
				sb.AppendLine(string.Format(SyncGroupFormat.IsBitmaskDirty, g.GetName()));
			}

			string content = sb.ToString();
			CodeFormat.AddIndent(ref content, 2);
			return string.Format(SyncGroupFormat.IsDirty, _modifier, _syncType, content);
		}

		public string Master_SerializeSync()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.SerializeFunctionDeclaration, _modifier, _syncType));

			if (_dirtyGroups.Count == 0)
				return sb.AppendLine(" { }").ToString();

			string content;

			if (_dirtyGroups.Count == 1)
			{
				content = CodeFormat.AddIndent(master_SerializeSyncOneDirtyGroup());
			}
			else if (_dirtyGroups.Count == 2)
			{
				content = CodeFormat.AddIndent(master_SerializeSyncTwoDirtyGroup());
			}
			else
			{
				content = CodeFormat.AddIndent(master_SerializeSyncMultipleDirtyGroup());
			}

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string master_SerializeSyncOneDirtyGroup()
		{
			return _dirtyGroups[0].Master_MemberSerializeIfDirtys();
		}

		private string master_SerializeSyncTwoDirtyGroup()
		{
			StringBuilder sb = new();
			for (int i = 0; i < 2; i++)
			{
				var group = _dirtyGroups[i];
				sb.AppendLine(string.Format(MemberFormat.WriteSerialize, group.GetName()));
				string content = group.Master_MemberSerializeIfDirtys(false);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirtyAny, group.GetName(), content));
			}
			return sb.ToString();
		}

		private string master_SerializeSyncMultipleDirtyGroup()
		{
			StringBuilder headers = new();
			StringBuilder contents = new();
			headers.AppendLine(SyncGroupFormat.MasterDirtyBitInstantiate);
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
			if (_dirtyGroups.Count == 0)
				return string.Format(SyncGroupFormat.ClearDirtyFunctionIfEmpty, _modifier, _syncType);

			StringBuilder sb = new();
			foreach (var d in _dirtyGroups)
				sb.AppendLine(d.Master_ClearDirtys());
			CodeFormat.AddIndent(sb);
			return string.Format(SyncGroupFormat.ClearDirtyFunction, _modifier, _syncType, sb.ToString());
		}
	}

	public class DeserializeSyncGroup
	{
		private List<DirtyGroup> _dirtyGroups;
		private SyncType _syncType;
		private string _modifier;

		public DeserializeSyncGroup(List<BaseMemberToken> memberTokens, SyncType syncType, string modifier)
		{
			_syncType = syncType;
			_modifier = modifier;
			_dirtyGroups = new();

			int m = 0;
			int groupCount = 0;
			while (m < memberTokens.Count)
			{
				List<BaseMemberToken> members = new();
				for (int i = 0; i < 8; i++)
				{
					if (m >= memberTokens.Count)
						break;

					members.Add(memberTokens[m]);
					m++;
				}
				_dirtyGroups.Add(new DirtyGroup(members, _syncType, groupCount++));
			}
		}

		public string Remote_DeserializeSync()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.DeserializeFunctionDeclaration, _modifier, _syncType));

			if (_dirtyGroups.Count == 0)
				return sb.AppendLine(" { }").ToString();

			string content;
			if (_dirtyGroups.Count == 1)
			{
				content = remote_DeserializeSyncOneDirtyGroup();
			}
			else if (_dirtyGroups.Count == 2)
			{
				content = remote_DeserializeSyncTwoDirtyGroup();
			}
			else
			{
				content = remote_DeserializeSyncMultipleDirtyGroup();
			}

			CodeFormat.AddIndent(ref content);

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string remote_DeserializeSyncOneDirtyGroup()
		{
			return _dirtyGroups[0].Remote_MemberDeserializeIfDirtys();
		}

		private string remote_DeserializeSyncTwoDirtyGroup()
		{
			StringBuilder sb = new();
			for (int i = 0; i < 2; i++)
			{
				var group = _dirtyGroups[i];
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, group.GetName()));
				string content = group.Remote_MemberDeserializeIfDirtys(false);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirtyAny, group.GetName(), content));
			}
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
				string content = _dirtyGroups[i].Remote_MemberDeserializeIfDirtys();
				CodeFormat.AddIndent(ref content);
				contents.AppendLine(string.Format(CommonFormat.IfDirty, SyncGroupFormat.MasterDirtyBitName, i, content));
			}
			headers.AppendLine(contents.ToString());
			return headers.ToString();
		}

		public string Remote_IgnoreSync()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.IgnoreSyncFunctionDeclaration, _syncType));

			if (_dirtyGroups.Count == 0)
				return sb.AppendLine(" { }").ToString();

			string content;
			if (_dirtyGroups.Count == 1)
			{
				content = remote_IgnoreSyncOneDirtyGroup();
			}
			else if (_dirtyGroups.Count == 2)
			{
				content = remote_IgnoreSyncTwoDirtyGroup();
			}
			else
			{
				content = remote_IgnoreSyncMultipleDirtyGroup();
			}

			CodeFormat.AddIndent(ref content);

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string remote_IgnoreSyncOneDirtyGroup()
		{
			return _dirtyGroups[0].Remote_IgnoreMembers(_syncType);
		}

		private string remote_IgnoreSyncTwoDirtyGroup()
		{
			StringBuilder sb = new();
			for (int i = 0; i < 2; i++)
			{
				var group = _dirtyGroups[i];
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, group.GetName()));
				string content = group.Remote_IgnoreMembers(_syncType, false);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirtyAny, group.GetName(), content));
			}
			return sb.ToString();
		}

		private string remote_IgnoreSyncMultipleDirtyGroup()
		{
			StringBuilder headers = new();
			headers.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize,
											 SyncGroupFormat.MasterDirtyBitName));

			StringBuilder contents = new();
			for (int i = 0; i < _dirtyGroups.Count; i++)
			{
				string content = _dirtyGroups[i].Remote_IgnoreMembers(_syncType);
				CodeFormat.AddIndent(ref content);
				contents.AppendLine(string.Format(CommonFormat.IfDirty, SyncGroupFormat.MasterDirtyBitName, i, content));
			}
			headers.AppendLine(contents.ToString());
			return headers.ToString();
		}
	}
}
