namespace CT.CorePatcher.Packets
{
	internal static class PacketFormat
	{
		public static readonly string ServerSidePacketPrefix = "Server_";
		public static readonly string ClientSidePacketPrefix = "Client_";

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

		/// <summary>
		/// {0} namespace<br/>
		/// </summary>
		public static readonly string UsingFormat = @"using {0};";

		/// <summary>
		/// {0} partial or sealed and class or struct<br/>
		/// {1} Name<br/>
		/// {2} interface<br/>
		/// {3} Content<br/>
		/// {4} SerializeSize<br/>
		/// {5} SerializeFunction<br/>
		/// {6} DeserializeFuntion<br/>
		/// </summary>
		public static readonly string DataTypeDefinition =
@"public {0} {1} : {2}
{{
{3}

{4}

{5}

{6}
}}";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Name<br/>
		/// {2} Default value or constructor<br/>
		/// </summary>
		public static readonly string MemberDeclaration = @"public {0} {1} = {2};";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Name<br/>
		/// </summary>
		public static readonly string MemberPrimitiveDeclaration = @"public {0} {1};";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Generic Type<br/>
		/// {2} Name<br/>
		/// </summary>
		public static readonly string MemberDeclarationGeneric = @"public {0}<{1}> {2} = new {0}<{1}>();";

		/// <summary>
		/// {0} Size getter name<br/>
		/// {1} Constant size or expression<br/>
		/// </summary>
		public static readonly string SerializeSize = @"public int {0} => {1};";

		/// <summary>
		/// {0} Serialize function signature<br/>
		/// {1} Writer type name<br/>
		/// {2} Writer argument name<br/>
		/// {3} Content<br/>
		/// </summary>
		public static readonly string SerializeFunction =
@"public void {0}({1} {2})
{{
{3}
}}";

		/// <summary>
		/// {0} Deserialize function signature<br/>
		/// {1} Reader type name<br/>
		/// {2} Reader argument name<br/>
		/// {3} Content<br/>
		/// </summary>
		public static readonly string DeserializeFunction =
@"public void {0}({1} {2})
{{
{3}
}}";

		/// <summary>
		/// {0} Member name<br/>
		/// value.Serialize(writer);
		/// </summary>
		public static readonly string MemberSerializeBySelf =
			@"{0}.Serialize(writer);";

		/// <summary>
		/// {0} Member name<br/>
		/// value.Deserialize(reader);
		/// </summary>
		public static readonly string MemberDeserializeBySelf =
			@"{0}.Deserialize(reader);";

		/// <summary>
		/// {0} Member name<br/>
		/// writer.Put(value);
		/// </summary>
		public static readonly string MemberSerializeByWriter =
			@"writer.Put({0});";

		/// <summary>
		/// {0} Member name<br/>
		/// {1} Deserialize type signature<br/>
		/// value = reader.ReadInt16();
		/// </summary>
		public static readonly string MemberDeserializeByReader =
			@"{0} = reader.Read{1}();";
	}
}
