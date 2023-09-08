using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public enum GameSystemState : byte
	{
		None = 0,
		Lobby,
		GameStartCountdown,
		Ready,
		InGame,
	}

	public static class GameSystemStateExtension
	{
		public static void Put(this IPacketWriter writer, GameSystemState value)
		{
			writer.Put((byte)value);
		}

		public static GameSystemState ReadGameSystemState(this IPacketReader reader)
		{
			return (GameSystemState)reader.ReadByte();
		}
	}
}
