﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Xml;
using CT.Common.Serialization;
using CT.CorePatcher.Exceptions;
using CT.CorePatcher.Helper;
using CT.Packets;

namespace CT.CorePatcher.Packets
{
	public class PacketInfo
	{
		public string PacketName = string.Empty;
		public bool IsCustom = false;
		public string PacketEnumName => $"PacketType.{PacketName}";
	}

	/// <summary>XML 패킷 정의로 부터 C# 코드를 생성합니다.</summary>
	internal class PacketParser
	{
		public static string NewLine { get; set; } = TextFormat.LF;
		public static string Indent { get; set; } = TextFormat.Indent;
		public string WriterName { get; set; } = "writer";
		public string ReaderName { get; set; } = "reader";
		public List<Assembly> ReferenceAssemblys { get; set; } = new List<Assembly>();
		private Dictionary<string, int> _enumSizeByTypeName = new Dictionary<string, int>();

		public void Initialize()
		{
			// Find enums from assemblys
			foreach (var a in ReferenceAssemblys)
			{
				var enumTypes = ReflectionExtension.GetTypeNamesBy(a, typeof(Enum));

				foreach (var et in enumTypes)
				{
					var enumSizeTypeName = Enum.GetUnderlyingType(et).Name;
					int enumSize = ReflectionHelper.GetByteSizeByTypeName(enumSizeTypeName);
					_enumSizeByTypeName.Add(et.Name, enumSize);
				}
			}
		}

		public void GenerateDispatcherCode(List<PacketInfo> packetInfos, out string code, bool isClient, string fileName)
		{
			string handleByTypeContent = string.Empty;
			string handleRawByTypeContent = string.Empty;
			string typeEnumerator = string.Empty;

			foreach (var pInfo in packetInfos)
			{
				if (isClient && pInfo.PacketName.Contains(PacketFormat.ClientSidePacketPrefix) ||
					!isClient && pInfo.PacketName.Contains(PacketFormat.ServerSidePacketPrefix))
				{
					continue;
				}

				string element = string.Format(PacketFormat.PacketDispatcherMember, pInfo.PacketName) + NewLine;

				if (pInfo.IsCustom)
				{
					typeEnumerator += pInfo.PacketEnumName + "," + NewLine;
					handleRawByTypeContent += element;
				}
				else
				{
					handleByTypeContent += element;
				}
			}

			CodeFormat.AddIndent(ref handleByTypeContent, 3);
			CodeFormat.AddIndent(ref handleRawByTypeContent, 3);
			CodeFormat.AddIndent(ref typeEnumerator, 3);

			string format = isClient ? 
				PacketFormat.PacketDispatcherClientFormat :
				PacketFormat.PacketDispatcherServerFormat;

			code = string.Format(format, handleByTypeContent, handleRawByTypeContent, typeEnumerator);
			code = string.Format(CodeFormat.GeneratorMetadata, fileName, code);
		}

		public void GenerateFactoryCode(List<PacketInfo> packetInfos, out string code, string fileName, bool isServer)
		{
			string createByEnum = string.Empty;
			string createByType = string.Empty;
			string matchTypeEnum = string.Empty;

			foreach (var pInfo in packetInfos)
			{
				if (pInfo.IsCustom)
					continue;

				createByEnum += string.Format(PacketFormat.PacketCreateByEnumItem, pInfo.PacketName) + NewLine;
				createByType += string.Format(PacketFormat.PacketCreateByTypeItem, pInfo.PacketName) + NewLine;
				matchTypeEnum += string.Format(PacketFormat.PacketMatchTypeEnumItem, pInfo.PacketName) + NewLine;
			}

			for (int i = 0; i < 3; i++)
			{
				createByEnum = addIndent(createByEnum);
				createByType = addIndent(createByType);
				matchTypeEnum = addIndent(matchTypeEnum);
			}

			string factoryFormat = isServer ?
				PacketFormat.PacketFactoryServerFormat : 
				PacketFormat.PacketFactoryClientFormat;

			code = string.Format(factoryFormat, createByEnum, createByType, matchTypeEnum);
			code = string.Format(CodeFormat.GeneratorMetadata, fileName, code);
		}

		public void ParseFromXml(string path, out string code, out List<PacketInfo> packetInfos)
		{
			// Set XML parse option
			XmlReaderSettings settings = new XmlReaderSettings()
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
			};

			// XML parsing start
			using XmlReader r = XmlReader.Create(path, settings);

			string usingStatements = string.Empty;
			string packetNamespace = string.Empty;
			string content = string.Empty;

			// Check validation
			r.MoveToContent();

