/*
 * Generated File : PacketFactory.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using System;
using System.Collections.Generic;
using CT.Packets;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;

namespace CTS.Instance.Packets
{
	public delegate PacketBase ReadPacket(IPacketReader reader);
	public delegate PacketBase CreatePacket();

	public static partial class PacketFactory
	{
		private static Dictionary<PacketType, CreatePacket> _packetCreateByEnum = new()
		{
			{ PacketType.CS_Req_TryEnterGameInstance, () => new CS_Req_TryEnterGameInstance() },
			{ PacketType.SC_Ack_TryEnterGameInstance, () => new SC_Ack_TryEnterGameInstance() },
			{ PacketType.CS_Req_ReadyToSync, () => new CS_Req_ReadyToSync() },
			
		};

		private static Dictionary<Type, CreatePacket> _packetCreateByType = new()
		{
			{ typeof(CS_Req_TryEnterGameInstance), () => new CS_Req_TryEnterGameInstance() },
			{ typeof(SC_Ack_TryEnterGameInstance), () => new SC_Ack_TryEnterGameInstance() },
			{ typeof(CS_Req_ReadyToSync), () => new CS_Req_ReadyToSync() },
			
		};

		private static BidirectionalMap<Type, PacketType> _packetTypeTable = new()
		{
			{ typeof(CS_Req_TryEnterGameInstance), PacketType.CS_Req_TryEnterGameInstance },
			{ typeof(SC_Ack_TryEnterGameInstance), PacketType.SC_Ack_TryEnterGameInstance },
			{ typeof(CS_Req_ReadyToSync), PacketType.CS_Req_ReadyToSync },
			
		};

		public static T CreatePacket<T>() where T : PacketBase
		{
			return (T)_packetCreateByType[typeof(T)]();
		}

		public static PacketBase CreatePacket(PacketType type)
		{
			return _packetCreateByEnum[type]();
		}

		public static Type GetTypeByEnum(PacketType value)
		{
			return _packetTypeTable.GetValue(value);
		}

		public static PacketType GetEnumByType(Type value)
		{
			return _packetTypeTable.GetValue(value);
		}
	}
}