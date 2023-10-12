using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
#if CT_SERVER
using CTS.Instance.SyncObjects;
using log4net;
#elif CT_CLIENT
using CTC.Networks.SyncObjects.SyncObjects;
using CT.Logger;
#endif

namespace CTS.Instance.ClientShared.Databases
{
	public static class CharacterStatusDataDB
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(CharacterStatusDataDB));

		private static readonly Dictionary<NetworkObjectType, CharacterStatus> _statusByType = new();

		private static readonly CharacterStatus _defaultStatus = new CharacterStatus()
		{
			MoveSpeed = 7.0f,
			ActionAnimation = DokzaAnimationState.Action_Hammer,
			ActionCoolTime = 0,
			ActionRadius = 1.5f,
			ActionPower = 12.0f,
			ActionFriction = 2.0f,
			ActionDuration = 1.0f,
			KnockbackPower = 9.0f,
			KnockbackFriction = 2.0f,
			KnockbackDuration = 1.0f,
		};

#if CT_CLIENT
		static CharacterStatusDataDB()
		{
			TryLoad();
		}
#endif

		public static bool TryLoad()
		{
			_statusByType.Clear();
			_statusByType.Add(NetworkObjectType.PlayerCharacter, _defaultStatus);
			_statusByType.Add(NetworkObjectType.NormalCharacter, _defaultStatus);
			_statusByType.Add(NetworkObjectType.RedHoodCharacter, _defaultStatus);

			CharacterStatus wolfStatus = new(_defaultStatus);
			wolfStatus.ActionAnimation = DokzaAnimationState.Action_WolfCatch;
			_statusByType.Add(NetworkObjectType.WolfCharacter, wolfStatus);
			return true;
		}

		public static CharacterStatus GetCharacterStatus(NetworkObjectType type)
		{
			return _statusByType[type];
		}
	}
}
