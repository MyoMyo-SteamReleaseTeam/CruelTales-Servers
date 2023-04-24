﻿/*
 * Generated File : Master_TestNetworkObject.cs
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

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
#region FromMaster
		[SyncVar]
		private UserToken _userToken = new();
		[SyncRpc]
		public partial void Server_DoSomethiing();
		[SyncVar(SyncType.Unreliable)]
		private float _floatValue;
		[SyncRpc(SyncType.Unreliable)]
		public partial void Server_SendMessage(NetString message);
#endregion
#region FromRemote
		[SyncVar(dir: SyncDirection.FromRemote)]
		private NetTransform _remote_netTransform = new();
		public event Action<NetTransform>? OnRemote_netTransformChanged;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_DoSomethiing() { }
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_SendMessage(NetString message) { }
		[SyncVar(dir: SyncDirection.FromRemote, sync: SyncType.Unreliable)]
		private int _remote_Value;
		public event Action<int>? OnRemote_ValueChanged;
#endregion
		private BitmaskByte _propertyDirty_0 = new();
		private BitmaskByte _rpcDirty_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _propertyDirty_0.AnyTrue();
				isDirty |= _rpcDirty_0.AnyTrue();
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
				_propertyDirty_0[0] = true;
			}
		}
		public partial void Server_DoSomethiing()
		{
			Server_DoSomethiingCallstackCount++;
			_rpcDirty_0[0] = true;
		}
		private byte Server_DoSomethiingCallstackCount = 0;
		private BitmaskByte _unreliablePropertyDirty_0 = new();
		private BitmaskByte _unreliableRpcDirty_0 = new();
		public override bool IsDirtyUnreliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _unreliablePropertyDirty_0.AnyTrue();
				isDirty |= _unreliableRpcDirty_0.AnyTrue();
				return isDirty;
			}
		}
		private float FloatValue
		{
			get => _floatValue;
			set
			{
				if (_floatValue == value) return;
				_floatValue = value;
				_unreliablePropertyDirty_0[0] = true;
			}
		}
		public partial void Server_SendMessage(NetString message)
		{
			Server_SendMessageCallstack.Enqueue(message);
			_unreliableRpcDirty_0[0] = true;
		}
		private Queue<NetString> Server_SendMessageCallstack = new();
#region FromMaster
		public override void SerializeSyncReliable(PacketWriter writer)
		{
			BitmaskByte objectDirty = new BitmaskByte();
			objectDirty[0] = _propertyDirty_0.AnyTrue();
			objectDirty[4] = _rpcDirty_0.AnyTrue();
			objectDirty.Serialize(writer);
			if (objectDirty[0])
			{
				_propertyDirty_0.Serialize(writer);
				if (_propertyDirty_0[0]) _userToken.Serialize(writer);
			}
			if (objectDirty[4])
			{
				_rpcDirty_0.Serialize(writer);
				if (_rpcDirty_0[0])
				{
					writer.Put(Server_DoSomethiingCallstackCount);
					Server_DoSomethiingCallstackCount = 0;
				}
			}
		}
		public override void SerializeSyncUnreliable(PacketWriter writer)
		{
			BitmaskByte objectDirty = new BitmaskByte();
			objectDirty[0] = _unreliablePropertyDirty_0.AnyTrue();
			objectDirty[4] = _unreliableRpcDirty_0.AnyTrue();
			objectDirty.Serialize(writer);
			if (objectDirty[0])
			{
				_unreliablePropertyDirty_0.Serialize(writer);
				if (_unreliablePropertyDirty_0[0]) writer.Put(_floatValue);
			}
			if (objectDirty[4])
			{
				_unreliableRpcDirty_0.Serialize(writer);
				if (_unreliableRpcDirty_0[0])
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
		}
		public override void SerializeEveryProperty(PacketWriter writer)
		{
			_userToken.Serialize(writer);
			writer.Put(_floatValue);
		}
#endregion
#region FromRemote
		public override void DeserializeSyncReliable(PacketReader reader)
		{
			BitmaskByte objectDirty = reader.ReadBitmaskByte();
			if (objectDirty[0])
			{
				BitmaskByte _propertyDirty_0 = reader.ReadBitmaskByte();
				if (_propertyDirty_0[0])
				{
					_remote_netTransform.Deserialize(reader);
					OnRemote_netTransformChanged?.Invoke(_remote_netTransform);
				}
			}
			if (objectDirty[4])
			{
				BitmaskByte _rpcDirty_0 = reader.ReadBitmaskByte();
				if (_rpcDirty_0[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Client_DoSomethiing();
					}
				}
				if (_rpcDirty_0[1])
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
		}
		public override void DeserializeSyncUnreliable(PacketReader reader) { }
		public override void DeserializeEveryProperty(PacketReader reader)
		{
			_remote_netTransform.Deserialize(reader);
			_remote_Value = reader.ReadInt32();
		}
#endregion
		public override void ClearDirtyReliable()
		{
			_propertyDirty_0.Clear();
			_rpcDirty_0.Clear();
		}
		public override void ClearDirtyUnreliable()
		{
			_unreliablePropertyDirty_0.Clear();
			_unreliableRpcDirty_0.Clear();
		}
	}
}