/*
 * Generated File : PacketFactory.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using System;
using System.Collections.Generic;
using CT.Common.Serialization;
using CT.Packets;
using CT.Tools.Collections;
using CTS.Instance.PacketCustom;

namespace CTS.Instance.Packets
{
	public delegate PacketBase ReadPacket(PacketReader reader);
	public delegate PacketBase CreatePacket();

	public static class PacketFactory
	{
		private static Dictionary<PacketType, CreatePacket> _packetCreateByEnum = new()
		{
			{ PacketType.CS_Req_TryJoinGameInstance, () => new CS_Req_TryJoinGameInstance() },
			{ PacketType.SC_Ack_TryJoinGameInstance, () => new SC_Ack_TryJoinGameInstance() },
			{ PacketType.SC_OnClientEnter, () => new SC_OnClientEnter() },
			{ PacketType.SC_OnClientLeave, () => new SC_OnClientLeave() },
			{ PacketType.SC_MiniGameData, () => new SC_MiniGameData() },
			{ PacketType.SC_SpawnEntities, () => new SC_SpawnEntities() },
			{ PacketType.CS_Req_PlayerInput_Movement, () => new CS_Req_PlayerInput_Movement() },
			{ PacketType.CS_Req_PlayerInput_Action, () => new CS_Req_PlayerInput_Action() },
			
		};

		private static Dictionary<Type, CreatePacket> _packetCreateByType = new()
		{
			{ typeof(CS_Req_TryJoinGameInstance), () => new CS_Req_TryJoinGameInstance() },
			{ typeof(SC_Ack_TryJoinGameInstance), () => new SC_Ack_TryJoinGameInstance() },
			{ typeof(SC_OnClientEnter), () => new SC_OnClientEnter() },
			{ typeof(SC_OnClientLeave), () => new SC_OnClientLeave() },
			{ typeof(SC_MiniGameData), () => new SC_MiniGameData() },
			{ typeof(SC_SpawnEntities), () => new SC_SpawnEntities() },
			{ typeof(CS_Req_PlayerInput_Movement), () => new CS_Req_PlayerInput_Movement() },
			{ typeof(CS_Req_PlayerInput_Action), () => new CS_Req_PlayerInput_Action() },
			
		};

		private static BidirectionalMap<Type, PacketType> _packetTypeTable = new()
		{
			{ typeof(CS_Req_TryJoinGameInstance), PacketType.CS_Req_TryJoinGameInstance },
			{ typeof(SC_Ack_TryJoinGameInstance), PacketType.SC_Ack_TryJoinGameInstance },
			{ typeof(SC_OnClientEnter), PacketType.SC_OnClientEnter },
			{ typeof(SC_OnClientLeave), PacketType.SC_OnClientLeave },
			{ typeof(SC_MiniGameData), PacketType.SC_MiniGameData },
			{ typeof(SC_SpawnEntities), PacketType.SC_SpawnEntities },
			{ typeof(CS_Req_PlayerInput_Movement), PacketType.CS_Req_PlayerInput_Movement },
			{ typeof(CS_Req_PlayerInput_Action), PacketType.CS_Req_PlayerInput_Action },
			
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