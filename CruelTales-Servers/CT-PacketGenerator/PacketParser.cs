using System.Xml;

namespace CT.PacketGenerator
{
	internal class PacketParser
	{
		public PacketParser()
		{
				
		}
		
		public void ParseFromXml(string path)
		{
			XmlReaderSettings settings = new XmlReaderSettings()
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
			};

			using (XmlReader r =  XmlReader.Create(path, settings))
			{
				
			}

		}
	}
}
