using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using CT.Network.Serialization;
using CT.Network.Serialization.Type;
using CT.PacketGenerator.Exceptions;

namespace CT.PacketGenerator
{

	internal class PacketParser
	{
		public string NewLine { get; set; } = "\n";
		public string Indent { get; set; } = "\t";
		public List<string> UsingSegments { get; set; } = new List<string>();
		public List<Assembly> ReferenceAssemblys { get; set; } = new List<Assembly>();
		private List<string> _enumTypes = new List<string>();

		public PacketParser()
		{

		}

		public string ParseFromXml(string path)
		{
			foreach (var a in ReferenceAssemblys)
			{
				_enumTypes = getTypeNamesBy(a, typeof(Enum));
			}

			XmlReaderSettings settings = new XmlReaderSettings()
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
			};

			using (XmlReader r =  XmlReader.Create(path, settings))
			{
				string usingStatements = "";

				foreach (var u in UsingSegments)
				{
					usingStatements += string.Format(PacketFormat.UsingFormat, u) + NewLine;
				}

				string packetNamespace = "";

				r.MoveToContent();

				if (PacketHelper.GetPacketDataType(r) != PacketDataType.Definition)
					throw new WrongDefinitionException();

				if (tryParse(r, PacketAttributeType.Namespace, out packetNamespace) == false)
					throw new WrongDefinitionException();

				string content = "";

				r.Read();

				while (!r.EOF)
				{
					if (isValidElement(r) == false)
					{
						r.Read();
						continue;
					}

					if (PacketHelper.GetPacketDataType(r) != PacketDataType.Other)
					{
						parseDataType(r, out string parseContent);
						content += parseContent;
						if (!r.EOF)
						{
							content += NewLine + NewLine;
						}
					}
				}

				for (int i = 0; i < NewLine.Length * 2; i++)
				{
					content = content.Substring(0, content.Length - 1);
				}

				content = addIndent(content);
				return string.Format(PacketFormat.FileFormat,
									 usingStatements, packetNamespace, content);
			}
		}

		private bool parseDataType(XmlReader r, out string content)
		{
			if (isValidElement(r) == false)
				throw new WrongElementException(r);

			int currentDepth = r.Depth;

			PacketDataType dataType = PacketHelper.GetPacketDataType(r);
			string className = string.Empty;
			string declaration = string.Empty;
			string interfaceName = nameof(IPacketSerializable);
			string dataTypeContent = string.Empty;
			string memberContent = string.Empty;

			if (dataType == PacketDataType.Other)
				throw new WrongDataTypeException(r);

			if (!PacketHelper.TryGetDeclarationBy(dataType, out declaration))
				throw new WrongDeclarationException(r);

			if (!tryParse(r, PacketAttributeType.Name, out className))
				throw new WrongAttributeException(r, PacketAttributeType.Name);

			if (dataType == PacketDataType.ServerPacket)
			{
				className = PacketFormat.ServerSidePacketPrefix + className;
			}
			else if (dataType == PacketDataType.ClientPacket)
			{
				className = PacketFormat.ClientSidePacketPrefix + className;
			}

			r.Read();
			while (true)
			{
				if (r.Depth < currentDepth)
					break;

				if (isValidElement(r) == false)
				{
					r.Read();
					continue;
				}

				var nextDataType = PacketHelper.GetPacketDataType(r);
				if (nextDataType == PacketDataType.Other)
				{
					ParseMembers(r, out string parseMemberContent);
					memberContent += parseMemberContent;
				}
				else
				{
					parseDataType(r, out string dataContent);
					dataTypeContent += addIndentWithNewLine(dataContent) + NewLine;
				}

				if (r.Depth <= currentDepth)
					break;
			}

			var combineContent = dataTypeContent;
			if (!string.IsNullOrEmpty(memberContent))
			{
				combineContent += memberContent;
			}

			content = string.Format(PacketFormat.DataTypeDefinition,
									declaration, className,
									interfaceName, combineContent);

			return true;
		}

		private struct MemberDefinition
		{
			public string Type;
			public string MemeberName;
			public bool IsNative;
			public string GenericType;

			public bool HasGeneric => !string.IsNullOrEmpty(GenericType);
		}

		public bool ParseMembers(XmlReader r, out string content)
		{
			int currentDepth = r.Depth;
			content = string.Empty;

			List<MemberDefinition> members = new();

			do
			{
				if (r.Depth != currentDepth)
					break;

				if (isValidElement(r) == false)
					continue;

				if (string.IsNullOrEmpty(r.Name))
					throw new WrongElementException(r);

				if (PacketHelper.GetPacketDataType(r) != PacketDataType.Other)
				{
					if (parseDataType(r, out string dataContent))
					{
						content += addIndentWithNewLine(dataContent) + NewLine;
					}
					continue;
				}

				MemberDefinition member = new MemberDefinition();

				string dataTypeName = r.Name;
				if (_enumTypes.Contains(dataTypeName))
				{
					member.IsNative = true;
					member.Type = dataTypeName;
				}
				else
				{
					member.IsNative = PacketHelper.TryGetTypeByCLRType(dataTypeName, out var value);
					member.Type = member.IsNative ? value : dataTypeName;
				}

				if (tryParse(r, PacketAttributeType.Name, out string name))
					member.MemeberName = name;
				else
					throw new WrongAttributeException(r, PacketAttributeType.Name);

				if (tryParse(r, PacketAttributeType.Type, out string genericType))
					member.GenericType = genericType;

				members.Add(member);
			}
			while (r.Read());

			string memberContent = "";
			for (int i = 0; i < members.Count; i++)
			{
				var m = members[i];
				if (m.IsNative)
				{
					memberContent += string.Format(PacketFormat.MemberPrimitiveDeclaration,
												   m.Type, m.MemeberName);
				}
				else
				{
					memberContent += m.HasGeneric ?
						string.Format(PacketFormat.MemberDeclarationGeneric,
									  m.Type, m.GenericType, m.MemeberName) :
						string.Format(PacketFormat.MemberDeclaration,
									  m.Type, m.MemeberName, @"new()");
				}

				if (i < members.Count - 1)
				{
					memberContent += NewLine;
				}
			}

			content += addIndent(memberContent);
			return true;
		}

		private bool tryParse(XmlReader r, PacketAttributeType type, out string token)
		{
			var tokenType = type.ToString().ToLower();
			token = r[tokenType] ?? "";
			return !string.IsNullOrEmpty(token);
		}

		private bool isValidElement(XmlReader r)
		{
			return r.Depth > 0 && r.NodeType == XmlNodeType.Element;
		}

		private string addIndent(string content)
		{
			return Indent + content.Replace(NewLine, $"{NewLine}{Indent}");
		}

		private string addIndentWithNewLine(string content)
		{
			return Indent + content.Replace(NewLine, $"{NewLine + Indent}") + NewLine;
		}

		private List<string> getTypeNamesBy(Assembly targetAssembly, Type baseType)
		{
			return targetAssembly
				.GetTypes()
				.Where(t => t.BaseType == baseType)
				.Select(t => t.Name).ToList();
		}
	}
}
