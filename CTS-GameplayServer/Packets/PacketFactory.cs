/*
 * Generated File : PacketFactory.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Serialization;
using CT.Packets;
using CTS.Instance.PacketCustom;

namespace CTC.Networks.Packets
{
	public delegate PacketBase ReadPacket(PacketReader reader);
	public delegate PacketBase CreatePacket();

	public static class PacketFactory
	{
		private static Dictionary<PacketType, ReadPacket> _packetReadFactoryTable = new()
		{
			{ PacketType.CS_Req_TryJoinGameInstance, (r) => r.Read<CS_Req_TryJoinGameInstance>() },
			{ PacketType.SC_Ack_TryJoinGameInstance, (r) => r.Read<SC_Ack_TryJoinGameInstance>() },
			{ PacketType.SC_OnClientEnter, (r) => r.Read<SC_OnClientEnter>() },
			{ PacketType.SC_OnClientLeave, (r) => r.Read<SC_OnClientLeave>() },
			{ PacketType.SC_MiniGameData, (r) => r.Read<SC_MiniGameData>() },
			{ PacketType.SC_SpawnEntities, (r) => r.Read<SC_SpawnEntities>() },
			{ PacketType.CS_Req_PlayerInput_Movement, (r) => r.Read<CS_Req_PlayerInput_Movement>() },
			{ PacketType.CS_Req_PlayerInput_Action, (r) => r.Read<CS_Req_PlayerInput_Action>() },
			
		};

		private static Dictionary<PacketType, CreatePacket> _packetCreateFactoryTable = new()
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

		public static bool ReadPacket(PacketType packetType, PacketReader reader,
										[MaybeNullWhen(false)] out PacketBase packet)
		{
			if (_packetReadFactoryTable.TryGetValue(packetType, out var readFunc))
			{
				packet = readFunc(reader);
				return true;
			}

			packet = null;
			return false;
		}

		public static bool CreatePacket(PacketType packetType, [MaybeNullWhen(false)] out PacketBase packet)
		{
			if (_packetCreateFactoryTable.TryGetValue(packetType, out var createFunc))
			{
				packet = createFunc();
				return true;
			}

			packet = null;
			return false;
		}
	}
}