			if (PacketHelper.GetPacketDataType(r) != PacketDataType.Definition)
				throw new WrongDefinitionException();

			if (tryParse(r, PacketAttributeType.Namespace, out packetNamespace) == false)
				throw new WrongDefinitionException();

			// Parse XML packet definition to generate codes
			packetInfos = new List<PacketInfo>();
			r.Read();
			while (!r.EOF)
			{
				if (isValidElement(r) == false)
				{
					r.Read();
					continue;
				}

				var type = PacketHelper.GetPacketDataType(r);
				if (type == PacketDataType.Using)
				{
					r.Read();
					string u = r.Value;
					if (string.Equals(packetNamespace, u))
					{
						continue;
					}
					usingStatements += string.Format(PacketFormat.UsingFormat, u) + NewLine;
					continue;
				}

				if (type != PacketDataType.Other)
				{
					var dataTypeName = parseDataType(r, out string parseContent,
													 out bool isCustom);
					if (dataTypeName.Contains(PacketFormat.ServerSidePacketPrefix) ||
						dataTypeName.Contains(PacketFormat.ClientSidePacketPrefix) || isCustom)
					{
						packetInfos.Add(new PacketInfo() { PacketName = dataTypeName, IsCustom = isCustom });
					}

					if (parseContent != null && !isCustom)
					{
						content += parseContent;
					}

					if (!r.EOF)
					{
						content += NewLine + NewLine;
					}
				}
			}

			// Remove extra new line
			for (int i = 0; i < NewLine.Length * 2; i++)
			{
				content = content.Substring(0, content.Length - 1);
			}

			// Combine generated codes

			string fileName = Path.GetFileNameWithoutExtension(path) + ".cs";

			content = addIndent(content);
			code = string.Format(PacketFormat.FileFormat,
								 usingStatements, packetNamespace, content);
			code = string.Format(CodeFormat.GeneratorMetadata, fileName, code);
		}

		/// <summary>
		/// XML을 파싱하여 데이터 타입 코드를 생성합니다. class 및 struct 데이터 타입을 생성합니다.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="content"></param>
		/// <returns>Data Type의 이름입니다.</returns>
		/// <exception cref="WrongElementException"></exception>
		/// <exception cref="WrongDataTypeException"></exception>
		/// <exception cref="WrongDeclarationException"></exception>
		/// <exception cref="WrongAttributeException"></exception>
		private string parseDataType(XmlReader r, out string content, out bool isCustom)
		{
			if (isValidElement(r) == false)
				throw new WrongElementException(r);

			int currentDepth = r.Depth;

			PacketDataType dataType = PacketHelper.GetPacketDataType(r);
			string className = string.Empty;
			string declaration = string.Empty;
			string dataTypeContent = string.Empty;
			string memberContent = string.Empty;
			List<MemberDefinitionToken> memberTokenList = new();

			// Check parse validations
			if (dataType == PacketDataType.Other)
				throw new WrongDataTypeException(r);

			if (!PacketHelper.TryGetDeclarationBy(dataType, out declaration))
				throw new WrongDeclarationException(r);

			if (!tryParse(r, PacketAttributeType.Name, out className))
				throw new WrongAttributeException(r, PacketAttributeType.Name);

			// Set class signature
			if (dataType == PacketDataType.ServerPacket)
			{
				className = PacketFormat.ServerSidePacketPrefix + "_" + className;
			}
			else if (dataType == PacketDataType.ClientPacket)
			{
				className = PacketFormat.ClientSidePacketPrefix + "_" + className;
			}

			// Return if it's custom definition
			if (tryParse(r, PacketAttributeType.Custom, out var custom))
			{
				isCustom = custom.ToLower() == "true";
				content = string.Empty;
				r.Read();
				return className;
			}
			else
			{
				isCustom = false;
			}

			// Generate data type definition codes
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
					// Parse members
					parseMembers(dataType, r, out string parseMemberContent, out var memberTokens);
					memberTokenList.AddRange(memberTokens);
					memberContent += parseMemberContent;
				}
				else
				{
					// Parse data type recursively
					parseDataType(r, out string dataContent, out _);
					dataTypeContent += addIndentWithNewLine(dataContent) + NewLine;
				}

