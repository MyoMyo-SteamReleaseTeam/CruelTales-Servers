/*
 * Generated File : Remote_ZTest_Value32NonTarget
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
#if UNITY_2021
using UnityEngine;
#endif

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_Value32NonTarget
	{
		[SyncVar]
		private int _v0;
		public int V0 => _v0;
		public event Action<int>? OnV0Changed;
		[SyncVar]
		private int _v1;
		public int V1 => _v1;
		public event Action<int>? OnV1Changed;
		[SyncVar]
		private int _v2;
		public int V2 => _v2;
		public event Action<int>? OnV2Changed;
		[SyncVar]
		private int _v3;
		public int V3 => _v3;
		public event Action<int>? OnV3Changed;
		[SyncVar]
		private int _v4;
		public int V4 => _v4;
		public event Action<int>? OnV4Changed;
		[SyncVar]
		private int _v5;
		public int V5 => _v5;
		public event Action<int>? OnV5Changed;
		[SyncVar]
		private int _v6;
		public int V6 => _v6;
		public event Action<int>? OnV6Changed;
		[SyncObject]
		private SyncList<UserId> _v7 = new();
		public event Action<SyncList<UserId>>? OnV7Changed;
		[SyncVar]
		private int _v8;
		public event Action<int>? OnV8Changed;
		[SyncVar]
		private int _v9;
		public event Action<int>? OnV9Changed;
		[SyncVar]
		private int _v10;
		public event Action<int>? OnV10Changed;
		[SyncVar]
		private int _v11;
		public event Action<int>? OnV11Changed;
		[SyncVar]
		private int _v12;
		public event Action<int>? OnV12Changed;
		[SyncVar]
		private int _v13;
		public event Action<int>? OnV13Changed;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private ZTest_InnerObject _v15 = new();
		public event Action<ZTest_InnerObject>? OnV15Changed;
		[SyncVar]
		private int _v16;
		public int V16 => _v16;
		public event Action<int>? OnV16Changed;
		[SyncVar]
		private int _v17;
		public int V17 => _v17;
		public event Action<int>? OnV17Changed;
		[SyncVar]
		private int _v18;
		public int V18 => _v18;
		public event Action<int>? OnV18Changed;
		[SyncVar]
		private int _v20;
		public int V20 => _v20;
		public event Action<int>? OnV20Changed;
		[SyncVar]
		private int _v21;
		public int V21 => _v21;
		public event Action<int>? OnV21Changed;
		[SyncObject]
		private SyncList<UserId> _v23 = new();
		public event Action<SyncList<UserId>>? OnV23Changed;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void ft15();
		[SyncRpc]
		public partial void f22();
		public override bool IsDirtyReliable => false;
		public override bool IsDirtyUnreliable => false;
		public override void ClearDirtyReliable() { }
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer) { }
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer) { }
		public override void InitializeMasterProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
				if (dirtyReliable_0[0])
				{
					if (!reader.TryReadInt32(out _v0)) return false;
					OnV0Changed?.Invoke(_v0);
				}
				if (dirtyReliable_0[1])
				{
					if (!reader.TryReadInt32(out _v1)) return false;
					OnV1Changed?.Invoke(_v1);
				}
				if (dirtyReliable_0[2])
				{
					if (!reader.TryReadInt32(out _v2)) return false;
					OnV2Changed?.Invoke(_v2);
				}
				if (dirtyReliable_0[3])
				{
					if (!reader.TryReadInt32(out _v3)) return false;
					OnV3Changed?.Invoke(_v3);
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadInt32(out _v4)) return false;
					OnV4Changed?.Invoke(_v4);
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadInt32(out _v5)) return false;
					OnV5Changed?.Invoke(_v5);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadInt32(out _v6)) return false;
					OnV6Changed?.Invoke(_v6);
				}
				if (dirtyReliable_0[7])
				{
					if (!_v7.TryDeserializeSyncReliable(reader)) return false;
					OnV7Changed?.Invoke(_v7);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadInt32(out _v8)) return false;
					OnV8Changed?.Invoke(_v8);
				}
				if (dirtyReliable_1[1])
				{
					if (!reader.TryReadInt32(out _v9)) return false;
					OnV9Changed?.Invoke(_v9);
				}
				if (dirtyReliable_1[2])
				{
					if (!reader.TryReadInt32(out _v10)) return false;
					OnV10Changed?.Invoke(_v10);
				}
				if (dirtyReliable_1[3])
				{
					if (!reader.TryReadInt32(out _v11)) return false;
					OnV11Changed?.Invoke(_v11);
				}
				if (dirtyReliable_1[4])
				{
					if (!reader.TryReadInt32(out _v12)) return false;
					OnV12Changed?.Invoke(_v12);
				}
				if (dirtyReliable_1[5])
				{
					if (!reader.TryReadInt32(out _v13)) return false;
					OnV13Changed?.Invoke(_v13);
				}
				if (dirtyReliable_1[6])
				{
					if (!_v15.TryDeserializeSyncReliable(reader)) return false;
					OnV15Changed?.Invoke(_v15);
				}
				if (dirtyReliable_1[7])
				{
					if (!reader.TryReadInt32(out _v16)) return false;
					OnV16Changed?.Invoke(_v16);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					if (!reader.TryReadInt32(out _v17)) return false;
					OnV17Changed?.Invoke(_v17);
				}
				if (dirtyReliable_2[1])
				{
					if (!reader.TryReadInt32(out _v18)) return false;
					OnV18Changed?.Invoke(_v18);
				}
				if (dirtyReliable_2[2])
				{
					if (!reader.TryReadInt32(out _v20)) return false;
					OnV20Changed?.Invoke(_v20);
				}
				if (dirtyReliable_2[3])
				{
					if (!reader.TryReadInt32(out _v21)) return false;
					OnV21Changed?.Invoke(_v21);
				}
				if (dirtyReliable_2[4])
				{
					if (!_v23.TryDeserializeSyncReliable(reader)) return false;
					OnV23Changed?.Invoke(_v23);
				}
				if (dirtyReliable_2[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						ft15();
					}
				}
				if (dirtyReliable_2[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						f22();
					}
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				if (!_v15.TryDeserializeSyncUnreliable(reader)) return false;
				OnV15Changed?.Invoke(_v15);
			}
			return true;
		}
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out _v0)) return false;
			OnV0Changed?.Invoke(_v0);
			if (!reader.TryReadInt32(out _v1)) return false;
			OnV1Changed?.Invoke(_v1);
			if (!reader.TryReadInt32(out _v2)) return false;
			OnV2Changed?.Invoke(_v2);
			if (!reader.TryReadInt32(out _v3)) return false;
			OnV3Changed?.Invoke(_v3);
			if (!reader.TryReadInt32(out _v4)) return false;
			OnV4Changed?.Invoke(_v4);
			if (!reader.TryReadInt32(out _v5)) return false;
			OnV5Changed?.Invoke(_v5);
			if (!reader.TryReadInt32(out _v6)) return false;
			OnV6Changed?.Invoke(_v6);
			if (!_v7.TryDeserializeEveryProperty(reader)) return false;
			OnV7Changed?.Invoke(_v7);
			if (!reader.TryReadInt32(out _v8)) return false;
			OnV8Changed?.Invoke(_v8);
			if (!reader.TryReadInt32(out _v9)) return false;
			OnV9Changed?.Invoke(_v9);
			if (!reader.TryReadInt32(out _v10)) return false;
			OnV10Changed?.Invoke(_v10);
			if (!reader.TryReadInt32(out _v11)) return false;
			OnV11Changed?.Invoke(_v11);
			if (!reader.TryReadInt32(out _v12)) return false;
			OnV12Changed?.Invoke(_v12);
			if (!reader.TryReadInt32(out _v13)) return false;
			OnV13Changed?.Invoke(_v13);
			if (!_v15.TryDeserializeEveryProperty(reader)) return false;
			OnV15Changed?.Invoke(_v15);
			if (!reader.TryReadInt32(out _v16)) return false;
			OnV16Changed?.Invoke(_v16);
			if (!reader.TryReadInt32(out _v17)) return false;
			OnV17Changed?.Invoke(_v17);
			if (!reader.TryReadInt32(out _v18)) return false;
			OnV18Changed?.Invoke(_v18);
			if (!reader.TryReadInt32(out _v20)) return false;
			OnV20Changed?.Invoke(_v20);
			if (!reader.TryReadInt32(out _v21)) return false;
			OnV21Changed?.Invoke(_v21);
			if (!_v23.TryDeserializeEveryProperty(reader)) return false;
			OnV23Changed?.Invoke(_v23);
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_v0 = 0;
			_v1 = 0;
			_v2 = 0;
			_v3 = 0;
			_v4 = 0;
			_v5 = 0;
			_v6 = 0;
			_v7.InitializeRemoteProperties();
			_v8 = 0;
			_v9 = 0;
			_v10 = 0;
			_v11 = 0;
			_v12 = 0;
			_v13 = 0;
			_v15.InitializeRemoteProperties();
			_v16 = 0;
			_v17 = 0;
			_v18 = 0;
			_v20 = 0;
			_v21 = 0;
			_v23.InitializeRemoteProperties();
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
				if (dirtyReliable_0[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[5])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[7])
				{
					_v7.IgnoreSyncReliable(reader);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[5])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[6])
				{
					_v15.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_1[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[4])
				{
					_v23.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_2[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_2[6])
				{
					reader.Ignore(1);
				}
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
				if (dirtyReliable_0[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[5])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[7])
				{
					SyncList<UserId>.IgnoreSyncStaticReliable(reader);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[5])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[6])
				{
					ZTest_InnerObject.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_1[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[4])
				{
					SyncList<UserId>.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_2[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_2[6])
				{
					reader.Ignore(1);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				_v15.IgnoreSyncUnreliable(reader);
			}
		}
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				ZTest_InnerObject.IgnoreSyncStaticUnreliable(reader);
			}
		}
	}
}
#pragma warning restore CS0649