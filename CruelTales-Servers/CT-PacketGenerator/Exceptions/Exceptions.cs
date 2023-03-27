﻿using System;
using System.Xml;

namespace CT.PacketGenerator.Exceptions
{
	internal class WrongDefinitionException : Exception
	{
		internal WrongDefinitionException()
			: base($"Wrong packet xml difinition.") { }
	}

	internal class WrongElementException : Exception
	{
		internal WrongElementException(XmlReader r)
			: base($"Wrong xml element. {XmlExceptionHelper.GetNodeInfo(r)}") { }
	}

	internal class WrongDataTypeException : Exception
	{
		internal WrongDataTypeException(XmlReader r)
			: base($"Wrong data type detected. Element name : {r.Name}") { }
	}

	internal class WrongAttributeException : Exception
	{
		internal WrongAttributeException(XmlReader r, PacketAttributeType parseType)
			: base($"Wrong \"{parseType}\" attribute in : {XmlExceptionHelper.GetNodeInfo(r)}") { }
	}

	internal class WrongDeclarationException : Exception
	{
		internal WrongDeclarationException(XmlReader r)
			: base($"Wrong declaration in : {XmlExceptionHelper.GetNodeInfo(r)}") { }
	}

	internal static class XmlExceptionHelper
	{
		public static string GetNodeInfo(XmlReader r)
		{
			return $"NodeType({r.NodeType}) Depth({r.Depth}) Name({r.Name})";
		}
	}
}
