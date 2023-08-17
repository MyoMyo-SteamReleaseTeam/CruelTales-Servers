using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CT.Common.Gameplay.Players
{
	public static class SkinDB
	{
		public static readonly Dictionary<int, List<string>> _skinDB = new Dictionary<int, List<string>>
		{
			{
				0, new List<string> 
					{ { "back/back_simple_angel_wing" }, { "cheek/cheek_simple_warm_circle" } }
			},
			
			{
				1, new List<string> 
					{ {"team/wolf"}, {"eye/eye_simple_button"}, {"lips/lips_smiling_cat"}, {"shoes/shoes_simple_brown"} }
			}
		};
	}
}