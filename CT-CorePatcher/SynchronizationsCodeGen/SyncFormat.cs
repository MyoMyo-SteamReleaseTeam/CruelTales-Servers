using CT.CorePatcher.Synchronizations;
using CT.Tools.Collections;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	public static class SyncFormat
	{
		/// <summary>
		/// {0} using statements<br/>
		/// {1} namespace<br/>
		/// {2} Content<br/>
		/// </summary>
		public static readonly string FileFormat =
@"{0}
namespace {1}
{{
{2}
}}";

		public static readonly string SyncVarReliable = @"SyncVar";
		public static readonly string SyncVarUnreliable = @"SyncVar(SyncType.Unreliable)";

		public static readonly string SyncRpcReliable = @"SyncRpc";
		public static readonly string SyncRpcUnreliable = @"SyncRpc(SyncType.Unreliable)";

		/// <summary>
		/// {0} Object name<br/>
		/// {1} Inherit type name<br/>
		/// {2} Declaration Content<br/>
		/// {3} Synchronization Content<br/>
		/// {4} Serilalize Content<br/>
		/// </summary>
		public static readonly string ServerDeclaration =
@"
[Serializable]
public partial class {0} : {1}
{{
{2}
	#region Synchronization
{3}
{4}
	#endregion
}}
";

		/// <summary>
		/// {0} Object name<br/>
		/// {1} Inherit type name<br/>
		/// {2} Declaration Content<br/>
		/// {3} Deserilalize Content<br/>
		/// </summary>
		public static readonly string ClientDeclaration =
@"
[Serializable]
public partial class {0} : {1}
{{
{2}
	#region Synchronization
{3}
	#endregion
}}
";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Argument name<br/>
		/// </summary>
		public static readonly string Parameter = @"{0} {1}";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Type name<br/>
		/// {2} Property name<br/>
		/// {3} Initialize<br/>
		/// </summary>
		public static readonly string PrivateDeclaration =
@"[{0}]
private {1} {2}{3};";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Function name<br/>
		/// {2} Arguments content<br/>
		/// </summary>
		public static readonly string FunctionPartialDeclaration =
@"[{0}]
public partial void {1}({2});";

		/// <summary>
		/// {0} Attribute<br/>
		/// {1} Type name<br/>
		/// {2} Private property name<br/>
		/// {3} Public property name<br/>
		/// {4} Initialize<br/>
		/// </summary>
		public static readonly string RemotePropertyDeclaration =
@"[{0}]
private {1} {2}{4};
public event Action<{1}>? On{3}Changed;";

		/// <summary>
		/// {0} Dirty bits type name<br/>
		/// {1} Dirty bits name<br/>
		/// </summary>
		public static readonly string DeclarationDirtyBits = @"private {0} {1} = new();";

		/// <summary>
		/// {0} Dirty bits Count<br/>
		/// </summary>
		public static readonly string PropertyDirtyBitName = @"_propertyDirty_{0}";

		/// <summary>
		/// {0} Dirty bits Count<br/>
		/// </summary>
		public static readonly string FunctionDirtyBitName = @"_rpcDirty_{0}";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Public property name<br/>
		/// {2} Private member name<br/>
		/// {3} Dirty bits name<br/>
		/// {4} Dirty index<br/>
		/// </summary>
		public static readonly string PropertyGetSet =
@"public {0} {1}
{{
	get => {2};
	set
	{{
		if ({2} == value) return;
		{2} = value;
		{3}[{4}] = true;
	}}
}}

";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Parameter content<br/>
		/// {2} Callstack tuple content<br/>
		/// {3} Dirty bits name<br/>
		/// {4} Dirty index<br/>
		/// </summary>
		public static readonly string FunctionCallWithStack =
@"public partial void {0}({1})
{{
	{0}Callstack.Enqueue(({2}));
	{3}[{4}] = true;
}}
private Queue<({1})> {0}Callstack = new();

";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Parameter content<br/>
		/// {2} Dirty bits name<br/>
		/// {3} Dirty index<br/>
		/// </summary>
		public static readonly string FunctionCallWithStackVoid =
@"public partial void {0}({1})
{{
	{0}CallstackCount++;
	{2}[{3}] = true;
}}
private byte {0}CallstackCount = 0;

";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Bitmask type name<br/>
		/// {2} Check any dirty content<br/>
		/// {3} Property serialize group content<br/>
		/// {4} Function serialize group content<br/>
		/// {5} Dirty bits clear content<br/>
		/// </summary>
		public static readonly string SerializeSync =
@"public override void {0}(PacketWriter writer)
{{
	{1} objectDirty = new {1}();

{2}

	objectDirty.Serialize(writer);

	// Serialize Property
{3}

	// Serialize RPC callstack
{4}

{5}
}}
";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Dirty bits name<br/>
		/// </summary>
		public static readonly string AnyDirtyBits = @"objectDirty[{0}] = {1}.AnyTrue();";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Dirty bits name<br/>
		/// {2} Serialize content<br/>
		/// </summary>
		public static readonly string PropertySerializeGroup =
@"
if (objectDirty[{0}])
{{
	{1}.Serialize(writer);

{2}
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Serialize content<br/>
		/// </summary>
		public static readonly string PropertySerializeIfDirty =
@"if ({0}[{1}])
{2}
";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Dirty bits name<br/>
		/// {2} Serialize content<br/>
		/// </summary>
		public static readonly string FunctionSerializeGroup =
@"if (objectDirty[{0}])
{{
	{1}.Serialize(writer);

{2}
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Function name<br/>
		/// {3} Callstack serialize content<br/>
		/// </summary>
		public static readonly string FunctionSerializeIfDirty =
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
		/// </summary>
		public static readonly string FunctionSerializeIfDirtyVoid =
@"if ({0}[{1}])
{{
	writer.Put({2}CallstackCount);
	{2}CallstackCount = 0;
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// </summary>
		public static readonly string DirtyBitsClear = @"{0}.Clear();";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static readonly string WritePut = @"writer.Put({0});";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static readonly string WriteSerialize = @"{0}.Serialize(writer);";

		/// <summary>
		/// {0} Enum size type name<br/>
		/// {1} Private member name<br/>
		/// </summary>
		public static readonly string WriteEnum = @"writer(({0}){1});";

		/// <summary>
		/// {0} Function name<br/>
		/// {1} Bitmask type name<br/>
		/// {2} Property serialize group content<br/>
		/// {3} Function serialize group content<br/>
		/// </summary>
		public static readonly string DeserializeSync =
@"public override void {0}(PacketWriter writer)
{{
	{1} objectDirty = reader.Read{1}();

	// Deserialize Property
{2}

	// Deserialize RPC callstack
{3}
}}
";

		/// <summary>
		/// {0} Master dirty bits index<br/>
		/// {1} Bitmask type name<br/>
		/// {2} Dirty bits name<br/>
		/// {3} Deserialize content<br/>
		/// </summary>
		public static readonly string FunctionDeserializeGroup =
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
		public static readonly string PropertyDeserializeGroup =
@"
if (objectDirty[{0}])
{{
	{1} {2} = reader.Read{1}();

{3}
}}
";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Public property name<br/>
		/// {3} Private property name<br/>
		/// {4} Deserialize content<br/>
		/// </summary>
		public static readonly string PropertyDeserializeIfDirty =
@"if ({0}[{1}])
{{
	{4}
	On{2}Changed?.Invoke({3});
}}
";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} CLR type name<br/>
		/// </summary>
		public static readonly string ReadEmbededTypeProperty = @"{0} = reader.Read{1}();";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static readonly string ReadByDeserializer= @"{0}.Deserialize(reader);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Enum type name<br/>
		/// {2} CLR enum size type name<br/>
		/// </summary>
		public static readonly string ReadEnum = @"{0} = ({1})reader.Read{2}();";

		/// <summary>
		/// {0} Dirty bits name<br/>
		/// {1} Dirty bits index<br/>
		/// {2} Function name<br/>
		/// {3} Callstack serialize content<br/>
		/// {4} Arguments content<br/>
		/// </summary>
		public static readonly string FunctionDeserializeIfDirty =
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
		public static readonly string FunctionDeserializeIfDirtyVoid =
@"if ({0}[{1}])
{{
	byte count = reader.ReadByte();
	for (int i = 0; i < count; i++)
	{{
		{2}();
	}}
}}
";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Private member name<br/>
		/// {2} CLR type name<br/>
		/// </summary>
		public static readonly string TempReadEmbededTypeProperty = @"{0} {1} = reader.Read{2}();";

		/// <summary>
		/// {0} Type name<br/>
		/// {1} Argument name<br/>
		/// </summary>
		public static readonly string TempReadByDeserializerStruct =
@"{0} {1} = new();
{1}.Deserialize(reader);";

		/// <summary>
		/// {0} Private member name<br/>
		/// </summary>
		public static readonly string TempReadByDeserializerClass = @"{0}.Deserialize(reader);";

		/// <summary>
		/// {0} Private member name<br/>
		/// {1} Enum type name<br/>
		/// {2} CLR enum size type name<br/>
		/// </summary>
		public static readonly string TempReadEnum = @"{1} {0} = ({1})reader.Read{2}();";



		public static string GetSyncVarAttribute(SyncType syncType)
		{
			if (syncType == SyncType.Reliable)
			{
				return SyncVarReliable;
			}
			else if (syncType == SyncType.Unreliable)
			{
				return SyncVarUnreliable;
			}

			return string.Empty;
		}

		public static string GetSyncRpcAttribute(SyncType syncType)
		{
			if (syncType == SyncType.Reliable)
			{
				return SyncRpcReliable;
			}
			else if (syncType == SyncType.Unreliable)
			{
				return SyncRpcUnreliable;
			}

			return string.Empty;
		}
	}
}
