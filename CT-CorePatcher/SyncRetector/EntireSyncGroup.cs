﻿using System.Collections.Generic;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.SyncRetector.PropertyDefine;

namespace CT.CorePatcher.SyncRetector
{
	public class EntireSerializeSyncGroup
	{
		List<BaseMemberToken> _members;
		private SyncType _syncType;
		private string _modifier;

		public EntireSerializeSyncGroup(List<BaseMemberToken> members, SyncType syncType, string modifier)
		{
			_members = members;
			_syncType = syncType;
			_modifier = modifier;
		}

		public string Master_SerializeSyncAll()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.EntireSerializeFunctionDeclaration, _modifier, _syncType));

			if (_members.Count == 0)
				return sb.AppendLine(" { }").ToString();

			StringBuilder contents = new();
			foreach (var m in _members)
				contents.AppendLine(m.Master_SerializeByWriter());
			CodeFormat.AddIndent(contents);

			sb.AppendLine("{");
			sb.AppendLine(contents.ToString());
			sb.AppendLine("}");

			return sb.ToString();
		}
	}

	public class EntireDeserializeSyncGroup
	{
		List<BaseMemberToken> _members;
		private SyncType _syncType;
		private string _modifier;

		public EntireDeserializeSyncGroup(List<BaseMemberToken> members, SyncType syncType, string modifier)
		{
			_members = members;
			_syncType = syncType;
			_modifier = modifier;
		}

		public string Remote_DeserializeSyncAll()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.EntireDeserializeFunctionDeclaration, _modifier, _syncType));

			if (_members.Count == 0)
				return sb.AppendLine(" { }").ToString();

			StringBuilder contents = new();
			foreach (var m in _members)
				contents.AppendLine(m.Remote_DeserializeByReader());
			CodeFormat.AddIndent(contents);

			sb.AppendLine("{");
			sb.AppendLine(contents.ToString());
			sb.AppendLine("}");

			return sb.ToString();
		}
	}
}
