using System;
using System.Collections.Generic;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;

namespace CT.CorePatcher.SyncRetector.Previous
{
	public class GenerateOption
	{
		public string ObjectName = string.Empty;
		public string SerializeFunctionName = string.Empty;
		public string DeserializeFunctionName = string.Empty;
		public string ClearFunctionName = string.Empty;
		public string Modifier = string.Empty;
		public string BitmaskTypeName => nameof(BitmaskByte);
		public SyncType SyncType;

		public List<SyncPropertyToken> Properties { get; private set; }
		public List<SyncFunctionToken> Functions { get; private set; }

		public bool HasProperties => Properties.Count > 0;
		public bool HasFunctions => Functions.Count > 0;
		public bool HasSyncElement => HasProperties && HasFunctions;

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Dirty bits content<br/>
		/// </summary>
		public readonly string IsDirtyGetter =
@"public {0}bool IsDirtyReliable
{{
	get
	{{
		bool isDirty = false;
{1}
		return isDirty;
	}}
}}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// </summary>
		public readonly string IsDirtyNoElement = @"public {0}bool IsDirtyReliable => false;";

		/// <summary>
		/// {0} Dirty bits Count<br/>
		/// </summary>
		public readonly string PropertyDirtyBitName = @"_propertyDirty_{0}";

		/// <summary>
		/// {0} Dirty bits Count<br/>
		/// </summary>
		public readonly string FunctionDirtyBitName = @"_rpcDirty_{0}";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// </summary>
		public readonly string IsDirtyBinder = @"isDirty |= {0}.AnyTrue();";

		/// <summary>
		/// {0} Private property name<br/>
		/// </summary>
		public readonly string IsObjectDirtyBinder = @"isDirty |= {0}.IsDirty;";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Private name<br/>
		/// </summary>
		public readonly string CheckSyncObjectDirty = @"{0}[{1}] = {2}.IsDirtyReliable;";

		public GenerateOption(SyncType syncType,
							  List<SyncPropertyToken> properties,
							  List<SyncFunctionToken> functions)
		{
			SyncType = syncType;
			Properties = properties;
			Functions = functions;

			switch (SyncType)
			{
				case SyncType.Reliable:
					SerializeFunctionName = nameof(ISynchronizable.SerializeSyncReliable);
					DeserializeFunctionName = nameof(ISynchronizable.DeserializeSyncReliable);
					ClearFunctionName = nameof(ISynchronizable.ClearDirtyReliable);
					break;
				case SyncType.Unreliable:
					SerializeFunctionName = nameof(ISynchronizable.SerializeSyncUnreliable);
					DeserializeFunctionName = nameof(ISynchronizable.DeserializeSyncUnreliable);
					ClearFunctionName = nameof(ISynchronizable.ClearDirtyUnreliable);
					break;
				case SyncType.RelibaleOrUnreliable:
					SerializeFunctionName = nameof(ISynchronizable.SerializeEveryProperty);
					DeserializeFunctionName = nameof(ISynchronizable.DeserializeEveryProperty);
					break;
			}

			if (SyncType == SyncType.Unreliable)
			{
				IsDirtyGetter =
@"public {0}bool IsDirtyUnreliable
{{
	get
	{{
		bool isDirty = false;
{1}
		return isDirty;
	}}
}}
";
				IsObjectDirtyBinder = @"isDirty |= {0}.IsDirtyUnreliable;";
				IsDirtyNoElement = @"public {0}bool IsDirtyUnreliable => false;";

				PropertyDirtyBitName = @"_unreliablePropertyDirty_{0}";
				FunctionDirtyBitName = @"_unreliableRpcDirty_{0}";
				CheckSyncObjectDirty = @"{0}[{1}] = {2}.IsDirtyUnreliable;";
			}
		}
	}

	public static class PropertyFormat
	{
		/// <summary>
		/// {0} Type name<br/>
		/// {1} Argument name<br/>
		/// </summary>
		public static string Parameter => @"{0} {1}";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Type name<br/>
		/// {2} Property name<br/>
		/// {3} Initialize<br/>
		/// </summary>
		public static string Declaration =>
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
}}

";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Serialize content<br/>
		/// </summary>
		public static string SerializeIfDirty =>
@"if ({0}[{1}]) {2}";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Public property name<br/>
		/// {3} Private property name<br/>
		/// {4} Deserialize content<br/>
		/// </summary>
		public static string DeserializeIfDirty =>
