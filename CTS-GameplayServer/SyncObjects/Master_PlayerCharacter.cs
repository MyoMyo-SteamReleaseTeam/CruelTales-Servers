/*
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
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

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
		[SyncVar]
		private Vector2 _test = new();
		[SyncRpc(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		public partial void Client_InputMovement(NetworkPlayer player, Vector2 direction);
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
		public Vector2 Test
		{
			get => _test;
			set
			{
				if (_test == value) return;
				_test = value;
				_dirtyReliable_0[3] = true;
			}
		}
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
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
				writer.Put(_test);
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			_userId.Serialize(writer);
			_username.Serialize(writer);
			writer.Put(_costume);
			writer.Put(_test);
		}
		public override void InitializeProperties()
		{
			_userId = new();
			_username = new();
			_costume = 0;
			_test = new();
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
					Client_InputMovement(player, direction);
				}
			}
			return true;
		}
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
				}
			}
		}
	}
}
#pragma warning restore CS0649
