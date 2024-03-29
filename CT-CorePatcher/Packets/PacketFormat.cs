﻿namespace CT.CorePatcher.Packets
{
	public static class PacketFormat
	{
		#region Global

		public static string ServerSidePacketPrefix => "SC";
		public static string ClientSidePacketPrefix => "CS";

		#endregion

		#region Packet Data Type

		/// <summary>
		/// {0} using statements<br/>
		/// {1} namespace<br/>
		/// {2} Content<br/>
		/// </summary>
		public static string FileFormat =>
@"{0}
namespace {1}
{{
{2}
}}";

		/// <summary>
		/// {0} namespace<br/>
		/// </summary>
		public static string UsingFormat => @"using {0};";

		/// <summary>
		/// {0} partial or sealed and class or struct<br/>
		/// {1} Name<br/>
		/// {2} inherit<br/>
		/// {3} Content<br/>
		/// {4} SerializeSize<br/>
		/// {5} SerializeFunction<br/>
		/// {6} DeserializeFuntion<br/>
		/// {7} IgnoreFunction<br/>
		/// </summary>
		public static string DataTypeDefinition =>
@"public {0} {1} : {2}
{{
{3}

{4}

{5}

{6}

{7}
}}";

		/// <summary>
		/// {0} PacketType enum name<br/>
		/// {1} PacketType property name<br/>
		/// {2} PacketType enum<br/>
		/// </summary>
		public static string PacketTypeDeclaration => @"public override {0} {1} => {2};";

		/// <summary>
		/// {0} partial or sealed and class or struct<br/>
		/// {1} Name<br/>
		/// {2} inherit<br/>
		/// {3} Packet Type <br/>
		/// {4} Content<br/>
		/// {5} SerializeSize<br/>
		/// {6} SerializeFunction<br/>
		/// {7} DeserializeFuntion<br/>
		/// </summary>
		public static string PacketDataTypeDefinition =>
@"public {0} {1} : {2}
{{
{3}

{4}

{5}

{6}

{7}
}}";

		/// <summary>
		/// {0} Size getter name<br/>
		/// {1} Constant size or expression<br/>
		/// </summary>
		public static string PacketSerializeSize => @"public override int {0} => {1};";

		/// <summary>
		/// {0} Serialize function signature<br/>
		/// {1} Writer type name<br/>
		/// {2} Writer argument name<br/>
		/// {3} Content<br/>
		/// </summary>
		public static string PacketSerializeFunction =>
@"public override void {0}({1} {2})
{{
{3}
}}";

		/// <summary>
		/// {0} Deserialize function signature<br/>
		/// {1} Reader type name<br/>
		/// {2} Reader argument name<br/>
		/// {3} Content<br/>
		/// </summary>
		public static string PacketDeserializeFunction =>
@"public override bool {0}({1} {2})
{{
{3}
	return true;
}}";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Name<br/>
		/// {2} Default value or constructor<br/>
		/// </summary>
		public static string MemberDeclaration => @"public {0} {1} = {2};";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Name<br/>
		/// {2} Default value or constructor<br/>
		/// </summary>
		public static string MemberDeclarationNotInitialize => @"public {0} {1};";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Name<br/>
		/// </summary>
		public static string MemberPrimitiveDeclaration => @"public {0} {1};";

		/// <summary>
		/// {0} Data Type<br/>
		/// {1} Generic Type<br/>
		/// {2} Name<br/>
		/// </summary>
		public static string MemberDeclarationGeneric => @"public {0}<{1}> {2} = new {0}<{1}>();";

		/// <summary>
		/// {0} Size getter name<br/>
		/// {1} Constant size or expression<br/>
		/// </summary>
		public static string SerializeSize => @"public int {0} => {1};";

		/// <summary>
		/// {0} Serialize function signature<br/>
		/// {1} Writer type name<br/>
		/// {2} Writer argument name<br/>
		/// {3} Content<br/>
		/// </summary>
		public static string SerializeFunction =>
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
		public static string DeserializeFunction =>
@"public bool {0}({1} {2})
{{
{3}
	return true;
}}";

		public static string IgnoreFunction =>
@"public static void IgnoreStatic(IPacketReader reader) => throw new System.NotImplementedException();
public void Ignore(IPacketReader reader) => throw new System.NotImplementedException();";

		/// <summary>
		/// {0} Member name<br/>
		/// value.Serialize(writer);
		/// </summary>
		public static string MemberSerializeBySelf => @"{0}.Serialize(writer);";

		/// <summary>
		/// {0} Member name<br/>
		/// value.Deserialize(reader);
		/// </summary>
		public static string MemberDeserializeBySelf => @"if (!{0}.TryDeserialize(reader)) return false;";

		/// <summary>
		/// {0} Member name<br/>
		/// writer.Put(value);
		/// </summary>
		public static string MemberSerializeByWriter => @"writer.Put({0});";

		/// <summary>
		/// {0} Member name<br/>
		/// {1} Deserialize type signature<br/>
		/// value = reader.ReadInt16();
		/// </summary>
		public static string MemberDeserializeByReader => @"if (!reader.TryRead{1}(out {0})) return false;";

		#endregion

		public static string PacketFactoryFileName => "PacketFactory";
		public static string PacketDispatcherFileName => "PacketDispatcher";

		/// <summary>
		/// {0} Create by enum function content<br/>
		/// {1} Create by type function content<br/>
		/// {2} Match type and enum content<br/>
		/// </summary>
		public static string PacketFactoryServerFormat =>
@"using System;
using System.Collections.Generic;
using CT.Packets;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;

namespace CTS.Instance.Packets
{{
	public delegate PacketBase ReadPacket(IPacketReader reader);
	public delegate PacketBase CreatePacket();

	public static partial class PacketFactory
	{{
		private static Dictionary<PacketType, CreatePacket> _packetCreateByEnum = new()
		{{
{0}
		}};

		private static Dictionary<Type, CreatePacket> _packetCreateByType = new()
		{{
{1}
		}};

		private static BidirectionalMap<Type, PacketType> _packetTypeTable = new()
		{{
{2}
		}};

		public static T CreatePacket<T>() where T : PacketBase
		{{
			return (T)_packetCreateByType[typeof(T)]();
		}}

		public static PacketBase CreatePacket(PacketType type)
		{{
			return _packetCreateByEnum[type]();
		}}

		public static Type GetTypeByEnum(PacketType value)
		{{
			return _packetTypeTable.GetValue(value);
		}}

		public static PacketType GetEnumByType(Type value)
		{{
			return _packetTypeTable.GetValue(value);
		}}
	}}
}}";

		/// <summary>
		/// {0} Create by enum function content<br/>
		/// {1} Create by type function content<br/>
		/// {2} Match type and enum content<br/>
		/// </summary>
		public static string PacketFactoryClientFormat =>
@"using System;
using System.Collections.Generic;
using CT.Packets;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;

namespace CTC.Networks.Packets
{{
	public delegate PacketBase ReadPacket(IPacketReader reader);
	public delegate PacketBase CreatePacket();

	public static partial class PacketFactory
	{{
		private static Dictionary<PacketType, CreatePacket> _packetCreateByEnum = new()
		{{
{0}
		}};

		private static Dictionary<Type, CreatePacket> _packetCreateByType = new()
		{{
{1}
		}};

		private static BidirectionalMap<Type, PacketType> _packetTypeTable = new()
		{{
{2}
		}};

		public static T CreatePacket<T>() where T : PacketBase
		{{
			return (T)_packetCreateByType[typeof(T)]();
		}}

		public static PacketBase CreatePacket(PacketType type)
		{{
			return _packetCreateByEnum[type]();
		}}

		public static Type GetTypeByEnum(PacketType value)
		{{
			return _packetTypeTable.GetValue(value);
		}}

		public static PacketType GetEnumByType(Type value)
		{{
			return _packetTypeTable.GetValue(value);
		}}
	}}
}}";

		/// <summary>
		/// {0} Packet name
		/// </summary>
		public static string PacketCreateByEnumItem => @"{{ PacketType.{0}, () => new {0}() }},";

		/// <summary>
		/// {0} Packet name
		/// </summary>
		public static string PacketCreateByTypeItem => @"{{ typeof({0}), () => new {0}() }},";

		/// <summary>
		/// {0} Packet name
		/// </summary>
		public static string PacketMatchTypeEnumItem => @"{{ typeof({0}), PacketType.{0} }},";

		/// <summary>
		/// {0} Handle by packet type content<br/>
		/// {1} Handle raw by packet type content<br/>
		/// {2} Packet type enumerator<br/>
		/// </summary>
		public static string PacketDispatcherServerFormat =>
@"using System.Collections.Generic;
using CT.Packets;
using CT.Common.Serialization;
using CTS.Instance.Networks;

namespace CTS.Instance.Packets
{{
	public delegate void HandlePacket(PacketBase receivedPacket, UserSession session);
	public delegate void HandlePacketRaw(IPacketReader receivedPacket, UserSession session);

	public static class PacketDispatcher
	{{
		public static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{{
{0}
		}};

		private static Dictionary<PacketType, HandlePacketRaw> _dispatcherRawTable = new()
		{{
{1}
		}};

		private static HashSet<PacketType> _customPacketSet = new()
		{{
{2}
		}};

		public static void Dispatch(PacketBase receivedPacket, UserSession session)
		{{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, session);
		}}

		public static void DispatchRaw(PacketType packetType, IPacketReader receivedPacket, UserSession session)
		{{
			_dispatcherRawTable[packetType](receivedPacket, session);
		}}

		public static bool IsCustomPacket(PacketType packetType)
		{{
			return _customPacketSet.Contains(packetType);
		}}
	}}
}}";

		/// <summary>
		/// {0} Handle by packet type content<br/>
		/// {1} Handle raw by packet type content<br/>
		/// {2} Packet type enumerator<br/>
		/// </summary>
		public static string PacketDispatcherClientFormat =>
@"using System.Collections.Generic;
using CT.Packets;
using CT.Common.Serialization;

namespace CTC.Networks.Packets
{{
	public delegate void HandlePacket(PacketBase receivedPacket, NetworkManager networkManager);
	public delegate void HandlePacketRaw(IPacketReader receivedPacket, NetworkManager networkManager);

	public static class PacketDispatcher
	{{
		private static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{{
{0}
		}};

		private static Dictionary<PacketType, HandlePacketRaw> _dispatcherRawTable = new()
		{{
{1}
		}};

		private static HashSet<PacketType> _customPacketSet = new()
		{{
{2}
		}};

		public static void Dispatch(PacketBase receivedPacket, NetworkManager networkManager)
		{{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, networkManager);
		}}

		public static void DispatchRaw(PacketType packetType, IPacketReader receivedPacket, NetworkManager networkManager)
		{{
			_dispatcherRawTable[packetType](receivedPacket, networkManager);
		}}

		public static bool IsCustomPacket(PacketType packetType)
		{{
			return _customPacketSet.Contains(packetType);
		}}
	}}

}}";

		/// <summary>
		/// {0} Packet name
		/// </summary>
		public static string PacketDispatcherMember => @"{{ PacketType.{0}, PacketHandler.Handle_{0} }},";
	}
}
