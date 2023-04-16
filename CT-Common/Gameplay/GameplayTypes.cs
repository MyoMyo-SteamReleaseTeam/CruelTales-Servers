using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Common.Gameplay
{

	public enum MiniGameModeType
	{
		None = 0,
		MG_RedHood,
	}

	public enum CompetitionType
	{
		Individual = 0,
		IndividualOrTeam,
		Team,
	}

	public enum InteractActionType
	{
		None = 0,
		Switch,
		Prograss,
	}
}
