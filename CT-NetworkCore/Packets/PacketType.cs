namespace CT.Packets
{
	public enum PacketType
	{
		None = 0,
		Client_Req_TryJoinMatching,
		Server_Ack_TryJoinMatching,
		Server_InitialWorldState,
		Server_SpawnEntities,
		Server_DespawnEntities,
	}
}
