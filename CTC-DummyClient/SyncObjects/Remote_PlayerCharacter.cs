/*
 * Generated File : Remote_PlayerCharacter
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTC.Networks.Synchronizations;
#if UNITY_2021
using UnityEngine;
#endif

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class PlayerCharacter
	{
		public override NetworkObjectType Type => NetworkObjectType.PlayerCharacter;
		[SyncRpc(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		public partial void Client_InputMovement(Vector2 direction, bool isWalk);
		[SyncRpc(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		public partial void Client_InputInteraction(NetworkIdentity target, Input_InteractType interactType);
		[SyncRpc(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		public partial void Client_InputAction(Input_PlayerAction actionType, Vector2 direction);
		[SyncVar]
		private UserId _userId = new();
		public UserId UserId => _userId;
		public event Action<UserId>? OnUserIdChanged;
		[SyncVar]
		private NetStringShort _username = new();
		public NetStringShort Username => _username;
		public event Action<NetStringShort>? OnUsernameChanged;
		[SyncVar]
		private int _costume;
		public int Costume => _costume;
		public event Action<int>? OnCostumeChanged;
		[SyncVar]
		private bool _isFreezed;
		public bool IsFreezed => _isFreezed;
		public event Action<bool>? OnIsFreezedChanged;
		[SyncVar(SyncType.ColdData)]
		private DokzaAnimation _currentAnimation;
		public DokzaAnimation CurrentAnimation => _currentAnimation;
		public event Action<DokzaAnimation>? OnCurrentAnimationChanged;
		[SyncVar(SyncType.ColdData)]
		private float _animationTime;
		public float AnimationTime => _animationTime;
		public event Action<float>? OnAnimationTimeChanged;
		[SyncVar(SyncType.ColdData)]
		private bool _isRight;
		public bool IsRight => _isRight;
		public event Action<bool>? OnIsRightChanged;
		[SyncVar(SyncType.ColdData)]
		private bool _isUp;
		public bool IsUp => _isUp;
		public event Action<bool>? OnIsUpChanged;
		[SyncRpc]
		public partial void Server_OnAnimationChanged(DokzaAnimation animation, bool isRight, bool isUp);
		private BitmaskByte _dirtyUnreliable_0 = new();
		public override bool IsDirtyReliable => false;
		public override bool IsDirtyUnreliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _dirtyUnreliable_0.AnyTrue();
				return isDirty;
			}
		}
		public partial void Client_InputMovement(Vector2 direction, bool isWalk)
		{
			Client_InputMovementCallstack.Add((direction, isWalk));
			_dirtyUnreliable_0[0] = true;
		}
		private List<(Vector2 direction, bool isWalk)> Client_InputMovementCallstack = new(4);
		public partial void Client_InputInteraction(NetworkIdentity target, Input_InteractType interactType)
		{
			Client_InputInteractionCallstack.Add((target, interactType));
			_dirtyUnreliable_0[1] = true;
		}
		private List<(NetworkIdentity target, Input_InteractType interactType)> Client_InputInteractionCallstack = new(4);
		public partial void Client_InputAction(Input_PlayerAction actionType, Vector2 direction)
		{
			Client_InputActionCallstack.Add((actionType, direction));
			_dirtyUnreliable_0[2] = true;
		}
		private List<(Input_PlayerAction actionType, Vector2 direction)> Client_InputActionCallstack = new(4);
		public override void ClearDirtyReliable() { }
		public override void ClearDirtyUnreliable()
		{
			_dirtyUnreliable_0.Clear();
			Client_InputMovementCallstack.Clear();
			Client_InputInteractionCallstack.Clear();
			Client_InputActionCallstack.Clear();
		}
		public override void SerializeSyncReliable(IPacketWriter writer) { }
		public override void SerializeSyncUnreliable(IPacketWriter writer)
		{
			_dirtyUnreliable_0.Serialize(writer);
			if (_dirtyUnreliable_0[0])
			{
				byte count = (byte)Client_InputMovementCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_InputMovementCallstack[i];
					writer.Put(arg.direction);
					writer.Put(arg.isWalk);
				}
			}
			if (_dirtyUnreliable_0[1])
			{
				byte count = (byte)Client_InputInteractionCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_InputInteractionCallstack[i];
					arg.target.Serialize(writer);
					writer.Put((byte)arg.interactType);
				}
			}
			if (_dirtyUnreliable_0[2])
			{
				byte count = (byte)Client_InputActionCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_InputActionCallstack[i];
					writer.Put((byte)arg.actionType);
					writer.Put(arg.direction);
				}
			}
		}
		public override void SerializeEveryProperty(IPacketWriter writer) { }
		public override void InitializeMasterProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!_userId.TryDeserialize(reader)) return false;
				OnUserIdChanged?.Invoke(_userId);
			}
			if (dirtyReliable_0[1])
			{
				if (!_username.TryDeserialize(reader)) return false;
				OnUsernameChanged?.Invoke(_username);
			}
			if (dirtyReliable_0[2])
			{
				if (!reader.TryReadInt32(out _costume)) return false;
				OnCostumeChanged?.Invoke(_costume);
			}
			if (dirtyReliable_0[3])
			{
				if (!reader.TryReadBoolean(out _isFreezed)) return false;
				OnIsFreezedChanged?.Invoke(_isFreezed);
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out var animationValue)) return false;
					DokzaAnimation animation = (DokzaAnimation)animationValue;
					if (!reader.TryReadBoolean(out bool isRight)) return false;
					if (!reader.TryReadBoolean(out bool isUp)) return false;
					Server_OnAnimationChanged(animation, isRight, isUp);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_userId.TryDeserialize(reader)) return false;
			OnUserIdChanged?.Invoke(_userId);
			if (!_username.TryDeserialize(reader)) return false;
			OnUsernameChanged?.Invoke(_username);
			if (!reader.TryReadInt32(out _costume)) return false;
			OnCostumeChanged?.Invoke(_costume);
			if (!reader.TryReadBoolean(out _isFreezed)) return false;
			OnIsFreezedChanged?.Invoke(_isFreezed);
			if (!reader.TryReadInt32(out var _currentAnimationValue)) return false;
			_currentAnimation = (DokzaAnimation)_currentAnimationValue;
			OnCurrentAnimationChanged?.Invoke(_currentAnimation);
			if (!reader.TryReadSingle(out _animationTime)) return false;
			OnAnimationTimeChanged?.Invoke(_animationTime);
			if (!reader.TryReadBoolean(out _isRight)) return false;
			OnIsRightChanged?.Invoke(_isRight);
			if (!reader.TryReadBoolean(out _isUp)) return false;
			OnIsUpChanged?.Invoke(_isUp);
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_userId = new();
			_username = new();
			_costume = 0;
			_isFreezed = false;
			_currentAnimation = (DokzaAnimation)0;
			_animationTime = 0;
			_isRight = false;
			_isUp = false;
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				UserId.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[1])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(1);
					reader.Ignore(1);
				}
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				UserId.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[1])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[4])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(1);
					reader.Ignore(1);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
