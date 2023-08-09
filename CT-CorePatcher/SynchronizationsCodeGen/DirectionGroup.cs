﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SerializeDirectionGroup
	{
		private string _modifier;
		private SyncDirection _direction;
		private InheritType _inheritType;

		private SerializeSyncGroup _reliableGruop;
		private SerializeSyncGroup _unreliableGruop;
		private EntireSerializeSyncGroup _entireGroup;

		public SerializeDirectionGroup(List<MemberToken> serializeMembers,
									   SyncDirection syncDirection,
									   InheritType inheritType,
									   string modifier)
		{
			_direction = syncDirection;
			_inheritType = inheritType;
			_modifier = modifier;

			List<BaseMemberToken> sReliableMembers = serializeMembers
				.Where(m => m.SyncType == SyncType.Reliable ||
					   m.SyncType == SyncType.ReliableOrUnreliable ||
					   (m.Member is TargetFunctionMemberToken &&
					    m.SyncType == SyncType.ReliableTarget))
				.Select(m => m.Member).ToList();

			List<BaseMemberToken> sUnreliableMembers = serializeMembers
				.Where(m => m.SyncType == SyncType.Unreliable ||
					   m.SyncType == SyncType.ReliableOrUnreliable ||
					   (m.Member is TargetFunctionMemberToken &&
					    m.SyncType == SyncType.UnreliableTarget))
				.Select(m => m.Member).ToList();

			List<BaseMemberToken> sAllMembers = serializeMembers
				.Select(m => m.Member).ToList();

			_reliableGruop = new SerializeSyncGroup(sReliableMembers, SyncType.Reliable, _direction, _modifier);
			_unreliableGruop = new SerializeSyncGroup(sUnreliableMembers, SyncType.Unreliable, _direction, _modifier);
			_entireGroup = new EntireSerializeSyncGroup(sAllMembers, SyncType.None, _direction, _modifier);
		}

		public string Gen_SynchronizerProperties()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(_reliableGruop.Master_BitmaskDeclarations(_inheritType));
			sb.AppendLine(_unreliableGruop.Master_BitmaskDeclarations(_inheritType));

			sb.AppendLine(_reliableGruop.Master_DirtyProperty());
			sb.AppendLine(_unreliableGruop.Master_DirtyProperty());

			sb.AppendLine(_reliableGruop.Master_GetterSetter());
			sb.AppendLine(_unreliableGruop.Master_GetterSetter());
			return sb.ToString();
		}

		public string Gen_SerializeSyncFuntions()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(_reliableGruop.Master_ClearDirty());
			sb.AppendLine(_unreliableGruop.Master_ClearDirty());

			sb.AppendLine(_reliableGruop.Master_SerializeSync());
			sb.AppendLine(_unreliableGruop.Master_SerializeSync());

			if (_direction == SyncDirection.FromMaster)
			{
				sb.AppendLine(_entireGroup.Master_SerializeSyncAll());
			}
			sb.AppendLine(_entireGroup.Master_InitilaizeProperties());
			return sb.ToString();
		}
	}

	public class DeserializeDirectionGroup
	{
		private string _modifier;
		private SyncDirection _direction;
		private InheritType _inheritType;

		private DeserializeSyncGroup _reliableGruop;
		private DeserializeSyncGroup _unreliableGruop;
		private EntireDeserializeSyncGroup _entireGroup;

		public DeserializeDirectionGroup(List<MemberToken> deserializeMembers, 
										 SyncDirection direction, 
										 InheritType inheritType,
										 string modifier)
		{
			_direction = direction;
			_inheritType = inheritType;
			_modifier = modifier;

			List<BaseMemberToken> dReliableMembers = deserializeMembers
				.Where(m => m.SyncType == SyncType.Reliable ||
					   m.SyncType == SyncType.ReliableOrUnreliable ||
					   (m.Member is TargetFunctionMemberToken &&
						m.SyncType == SyncType.ReliableTarget))
				.Select(m => m.Member).ToList();

			List<BaseMemberToken> dUnreliableMembers = deserializeMembers
				.Where(m => m.SyncType == SyncType.Unreliable ||
					   m.SyncType == SyncType.ReliableOrUnreliable ||
					   (m.Member is TargetFunctionMemberToken &&
						m.SyncType == SyncType.UnreliableTarget))
				.Select(m => m.Member).ToList();

			List<BaseMemberToken> dAllMembers = deserializeMembers
				.Select(m => m.Member).ToList();

			_reliableGruop = new DeserializeSyncGroup(dReliableMembers, SyncType.Reliable, direction, _modifier);
			_unreliableGruop = new DeserializeSyncGroup(dUnreliableMembers, SyncType.Unreliable, direction, _modifier);
			_entireGroup = new EntireDeserializeSyncGroup(dAllMembers, SyncType.None, direction, _modifier);
		}

		public string Gen_SerializeSyncFuntions()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(_reliableGruop.Remote_DeserializeSync());
			sb.AppendLine(_unreliableGruop.Remote_DeserializeSync());

			sb.AppendLine(_entireGroup.Remote_DeserializeSyncAll());
			sb.AppendLine(_entireGroup.Remote_InitilaizeProperties());

			sb.AppendLine(_reliableGruop.Remote_IgnoreSync(_inheritType, isStatic: false));
			sb.AppendLine(_reliableGruop.Remote_IgnoreSync(_inheritType, isStatic: true));

			sb.AppendLine(_unreliableGruop.Remote_IgnoreSync(_inheritType, isStatic: false));
			sb.AppendLine(_unreliableGruop.Remote_IgnoreSync(_inheritType, isStatic: true));
			return sb.ToString();
		}
	}
}
