using System;
using System.Collections.Generic;
using System.Xml;

namespace CT.PacketGenerator
{
	internal class PacketParser
	{
		public List<string> DataTypeList = new()
		{
			"struct", "class", "packet-server", "packet-client"
		};

		public void ParseFromXml(string path)
		{
			XmlReaderSettings settings = new XmlReaderSettings()
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
			};

			using (XmlReader r =  XmlReader.Create(path, settings))
			{
				r.MoveToContent();

				while (r.Read())
				{
					if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
					{
						if (DataTypeList.Contains(r.Name))
						{
							ParseDataType(r);
						}
					}

					Console.WriteLine($"Depth({r.Depth}) Type({r.NodeType}) Name({r.Name}) ElemName({r["name"]}) Type({r["type"]})");

					if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
					{

					}
				}
			}
		}

		public string ParseDataType(XmlReader r)
		{
			string 

			return "";
		}

		public string  
	}
}
