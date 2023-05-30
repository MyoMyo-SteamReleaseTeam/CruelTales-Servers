using System;
using System.Collections.Generic;
using CT.Common.Serialization;
using CT.Packets;
using log4net;

namespace CTS.Instance.Packets
{
	public class MtuBufferPool
	{
#pragma warning disable IDE0052
		private static readonly ILog _log = LogManager.GetLogger(typeof(MtuBufferPool));
#pragma warning restore IDE0052

		private Dictionary<Type, Stack<PacketBase>> _packetByType = new();
		private Dictionary<PacketType, Stack<PacketBase>> _packetByEnum = new();

		public PacketBase GetBuffer(PacketType type)
		{
			if (_packetByEnum.TryGetValue(type, out var packetStack))
			{
				if (packetStack.TryPop(out var poolPacket))
				{
					return poolPacket;
				}
			}
			else
			{
				var pool = new Stack<PacketBase>();
				_packetByEnum.Add(type, pool);
				var packetType = PacketFactory.GetTypeByEnum(type);
				_packetByType.Add(packetType, pool);
			}

			return PacketFactory.CreatePacket(type);
		}

		public void Return(PacketBase packet)
		{
			var type = packet.PacketType;
			if (_packetByEnum.TryGetValue(type, out var packetStack))
			{
				packetStack.Push(packet);
				return;
			}

			var pool = new Stack<PacketBase>();
			_packetByEnum.Add(type, pool);
			var packetType = PacketFactory.GetTypeByEnum(type);
			_packetByType.Add(packetType, pool);
		}
	}
}