				// If read all current context in this depth, break the loop
				if (r.Depth <= currentDepth)
					break;
			}

			// Generate serialize size code
			int calcSerializeSize = 0;
			string sizeExpression = string.Empty;

			if (dataType == PacketDataType.ClientPacket ||
				dataType == PacketDataType.ServerPacket)
			{
				calcSerializeSize += sizeof(ushort);
			}

			foreach (var m in memberTokenList)
			{
				string typeName = m.Type;

				if (ReflectionHelper.CanGetByteSizeByTypeName(typeName))
				{
					calcSerializeSize += ReflectionHelper.GetByteSizeByTypeName(typeName);
				}
				else if (_enumSizeByTypeName.TryGetValue(typeName, out var size))
				{
					calcSerializeSize += size;
				}
				else
				{
					if (!string.IsNullOrEmpty(sizeExpression))
					{
						sizeExpression += " + ";
					}
					sizeExpression += $"{m.MemeberName}.{nameof(IPacketSerializable.SerializeSize)}";
				}
			}

			if (calcSerializeSize > 0)
			{
				sizeExpression += $" + {calcSerializeSize}";
			}

			if (dataType == PacketDataType.ClientPacket ||
				dataType == PacketDataType.ServerPacket)
			{
				sizeExpression = string.Format(PacketFormat.PacketSerializeSize,
											   nameof(IPacketSerializable.SerializeSize),
											   sizeExpression);
			}
			else
			{
				sizeExpression = string.Format(PacketFormat.SerializeSize,
											   nameof(IPacketSerializable.SerializeSize),
											   sizeExpression);
			}

			// Generate serialization codes
			string serializeFunction = string.Empty;
			string deserializeFunction = string.Empty;

			string serializeContent = string.Empty;
			string deserializeContent = string.Empty;

			if (dataType == PacketDataType.ClientPacket ||
				dataType == PacketDataType.ServerPacket)
			{
				serializeContent += string.Format(PacketFormat.MemberSerializeByWriter,
												  nameof(PacketBase.PacketType));
				serializeContent += NewLine;
			}

			for (int i = 0; i < memberTokenList.Count; i++)
			{
				var m = memberTokenList[i];

				//if (m.Type == nameof(NetString) || m.Type == nameof(NetStringShort))
				//{
				//	m.IsNative = true;
				//}

				if (m.IsNative)
				{
					// use writer.Put()
					serializeContent += string.Format(PacketFormat.MemberSerializeByWriter, m.MemeberName);

					// use reader.ReadTypeName()
					string typeName = string.Empty;

					// Check enum
					if (_enumSizeByTypeName.ContainsKey(m.Type))
					{
						typeName = m.Type;
					}
					// Check CLR type name
					else if (ReflectionHelper.TryGetCLRTypeByPrimitive(m.Type, out string clrType))
					{
						typeName = clrType;
					}

					deserializeContent += string.Format(PacketFormat.MemberDeserializeByReader, m.MemeberName, typeName);
				}
				else
				{
					serializeContent += string.Format(PacketFormat.MemberSerializeBySelf, m.MemeberName);
					deserializeContent += string.Format(PacketFormat.MemberDeserializeBySelf, m.MemeberName);
				}

				if (i < memberTokenList.Count - 1)
				{
					serializeContent += NewLine;
					deserializeContent += NewLine;
				}
			}

			serializeContent = addIndent(serializeContent);
			deserializeContent = addIndent(deserializeContent);

			if (dataType == PacketDataType.ClientPacket ||
				dataType == PacketDataType.ServerPacket)
			{
				serializeFunction = string.Format(PacketFormat.PacketSerializeFunction,
												  nameof(IPacketSerializable.Serialize),
												  nameof(IPacketWriter),
												  WriterName,
												  serializeContent);

				deserializeFunction = string.Format(PacketFormat.PacketDeserializeFunction,
												  nameof(IPacketSerializable.TryDeserialize),
												  nameof(IPacketReader),
												  ReaderName,
												  deserializeContent);
			}
			else
			{
				serializeFunction = string.Format(PacketFormat.SerializeFunction,
												  nameof(IPacketSerializable.Serialize),
												  nameof(IPacketWriter),
												  WriterName,
												  serializeContent);

				deserializeFunction = string.Format(PacketFormat.DeserializeFunction,
												  nameof(IPacketSerializable.TryDeserialize),
												  nameof(IPacketReader),
												  ReaderName,
												  deserializeContent);
			}

			// Combine generated codes
			var combineContent = dataTypeContent;
			if (!string.IsNullOrEmpty(memberContent))
			{
				combineContent += memberContent;
			}

			sizeExpression = addIndent(sizeExpression);
			serializeFunction = addIndent(serializeFunction);
			deserializeFunction = addIndent(deserializeFunction);

			if (dataType == PacketDataType.ClientPacket ||
				dataType == PacketDataType.ServerPacket)
			{
				string packetType = string.Format(PacketFormat.PacketTypeDeclaration,
												  nameof(PacketType),
												  nameof(PacketBase.PacketType),
												  $"{nameof(PacketType)}.{className}");

				packetType = addIndent(packetType);

				content = string.Format(PacketFormat.PacketDataTypeDefinition,
										declaration,
										className,
										nameof(PacketBase),
										packetType,
										combineContent,
										sizeExpression,
										serializeFunction,
										deserializeFunction);
			}
			else
			{
				content = string.Format(PacketFormat.DataTypeDefinition,
										declaration,
										className,
										nameof(IPacketSerializable),
										combineContent,
										sizeExpression,
										serializeFunction,
										deserializeFunction,
										addIndent(PacketFormat.IgnoreFunction));
			}

			return className;
		}

		/// <summary>
		/// 코드 생성을 위한 멤버 정의 토큰입니다. 구문 분석을 통해 생성됩니다.
		/// </summary>
		private struct MemberDefinitionToken
		{
			/// <summary>멤버의 타입 이름입니다.</summary>
			public string Type;
			///// <summary>데이터 타입입니다.</summary>
			//public PacketDataType DataType;
			/// <summary>멤버 변수의 이름입니다.</summary>
			public string MemeberName;
			/// <summary>원시 타입이거나 enum 타입인지 여부입니다.</summary>
			public bool IsNative;
			/// <summary>제너릭 타입 이름입니다. 없다면 공백입니다.</summary>
			public string GenericType;
			/// <summary>제너릭 타입 여부입니다.</summary>
			public bool HasGeneric => !string.IsNullOrEmpty(GenericType);
		}

		/// <summary>
		/// XML을 파싱하여 멤버 변수 선언 코드를 생성합니다.
		/// </summary>
		/// <exception cref="WrongElementException"></exception>
		/// <exception cref="WrongAttributeException"></exception>
		private void parseMembers(PacketDataType dataType, XmlReader r, out string content,
								  out List<MemberDefinitionToken> members)
		{
			int currentDepth = r.Depth;
			content = string.Empty;
			members = new();

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
					parseDataType(r, out string dataContent, out _);
					content += addIndentWithNewLine(dataContent) + NewLine;
					continue;
				}

				MemberDefinitionToken member = new MemberDefinitionToken();

				string dataTypeName = r.Name;
				if (_enumSizeByTypeName.ContainsKey(dataTypeName))
				{
					member.IsNative = true;
					member.Type = dataTypeName;
				}
				else
				{
					member.IsNative = ReflectionHelper.TryGetTypeByCLRType(dataTypeName, out var value);
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
					if (m.HasGeneric)
					{
						memberContent += string.Format(PacketFormat.MemberDeclarationGeneric,
													   m.Type, m.GenericType, m.MemeberName);
					}
					else
					{
						if (dataType == PacketDataType.Struct)
						{
							memberContent += string.Format(PacketFormat.MemberDeclarationNotInitialize,
														   m.Type, m.MemeberName);
						}
						else
						{
							memberContent += string.Format(PacketFormat.MemberDeclaration,
														   m.Type, m.MemeberName, @"new()");
						}
					}
				}

				if (i < members.Count - 1)
				{
					memberContent += NewLine;
				}
			}

			content += addIndent(memberContent);
		}

		/// <summary>
		/// 현재 XML 요소에서 해당되는 패킷 속성의 내용을 반환합니다.
		/// </summary>
		/// <param name="type">패킷 속성입니다.</param>
		/// <param name="token">속성의 내용입니다.</param>
		private bool tryParse(XmlReader r, PacketAttributeType type, out string token)
		{
			var tokenType = type.ToString().ToLower();
			token = r[tokenType] ?? "";
			return !string.IsNullOrEmpty(token);
		}

		/// <summary>
		/// 현재 XML 요소가 파싱 가능한 유효한 요소인지 여부를 반환합니다.
		/// XML 요소가 Element인지, Depth가 0보다 크다면 파싱 가능한 요소입니다.
		/// </summary>
		private bool isValidElement(XmlReader r)
		{
			return r.Depth > 0 && r.NodeType == XmlNodeType.Element;
		}

		/// <summary>입력된 문자열에 들여쓰기를 합니다.</summary>
		private string addIndent(string content)
		{
			return Indent + content.Replace(NewLine, $"{NewLine}{Indent}");
		}

		/// <summary>입력된 문자열에 들여쓰기 후 개행을 합니다.</summary>
		private string addIndentWithNewLine(string content)
		{
			return Indent + content.Replace(NewLine, $"{NewLine + Indent}") + NewLine;
		}
	}
}