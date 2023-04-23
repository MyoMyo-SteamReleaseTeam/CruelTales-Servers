using System;
using System.Reflection.PortableExecutable;
using CT.Common.Serialization;
using CT.Packets;

namespace CTC.Networks.PacketCustom
{
	public sealed partial class SC_SpawnEntities : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_SpawnEntities;

		private ArraySegment<byte> _data;
		private PacketWriter _writer;
		private PacketReader _reader;

		public SC_SpawnEntities()
		{
			_data = new(new byte[1500]);
			_writer = new PacketWriter(_data);
			_reader = new PacketReader(_data);
		}

		public override int SerializeSize => 1;

		public override void Serialize(PacketWriter writer) { }

		public override void Deserialize(PacketReader reader)
		{
			_data = reader.ReadBytes();

			var count = reader.ReadByte();
			for (int i = 0; i < count; ++i)
			{

			}
		}
	}
	public sealed partial class SC_InitialData : PacketBase
	{
		public override PacketType PacketType => PacketType.SC_SpawnEntities;

		private ArraySegment<byte> _data;
		private PacketWriter _writer;
		private PacketReader _reader;

		public SC_InitialData()
		{
			_data = new(new byte[1500]);
			_writer = new PacketWriter(_data);
			_reader = new PacketReader(_data);
		}

		public override int SerializeSize => 1;

		public override void Serialize(PacketWriter writer) { }

		public override void Deserialize(PacketReader reader)
		{
			_data = reader.ReadBytes();

			var count = reader.ReadByte();
			for (int i = 0; i < count; ++i)
			{

			}
		}
	}
	
}