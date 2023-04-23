﻿//using System;
//using System.Collections.Generic;
//using CT.Common.DataType;
//using CT.Common.Serialization;
//using CT.Common.Serialization.Type;
//using CT.Common.Synchronizations;
//using CT.Tools.Collections;
//using CTS.Instance.SyncDefinitions;
//using CTS.Instance.Synchronizations;


//namespace CTS.Instance.SyncObjects
//{

//	[Serializable]
//	public partial class TestNetworkObject : MasterNetworkObject
//	{
//		public override NetworkObjectType Type => NetworkObjectType.TestNetworkObject;

//		[SyncVar]
//		private UserToken _userToken = new();

//		[SyncVar]
//		private float _floatValue;

//		[SyncObject]
//		private TestSyncObject _testSyncObject = new();

//		[SyncRpc]
//		public partial void Server_DoSomethiing();

//		[SyncRpc]
//		public partial void Server_Response(NetString message);


//		[SyncObject(SyncType.Unreliable)]
//		private TestSyncObject _testUnreliableObject = new();

//		[SyncRpc(SyncType.Unreliable)]
//		public partial void Server_SendMessage(NetString message);


//		#region Synchronization
//		private BitmaskByte _propertyDirty_0 = new();
//		private BitmaskByte _rpcDirty_0 = new();

//		public override bool IsDirtyReliable
//		{
//			get
//			{
//				bool isDirty = false;
//				isDirty |= _propertyDirty_0.AnyTrue();
//				isDirty |= _rpcDirty_0.AnyTrue();

//				return isDirty;
//			}
//		}

//		public UserToken UserToken
//		{
//			get => _userToken;
//			set
//			{
//				if (_userToken == value) return;
//				_userToken = value;
//				_propertyDirty_0[0] = true;
//			}
//		}

//		public float FloatValue
//		{
//			get => _floatValue;
//			set
//			{
//				if (_floatValue == value) return;
//				_floatValue = value;
//				_propertyDirty_0[1] = true;
//			}
//		}

//		public partial void Server_DoSomethiing()
//		{
//			Server_DoSomethiingCallstackCount++;
//			_rpcDirty_0[0] = true;
//		}
//		private byte Server_DoSomethiingCallstackCount = 0;

//		public partial void Server_Response(NetString message)
//		{
//			Server_ResponseCallstack.Enqueue(message);
//			_rpcDirty_0[1] = true;
//		}
//		private Queue<NetString> Server_ResponseCallstack = new();


//		private BitmaskByte _unreliablePropertyDirty_0 = new();
//		private BitmaskByte _unreliableRpcDirty_0 = new();

//		public override bool IsDirtyUnreliable
//		{
//			get
//			{
//				bool isDirty = false;
//				isDirty |= _unreliablePropertyDirty_0.IsDirtyUnreliable;
//				isDirty |= _unreliableRpcDirty_0.AnyTrue();

//				return isDirty;
//			}
//		}

//		public partial void Server_SendMessage(NetString message)
//		{
//			Server_SendMessageCallstack.Enqueue(message);
//			_unreliableRpcDirty_0[0] = true;
//		}
//		private Queue<NetString> Server_SendMessageCallstack = new();


//		public override void SerializeSyncReliable(PacketWriter writer)
//		{
//			BitmaskByte objectDirty = new BitmaskByte();

//			_propertyDirty_0[2] = _testSyncObject.IsDirtyReliable;

//			objectDirty[0] = _propertyDirty_0.AnyTrue();
//			objectDirty[4] = _rpcDirty_0.AnyTrue();


//			objectDirty.Serialize(writer);


//			if (objectDirty[0])
//			{
//				_propertyDirty_0.Serialize(writer);

//				if (_propertyDirty_0[0]) _userToken.Serialize(writer);
//				if (_propertyDirty_0[1]) writer.Put(_floatValue);
//				if (_propertyDirty_0[2]) _testSyncObject.SerializeSyncReliable(writer);

//			}

//			if (objectDirty[4])
//			{
//				_rpcDirty_0.Serialize(writer);

//				if (_rpcDirty_0[0])
//				{
//					writer.Put(Server_DoSomethiingCallstackCount);
//					Server_DoSomethiingCallstackCount = 0;
//				}
//				if (_rpcDirty_0[1])
//				{
//					byte count = (byte)Server_ResponseCallstack.Count;
//					writer.Put(count);
//					for (int i = 0; i < count; i++)
//					{
//						var arg = Server_ResponseCallstack.Dequeue();
//						arg.Serialize(writer);

//					}
//				}

//			}

//		}

//		public override void SerializeSyncUnreliable(PacketWriter writer)
//		{
//			BitmaskByte objectDirty = new BitmaskByte();

//			_unreliablePropertyDirty_0[0] = _testUnreliableObject.IsDirtyUnreliable;

//			objectDirty[0] = _unreliablePropertyDirty_0.AnyTrue();
//			objectDirty[4] = _unreliableRpcDirty_0.AnyTrue();


//			objectDirty.Serialize(writer);


//			if (objectDirty[0])
//			{
//				_unreliablePropertyDirty_0.Serialize(writer);

//				if (_unreliablePropertyDirty_0[0]) _testUnreliableObject.SerializeSyncUnreliable(writer);

//			}

//			if (objectDirty[4])
//			{
//				_unreliableRpcDirty_0.Serialize(writer);

//				if (_unreliableRpcDirty_0[0])
//				{
//					byte count = (byte)Server_SendMessageCallstack.Count;
//					writer.Put(count);
//					for (int i = 0; i < count; i++)
//					{
//						var arg = Server_SendMessageCallstack.Dequeue();
//						arg.Serialize(writer);

//					}
//				}

//			}

//		}

//		public override void ClearDirtyReliable()
//		{
//			_propertyDirty_0.Clear();
//			_rpcDirty_0.Clear();

//		}

//		public override void ClearDirtyUnreliable()
//		{
//			_unreliablePropertyDirty_0.Clear();
//			_unreliableRpcDirty_0.Clear();

//		}

//		public override void SerializeEveryProperty(PacketWriter writer)
//		{
//			_userToken.Serialize(writer);
//			writer.Put(_floatValue);
//			_testSyncObject.(writer);
//			_testUnreliableObject.(writer);

//		}


//		#endregion
//	}

//}