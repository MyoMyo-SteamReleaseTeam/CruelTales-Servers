﻿using System;
using System.Numerics;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject
	{
		public override VisibilityType Visibility => VisibilityType.View;
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.All;

		public float Speed = 5.0f;

		public NetworkPlayer? NetworkPlayer { get; private set; }

		public void BindNetworkPlayer(NetworkPlayer player)
		{
			this.UserId = player.UserId;
			this.Username = player.Username;
			NetworkPlayer = player;
			NetworkPlayer.BindViewTarget(this.Transform);
		}

		public override void OnDestroyed()
		{
			NetworkPlayer?.ReleaseViewTarget();
		}

		public partial void Client_InputMovement(NetworkPlayer player, Vector2 direction)
		{
			if (_userId != player.UserId)
				return;

			Transform.Move(Transform.Position, new Vector3(direction.X, 0, direction.Y) * Speed);
		}
	}
}