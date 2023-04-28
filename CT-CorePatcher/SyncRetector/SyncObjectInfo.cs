using System.Collections.Generic;
using System.Text;
using CT.Common.Synchronizations;

namespace CT.CorePatcher.SyncRetector
{
	public class SyncObjectInfo
	{
		private string _objectName;
		private string _modifier;
		private bool _isNetworkObject = false;

		private SerializeDirectionGroup _masterSerializeGroup;
		private DeserializeDirectionGroup _masterDeserializeGroup;

		private SerializeDirectionGroup _remoteSerializeGroup;
		private DeserializeDirectionGroup _remoteDeserializeGroup;

		private string _masterInheritName = CommonFormat.InterfaceName;
		private string _remoteInheritName = CommonFormat.InterfaceName;

		private List<MemberToken> _masterSideMembers;
		private List<MemberToken> _remoteSideMembers;

		public SyncObjectInfo(string objectName,
							  List<MemberToken> masterSideMembers,
							  List<MemberToken> remoteSideMembers,
							  bool isNetworkObject)
		{
			_objectName = objectName;
			_modifier = _isNetworkObject ? "override" : string.Empty;
			_isNetworkObject = isNetworkObject;

			_masterSideMembers = masterSideMembers;
			_remoteSideMembers = remoteSideMembers;

			if (_isNetworkObject)
			{
				_masterInheritName = CommonFormat.MasterNetworkObjectTypeName;
				_remoteInheritName = CommonFormat.RemoteNetworkObjectTypeName;
			}

			// Add forward direction
			_masterSerializeGroup = new(_masterSideMembers, _modifier);
			_masterDeserializeGroup = new(_remoteSideMembers, _modifier);

			// Add reverse direction
			_remoteSerializeGroup = new(_remoteSideMembers, _modifier);
			_remoteDeserializeGroup = new(_masterSideMembers, _modifier);
		}

		public string Gen_MasterCode()
		{
			StringBuilder sb = new();
			AddNetworkTypeDefinition(sb);

			string declaration = gnerateDeclarationCode(SyncDirection.FromMaster, _masterSideMembers, _remoteSideMembers);
			sb.AppendLine(declaration);

			string content = generateContentCode(_masterSerializeGroup, _masterDeserializeGroup);
			return sb.AppendLine(string.Format(CommonFormat.SyncObjectFormat,
											   _objectName, _masterInheritName, content)).ToString();
		}

		public string Gen_RemoteCode()
		{
			StringBuilder sb = new();
			AddNetworkTypeDefinition(sb);

			string declaration = gnerateDeclarationCode(SyncDirection.FromRemote, _remoteSideMembers, _masterSideMembers);
			sb.AppendLine(declaration);

			string content = generateContentCode(_remoteSerializeGroup, _remoteDeserializeGroup);
			return sb.AppendLine(string.Format(CommonFormat.SyncObjectFormat,
											   _objectName, _remoteInheritName, content)).ToString();
		}

		private string gnerateDeclarationCode(SyncDirection direction,
											  List<MemberToken> serializeSideMembers,
											  List<MemberToken> deserializeSideMembers)
		{
			StringBuilder sb = new();
			foreach (var m in serializeSideMembers)
				sb.AppendLine(m.Member.Master_Declaration(direction));
			foreach (var m in deserializeSideMembers)
				sb.AppendLine(m.Member.Remote_Declaration(direction));
			return sb.ToString();
		}

		private string generateContentCode(SerializeDirectionGroup serialize,
										   DeserializeDirectionGroup deserialize)
		{
			StringBuilder sb = new();
			sb.AppendLine(serialize.Gen_SynchronizerProperties());
			sb.AppendLine(serialize.Gen_SerializeSyncFuntions());
			sb.AppendLine(deserialize.Gen_SerializeSyncFuntions());
			CodeFormat.AddIndent(sb);
			return sb.ToString();
		}

		private void AddNetworkTypeDefinition(StringBuilder sb)
		{
			if (_isNetworkObject)
			{
				sb.AppendLine(string.Format(CommonFormat.NetworkTypeDeclaration,
											CommonFormat.NetworkObjectTypeTypeName, _objectName));
			}
		}
	}
}