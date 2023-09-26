using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using KaNet.Physics;
using static System.Collections.Specialized.BitVector32;

namespace CTS.Instance.SyncObjects
{
	public partial class CameraController : MasterNetworkObject
	{
		public override VisibilityType Visibility => VisibilityType.Global;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.Owner;

		[AllowNull] public NetworkPlayer NetworkPlayer { get; private set; }
		public PlayerCharacter? TargetPlayerCharacter { get; private set; }

		public Vector2 ViewPosition { get; private set; }

		private Action _moveToTarget;

		public override void Constructor()
		{
			_moveToTarget = moveToTarget;
		}

		public override void OnCreated()
		{
			IsPersistent = true;
			FollowSpeed = 10;
		}

		public override void OnDestroyed()
		{
			Owner = new UserId(0);
			NetworkPlayer = null;
			TargetPlayerCharacter = null;
		}

		public override void OnUpdate(float deltaTime)
		{
			Vector2 targetPosition = TargetPlayerCharacter == null ?
				ViewPosition : TargetPlayerCharacter.Position;

			float ratio = FollowSpeed * deltaTime;
			if (ratio > 1)
			{
				ratio = 1;
			}

			ViewPosition = Vector2.Lerp(ViewPosition, targetPosition, ratio);
			NetworkPlayer.ViewPosition = ViewPosition;
		}

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			Owner = player.UserId;
			NetworkPlayer = player;
		}

		public void ReleaseNetworkPlayer(NetworkPlayer player)
		{
			if (NetworkPlayer != player)
				return;

			Owner = new UserId(0);
			NetworkPlayer = null;
		}

		public void BindPlayerCharacter(PlayerCharacter player)
		{
			TargetPlayerCharacter = player;
			TargetId = player.Identity;
			MoveToTarget();
		}

		public void ReleasePlayerCharacter(PlayerCharacter player)
		{
			if (TargetPlayerCharacter == player)
			{
				TargetPlayerCharacter = null;
				TargetId = new NetworkIdentity(0);
			}
		}

		public void MoveToTarget()
		{
			if (TargetPlayerCharacter == null)
				return;
			ViewPosition = TargetPlayerCharacter.Position;
			StartCoroutine(_moveToTarget, 0);
		}

		public partial void Client_CannotFindBindTarget(NetworkPlayer player)
		{
			if (TargetPlayerCharacter != null)
			{
				ViewPosition = TargetPlayerCharacter.Position;
				StartCoroutine(_moveToTarget, 0);
			}
		}

		private void moveToTarget()
		{
			Server_MoveTo(ViewPosition);
			ResetZoom();
		}

		public void ResetZoom()
		{
			float zoom = 7.0f;
			if (TargetPlayerCharacter != null)
			{
				zoom = TargetPlayerCharacter.Section == 0 ? 7 : 6;
			}
			Server_Zoom(zoom);
		}
	}
}