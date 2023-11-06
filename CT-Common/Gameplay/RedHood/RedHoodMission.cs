using System.Runtime.CompilerServices;
using CT.Common.DataType.Primitives;
using CT.Common.Gameplay.RedHood;

namespace CT.Common.Gameplay.RedHood
{
	public enum RedHoodMission : byte
	{
		None,
		Letter,
		HouseDust,
		FoodStore,
		MissionFlower,
		PackStore,
		GrandmotherHouseDust,
		SpringWater,
		Herb,
		BirdHouse,
		Stump,
		FirePlace,
		Wanted,
	}

	public static class RedHoodMissionExtension
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NetByte GetKey(this RedHoodMission value) => (byte)value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RedHoodMission GetRedHoodMission(this NetByte value) => (RedHoodMission)value.Value;
	}
}