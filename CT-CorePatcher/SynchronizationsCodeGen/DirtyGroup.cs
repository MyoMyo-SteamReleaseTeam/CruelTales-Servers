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
				else if (m is SyncObjectMemberToken)
					return SynchronizerGenerator.HasTarget(m.TypeName);
				else
					return false;
			});

			HasChildMember = members.Any((m) => m.InheritType == InheritType.Child);
		}

		public string GetName() => $"_dirty{_syncType}_{_dirtyIndex}";
		public string GetTempName() => $"dirty{_syncType}_{_dirtyIndex}";

		public string Master_MemberCheckDirtys(CodeGenDirection codeGenDirection)
		{
			StringBuilder sb = new();

			GenOption genOption = new GenOption()
			{
				GenDirection = codeGenDirection,
				SyncType = _syncType
			};
			foreach (var m in _members)
				sb.AppendLine(m.Master_CheckDirty(genOption));
			return sb.ToString();
		}

		public string Master_MarkObjectDirtyBit(GenOption option, string dirtyBitName)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _members.Count; i++)
			{
				if (_members[i] is not SyncObjectMemberToken token)
					continue;

				sb.AppendLine(string.Format(SyncGroupFormat.SetDirtyBit, dirtyBitName, i, token.Master_IsDirty(option)));
			}

			return sb.ToString();
		}

		public string Master_MemberSerializeIfDirtys(GenOption option,
													 string dirtyBitName,
													 string tempDirtyBitName)
		{
			StringBuilder sb = new();
			int index = 0;
			foreach (var m in _members)
			{
				string serialize = m.Master_SerializeByWriter(option, tempDirtyBitName, index);
				CodeFormat.AddIndent(ref serialize);
				string content = string.Format(CommonFormat.IfDirty, dirtyBitName, index, serialize);
				index++;
				sb.AppendLine(content);
			}
			return sb.ToString();
		}

		public string Master_MemberSetterSetters(GenOption option)
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
				sb.AppendLine(m.Master_GetterSetter(option, GetName(), index++));
			}
			return sb.ToString();
		}

		public string Master_ClearDirtys(GenOption genOption)
		{
			StringBuilder sb = new();
			sb.AppendLine($"{GetName()}.Clear();");

			foreach (var m in _members)
				sb.AppendLine(m.Master_ClearDirty(genOption));
			return sb.ToString();
		}

		public string Remote_MemberDeserializeIfDirtys(GenOption option, bool readDirtyBit = true)
		{
			StringBuilder sb = new();
			int index = 0;
			if (readDirtyBit)
			{
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, GetTempName()));
			}

			foreach (var m in _members)
			{
				string content = m.Remote_DeserializeByReader(option);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirty, GetTempName(), index++, content));
			}
			return sb.ToString();
		}

		public string Remote_IgnoreMembers(GenOption option, bool isStatic, bool readDirtyBit = true)
		{
			StringBuilder sb = new();
			int index = 0;
			if (readDirtyBit)
			{
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, GetTempName()));
			}

			foreach (var m in _members)
			{
				string content = m.Remote_IgnoreDeserialize(option, isStatic);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirty, GetTempName(), index++, content));
			}
			return sb.ToString();
		}
	}
}
