namespace CT.Packets
{
	public enum PacketType
	{
		None = 0,
		Client_TryConnect,
		Server_AckConnect,
		Client_TrySendUserProfile,
		Server_InitialWorldState,
		Server_SpawnEntities,
		Server_DespawnEntities,
	}
}
