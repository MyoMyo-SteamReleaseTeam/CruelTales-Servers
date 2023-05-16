using System;
using System.Collections.Generic;
using CT.Common.Serialization;
using CT.Packets;
#if NET
using log4net;
#elif UNITY_2021
using CT.Logger;
#endif

namespace CTC.Networks.Packets
{
	public class PacketPool
	{
#pragma warning disable IDE0052
		private static readonly ILog _log = LogManager.GetLogger(typeof(PacketPool));
#pragma warning restore IDE0052

		private Dictionary<Type, Stack<PacketBase>> _packetByType = new();
		private Dictionary<PacketType, Stack<PacketBase>> _packetByEnum = new();

		public T GetPacket<T>() where T : PacketBase
		{
			Type type = typeof(T);
			if (_packetByType.TryGetValue(type, out var packetStack))
			{
				if (packetStack.TryPop(out var poolPacket))
				{
					return (T)poolPacket;
				}
			}
			else
			{
				var pool = new Stack<PacketBase>();
				_packetByType.Add(type, pool);
				var packetEnum = PacketFactory.GetEnumByType(type);
				_packetByEnum.Add(packetEnum, pool);
			}

			return PacketFactory.CreatePacket<T>();
		}

		public PacketBase GetPacket(PacketType type)
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

		public PacketBase ReadPacket(PacketType packetType, IPacketReader reader)
		{
			var packet = GetPacket(packetType);
			packet.Deserialize(reader);
			return packet;
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
