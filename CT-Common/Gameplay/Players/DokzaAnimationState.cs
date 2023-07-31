using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CT.Common.Gameplay.Players
{
	public enum DokzaAnimationState : byte
	{
		None = 0,
		Idle,
		Run,
		Walk,
		Jump,
		Knockback,
		Stun,
		Action,
		AFK,
		Push,
		Pushed,
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

		private readonly static Dictionary<DokzaAnimationState, DokzaAnimationInfo> _animationTable = new()
		{
			{ DokzaAnimationState.Idle , Idle },
			{ DokzaAnimationState.Run , Run },
			{ DokzaAnimationState.Walk , Walk },
			{ DokzaAnimationState.Action , Action },
		};

		public static bool TryGetAnimationInfo(DokzaAnimationState state, out DokzaAnimationInfo info)
		{
			return _animationTable.TryGetValue(state, out info);
		}
	}
}
