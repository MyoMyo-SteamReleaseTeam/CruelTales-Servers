namespace CT.PacketGenerator
{
	internal static class PacketFormat
	{
		/// <summary>
		/// {0} using statements<br/>
		/// {1} namespace<br/>
		/// {2} Content<br/>
		/// </summary>
		public static string FileFormat =
@"{0}

namespace {1}
{
	{2}
}";

		/// <summary>
		/// {0} namespace<br/>
		/// </summary>
		public static string UsingFormat = @"using {0};";

		/// <summary>
		/// {0} partial or sealed<br/>
		/// {1} class or struct<br/>
		/// {2} Name<br/>
		/// {3} interface<br/>
		/// {4} Content<br/>
		/// </summary>
		public static string DataTypeDefinition = 
@"public {0} {1} {2} : {3}
{
	{4}
}";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Name<br/>
		/// {2} Default value or constructor<br/>
		/// </summary>
		public static string MemberDeclaration = @"public {0} {1} = {2};";

		/// <summary>
		/// {0} Size getter name<br/>
		/// {1} Constant size or expression<br/>
		/// </summary>
		public static string SerializeSize = @"public int {0} => {1};";

		/// <summary>
		/// {0} Serialize function signature<br/>
		/// {1} Writer type name<br/>
		/// {2} Writer argument name<br/>
		/// {3} Content<br/>
		/// </summary>
		public static string SerializeFunction =
@"public void {0}({1} {2})
{
	{3}
}";

		/// <summary>
		/// {0} Deserialize function signature<br/>
		/// {1} Reader type name<br/>
		/// {2} Reader argument name<br/>
		/// {3} Content<br/>
		/// </summary>
		public static string DeserializeFunction =
@"public void {0}({1} {2})
{
	{3}
}";

		/// <summary>
		/// {0} Member name<br/>
		/// value.Serialize(writer);
		/// </summary>
		public static string MemberSerializeBySelf = @"{0}.Serialize(writer);";

		/// <summary>
		/// {0} Member name<br/>
		/// value.Deserialize(reader);
		/// </summary>
		public static string MemberDeserializeBySelf = @"{0}.Deserialize(reader);";

		/// <summary>
		/// {0} Member name<br/>
		/// writer.Put(value);
		/// </summary>
		public static string MemberSerializeByWriter = @"writer.Put({0});";

		/// <summary>
		/// {0} Member name<br/>
		/// {1} Deserialize type signature<br/>
		/// value = reader.ReadInt16();
		/// </summary>
		public static string MemberDeserializeByReader = @"{0} = reader.Read{1}();";
	}
}
