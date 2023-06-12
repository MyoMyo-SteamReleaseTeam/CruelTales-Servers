using System.Collections.Generic;
using System.Linq;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class DirtyGroup
	{
		private List<BaseMemberToken> _members = new();
		private SyncType _syncType;
		private int _dirtyIndex;
		private bool _hasTargetMember = false;
		public bool HasTargetMember => _hasTargetMember;

		public DirtyGroup(List<BaseMemberToken> members, SyncType syncType, int dirtyIndex)
		{
			_members = members;
			_syncType = syncType;
			_dirtyIndex = dirtyIndex;
			_hasTargetMember = members.FirstOrDefault((m) => m is TargetFunctionMemberToken) != null;
		}

		public string GetName() => $"_dirty{_syncType}_{_dirtyIndex}";
		public string GetTempName() => $"dirty{_syncType}_{_dirtyIndex}";

		public string Master_MemberCheckDirtys()
		{
			StringBuilder sb = new();
			foreach (var m in _members)
				sb.AppendLine(m.Master_CheckDirty(_syncType));
			return sb.ToString();
		}

		public string Master_MemberSerializeIfDirtys(string dirtyBitName, string tempDirtyBitName, string rollbackContents = "")
		{
			StringBuilder sb = new();
			int index = 0;
			if (_hasTargetMember)
			{
				sb.AppendLine(string.Format(DirtyGroupFormat.JumpAndSerializeMask, dirtyBitName, tempDirtyBitName));
			}
			else
			{
				sb.AppendLine(string.Format(MemberFormat.WriteSerialize, dirtyBitName));
			}
			foreach (var m in _members)
			{
				string serialize = m.Master_SerializeByWriter(_syncType, tempDirtyBitName, index);
				CodeFormat.AddIndent(ref serialize);
				string content = string.Format(CommonFormat.IfDirty, dirtyBitName, index, serialize);
				index++;
				sb.AppendLine(content);
			}
			if (_hasTargetMember)
			{
				CodeFormat.AddIndent(ref rollbackContents);
				sb.AppendLine(string.Format(DirtyGroupFormat.RollBackSerializeMask, tempDirtyBitName, rollbackContents));
			}
			return sb.ToString();
		}

		public string Master_MemberSetterSetters()
		{
			StringBuilder sb = new();
			int index = 0;
			foreach (var m in _members)
				sb.AppendLine(m.Master_GetterSetter(GetName(), index++));
			return sb.ToString();
		}

		public string Master_ClearDirtys()
		{
			StringBuilder sb = new();
			sb.AppendLine($"{GetName()}.Clear();");
			foreach (var m in _members)
				sb.AppendLine(m.Master_ClearDirty(_syncType));
			return sb.ToString();
		}

		public string Master_InitializeProperties()
		{
			StringBuilder sb = new();
			foreach (var m in _members)
				sb.AppendLine(m.Master_InitializeProperty());
			return sb.ToString();
		}

		public string Remote_MemberDeserializeIfDirtys(SyncDirection direction, bool readDirtyBit = true)
		{
			StringBuilder sb = new();
			int index = 0;
			if (readDirtyBit)
			{
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, GetTempName()));
			}
			foreach (var m in _members)
			{
				string content = m.Remote_DeserializeByReader(_syncType, direction);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirty, GetTempName(), index++, content));
			}
			return sb.ToString();
		}

		public string Remote_IgnoreMembers(SyncType syncType, bool readDirtyBit = true)
		{
			StringBuilder sb = new();
			int index = 0;
			if (readDirtyBit)
			{
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, GetTempName()));
			}
			foreach (var m in _members)
			{
				string content = m.Remote_IgnoreDeserialize(syncType);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirty, GetTempName(), index++, content));
			}
			return sb.ToString();
		}
	}
}
