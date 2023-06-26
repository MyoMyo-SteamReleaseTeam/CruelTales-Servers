using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using CT.Common.Synchronizations;
using CT.CorePatcher.Exceptions;
using CT.CorePatcher.SynchronizationsCodeGen.PropertyDefine;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public class SyncObjectInfo
	{
		public string ObjectName => _objectName;
		private string _objectName;
		private string _modifier;
		public  bool IsNetworkObject { get; private set; } = false;
		public int Capacity { get; private set; } = 0;
		public bool MultiplyByMaxUser { get; private set; } = false;
		public bool _isDebugObject = false;

		[AllowNull] private SerializeDirectionGroup _masterSerializeGroup;
		[AllowNull] private DeserializeDirectionGroup _masterDeserializeGroup;

		[AllowNull] private SerializeDirectionGroup _remoteSerializeGroup;
		[AllowNull] private DeserializeDirectionGroup _remoteDeserializeGroup;

		private string _masterInheritName = CommonFormat.MasterInterfaceName;
		private string _remoteInheritName = CommonFormat.RemoteInterfaceName;

		private List<MemberToken> _masterSideMembers;
		private List<MemberToken> _remoteSideMembers;

		public bool HasReliable { get; set; }
		public bool HasUnreliable { get; set; }
		public bool HasTarget { get; set; }

		public SyncObjectInfo(string objectName,
							  List<MemberToken> masterSideMembers,
							  List<MemberToken> remoteSideMembers,
							  bool isNetworkObject,
							  int capacity,
							  bool multiplyByMaxUser,
							  bool isDebugObject)
		{
			_objectName = objectName;
			IsNetworkObject = isNetworkObject;
			Capacity = capacity;
			_isDebugObject = isDebugObject;
			MultiplyByMaxUser = multiplyByMaxUser;
			_modifier = IsNetworkObject ? "override " : string.Empty;

			_masterSideMembers = masterSideMembers;
			_remoteSideMembers = remoteSideMembers;

			if (IsNetworkObject)
			{
				_masterInheritName = string.Empty; //CommonFormat.MasterNetworkObjectTypeName;
				_remoteInheritName = string.Empty; //CommonFormat.RemoteNetworkObjectTypeName;
			}
		}

		public void InitializeSyncGroup()
		{
			// Add forward direction
			_masterSerializeGroup = new(_masterSideMembers, SyncDirection.FromMaster, _modifier);
			_masterDeserializeGroup = new(_remoteSideMembers, SyncDirection.FromMaster, _modifier);

			// Add reverse direction
			_remoteSerializeGroup = new(_remoteSideMembers, SyncDirection.FromRemote, _modifier);
			_remoteDeserializeGroup = new(_masterSideMembers, SyncDirection.FromRemote, _modifier);
		}

		public void CheckValidation()
		{
			var syncObj = _masterSideMembers
				.Where((m) => m.Member is SyncObjectMemberToken);

			foreach (var m in syncObj)
			{
				string typeName = m.Member.TypeName;
				if (typeName.Contains(NameTable.SyncList))
				{
					continue;
				}

				if (!SynchronizerGenerator.TryGetSyncObjectByTypeName(typeName, out var syncObjectInfo) || syncObjectInfo == null)
				{
					PatcherConsole.PrintError($"There is no such type in sync object info : {typeName}");
					throw new System.Exception();
				}

				bool hasReliable = syncObjectInfo.HasReliable;
				bool hasUnreliable = syncObjectInfo.HasUnreliable;

				if (m.SyncType.IsReliable() != hasReliable || m.SyncType.IsUnreliable() != hasUnreliable)
				{
					PatcherConsole.PrintError($"Attribute error on {this.ObjectName}");
					PatcherConsole.PrintError($"You set wrong attribute on {typeName}!");
					PatcherConsole.PrintError($"The {typeName}'s sync attribute is currently {m.SyncType}.");
					PatcherConsole.PrintError($"{typeName} : HasReliable [{hasReliable}], HasUnreliable [{hasUnreliable}]");
					throw new System.Exception();
				}
			}
		}

		public string Gen_MasterCode()
		{
			string genCode = gnerateCode(CommonFormat.MasterUsingStatements,
										 CommonFormat.MasterNamespace,
										 SyncDirection.FromMaster,
										 _masterSideMembers,
										 _remoteSideMembers,
										 _masterSerializeGroup,
										 _masterDeserializeGroup,
										 _masterInheritName);

			CodeFormat.ReformCode(ref genCode, startFromNamespace: true);
			return genCode;
		}

		public string Gen_RemoteCode()
		{
			string usingStatements = _isDebugObject ? 
				CommonFormat.DebugRemoteUsingStatements : 
				CommonFormat.RemoteUsingStatements;

			string genCode = gnerateCode(usingStatements,
										 CommonFormat.RemoteNamespace,
										 SyncDirection.FromRemote,
										 _remoteSideMembers,
										 _masterSideMembers,
										 _remoteSerializeGroup,
										 _remoteDeserializeGroup,
										 _remoteInheritName);

			CodeFormat.ReformCode(ref genCode, startFromNamespace: true);
			return genCode;
		}

		private string getNetworkTypeDefinition()
		{
			if (_isDebugObject)
				return string.Empty;

			if (!IsNetworkObject)
				return string.Empty;

			return string.Format(CommonFormat.NetworkTypeDeclaration,
								 CommonFormat.NetworkObjectTypeTypeName, _objectName);
		}

		private string gnerateCode(string usingStatements,
								   string namespaceName,
								   SyncDirection direction,
								   List<MemberToken> forwardMember,
								   List<MemberToken> backwardMember,
								   SerializeDirectionGroup forward,
								   DeserializeDirectionGroup backward,
								   string inheritName)
		{
			StringBuilder sb = new();
			sb.AppendLine(getNetworkTypeDefinition());

			foreach (var m in forwardMember)
				sb.AppendLine(m.Member.Master_Declaration(direction));
			foreach (var m in backwardMember)
				sb.AppendLine(m.Member.Remote_Declaration(direction.Reverse()));

			sb.AppendLine(forward.Gen_SynchronizerProperties());
			sb.AppendLine(forward.Gen_SerializeSyncFuntions());
			sb.AppendLine(backward.Gen_SerializeSyncFuntions());
			CodeFormat.AddIndent(sb);

			string content;
			if (string.IsNullOrWhiteSpace(inheritName))
			{
				content = string.Format(CommonFormat.SyncObjectFormat, _objectName, sb.ToString());
			}
			else
			{
				content = string.Format(CommonFormat.SyncObjectFormatHasInherit, _objectName, inheritName, sb.ToString());
			}
			CodeFormat.AddIndent(ref content);

			return string.Format(CommonFormat.FileFormat, usingStatements, namespaceName, content);
		}
	}
}