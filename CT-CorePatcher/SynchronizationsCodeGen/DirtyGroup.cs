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
		public bool HasTargetMember { get; private set; }
		/// <summary>자식 멤버가 있다면 더티 비트는 부모 클래스에서 선언되어 있음</summary>
		public bool HasChildMember { get; private set; }

		public DirtyGroup(List<BaseMemberToken> members, SyncType syncType, int dirtyIndex)
		{
			_members = members;
			_syncType = syncType;
			_dirtyIndex = dirtyIndex;
			HasTargetMember = members.Any((m) =>
			{
				if (m is TargetFunctionMemberToken)
					return true;
				else  if (m is SyncObjectMemberToken)
				{
					if (NameTable.IsPredefinedType(m.TypeName))
						return false;

					if (!SynchronizerGenerator.TryGetSyncObjectByTypeName(m.TypeName, out var syncObjectInfo) ||
						syncObjectInfo == null)
					{
						throw new System.Exception();
					}

					return syncObjectInfo.HasTarget;
				}

				return false;
			});

			HasChildMember = members.Any((m) => m.InheritType == InheritType.Child);
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

		public string Master_MarkObjectDirtyBit(string dirtyBitName)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _members.Count; i++)
			{
				if (_members[i] is not SyncObjectMemberToken token)
					continue;

				sb.AppendLine(string.Format(SyncGroupFormat.SetDirtyBit, dirtyBitName, i, token.Master_IsDirty(_syncType)));
			}

			return sb.ToString();
		}

		public string Master_MemberSerializeIfDirtys(SyncDirection direction, string dirtyBitName, string tempDirtyBitName)
		{
			StringBuilder sb = new();
			int index = 0;
			foreach (var m in _members)
			{
				string serialize = m.Master_SerializeByWriter(_syncType, direction, tempDirtyBitName, index);
				CodeFormat.AddIndent(ref serialize);
				string content = string.Format(CommonFormat.IfDirty, dirtyBitName, index, serialize);
				index++;
				sb.AppendLine(content);
			}
			return sb.ToString();
		}

		public string Master_MemberSetterSetters()
		{
			StringBuilder sb = new();
			int index = 0;
			foreach (var m in _members)
			{
				if (m.InheritType == InheritType.Child)
				{
					index++;
					continue;
				}
				sb.AppendLine(m.Master_GetterSetter(_syncType, GetName(), index++));
			}
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

		public string Remote_IgnoreMembers(SyncType syncType, bool isStatic, bool readDirtyBit = true)
		{
			StringBuilder sb = new();
			int index = 0;
			if (readDirtyBit)
			{
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, GetTempName()));
			}
			foreach (var m in _members)
			{
				string content = m.Remote_IgnoreDeserialize(syncType, isStatic);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirty, GetTempName(), index++, content));
			}
			return sb.ToString();
		}
	}
}
