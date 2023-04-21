/*
 * Generated File : PacketDispatcher.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

using System.Collections.Generic;
using CT.Common.Serialization;
using CT.Packets;
using CTS.Instance.Networks;

namespace CTS.Instance.Packets
{
	public delegate void HandlePacket(PacketBase receivedPacket, UserSession session);

	public static class PacketDispatcher
	{
		public static Dictionary<PacketType, HandlePacket> _dispatcherTable = new()
		{
			{ PacketType.CS_Req_TryJoinGameInstance, PacketHandler.Handle_CS_Req_TryJoinGameInstance },
			{ PacketType.CS_Req_UserInput_Movement, PacketHandler.Handle_CS_Req_UserInput_Movement },
			{ PacketType.CS_Req_UserInput_Action, PacketHandler.Handle_CS_Req_UserInput_Action },
			
		};

		public static void Dispatch(PacketBase receivedPacket, UserSession session)
		{
			_dispatcherTable[receivedPacket.PacketType](receivedPacket, session);
		}
	}
}