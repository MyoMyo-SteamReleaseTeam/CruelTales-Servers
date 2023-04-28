using System.Collections.Generic;
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

		public DirtyGroup(List<BaseMemberToken> members, SyncType syncType, int dirtyIndex)
		{
			_members = members;
			_syncType = syncType;
			_dirtyIndex = dirtyIndex;
		}

		public string Master_MemberCheckDirtys()
		{
			StringBuilder sb = new();
			foreach (var m in _members)
				sb.AppendLine(m.Master_CheckDirty());
			return sb.ToString();
		}

		public string Master_MemberSerializeIfDirtys(bool putDrityBitSerialize = true)
		{
			StringBuilder sb = new();
			int index = 0;
			if (putDrityBitSerialize)
			{
				sb.AppendLine(string.Format(MemberFormat.WriteSerialize, GetName()));
			}
			foreach (var m in _members)
			{
				string serialize = m.Master_SerializeByWriter();
				CodeFormat.AddIndent(ref serialize);
				string content = string.Format(CommonFormat.IfDirty, GetName(), index++, serialize);
				sb.AppendLine(content);
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
				sb.AppendLine(m.Master_ClearDirty());
			return sb.ToString();
		}

		public string GetName() => $"_dirty{_syncType}_{_dirtyIndex}";

		public string Remote_MemberDeserializeIfDirtys()
		{
			StringBuilder sb = new();
			int index = 0;
			sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, GetName()));
			foreach (var m in _members)
			{
				string content = m.Remote_DeserializeByReader();
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirty, GetName(), index++, content));
			}
			return sb.ToString();
		}
	}
}
