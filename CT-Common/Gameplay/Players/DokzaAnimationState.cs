using System.Collections.Generic;
using System.Runtime.InteropServices;
using CT.Common.Tools;

namespace CT.Common.Gameplay.Players
{
	public enum DokzaAnimationStateBase : byte
	{
		None = 0,

		/// <summary>대기 애니메이션 입니다.</summary>
		Idle,

		/// <summary>걷기 애니메이션 입니다.</summary>
		Walk,

		/// <summary>달리기 애니메이션 입니다.</summary
		Run,

		/// <summary>행동 애니메이션 입니다.</summary>
		Action,

		/// <summary>넉백 애니메이션 입니다.</summary>
		Knockback,

		/// <summary>이벤트 애니메이션 입니다.</summary>
		Event,
	}

	public enum DokzaAnimationState : byte
	{
		None = 0,

		/// <summary>대기 애니메이션 입니다.</summary>
		Idle,

		/// <summary>걷기 애니메이션 입니다.</summary>
		Walk,

		/// <summary>달리기 애니메이션 입니다.</summary>
		Run,

		/// <summary>행동 애니메이션 입니다.</summary>
		Action,

		Action_Hammer,
		Action_WolfCatch,

		/// <summary>넉백 애니메이션 입니다.</summary>
		Knockback,

		/// <summary>이벤트 애니메이션 입니다.</summary>
		Event,

		Event_RedHood_Bird,
		Event_RedHood_Clean1,
		Event_RedHood_Clean2,
		Event_RedHood_Drink,
		Event_RedHood_Fireplace,
		Event_RedHood_Flower,
		Event_RedHood_Food,
		Event_RedHood_Herb,
		Event_RedHood_Letter,
		Event_RedHood_Pack,
		Event_RedHood_Stump,
		Event_RedHood_Wanted,

		Event_AFK,
	}

	public static class DokzaAnimationStateExtension
	{
		private static readonly PartialEnumTable _enumTable = new()
		{
			{ (int)DokzaAnimationState.None, (int)DokzaAnimationStateBase.None },
			{ (int)DokzaAnimationState.Idle, (int)DokzaAnimationStateBase.Idle },
			{ (int)DokzaAnimationState.Walk, (int)DokzaAnimationStateBase.Walk },
			{ (int)DokzaAnimationState.Run, (int)DokzaAnimationStateBase.Run },
			{ (int)DokzaAnimationState.Action, (int)DokzaAnimationStateBase.Action },
			{ (int)DokzaAnimationState.Knockback, (int)DokzaAnimationStateBase.Knockback },
			{ (int)DokzaAnimationState.Event, (int)DokzaAnimationStateBase.Event },
		};

		public static DokzaAnimationStateBase GetBaseType(this DokzaAnimationState value)
		{
			return (DokzaAnimationStateBase)_enumTable.GetBaseTypeIndex((int)value);
		}

		public static bool IsBaseType(this DokzaAnimationState value, DokzaAnimationStateBase baseType)
		{
			return _enumTable.IsMatch((int)value, (int)baseType);
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct DokzaAnimationInfo
	{
		[FieldOffset(4)]
		public readonly DokzaAnimationState State;
		[FieldOffset(0)]
		public readonly float Duration;
		[FieldOffset(5)]
		public readonly bool IsLoop;

		public DokzaAnimationInfo(DokzaAnimationState state,
								  bool isLoop, float duration)
		{
			State = state;
			IsLoop = isLoop;
			Duration = duration;
		}
	}

	public static class DokzaAnimationDB
	{
		public static readonly DokzaAnimationInfo Idle = new(DokzaAnimationState.Idle, isLoop: true, 1.0f);
		public static readonly DokzaAnimationInfo Run = new(DokzaAnimationState.Run, isLoop: true, 1.0f);
		public static readonly DokzaAnimationInfo Walk = new(DokzaAnimationState.Walk, isLoop: true, 1.0f);
		public static readonly DokzaAnimationInfo Action = new(DokzaAnimationState.Action, isLoop: false, 1.0f);
		public static readonly DokzaAnimationInfo Knockback = new(DokzaAnimationState.Knockback, isLoop: false, 1.0f);

		private readonly static Dictionary<DokzaAnimationState, DokzaAnimationInfo> _animationTable = new()
		{
			{ DokzaAnimationState.Idle, Idle },
			{ DokzaAnimationState.Run , Run },
			{ DokzaAnimationState.Walk , Walk },
			{ DokzaAnimationState.Action , Action },
			{ DokzaAnimationState.Action_Hammer , Action },
			{ DokzaAnimationState.Action_WolfCatch , Action },
			{ DokzaAnimationState.Knockback , Knockback }
		};

		public static bool TryGetAnimationInfo(DokzaAnimationState state, out DokzaAnimationInfo info)
		{
			return _animationTable.TryGetValue(state, out info);
		}
	}
}
