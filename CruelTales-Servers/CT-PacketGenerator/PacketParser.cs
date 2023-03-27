using System;
using System.Collections.Generic;
using System.Xml;
using CT.Network.Serialization;
using CT.PacketGenerator.Exceptions;

namespace CT.PacketGenerator
{

	internal class PacketParser
	{
		public string ParseFromXml(string path)
		{
			XmlReaderSettings settings = new XmlReaderSettings()
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
			};

			using (XmlReader r =  XmlReader.Create(path, settings))
			{
				string usingStatements = "";

				usingStatements += string.Format(PacketFormat.UsingFormat,
					$"{nameof(CT)}.{nameof(CT.Network)}." +
					$"{nameof(CT.Network.Packet)}");
				usingStatements += TextFormat.LF;

				usingStatements += string.Format(PacketFormat.UsingFormat,
					$"{nameof(CT)}.{nameof(CT.Network)}." +
					$"{nameof(CT.Network.Serialization)}");
				usingStatements += TextFormat.LF;

				usingStatements += string.Format(PacketFormat.UsingFormat,
					$"{nameof(CT)}.{nameof(CT.Network)}." +
					$"{nameof(CT.Network.Serialization)}." +
					$"{nameof(CT.Network.Serialization.Type)}");
				usingStatements += TextFormat.LF;

				string packetNamespace = "";

				r.MoveToContent();

				if (PacketHelper.GetPacketDataType(r) != PacketDataType.Definition)
				{
					throw new WrongDefinitionException();
				}

				if (TryParse(r, PacketAttributeType.Namespace,
							 out packetNamespace) == false)
				{
					throw new WrongDefinitionException();
				}

				string content = "";

				while (r.Read())
				{
					if (IsValidElement(r) == false)
						continue;

					if (PacketHelper.GetPacketDataType(r) != PacketDataType.Other)
					{
						ParseDataType(r, out string parseContent);
						content += parseContent.Replace(TextFormat.LF.ToString(), $"{TextFormat.LF}\t");
					}
				}

				string generate = string.Format(PacketFormat.FileFormat,
												usingStatements, packetNamespace, content);

				Console.WriteLine(generate);
			}

			return "";
		}

		public bool ParseDataType(XmlReader r, out string content)
		{
			int currentDepth = r.Depth;
			content = string.Empty;

			if (IsValidElement(r) == false)
				throw new WrongElementException(r);

			var dataType = PacketHelper.GetPacketDataType(r);
			if (dataType == PacketDataType.Other)
			{
				throw new WrongDataTypeException(r);
			}

			string className = "";
			string declaration = "";
			string interfaceName = nameof(IPacketSerializable);
			string memberContent = "";

			// Parse declarations and definitions
			if (!PacketHelper.TryGetDeclarationBy(dataType, out declaration))
			{
				throw new WrongDeclarationException(r);
			}

			if (!TryParse(r, PacketAttributeType.Name, out className))
			{
				throw new WrongAttributeException(r, PacketAttributeType.Name);
			}

			if (dataType == PacketDataType.ServerPacket)
			{
				className = PacketFormat.ServerSidePacketPrefix + className;
			}
			else if (dataType == PacketDataType.ClientPacket)
			{
				className = PacketFormat.ClientSidePacketPrefix + className;
			}

			while (r.Read())
			{
				if (r.Depth < currentDepth)
					break;

				if (IsValidElement(r) == false)
					continue;

				var nextDataType = PacketHelper.GetPacketDataType(r);
				if (nextDataType == PacketDataType.Other)
				{
					ParseMembers(r, out string parseMemberContent);
					memberContent += parseMemberContent;
				}
				else
				{
					ParseDataType(r, out string parseContent);
					content += parseContent;
				}
			}

			content = string.Format(PacketFormat.DataTypeDefinition,
									declaration, className, interfaceName, memberContent);
			content = content.Replace(TextFormat.LF.ToString(), $"{TextFormat.LF}\t");

			return true;
		}

		public struct MemberDefinition
		{
			public string TypeName;
			public string MemeberName;
			public bool IsNative;
			public string GenericType;

			public bool HasGeneric => string.IsNullOrEmpty(GenericType);
		}

		public bool ParseMembers(XmlReader r, out string content)
		{
			int currentDepth = r.Depth;
			content = string.Empty;

			List<MemberDefinition> members = new();

			do
			{
				if (r.Depth < currentDepth)
					break;

				if (IsValidElement(r) == false)
					continue;

				if (string.IsNullOrEmpty(r.Name))
				{
					throw new WrongElementException(r);
				}

				if (PacketHelper.GetPacketDataType(r) != PacketDataType.Other)
				{
					if (ParseDataType(r, out string dataContent))
					{
						content += dataContent;
					}
				}

				MemberDefinition member = new MemberDefinition();

				member.IsNative = PacketHelper.TryGetCLRTypeByPrimitive(r.Name, out var value);
				member.TypeName = member.IsNative ? value : r.Name;

				if (TryParse(r, PacketAttributeType.Name, out string name))
					member.MemeberName = name;
				else
					throw new WrongAttributeException(r, PacketAttributeType.Name);

				if (TryParse(r, PacketAttributeType.Type, out string genericType))
					member.GenericType = genericType;

				members.Add(member);
			}
			while (r.Read());

			// TODO add memebers declarations

			content = content.Replace(TextFormat.LF.ToString(), $"{TextFormat.LF}\t");

			return true;
		}

		public bool TryParse(XmlReader r, PacketAttributeType type, out string token)
		{
			var tokenType = type.ToString().ToLower();
			token = r[tokenType] ?? "";
			return !string.IsNullOrEmpty(token);
		}

		public bool IsValidElement(XmlReader r)
		{
			return r.Depth > 0 && r.NodeType == XmlNodeType.Element;
		}
	}
}
