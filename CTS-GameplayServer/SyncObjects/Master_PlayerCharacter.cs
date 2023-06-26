﻿/*
 * Generated File : Master_PlayerCharacter
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class PlayerCharacter
	{
		public override NetworkObjectType Type => NetworkObjectType.PlayerCharacter;
		[SyncVar]
		private UserId _userId = new();
		[SyncVar]
		private NetStringShort _username = new();
		[SyncVar]
		private int _costume;
		[SyncVar(SyncType.ColdData)]
		private DokzaAnimation _currentAnimation = new();
		[SyncVar(SyncType.ColdData)]
		private float _animationTime;
		[SyncVar(SyncType.ColdData)]
		private bool _isRight;
		[SyncVar(SyncType.ColdData)]
		private bool _isUp;
		[SyncRpc]
		public partial void Server_OnAnimationChanged(DokzaAnimation animation, bool isRight, bool isUp);
		[SyncRpc(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		public partial void Client_InputMovement(NetworkPlayer player, Vector2 direction, bool isWalk);
		[SyncRpc(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		public partial void Client_InputInteraction(NetworkPlayer player, NetworkIdentity target, Input_InteractType interactType);
		[SyncRpc(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		public partial void Client_InputAction(NetworkPlayer player, Input_PlayerAction actionType, Vector2 direction);
		private BitmaskByte _dirtyReliable_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _dirtyReliable_0.AnyTrue();
				return isDirty;
			}
		}
		public override bool IsDirtyUnreliable => false;
		public UserId UserId
		{
			get => _userId;
			set
			{
				if (_userId == value) return;
				_userId = value;
				_dirtyReliable_0[0] = true;
			}
		}
		public NetStringShort Username
		{
			get => _username;
			set
			{
				if (_username == value) return;
				_username = value;
				_dirtyReliable_0[1] = true;
			}
		}
		public int Costume
		{
			get => _costume;
			set
			{
				if (_costume == value) return;
				_costume = value;
				_dirtyReliable_0[2] = true;
			}
		}
		public partial void Server_OnAnimationChanged(DokzaAnimation animation, bool isRight, bool isUp)
		{
			Server_OnAnimationChangedCallstack.Add((animation, isRight, isUp));
			_dirtyReliable_0[3] = true;
		}
		private List<(DokzaAnimation animation, bool isRight, bool isUp)> Server_OnAnimationChangedCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			Server_OnAnimationChangedCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				_userId.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				_username.Serialize(writer);
			}
			if (_dirtyReliable_0[2])
			{
				writer.Put(_costume);
			}
			if (_dirtyReliable_0[3])
			{
				byte count = (byte)Server_OnAnimationChangedCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_OnAnimationChangedCallstack[i];
					writer.Put((int)arg.animation);
					writer.Put(arg.isRight);
					writer.Put(arg.isUp);
				}
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			_userId.Serialize(writer);
			_username.Serialize(writer);
			writer.Put(_costume);
			writer.Put((int)_currentAnimation);
			writer.Put(_animationTime);
			writer.Put(_isRight);
			writer.Put(_isUp);
		}
		public override void InitializeMasterProperties()
		{
			_userId = new();
			_username = new();
			_costume = 0;
			_currentAnimation = (DokzaAnimation)0;
			_animationTime = 0;
			_isRight = false;
			_isUp = false;
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader) => true;
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadVector2(out var direction)) return false;
					if (!reader.TryReadBoolean(out bool isWalk)) return false;
					Client_InputMovement(player, direction, isWalk);
				}
			}
			if (dirtyUnreliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetworkIdentity target = new();
					if (!target.TryDeserialize(reader)) return false;
					if (!reader.TryReadByte(out var interactTypeValue)) return false;
					Input_InteractType interactType = (Input_InteractType)interactTypeValue;
					Client_InputInteraction(player, target, interactType);
				}
			}
			if (dirtyUnreliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadByte(out var actionTypeValue)) return false;
					Input_PlayerAction actionType = (Input_PlayerAction)actionTypeValue;
					if (!reader.TryReadVector2(out var direction)) return false;
					Client_InputAction(player, actionType, direction);
				}
			}
			return true;
		}
		public override void InitializeRemoteProperties() { }
		public override void IgnoreSyncReliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticReliable(IPacketReader reader) { }
		public override void IgnoreSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Vector2Extension.IgnoreStatic(reader);
					reader.Ignore(1);
				}
			}
			if (dirtyUnreliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetworkIdentity.IgnoreStatic(reader);
					reader.Ignore(1);
				}
			}
			if (dirtyUnreliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
					Vector2Extension.IgnoreStatic(reader);
				}
			}
		}
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Vector2Extension.IgnoreStatic(reader);
					reader.Ignore(1);
				}
			}
			if (dirtyUnreliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetworkIdentity.IgnoreStatic(reader);
					reader.Ignore(1);
				}
			}
			if (dirtyUnreliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
					Vector2Extension.IgnoreStatic(reader);
				}
			}
		}
	}
}
#pragma warning restore CS0649
