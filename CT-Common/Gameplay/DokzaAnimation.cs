using System.Collections.Generic;

namespace CT.Common.Gameplay
{
	public enum DokzaAnimation
	{
		None = 0,
		Action_Push,
		Action_Pushed,
		Action_wolf_attack_side,
		Action_wolf_attack_up,
		test_back_attach,
		Redhood_mission_Bird,
		Redhood_mission_Clean1,
		Redhood_mission_Clean2,
		Costume_wolftail,
		Redhood_mission_Drink,
		Redhood_mission_Fireplace,
		Redhood_mission_Flower,
		Redhood_mission_Food,
		Redhood_mission_Herb,
		Idle_Back, //
		Idle_Back_sleep, //
		Idle_Front,
		Idle_Front_sleep,
		Redhood_mission_Letter,
		Redhood_mission_Pack,
		test_R,
		Run_Back, //
		Run_Front,
		Redhood_mission_Stump,
		test_test,
		Walk_Back, //
		Walk_Front,
		Redhood_mission_Wanted,
	}

	public static class DokzaAnimationExtension
	{
		private static Dictionary<DokzaAnimation, string> mDokzaAnimationTable = new()
		{
			{ DokzaAnimation.None, "none" },
			{ DokzaAnimation.Action_Push, "Action_Push" },
			{ DokzaAnimation.Action_Pushed, "Action_Pushed" },
			{ DokzaAnimation.Action_wolf_attack_side, "Action_wolf_attack_side" },
			{ DokzaAnimation.Action_wolf_attack_up, "Action_wolf_attack_up" },
			{ DokzaAnimation.test_back_attach, "test/back_attach" },
			{ DokzaAnimation.Redhood_mission_Bird, "Redhood_mission/Bird" },
			{ DokzaAnimation.Redhood_mission_Clean1, "Redhood_mission/Clean1" },
			{ DokzaAnimation.Redhood_mission_Clean2, "Redhood_mission/Clean2" },
			{ DokzaAnimation.Costume_wolftail, "Costume_wolftail" },
			{ DokzaAnimation.Redhood_mission_Drink, "Redhood_mission/Drink" },
			{ DokzaAnimation.Redhood_mission_Fireplace, "Redhood_mission/Fireplace" },
			{ DokzaAnimation.Redhood_mission_Flower, "Redhood_mission/Flower" },
			{ DokzaAnimation.Redhood_mission_Food, "Redhood_mission/Food" },
			{ DokzaAnimation.Redhood_mission_Herb, "Redhood_mission/Herb" },
			{ DokzaAnimation.Idle_Back, "Idle_Back" },
			{ DokzaAnimation.Idle_Back_sleep, "Idle_Back_sleep" },
			{ DokzaAnimation.Idle_Front, "Idle_Front" },
			{ DokzaAnimation.Idle_Front_sleep, "Idle_Front_sleep" },
			{ DokzaAnimation.Redhood_mission_Letter, "Redhood_mission/Letter" },
			{ DokzaAnimation.Redhood_mission_Pack, "Redhood_mission/Pack" },
			{ DokzaAnimation.test_R, "test/R" },
			{ DokzaAnimation.Run_Back, "Run_Back" },
			{ DokzaAnimation.Run_Front, "Run_Front" },
			{ DokzaAnimation.Redhood_mission_Stump, "Redhood_mission/Stump" },
			{ DokzaAnimation.test_test, "test/test" },
			{ DokzaAnimation.Walk_Back, "Walk_Back" },
			{ DokzaAnimation.Walk_Front, "Walk_Front" },
			{ DokzaAnimation.Redhood_mission_Wanted, "Redhood_mission/Wanted" },
		};

		public static bool IsFront(this DokzaAnimation value)
		{
			return !IsBack(value);
		}

		public static bool IsBack(this DokzaAnimation value)
		{
			return value == DokzaAnimation.Idle_Back ||
				value == DokzaAnimation.Idle_Back_sleep ||
				value == DokzaAnimation.Run_Back ||
				value == DokzaAnimation.Walk_Back;
		}

		public static string GetName(this DokzaAnimation value)
		{
			return mDokzaAnimationTable[value];
		}
	}
}
