using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Serialization;
using CT.Packets;
using CTC.Networks.Packets;
using log4net;

namespace CTS.Instance.Packets
{
	public class PacketPool
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(PacketPool));

		private Dictionary<PacketType, Stack<PacketBase>> _packetPoolTable = new();

		public PacketPool()
		{
			var types = Enum.GetValues<PacketType>();
			foreach (var pt in types)
			{
				_packetPoolTable.Add(pt, new Stack<PacketBase>());
			}
		}

		public bool TryGetPacket(PacketType packetType,
								 [MaybeNullWhen(false)] out PacketBase packet)
		{
			if (_packetPoolTable.TryGetValue(packetType, out var packetStack))
			{
				if (packetStack.TryPop(out var poolPacket))
				{
					packet = poolPacket;
					return true;
				}

				if (PacketFactory.CreatePacket(packetType, out var createPacket))
				{
					packet = createPacket;
					return true;
				}
			}

			packet = null;
			_log.Fatal($"There is no {packetType} in the packet pool!");
			return false;
		}

		public bool TryReadPacket(PacketType packetType, PacketReader reader,
								  [MaybeNullWhen(false)] out PacketBase packet)
		{
			if (_packetPoolTable.TryGetValue(packetType, out var packetStack))
			{
				if (packetStack.TryPop(out var poolPacket))
				{
					poolPacket.Deserialize(reader);
					packet = poolPacket;
					return true;
				}
				
				if (PacketFactory.ReadPacket(packetType, reader, out var readPacket))
				{
					packet = readPacket;
					return true;
				}
			}

			packet = null;
			_log.Fatal($"There is no {packetType} in the packet pool!");
			return false;
		}

		public bool Return(PacketBase packet)
		{
			if (_packetPoolTable.TryGetValue(packet.PacketType, out var packetStack))
			{
				packetStack.Push(packet);
				return true;
			}

			_log.Fatal($"There is no {packet.PacketType} in the packet pool!");
			return false;
		}
	}
}
