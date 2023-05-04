/*
 * Generated File : Master_TestTypeSync
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
	public partial class TestTypeSync : MasterNetworkObject
	{
		public override NetworkObjectType Type => NetworkObjectType.TestTypeSync;
		[SyncVar]
		private UserToken _valueTypeUserToken = new();
		[SyncVar]
		private float _primitiveType;
		[SyncVar]
		private NetEntityType _enumType = new();
		[SyncVar]
		private NetTransform _valueTypeTransform = new();
		[SyncVar]
		private NetString _stringValue = new();
		[SyncObject(SyncType.RelibaleOrUnreliable)]
		private TestInnerObject _syncObjectBothSide = new();
		[SyncObject]
		private TestInnerObject _syncObjectReliable = new();
		[SyncRpc]
		public partial void Server_Reliable();
		[SyncRpc(SyncType.Unreliable)]
		public partial void Server_Unreliable(NetString message);
		private BitmaskByte _dirtyReliable_0 = new();
		private BitmaskByte _dirtyUnreliable_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _syncObjectBothSide.IsDirtyReliable;
				isDirty |= _syncObjectReliable.IsDirtyReliable;
				isDirty |= _dirtyReliable_0.AnyTrue();
				return isDirty;
			}
		}
		public override bool IsDirtyUnreliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _syncObjectBothSide.IsDirtyUnreliable;
				isDirty |= _dirtyUnreliable_0.AnyTrue();
				return isDirty;
			}
		}
		private UserToken ValueTypeUserToken
		{
			get => _valueTypeUserToken;
			set
			{
				if (_valueTypeUserToken == value) return;
				_valueTypeUserToken = value;
				_dirtyReliable_0[0] = true;
			}
		}
		private float PrimitiveType
		{
			get => _primitiveType;
			set
			{
				if (_primitiveType == value) return;
				_primitiveType = value;
				_dirtyReliable_0[1] = true;
			}
		}
		private NetEntityType EnumType
		{
			get => _enumType;
			set
			{
				if (_enumType == value) return;
				_enumType = value;
				_dirtyReliable_0[2] = true;
			}
		}
		private NetTransform ValueTypeTransform
		{
			get => _valueTypeTransform;
			set
			{
				if (_valueTypeTransform == value) return;
				_valueTypeTransform = value;
				_dirtyReliable_0[3] = true;
			}
		}
		public NetString StringValue
		{
			get => _stringValue;
			set
			{
				if (_stringValue == value) return;
				_stringValue = value;
				_dirtyReliable_0[4] = true;
			}
		}
		public partial void Server_Reliable()
		{
			Server_ReliableCallstackCount++;
			_dirtyReliable_0[7] = true;
		}
		private byte Server_ReliableCallstackCount = 0;
		public partial void Server_Unreliable(NetString message)
		{
			Server_UnreliableCallstack.Enqueue(message);
			_dirtyUnreliable_0[1] = true;
		}
		private Queue<NetString> Server_UnreliableCallstack = new();
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			_syncObjectBothSide.ClearDirtyReliable();
			_syncObjectReliable.ClearDirtyReliable();
		}
		public override void ClearDirtyUnreliable()
		{
			_dirtyUnreliable_0.Clear();
			_syncObjectBothSide.ClearDirtyUnreliable();
		}
		public override void SerializeSyncReliable(PacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				_valueTypeUserToken.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put(_primitiveType);
			}
			if (_dirtyReliable_0[2])
			{
				writer.Put((byte)_enumType);
			}
			if (_dirtyReliable_0[3])
			{
				_valueTypeTransform.Serialize(writer);
			}
			if (_dirtyReliable_0[4])
			{
				_stringValue.Serialize(writer);
			}
			if (_dirtyReliable_0[5])
			{
				_syncObjectBothSide.SerializeSyncReliable(writer);
			}
			if (_dirtyReliable_0[6])
			{
				_syncObjectReliable.SerializeSyncReliable(writer);
			}
			if (_dirtyReliable_0[7])
			{
				writer.Put((byte)Server_ReliableCallstackCount);
			}
		}
		public override void SerializeSyncUnreliable(PacketWriter writer)
		{
			_dirtyUnreliable_0.Serialize(writer);
			if (_dirtyUnreliable_0[0])
			{
				_syncObjectBothSide.SerializeSyncUnreliable(writer);
			}
			if (_dirtyUnreliable_0[1])
			{
				byte count = (byte)Server_UnreliableCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_UnreliableCallstack.Dequeue();
					arg.Serialize(writer);
				}
			}
		}
		public override void SerializeEveryProperty(PacketWriter writer)
		{
			_valueTypeUserToken.Serialize(writer);
			writer.Put(_primitiveType);
			writer.Put((byte)_enumType);
			_valueTypeTransform.Serialize(writer);
			_stringValue.Serialize(writer);
			_syncObjectBothSide.SerializeEveryProperty(writer);
			_syncObjectReliable.SerializeEveryProperty(writer);
		}
		public override void DeserializeSyncReliable(PacketReader reader) { }
		public override void DeserializeSyncUnreliable(PacketReader reader) { }
		public override void DeserializeEveryProperty(PacketReader reader) { }
		public static void IgnoreSyncReliable(PacketReader reader) { }
		public static void IgnoreSyncUnreliable(PacketReader reader) { }
	}
}
#pragma warning restore CS0649