@"if ({0}[{1}])
{{
	{4}	On{2}Changed?.Invoke({3});
}}
";

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
		public static string WriteEnum => @"writer(({0}){1});";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Write function name<br/>
		/// </summary>
		public static string WriteSyncObject => @"{0}.{1}(writer);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} CLR type name<br/>
		/// </summary>
		public static string ReadEmbededTypeProperty => @"{0} = reader.Read{1}();";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static string ReadByDeserializer => @"{0}.Deserialize(reader);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Enum type name<br/>
		/// {2} CLR enum size type name<br/>
		/// </summary>
		public static string ReadEnum => @"{0} = ({1})reader.Read{2}();";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Read function name<br/>
		/// </summary>
		public static string ReadSyncObject => @"{0}.{1}(reader);";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Private member name<br/>
		/// {2} CLR type name<br/>
		/// </summary>
		public static string TempReadEmbededTypeProperty => @"{0} {1} = reader.Read{2}();";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Argument name<br/>
		/// </summary>
		public static string TempReadByDeserializerStruct =>
@"{0} {1} = new();
{1}.Deserialize(reader);";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static string TempReadByDeserializerClass => @"{0}.Deserialize(reader);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Enum type name<br/>
		/// {2} CLR enum size type name<br/>
		/// </summary>
		public static string TempReadEnum => @"{1} {0} = ({1})reader.Read{2}();";
	}

	public static class FunctionFormat
	{

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Function name<br/>
		/// {2} Arguments content<br/>
		/// </summary>
		public static string Declaration =>
@"[{0}]
public partial void {1}({2});";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Parameter content<br/>
		/// {2} Callstack tuple content<br/>
		/// {3} Dirty bits name<br/>
		/// {4} Dirty index<br/>
		/// {5} Stack Generic Type<br/>
		/// </summary>
		public static string CallWithStack =>
@"public partial void {0}({1})
{{
	{0}Callstack.Enqueue({2});
	{3}[{4}] = true;
}}
private Queue<{5}> {0}Callstack = new();

";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Parameter content<br/>
		/// {2} Dirty bits name<br/>
		/// {3} Dirty index<br/>
		/// </summary>
		public static string CallWithStackVoid =>
@"public partial void {0}({1})
{{
	{0}CallstackCount++;
	{2}[{3}] = true;
}}
private byte {0}CallstackCount = 0;

";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Function name<br/>
		/// {3} Callstack serialize content<br/>
		/// </summary>
		public static string SerializeIfDirty =>
@"if ({0}[{1}])
{{
	byte count = (byte){2}Callstack.Count;
	writer.Put(count);
	for (int i = 0; i < count; i++)
	{{
		var args = {2}Callstack.Dequeue();
{3}
	}}
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Function name<br/>
		/// {3} Callstack serialize content<br/>
		/// </summary>
		public static string SerializeIfDirtyOneArg =>
@"if ({0}[{1}])
{{
	byte count = (byte){2}Callstack.Count;
	writer.Put(count);
	for (int i = 0; i < count; i++)
	{{
		var arg = {2}Callstack.Dequeue();
{3}
	}}
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Function name<br/>
		/// </summary>
		public static string SerializeIfDirtyVoid =>
@"if ({0}[{1}])
{{
	writer.Put({2}CallstackCount);
	{2}CallstackCount = 0;
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Function name<br/>
		/// {3} Callstack serialize content<br/>
		/// {4} Arguments content<br/>
		/// </summary>
		public static string DeserializeIfDirty =>
@"if ({0}[{1}])
{{
	byte count = reader.ReadByte();
	for (int i = 0; i < count; i++)
	{{
{3}
		{2}({4});
	}}
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Function name<br/>
		/// </summary>
		public static string DeserializeIfDirtyVoid =>
@"if ({0}[{1}])
{{
	byte count = reader.ReadByte();
	for (int i = 0; i < count; i++)
	{{
		{2}();
	}}
}}
";
	}

	public static class ObjectFormat
	{
		/// <summary>
		/// {0} Object name<br/>
		/// {1} Inherit type name<br/>
		/// {2} From content<br/>
		/// {3} To content<br/>
		/// </summary>
		public static string SyncObjectFormat =>
@"[Serializable]
public partial class {0} : {1}
{{
{2}
{3}
}}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Function name<br/>
		/// {2} Bitmask type name<br/>
		/// {3} Check any dirty content<br/>
		/// {4} Property serialize group content<br/>
		/// {5} Function serialize group content<br/>
		/// </summary>
		public static string SerializeSync =>
@"public {0}void {1}(PacketWriter writer)
{{
	{2} objectDirty = new {2}();

{3}

	objectDirty.Serialize(writer);

{4}
{5}
}}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Function name<br/>
		/// </summary>
		public static string SerializeSyncNoElement => @"public {0}void {1}(PacketWriter writer) {{ }}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Function name<br/>
		/// {2} Dirty bits clear content<br/>
		/// </summary>
		public static string ClearDirtyBitFunction =>
@"public {0}void {1}()
{{
{2}
}}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Function name<br/>
		/// </summary>
		public static string ClearDirtyBitFunctionNoElement =>
@"public {0}void {1}() {{}}
";
		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Dirty bits name<br/>
		/// </summary>
		public static string AnyDirtyBits => @"objectDirty[{0}] = {1}.AnyTrue();";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Dirty bits name<br/>
		/// {2} Serialize content<br/>
		/// </summary>
		public static string PropertySerializeGroup =>
@"
if (objectDirty[{0}])
{{
	{1}.Serialize(writer);

{2}
}}
";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Dirty bits name<br/>
		/// {2} Serialize content<br/>
		/// </summary>
		public static string FunctionSerializeGroup =>
@"if (objectDirty[{0}])
{{
	{1}.Serialize(writer);

{2}
}}
";

		/// <summary>
		/// {0} Dirty bits type name<br/>
		/// {1} Dirty bits name<br/>
		/// </summary>
		public static string DeclarationDirtyBits => @"private {0} {1} = new();";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// </summary>
		public static string DirtyBitsClear => @"{0}.Clear();";

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Function name<br/>
		/// {2} Bitmask type name<br/>
		/// {3} Property serialize group content<br/>
		/// {4} Function serialize group content<br/>
		/// </summary>
		public static string DeserializeSync =>
@"public {0}void {1}(PacketReader reader)
{{
	{2} objectDirty = reader.Read{2}();

{3}
{4}
}}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Function name<br/>
		/// </summary>
		public static string DeserializeSyncNoElement => @"public {0}void {1}(PacketReader reader) {{ }}
";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Bitmask type name<br/>
		/// {2} Dirty bits name<br/>
		/// {3} Deserialize content<br/>
		/// </summary>
		public static string FunctionDeserializeGroup =>
@"if (objectDirty[{0}])
{{
	{1} {2} = reader.Read{1}();

{3}
}}
";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Bitmask type name<br/>
		/// {2} Dirty bits name<br/>
		/// {3} Deserialize content<br/>
		/// </summary>
		public static string PropertyDeserializeGroup =>
@"
if (objectDirty[{0}])
{{
	{1} {2} = reader.Read{1}();

{3}
}}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// 
		/// {1} Function name<br/>
		/// {2} Serialize every property content<br/>
		/// </summary>
		public static string SerializeEveryProperty =>
@"public {0}void {1}(PacketWriter writer)
{{
{2}
}}
";

		/// <summary>
		/// {0} Modifier<br/>
		/// {1} Function name<br/>
		/// {2} Deserialize every property content<br/>
		/// </summary>
		public static string DeserializeEveryProperty =>
@"public {0}void {1}(PacketReader reader)
{{
{2}
}}
";
	}

	public static class SyncFormat
	{
		public static string MasterNetworkObjectTypeName => "MasterNetworkObject";
		public static string RemoteNetworkObjectTypeName => "RemoteNetworkObject";
		public static string NetworkObjectTypeTypeName => "NetworkObjectType";

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

		public static string GetSyncVarAttribute(SerializeType serializeType, SyncType syncType, SyncDirection syncDirection)
		{
			string attributeName = serializeType == SerializeType.SyncObject ? "SyncObject" : "SyncVar";
			string arguments = GetAttributeArgument(syncType, syncDirection);
			return string.IsNullOrWhiteSpace(arguments) ? attributeName : $"{attributeName}({arguments})";
		}

		public static string GetSyncRpcAttribute(SyncType syncType, SyncDirection syncDirection)
		{
			string attributeName = "SyncRpc";
			string arguments = GetAttributeArgument(syncType, syncDirection);
			return string.IsNullOrWhiteSpace(arguments) ? attributeName : $"{attributeName}({arguments})";
		}

		public static string GetAttributeArgument(SyncType syncType, SyncDirection direction)
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
