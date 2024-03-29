﻿using System;
using System.Collections.Generic;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	/// Sync Group의 생성자로 들어오는 MemberToken 배열은 윗단에서
	/// SyncType이 결정 되어서 들어온다. 

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
		private SyncDirection _direction;
		private string _modifier;
		private bool _hasAnyTargetMember;

		public SerializeSyncGroup(List<BaseMemberToken> memberTokens,
								  SyncType syncType, SyncDirection direction, string modifier)
		{
			_syncType = syncType;
			_direction = direction;
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

			_hasAnyTargetMember = false;
			foreach (var dirtyGroup in _dirtyGroups)
			{
				_hasAnyTargetMember |= dirtyGroup.HasTargetMember;
			}
		}

		public string Master_BitmaskDeclarations(GenOption option)
		{
			string accessModifier = option.InheritType switch
			{
				InheritType.Parent => "protected",
				InheritType.Child => "protected",
				InheritType.None => "private",
				_ => throw new ArgumentException($"There is no such inheritType to declar bitmask. {option.InheritType}")
			};

			StringBuilder sb = new();

			foreach (var g in _dirtyGroups)
			{
				if (g.HasChildMember)
					continue;
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeclaration, g.GetName(), accessModifier));
			}
			return sb.ToString();
		}

		public string Master_GetterSetter(GenOption option)
		{
			option.SyncType = _syncType;
			option.Direction = _direction;

			StringBuilder sb = new();
			foreach (var g in _dirtyGroups)
				sb.AppendLine(g.Master_MemberSetterSetters(option));
			return sb.ToString();
		}

		public string Master_DirtyProperty(GenOption option)
		{
			return string.Format(SyncGroupFormat.IsDirtyField, _syncType);

			// IsDirty는 프로퍼티가 아니라 필드로 변경됨
			/*
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
			*/
		}

		public string Master_SerializeSync(GenOption option)
		{
			option.SyncType = _syncType;
			option.Direction = _direction;

			StringBuilder sb = new StringBuilder();
			if (_direction == SyncDirection.FromMaster)
			{
				sb.Append(string.Format(SyncGroupFormat.MasterSerializeFunctionDeclaration, _modifier, _syncType));
			}
			else if (_direction == SyncDirection.FromRemote)
			{
				sb.Append(string.Format(SyncGroupFormat.RemoteSerializeFunctionDeclaration, _modifier, _syncType));
			}

			if (_dirtyGroups.Count == 0)
				return sb.AppendLine(" { }").ToString();

			string content;

			if (_dirtyGroups.Count == 1)
			{
				content = CodeFormat.AddIndent(master_SerializeSyncOneDirtyGroup(option));
			}
			else if (_dirtyGroups.Count == 2)
			{
				content = CodeFormat.AddIndent(master_SerializeSyncTwoDirtyGroup(option));
			}
			else
			{
				content = CodeFormat.AddIndent(master_SerializeSyncMultipleDirtyGroup(option));
			}

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string master_SerializeSyncOneDirtyGroup(GenOption option)
		{
			StringBuilder contents = new StringBuilder();
			var dirtyGroup = _dirtyGroups[0];
			string dirtyBitName = dirtyGroup.GetName();
			string tempDirtyBitName = dirtyGroup.GetTempName();

			contents.AppendLine(dirtyGroup.Master_MarkObjectDirtyBit(option, dirtyBitName));

			if (_hasAnyTargetMember)
				contents.AppendLine(string.Format(DirtyGroupFormat.JumpSerializeDirtyMask, dirtyBitName, tempDirtyBitName));
			else
				contents.AppendLine(string.Format(MemberFormat.WriteSerialize, dirtyBitName));

			contents.AppendLine(dirtyGroup.Master_MemberSerializeIfDirtys(option, dirtyBitName, tempDirtyBitName));

			if (_hasAnyTargetMember)
				contents.AppendLine(string.Format(DirtyGroupFormat.RollBackSerializeMask, tempDirtyBitName, string.Empty));

			return contents.ToString();
		}

		private string master_SerializeSyncTwoDirtyGroup(GenOption option)
		{
			StringBuilder contents = new();

			if (_hasAnyTargetMember)
				contents.AppendLine(SyncGroupFormat.CacheOriginSize);

			for (int i = 0; i < 2; i++)
			{
				var dirtyGroup = _dirtyGroups[i];
				string dirtyBitName = dirtyGroup.GetName();
				string tempDirtyBitName = dirtyGroup.GetTempName();

				contents.AppendLine(dirtyGroup.Master_MarkObjectDirtyBit(option, dirtyBitName));

				if (dirtyGroup.HasTargetMember)
					contents.AppendLine(string.Format(DirtyGroupFormat.JumpSerializeDirtyMask, dirtyBitName, tempDirtyBitName));
				else
					contents.AppendLine(string.Format(MemberFormat.WriteSerialize, dirtyBitName));

				string content = dirtyGroup.Master_MemberSerializeIfDirtys(option, dirtyBitName, tempDirtyBitName);
				CodeFormat.AddIndent(ref content);
				contents.AppendLine(string.Format(CommonFormat.IfDirtyAny, dirtyBitName, content));

				if (dirtyGroup.HasTargetMember)
					contents.AppendLine(string.Format(SyncGroupFormat.PutDirtyBitTo, tempDirtyBitName));
			}

			if (_hasAnyTargetMember)
				contents.AppendLine(string.Format(SyncGroupFormat.RollbackWriter, sizeof(byte) * 2));

			return contents.ToString();
		}

		private string master_SerializeSyncMultipleDirtyGroup(GenOption option)
		{
			StringBuilder headers = new();
			StringBuilder contents = new();

			// Set master dirty bits
			headers.AppendLine(SyncGroupFormat.MasterDirtyBitInstantiate);
			for (int i = 0; i < _dirtyGroups.Count; i++)
			{
				var dirtyGroup = _dirtyGroups[i];
				headers.AppendLine(dirtyGroup.Master_MarkObjectDirtyBit(option, dirtyGroup.GetName()));
				headers.AppendLine(string.Format(SyncGroupFormat.MasterDirtyAnyTrue, i, dirtyGroup.GetName()));
			}

			// Jump or serialize master dirty bits
			if (_hasAnyTargetMember)
				headers.AppendLine(string.Format(SyncGroupFormat.JumpDirtyBit, SyncGroupFormat.MasterDirtyBitName));
			else
				headers.AppendLine(string.Format(MemberFormat.WriteSerialize, SyncGroupFormat.MasterDirtyBitName));

			for (int i = 0; i < _dirtyGroups.Count; i++)
			{
				var dirtyGroup = _dirtyGroups[i];
				string dirtyBitName = dirtyGroup.GetName();
				string tempDirtyBitName = dirtyGroup.GetTempName();

				StringBuilder ctx = new StringBuilder(1024);

				if (dirtyGroup.HasTargetMember)
					ctx.AppendLine(string.Format(DirtyGroupFormat.JumpSerializeDirtyMask, dirtyBitName, tempDirtyBitName));
				else
					ctx.AppendLine(string.Format(MemberFormat.WriteSerialize, dirtyBitName));

				// Content
				ctx.AppendLine(dirtyGroup.Master_MemberSerializeIfDirtys(option, dirtyBitName, tempDirtyBitName));

				if (dirtyGroup.HasTargetMember)
				{
					string rollContent = string.Format(SyncGroupFormat.SetDirtyBit, SyncGroupFormat.MasterDirtyBitName, i, "false");
					CodeFormat.AddIndent(ref rollContent);
					ctx.AppendLine(string.Format(DirtyGroupFormat.RollBackSerializeMask, tempDirtyBitName, rollContent));
				}

				CodeFormat.AddIndent(ctx);
				string content = string.Format(CommonFormat.IfDirty, SyncGroupFormat.MasterDirtyBitName, i, ctx.ToString());
				contents.AppendLine(content);

			}

			headers.AppendLine(contents.ToString());

			if (_hasAnyTargetMember)
				headers.AppendLine(string.Format(DirtyGroupFormat.RollBackSerializeMask,
												 SyncGroupFormat.MasterDirtyBitName, string.Empty));

			return headers.ToString();
		}

		public string Master_ClearDirty(GenOption option)
		{
			option.SyncType = _syncType;
			option.Direction = _direction;

			if (_dirtyGroups.Count == 0)
				return string.Format(SyncGroupFormat.ClearDirtyFunctionIfEmpty, _modifier, _syncType);

			StringBuilder sb = new();
			sb.AppendLine(string.Format(SyncGroupFormat.ClearObjectDirty, _syncType));
			foreach (var d in _dirtyGroups)
				sb.AppendLine(d.Master_ClearDirtys(option));
			CodeFormat.AddIndent(sb);
			return string.Format(SyncGroupFormat.ClearDirtyFunction, _modifier, _syncType, sb.ToString());
		}
	}

	public class DeserializeSyncGroup
	{
		private List<DirtyGroup> _dirtyGroups;
		private SyncType _syncType;
		private SyncDirection _direction;
		private string _modifier;

		public DeserializeSyncGroup(List<BaseMemberToken> memberTokens,
									SyncType syncType, SyncDirection direction, string modifier)
		{
			_syncType = syncType;
			_direction = direction;
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

		public string Remote_DeserializeSync(GenOption option)
		{
			option.SyncType = _syncType;
			option.Direction = _direction;

			StringBuilder sb = new StringBuilder();
			if (_direction == SyncDirection.FromMaster)
			{
				sb.Append(string.Format(SyncGroupFormat.MasterDeserializeFunctionDeclaration, _modifier, _syncType));
			}
			else
			{
				sb.Append(string.Format(SyncGroupFormat.RemoteDeserializeFunctionDeclaration, _modifier, _syncType));
			}

			if (_dirtyGroups.Count == 0)
				return sb.AppendLine(SyncGroupFormat.EmptyDeserializeImplementation).ToString();

			string content;
			if (_dirtyGroups.Count == 1)
			{
				content = remote_DeserializeSyncOneDirtyGroup(option);
			}
			else if (_dirtyGroups.Count == 2)
			{
				content = remote_DeserializeSyncTwoDirtyGroup(option);
			}
			else
			{
				content = remote_DeserializeSyncMultipleDirtyGroup(option);
			}

			CodeFormat.AddIndent(ref content);

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("\treturn true;");
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string remote_DeserializeSyncOneDirtyGroup(GenOption option)
		{
			return _dirtyGroups[0].Remote_MemberDeserializeIfDirtys(option);
		}

		private string remote_DeserializeSyncTwoDirtyGroup(GenOption option)
		{
			StringBuilder sb = new();
			for (int i = 0; i < 2; i++)
			{
				var group = _dirtyGroups[i];
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, group.GetTempName()));
				string content = group.Remote_MemberDeserializeIfDirtys(option, readDirtyBit: false);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirtyAny, group.GetTempName(), content));
			}
			return sb.ToString();
		}

		private string remote_DeserializeSyncMultipleDirtyGroup(GenOption option)
		{
			StringBuilder headers = new();
			headers.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize,
											 SyncGroupFormat.MasterDirtyBitName));

			StringBuilder contents = new();
			for (int i = 0; i < _dirtyGroups.Count; i++)
			{
				string content = _dirtyGroups[i].Remote_MemberDeserializeIfDirtys(option);
				CodeFormat.AddIndent(ref content);
				contents.AppendLine(string.Format(CommonFormat.IfDirty, SyncGroupFormat.MasterDirtyBitName, i, content));
			}
			headers.AppendLine(contents.ToString());
			return headers.ToString();
		}

		public string Remote_IgnoreSync(GenOption option, bool isStatic)
		{
			option.SyncType = _syncType;
			option.Direction = _direction;

			StringBuilder sb = new StringBuilder();

			if (isStatic)
			{
				if (option.InheritType == InheritType.Child)
				{
					sb.Append(string.Format(SyncGroupFormat.IgnoreSyncFunctionDeclarationStaticNew, _syncType));
				}
				else
				{
					sb.Append(string.Format(SyncGroupFormat.IgnoreSyncFunctionDeclarationStatic, _syncType));
				}
			}
			else
			{
				sb.Append(string.Format(SyncGroupFormat.IgnoreSyncFunctionDeclaration,
										_modifier, _syncType));
			}

			if (_dirtyGroups.Count == 0)
			{
				return sb.AppendLine(" { }").ToString();
			}

			string content;
			if (_dirtyGroups.Count == 1)
				content = remote_IgnoreSyncOneDirtyGroup(option, isStatic);
			else if (_dirtyGroups.Count == 2)
				content = remote_IgnoreSyncTwoDirtyGroup(option, isStatic);
			else
				content = remote_IgnoreSyncMultipleDirtyGroup(option, isStatic);

			CodeFormat.AddIndent(ref content);

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(content);
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string remote_IgnoreSyncOneDirtyGroup(GenOption option, bool isStatic)
		{
			return _dirtyGroups[0].Remote_IgnoreMembers(option, isStatic);
		}

		private string remote_IgnoreSyncTwoDirtyGroup(GenOption option, bool isStatic)
		{
			StringBuilder sb = new();
			for (int i = 0; i < 2; i++)
			{
				var group = _dirtyGroups[i];
				sb.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize, group.GetTempName()));
				string content = group.Remote_IgnoreMembers(option, isStatic, readDirtyBit: false);
				CodeFormat.AddIndent(ref content);
				sb.AppendLine(string.Format(CommonFormat.IfDirtyAny, group.GetTempName(), content));
			}
			return sb.ToString();
		}

		private string remote_IgnoreSyncMultipleDirtyGroup(GenOption option, bool isStatic)
		{
			StringBuilder headers = new();
			headers.AppendLine(string.Format(SyncGroupFormat.DirtyBitDeserialize,
											 SyncGroupFormat.MasterDirtyBitName));

			StringBuilder contents = new();
			for (int i = 0; i < _dirtyGroups.Count; i++)
			{
				string content = _dirtyGroups[i].Remote_IgnoreMembers(option, isStatic);
				CodeFormat.AddIndent(ref content);
				contents.AppendLine(string.Format(CommonFormat.IfDirty, SyncGroupFormat.MasterDirtyBitName, i, content));
			}
			headers.AppendLine(contents.ToString());
			return headers.ToString();
		}
	}
}
