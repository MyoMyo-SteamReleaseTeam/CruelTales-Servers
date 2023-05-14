/*
 * Generated File : Master_NetworkPlayer
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class NetworkPlayer
	{
		public override NetworkObjectType Type => NetworkObjectType.NetworkPlayer;
		[SyncVar]
		private UserId _userId = new();
		[SyncVar]
		private NetStringShort _username = new();
		[SyncVar]
		private int _costume;
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
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(PacketWriter writer)
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
		}
		public override void SerializeSyncUnreliable(PacketWriter writer) { }
		public override void SerializeEveryProperty(PacketWriter writer)
		{
			_userId.Serialize(writer);
			_username.Serialize(writer);
			writer.Put(_costume);
		}
		public override void DeserializeSyncReliable(PacketReader reader) { }
		public override void DeserializeSyncUnreliable(PacketReader reader) { }
		public override void DeserializeEveryProperty(PacketReader reader) { }
		public override void IgnoreSyncReliable(PacketReader reader) { }
		public static void IgnoreSyncStaticReliable(PacketReader reader) { }
		public override void IgnoreSyncUnreliable(PacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(PacketReader reader) { }
	}
}
#pragma warning restore CS0649
