using System.Collections.Generic;
using System.Linq;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class EntireSerializeSyncGroup
	{
		List<BaseMemberToken> _members;
		private SyncType _syncType;
		private SyncDirection _direction;
		private string _modifier;

		public EntireSerializeSyncGroup(List<BaseMemberToken> members,
										SyncType syncType, SyncDirection direction, string modifier)
		{
			_members = members;
			_syncType = syncType;
			_direction = direction;
			_modifier = modifier;
		}

		public bool HasProperty()
		{
			return _members.Where(m => m is not FunctionMemberToken).Any();
		}

		public string Master_SerializeSyncAll()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.EntireSerializeFunctionDeclaration, _modifier,
									SyncGroupFormat.EntireFunctionSuffix));

			if (!HasProperty())
				return sb.AppendLine(" { }").ToString();

			StringBuilder contents = new();
			foreach (var m in _members)
			{
				if (m is FunctionMemberToken)
					continue;
				contents.AppendLine(m.Master_SerializeByWriter(_syncType));
			}
			CodeFormat.AddIndent(contents);

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(contents.ToString());
			sb.AppendLine("}");

			return sb.ToString();
		}

		public string Master_InitilaizeProperties()
		{
			StringBuilder sb = new();
			sb.Append(string.Format(SyncGroupFormat.InitializeProperties, _modifier));

			if (!HasProperty())
				return sb.AppendLine(" { }").ToString();

			StringBuilder contents = new();
			foreach (var m in _members)
			{
				if (m is FunctionMemberToken)
					continue;
				contents.AppendLine(m.Master_InitializeProperty());
			}
			CodeFormat.AddIndent(contents);

			sb.AppendLine("");
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
		private SyncDirection _direction;
		private string _modifier;

		public EntireDeserializeSyncGroup(List<BaseMemberToken> members,
										  SyncType syncType, SyncDirection direction, string modifier)
		{
			_members = members;
			_syncType = syncType;
			_direction = direction;
			_modifier = modifier;
		}

		public bool HasProperty()
		{
			return _members.Where(m => m is not FunctionMemberToken).Any();
		}

		public string Remote_DeserializeSyncAll()
		{
			if (_direction == SyncDirection.FromMaster)
			{
				return string.Empty;
			}

			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format(SyncGroupFormat.EntireDeserializeFunctionDeclaration, _modifier,
									SyncGroupFormat.EntireFunctionSuffix));

			if (!HasProperty())
				return sb.AppendLine(SyncGroupFormat.EmptyDeserializeImplementation).ToString();

			StringBuilder contents = new();
			foreach (var m in _members)
			{
				if (m is FunctionMemberToken)
					continue;
				contents.AppendLine(m.Remote_DeserializeByReader(_syncType));
			}
			CodeFormat.AddIndent(contents);

			sb.AppendLine("");
			sb.AppendLine("{");
			sb.AppendLine(contents.ToString());
			sb.AppendLine("\treturn true;");
			sb.AppendLine("}");

			return sb.ToString();
		}
	}
}
