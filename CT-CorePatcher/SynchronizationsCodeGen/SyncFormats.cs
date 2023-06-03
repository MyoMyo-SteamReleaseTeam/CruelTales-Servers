using CT.Common.Synchronizations;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public static class NameTable
	{
		public static string NetworkPlayerParameterName => "player";
		public static string NetworkPlayerTypeName => "NetworkPlayer";
		public static string NetworkPlayerParameter => $"{NetworkPlayerTypeName} {NetworkPlayerParameterName}";
		public static string SerializeTargetName => "Target";
	}

	public static class CommonFormat
	{
		public static string MasterUsingStatements =>
@"using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;";

		public static string RemoteUsingStatements =>
@"using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTC.Networks.Synchronizations;";

		public static string MasterNamespace => $"CTS.Instance.SyncObjects";
		public static string RemoteNamespace => $"CTC.Networks.SyncObjects.TestSyncObjects";

		public static string MasterNetworkObjectTypeName => "MasterNetworkObject";
		public static string RemoteNetworkObjectTypeName => "RemoteNetworkObject";
		public static string NetworkObjectTypeTypeName => "NetworkObjectType";
		public static string InterfaceName => "ISynchronizable";

		/// <summary>
		/// {0} NetworkObjectType type name<br/>
		/// {1} Object name<br/>
		/// </summary>
		public static string NetworkTypeDeclaration => @"public override {0} Type => {0}.{1};";

		public static string MasterPrefix => "Master_";
		public static string RemotePrefix => "Remote_";

		/// <summary>
		/// {0} using statements<br/>
		/// {1} namespace<br/>
		/// {2} Content<br/>
		/// </summary>
		public static string FileFormat =>
@"#nullable enable
#pragma warning disable CS0649

{0}

namespace {1}
{{
{2}
}}

#pragma warning restore CS0649";

		/// <summary>
		/// {0} Object name<br/>
		/// {1} Content<br/>
		/// </summary>
		public static string SyncObjectFormat =>
@"[Serializable]
public partial class {0}
{{
{1}
}}
";

		/// <summary>
		/// {0} Object name<br/>
		/// {1} Inherit type name<br/>
		/// {2} Content<br/>
		/// </summary>
		public static string SyncObjectFormatHasInherit =>
@"[Serializable]
public partial class {0} : {1}
{{
{2}
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Index<br/>
		/// {2} Content<br/>
		/// </summary>
		public static string IfDirty =>
@"if ({0}[{1}])
{{
{2}
}}";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Content<br/>
		/// </summary>
		public static string IfDirtyAny =>
@"if ({0}.AnyTrue())
{{
{1}
}}";
	}

	public static class SyncGroupFormat
	{
		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// {2} Content<br/>
		public static string IsDirty =>
@"public {0}bool IsDirty{1}
{{
	get
	{{
		bool isDirty = false;
{2}
		return isDirty;
	}}
}}";

		/// <summary>
		/// {0} Dirty bit name<br/>
		/// </summary>
		public static string IsBitmaskDirty => @"isDirty |= {0}.AnyTrue();";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		public static string IsDirtyIfNoElement => @"public {0}bool IsDirty{1} => false;";

		public static string MasterDirtyBitName => @"masterDirty";

		/// <summary>
		/// {0} Master index<br/>
		/// {1} Dirty bit name<br/>
		/// </summary>
		public static string MasterDirtyAnyTrue => @"masterDirty[{0}] = {1}.AnyTrue();";

		public static string MasterDirtySerialize => @"masterDirty.Serialize(writer);";

		public static string MasterDirtyBitInstantiate => @"BitmaskByte masterDirty = new BitmaskByte();";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string SerializeFunctionDeclaration => @"public {0}void SerializeSync{1}(NetworkPlayer player, IPacketWriter writer)";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string MasterDeserializeFunctionDeclaration => @"public {0}bool TryDeserializeSync{1}(NetworkPlayer player, IPacketReader reader)";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string RemoteDeserializeFunctionDeclaration => @"public {0}bool TryDeserializeSync{1}(IPacketReader reader)";

		/// <summary>
		/// {0} SyncType<br/>
		/// </summary>
		public static string IgnoreSyncFunctionDeclaration => @"public override void IgnoreSync{0}(IPacketReader reader)";

		/// <summary>
		/// {0} SyncType<br/>
		/// </summary>
		public static string IgnoreSyncFunctionDeclarationStatic => @"public static void IgnoreSyncStatic{0}(IPacketReader reader)";

		public static string EntireFunctionSuffix => "EveryProperty";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string EntireSerializeFunctionDeclaration => @"public {0}void Serialize{1}(IPacketWriter writer)";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string EntireDeserializeFunctionDeclaration => @"public {0}bool TryDeserialize{1}(IPacketReader reader)";

		public static string EmptyDeserializeImplementation => " => true;";

		/// <summary>
		/// {0} Dirty bit name<br/>
		/// </summary>
		public static string DirtyBitDeserialize => @"BitmaskByte {0} = reader.ReadBitmaskByte();";

		/// <summary>
		/// {0} Dirty bit name<br/>
		/// </summary>
		public static string DirtyBitDeclaration => @"private BitmaskByte {0} = new();";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// {2} Content<br/>
		/// </summary>
		public static string ClearDirtyFunction =>
@"public {0}void ClearDirty{1}()
{{
{2}
}}";

		/// <summary>
		/// {0} Modifire<br/>
		/// {1} SyncType<br/>
		/// {2} Content<br/>
		/// </summary>
		public static string ClearDirtyFunctionIfEmpty => @"public {0}void ClearDirty{1}() {{ }}";

		/// <summary>
		/// {0} Modifire<br/>
		/// </summary>
		public static string InitializeProperties => @"public {0}void InitializeProperties()";
	}

	public static class FuncMemberFormat
	{
		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Access modifier<br/>
		/// {2} Function name<br/>
		/// {3} Parameter declaration <br/>
		/// </summary>
		public static string Declaration =>
@"[{0}]
{1} partial void {2}({3});";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Access modifier<br/>
		/// {2} Function name<br/>
		/// {3} Parameter declaration <br/>
		/// </summary>
		public static string DeclarationFromRemote =>
@"[{0}]
{1} partial void {2}(NetworkPlayer player, {3});";

		/// <summary>
		/// {0} Access modifier<br/>
		/// {1} Function name<br/>
		/// {2} Parameter declaration<br/>
		/// {3} Callstack tuple enqueue value<br/>
		/// {4} Callstack tuple declaration<br/>
		/// {5} Dirty bits name<br/>
		/// {6} Dirty index<br/>
		/// </summary>
		public static string CallWithStack =>
@"{0} partial void {1}({2})
{{
	{1}Callstack.Add({3});
	{5}[{6}] = true;
}}
private List<{4}> {1}Callstack = new(4);";

		/// <summary>
		/// {0} Access modifier<br/>
		/// {1} Function name<br/>
		/// {2} Dirty bits name<br/>
		/// {3} Member index<br/>
		/// </summary>
		public static string CallWithStackVoid =>
@"{0} partial void {1}()
{{
	{1}CallstackCount++;
	{2}[{3}] = true;
}}
private byte {1}CallstackCount = 0;";

		/// <summary>
		/// {0} Access modifier<br/>
		/// {1} Function name<br/>
		/// {2} Dirty bits name<br/>
		/// {3} Member index<br/>
		/// </summary>
		public static string TargetCallWithStackVoid =>
@"{0} partial void {1}(NetworkPlayer player)
{{
	{1}CallstackCount++;
	{2}[{3}] = true;
}}
private byte {1}CallstackCount = 0;
private Dictionary<NetworkPlayer, byte> {1}CallstackCount = new(7);";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Callstack serialize content<br/>
		/// </summary>
		public static string SerializeIfDirty =>
@"byte count = (byte){0}Callstack.Count;
writer.Put(count);
for (int i = 0; i < count; i++)
{{
	var arg = {0}Callstack[i];
{1}
}}";

		public static string TempArgumentName => @"arg";

		/// <summary>
		/// {0} Function name<br/>
		/// </summary>
		public static string SerializeIfDirtyVoid =>
@"writer.Put((byte){0}CallstackCount);";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Read parameters content<br/>
		/// {2} Call parameters<br/>
		/// </summary>
		public static string DeserializeIfDirty =>
@"byte count = reader.ReadByte();
for (int i = 0; i < count; i++)
{{
{1}
	{0}({2});
}}";

		/// <summary>
		/// {0} Function name<br/>
		/// </summary>
		public static string DeserializeIfDirtyVoid =>
@"byte count = reader.ReadByte();
for (int i = 0; i < count; i++)
{{
	{0}();
}}";

		public static string IgnoreVoid => @"reader.Ignore(1);";

		/// <summary>
		/// {0} Ignore contents<br/>
		/// </summary>
		public static string IgnoreFunction =>
@"byte count = reader.ReadByte();
for (int i = 0; i < count; i++)
{{
{0}
}}";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Parameter name<br/>
		/// {2} CLR type name<br/>
		/// </summary>
		//public static string TempReadPrimitiveTypeProperty => @"{0} {1} = reader.Read{2}();";
		public static string TempReadPrimitiveTypeProperty => @"if (!reader.TryRead{2}(out {0} {1})) return false;";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Parameter name<br/>
		/// </summary>
		public static string TempReadByDeserializerStruct =>
@"{0} {1} = new();
if (!{1}.TryDeserialize(reader)) return false;";

		/// <summary>
		/// {0} Enum type name<br/>
		/// {1} Parameter name<br/>
		/// {2} CLR enum size type name<br/>
		/// </summary>
		//public static string TempReadEnum => @"{0} {1} = ({0})reader.Read{2}();";
		public static string TempReadEnum =>
@"if (!reader.TryRead{2}(out var {1}Value)) return false;
{0} {1} = ({0}){1}Value;";

		/// <summary>
		/// {0} Function name<br/>
		/// </summary>
		public static string ClearCallStack => @"{0}Callstack.Clear();";
	}

	public static class MemberFormat
	{
		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Type name<br/>
		/// {2} Property name<br/>
		/// {3} Initialize<br/>
		/// </summary>
		public static string MasterDeclaration =>
@"[{0}]
private {1} {2}{3};";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Type name<br/>
		/// {2} Private property name<br/>
		/// {3} Public property name<br/>
		/// {4} Initialize<br/>
		/// </summary>
		public static string RemoteDeclaration =>
@"[{0}]
private {1} {2}{4};
public event Action<{1}>? On{3}Changed;";

		public static string NewInitializer => " = new()";

		/// <summary>
		/// {0} Access modifier<br/>
		/// {1} Type name<br/>
		/// {2} Public property name<br/>
		/// {3} Private member name<br/>
		/// {4} Dirty bits name<br/>
		/// {5} Dirty index<br/>
		/// </summary>
		public static string GetterSetter =>
@"{0} {1} {2}
{{
	get => {3};
	set
	{{
		if ({3} == value) return;
		{3} = value;
		{4}[{5}] = true;
	}}
}}";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static string WritePut => @"writer.Put({0});";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static string WriteSerialize => @"{0}.Serialize(writer);";

		/// <summary>
		/// {0} Enum size type name<br/>
		/// {1} Private member name<br/>
		/// </summary>
		public static string WriteEnum => @"writer.Put(({0}){1});";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string WriteSyncObjectEntire => @"{0}.Serialize{1}(writer);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string WriteSyncObject => @"{0}.SerializeSync{1}(writer);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} CLR type name<br/>
		/// </summary>
		public static string ReadEmbededTypeProperty => @"if (!reader.TryRead{1}(out {0})) return false;";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static string ReadByDeserializer => @"if (!{0}.TryDeserialize(reader)) return false;";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Enum type name<br/>
		/// {2} CLR enum size type name<br/>
		/// </summary>
		public static string ReadEnum =>
@"if (!reader.TryRead{2}(out var {0}Value)) return false;
{0} = ({1}){0}Value;";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Read function name<br/>
		/// </summary>
		public static string ReadSyncObject => @"if (!{0}.Deserialize{1}(reader)) return false;";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Default content<br/>
		/// </summary>
		public static string InitializeProperty => @"{0} = {1};";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static string InitializeSyncObjectProperty => @"{0}.InitializeProperties();";

		/// <summary>
		/// {0} Public property name<br/>
		/// {1} Private property name<br/>
		/// </summary>
		public static string CallbackEvent => @"On{0}Changed?.Invoke({1});";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string IsDirtyBinder => @"isDirty |= {0}.IsDirty{1};";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string ClearDirty => @"{0}.ClearDirty{1}();";

		/// <summary>
		/// {0} Primitive data size constant<br/>
		/// </summary>
		public static string IgnorePrimitive => @"reader.Ignore({0});";

		/// <summary>
		/// {0} Value type name<br/>
		/// </summary>
		public static string IgnoreValueType => @"{0}.IgnoreStatic(reader);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} SyncType<br/>
		/// </summary>
		public static string IgnoreObjectType => @"{0}.IgnoreSync{1}(reader);";

		public static string GetPrivateName(string name)
		{
			if (name[0] != '_')
			{
				name = '_' + name;
			}
			if (name[1].IsUpperCase())
			{
				name = '_' + $"{name[1]}".ToLower() + name[2..];
			}

			return name;
		}

		public static string GetPublicName(string name)
		{
			if (name[0] == '_')
				name = name[1..];

			return $"{name[0]}".ToUpper() + name[1..];
		}

		public static string GetSyncObjectAttribute(SyncType syncType, SyncDirection syncDirection)
		{
			string attributeName = "SyncObject";
			string arguments = getAttributeArgument(syncType, syncDirection);
			return string.IsNullOrWhiteSpace(arguments) ? attributeName : $"{attributeName}({arguments})";
		}

		public static string GetSyncVarAttribute(SyncType syncType, SyncDirection syncDirection)
		{
			string attributeName = "SyncVar";
			string arguments = getAttributeArgument(syncType, syncDirection);
			return string.IsNullOrWhiteSpace(arguments) ? attributeName : $"{attributeName}({arguments})";
		}

		public static string GetSyncRpcAttribute(SyncType syncType, SyncDirection syncDirection)
		{
			string attributeName = "SyncRpc";
			string arguments = getAttributeArgument(syncType, syncDirection);
			return string.IsNullOrWhiteSpace(arguments) ? attributeName : $"{attributeName}({arguments})";
		}

		private static string getAttributeArgument(SyncType syncType, SyncDirection direction)
		{
			string arguments = string.Empty;

			if (direction != SyncDirection.FromMaster)
			{
				arguments += $"dir: {nameof(SyncDirection)}.{direction}";
			}

			if (syncType != SyncType.Reliable)
			{
				arguments += string.IsNullOrWhiteSpace(arguments) ?
					$"{nameof(SyncType)}.{syncType}" :
					$", sync: {nameof(SyncType)}.{syncType}";
			}

			return arguments;
		}
	}
}
