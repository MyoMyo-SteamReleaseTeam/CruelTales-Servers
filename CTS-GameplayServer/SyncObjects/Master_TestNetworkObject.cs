﻿/*
 * Generated File : Master_TestNetworkObject
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
using CT.Common.Serialization.Type;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class TestNetworkObject : MasterNetworkObject
	{
		public override NetworkObjectType Type => NetworkObjectType.TestNetworkObject;
		[SyncVar]
		private UserToken _userToken = new();
		[SyncVar(SyncType.Unreliable)]
		private float _floatValue;
		[SyncRpc]
		public partial void Server_DoSomethiing();
		[SyncRpc(SyncType.Unreliable)]
		private partial void Server_SendMessage(NetString message);
		[SyncVar(dir: SyncDirection.FromRemote)]
		private NetTransform _remote_netTransform = new();
		public event Action<NetTransform>? OnRemote_netTransformChanged;
		[SyncVar(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		private int _remote_Value;
		public event Action<int>? OnRemote_ValueChanged;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_DoSomethiing();
		public partial void Client_DoSomethiing()
		{

		}
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_SendMessage(NetString message);
		public partial void Client_SendMessage(NetString message)
		{

		}
		private BitmaskByte _dirtyReliable_0 = new();
		private BitmaskByte _dirtyUnreliable_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _dirtyReliable_0.AnyTrue();
				return isDirty;
			}
		}
		public override bool IsDirtyUnreliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _dirtyUnreliable_0.AnyTrue();
				return isDirty;
			}
		}
		private UserToken UserToken
		{
			get => _userToken;
			set
			{
				if (_userToken == value) return;
				_userToken = value;
				_dirtyReliable_0[0] = true;
			}
		}
		public partial void Server_DoSomethiing()
		{
			Server_DoSomethiingCallstackCount++;
			_dirtyReliable_0[1] = true;
		}
		private byte Server_DoSomethiingCallstackCount = 0;
		private float FloatValue
		{
			get => _floatValue;
			set
			{
				if (_floatValue == value) return;
				_floatValue = value;
				_dirtyUnreliable_0[0] = true;
			}
		}
		private partial void Server_SendMessage(NetString message)
		{
			Server_SendMessageCallstack.Enqueue(message);
			_dirtyUnreliable_0[1] = true;
		}
		private Queue<NetString> Server_SendMessageCallstack = new();
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
		}
		public override void ClearDirtyUnreliable()
		{
			_dirtyUnreliable_0.Clear();
		}
		public override void SerializeSyncReliable(PacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				_userToken.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put((byte)Server_DoSomethiingCallstackCount);
			}
		}
		public override void SerializeSyncUnreliable(PacketWriter writer)
		{
			_dirtyUnreliable_0.Serialize(writer);
			if (_dirtyUnreliable_0[0])
			{
				writer.Put(_floatValue);
			}
			if (_dirtyUnreliable_0[1])
			{
				byte count = (byte)Server_SendMessageCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_SendMessageCallstack.Dequeue();
					arg.Serialize(writer);
				}
			}
		}
		public override void SerializeEveryProperty(PacketWriter writer)
		{
			_userToken.Serialize(writer);
			writer.Put(_floatValue);
		}
		public override void DeserializeSyncReliable(PacketReader reader)
		{
			BitmaskByte _dirtyReliable_0 = reader.ReadBitmaskByte();
			if (_dirtyReliable_0[0])
			{
				_remote_netTransform.Deserialize(reader);
				OnRemote_netTransformChanged?.Invoke(_remote_netTransform);
			}
			if (_dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_DoSomethiing();
				}
			}
			if (_dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetString message = new();
					message.Deserialize(reader);
					Client_SendMessage(message);
				}
			}
		}
		public override void DeserializeSyncUnreliable(PacketReader reader)
		{
			BitmaskByte _dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (_dirtyUnreliable_0[0])
			{
				_remote_Value = reader.ReadInt32();
				OnRemote_ValueChanged?.Invoke(_remote_Value);
			}
		}
		public override void DeserializeEveryProperty(PacketReader reader)
		{
			_remote_netTransform.Deserialize(reader);
			OnRemote_netTransformChanged?.Invoke(_remote_netTransform);
			_remote_Value = reader.ReadInt32();
			OnRemote_ValueChanged?.Invoke(_remote_Value);
		}
		public static void IgnoreSyncReliable(PacketReader reader)
		{
			BitmaskByte _dirtyReliable_0 = reader.ReadBitmaskByte();
			if (_dirtyReliable_0[0])
			{
				NetTransform.Ignore(reader);
			}
			if (_dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
			if (_dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetString.Ignore(reader);
				}
			}
		}
		public static void IgnoreSyncUnreliable(PacketReader reader)
		{
			BitmaskByte _dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (_dirtyUnreliable_0[0])
			{
				reader.Ignore(4);
			}
		}
	}
}
#pragma warning restore CS0649
