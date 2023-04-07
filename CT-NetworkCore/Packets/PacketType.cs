namespace CT.Packets
{
	public enum PacketType
	{
		None = 0,
		Client_Req_TryJoinGameInstance,
		Server_Ack_TryJoinGameInstance,
		Server_InitialWorldState,
		Server_SpawnEntities,
		Server_DespawnEntities,
	}
}
