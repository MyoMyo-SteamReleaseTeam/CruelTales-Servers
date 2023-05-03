/*
 * Generated File : Remote_TestNetworkObject
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
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class TestNetworkObject : RemoteNetworkObject
	{
		public override NetworkObjectType Type => NetworkObjectType.TestNetworkObject;
		[SyncVar(dir: SyncDirection.FromRemote)]
		private NetTransform _remote_netTransform = new();
		[SyncVar(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		private int _remote_Value;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_DoSomethiing();
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_SendMessage(NetString message);
		[SyncVar]
		private UserToken _userToken = new();
		public event Action<UserToken>? OnUserTokenChanged;
		[SyncVar(SyncType.Unreliable)]
		private float _floatValue;
		public event Action<float>? OnFloatValueChanged;
		[SyncRpc]
		public partial void Server_DoSomethiing();
		[SyncRpc(SyncType.Unreliable)]
		private partial void Server_SendMessage(NetString message);
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
		private NetTransform Remote_netTransform
		{
			get => _remote_netTransform;
			set
			{
				if (_remote_netTransform == value) return;
				_remote_netTransform = value;
				_dirtyReliable_0[0] = true;
			}
		}
		public partial void Client_DoSomethiing()
		{
			Client_DoSomethiingCallstackCount++;
			_dirtyReliable_0[1] = true;
		}
		private byte Client_DoSomethiingCallstackCount = 0;
		public partial void Client_SendMessage(NetString message)
		{
			Client_SendMessageCallstack.Enqueue(message);
			_dirtyReliable_0[2] = true;
		}
		private Queue<NetString> Client_SendMessageCallstack = new();
		public int Remote_Value
		{
			get => _remote_Value;
			set
			{
				if (_remote_Value == value) return;
				_remote_Value = value;
				_dirtyUnreliable_0[0] = true;
			}
		}
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
				_remote_netTransform.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put((byte)Client_DoSomethiingCallstackCount);
			}
			if (_dirtyReliable_0[2])
			{
				byte count = (byte)Client_SendMessageCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_SendMessageCallstack.Dequeue();
					arg.Serialize(writer);
				}
			}
		}
		public override void SerializeSyncUnreliable(PacketWriter writer)
		{
			_dirtyUnreliable_0.Serialize(writer);
			if (_dirtyUnreliable_0[0])
			{
				writer.Put(_remote_Value);
			}
		}
		public override void SerializeEveryProperty(PacketWriter writer)
		{
			_remote_netTransform.Serialize(writer);
			writer.Put(_remote_Value);
		}
		public override void DeserializeSyncReliable(PacketReader reader)
		{
			BitmaskByte _dirtyReliable_0 = reader.ReadBitmaskByte();
			if (_dirtyReliable_0[0])
			{
				_userToken.Deserialize(reader);
				OnUserTokenChanged?.Invoke(_userToken);
			}
			if (_dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Server_DoSomethiing();
				}
			}
		}
		public override void DeserializeSyncUnreliable(PacketReader reader)
		{
			BitmaskByte _dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (_dirtyUnreliable_0[0])
			{
				_floatValue = reader.ReadSingle();
				OnFloatValueChanged?.Invoke(_floatValue);
			}
			if (_dirtyUnreliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetString message = new();
					message.Deserialize(reader);
					Server_SendMessage(message);
				}
			}
		}
		public override void IgnoreSyncReliable(PacketReader reader)
		{
			BitmaskByte _dirtyReliable_0 = reader.ReadBitmaskByte();
			if (_dirtyReliable_0[0])
			{
				UserToken.Ignore(reader);
			}
			if (_dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
		}
		public override void IgnoreSyncUnreliable(PacketReader reader)
		{
			BitmaskByte _dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (_dirtyUnreliable_0[0])
			{
				reader.Ignore(4);
			}
			if (_dirtyUnreliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetString.Ignore(reader);
				}
			}
		}
		public override void DeserializeEveryProperty(PacketReader reader)
		{
			_userToken.Deserialize(reader);
			OnUserTokenChanged?.Invoke(_userToken);
			_floatValue = reader.ReadSingle();
			OnFloatValueChanged?.Invoke(_floatValue);
		}
	}
}
#pragma warning restore CS0649
