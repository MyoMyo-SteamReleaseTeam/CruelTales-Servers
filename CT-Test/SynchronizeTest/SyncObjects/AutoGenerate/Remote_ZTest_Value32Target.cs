/*
 * Generated File : Remote_ZTest_Value32Target
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
using CT.Common.DataType.Input;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_Value32Target
	{
		[SyncVar]
		private int _v0;
		public int V0 => _v0;
		private Action<int>? _onV0Changed;
		public event Action<int> OnV0Changed
		{
			add => _onV0Changed += value;
			remove => _onV0Changed -= value;
		}
		[SyncVar]
		private int _v1;
		public int V1 => _v1;
		private Action<int>? _onV1Changed;
		public event Action<int> OnV1Changed
		{
			add => _onV1Changed += value;
			remove => _onV1Changed -= value;
		}
		[SyncVar]
		private int _v2;
		public int V2 => _v2;
		private Action<int>? _onV2Changed;
		public event Action<int> OnV2Changed
		{
			add => _onV2Changed += value;
			remove => _onV2Changed -= value;
		}
		[SyncVar]
		private int _v3;
		public int V3 => _v3;
		private Action<int>? _onV3Changed;
		public event Action<int> OnV3Changed
		{
			add => _onV3Changed += value;
			remove => _onV3Changed -= value;
		}
		[SyncVar]
		private int _v4;
		public int V4 => _v4;
		private Action<int>? _onV4Changed;
		public event Action<int> OnV4Changed
		{
			add => _onV4Changed += value;
			remove => _onV4Changed -= value;
		}
		[SyncVar]
		private int _v5;
		public int V5 => _v5;
		private Action<int>? _onV5Changed;
		public event Action<int> OnV5Changed
		{
			add => _onV5Changed += value;
			remove => _onV5Changed -= value;
		}
		[SyncVar]
		private int _v6;
		public int V6 => _v6;
		private Action<int>? _onV6Changed;
		public event Action<int> OnV6Changed
		{
			add => _onV6Changed += value;
			remove => _onV6Changed -= value;
		}
		[SyncObject]
		private readonly SyncList<UserId> _v7 = new();
		public SyncList<UserId> V7 => _v7;
		private Action<SyncList<UserId>>? _onV7Changed;
		public event Action<SyncList<UserId>> OnV7Changed
		{
			add => _onV7Changed += value;
			remove => _onV7Changed -= value;
		}
		[SyncVar]
		private int _v8;
		private Action<int>? _onV8Changed;
		public event Action<int> OnV8Changed
		{
			add => _onV8Changed += value;
			remove => _onV8Changed -= value;
		}
		[SyncVar]
		private int _v9;
		private Action<int>? _onV9Changed;
		public event Action<int> OnV9Changed
		{
			add => _onV9Changed += value;
			remove => _onV9Changed -= value;
		}
		[SyncVar]
		private int _v10;
		private Action<int>? _onV10Changed;
		public event Action<int> OnV10Changed
		{
			add => _onV10Changed += value;
			remove => _onV10Changed -= value;
		}
		[SyncVar]
		private int _v11;
		private Action<int>? _onV11Changed;
		public event Action<int> OnV11Changed
		{
			add => _onV11Changed += value;
			remove => _onV11Changed -= value;
		}
		[SyncVar]
		private int _v12;
		private Action<int>? _onV12Changed;
		public event Action<int> OnV12Changed
		{
			add => _onV12Changed += value;
			remove => _onV12Changed -= value;
		}
		[SyncVar]
		private int _v13;
		private Action<int>? _onV13Changed;
		public event Action<int> OnV13Changed
		{
			add => _onV13Changed += value;
			remove => _onV13Changed -= value;
		}
		[SyncVar]
		private int _v14;
		public int V14 => _v14;
		private Action<int>? _onV14Changed;
		public event Action<int> OnV14Changed
		{
			add => _onV14Changed += value;
			remove => _onV14Changed -= value;
		}
		[SyncVar]
		private int _v16;
		public int V16 => _v16;
		private Action<int>? _onV16Changed;
		public event Action<int> OnV16Changed
		{
			add => _onV16Changed += value;
			remove => _onV16Changed -= value;
		}
		[SyncVar]
		private int _v17;
		public int V17 => _v17;
		private Action<int>? _onV17Changed;
		public event Action<int> OnV17Changed
		{
			add => _onV17Changed += value;
			remove => _onV17Changed -= value;
		}
		[SyncVar]
		private int _v18;
		public int V18 => _v18;
		private Action<int>? _onV18Changed;
		public event Action<int> OnV18Changed
		{
			add => _onV18Changed += value;
			remove => _onV18Changed -= value;
		}
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObjectTarget _v19 = new();
		public ZTest_InnerObjectTarget V19 => _v19;
		private Action<ZTest_InnerObjectTarget>? _onV19Changed;
		public event Action<ZTest_InnerObjectTarget> OnV19Changed
		{
			add => _onV19Changed += value;
			remove => _onV19Changed -= value;
		}
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObject _v20 = new();
		public ZTest_InnerObject V20 => _v20;
		private Action<ZTest_InnerObject>? _onV20Changed;
		public event Action<ZTest_InnerObject> OnV20Changed
		{
			add => _onV20Changed += value;
			remove => _onV20Changed -= value;
		}
		[SyncVar]
		private int _v21;
		public int V21 => _v21;
		private Action<int>? _onV21Changed;
		public event Action<int> OnV21Changed
		{
			add => _onV21Changed += value;
			remove => _onV21Changed -= value;
		}
		[SyncObject]
		private readonly SyncList<UserId> _v23 = new();
		public SyncList<UserId> V23 => _v23;
		private Action<SyncList<UserId>>? _onV23Changed;
		public event Action<SyncList<UserId>> OnV23Changed
		{
			add => _onV23Changed += value;
			remove => _onV23Changed -= value;
		}
		[SyncVar]
		private int _v25;
		public int V25 => _v25;
		private Action<int>? _onV25Changed;
		public event Action<int> OnV25Changed
		{
			add => _onV25Changed += value;
			remove => _onV25Changed -= value;
		}
		[SyncVar]
		private int _v26;
		public int V26 => _v26;
		private Action<int>? _onV26Changed;
		public event Action<int> OnV26Changed
		{
			add => _onV26Changed += value;
			remove => _onV26Changed -= value;
		}
		[SyncVar]
		private int _v27;
		public int V27 => _v27;
		private Action<int>? _onV27Changed;
		public event Action<int> OnV27Changed
		{
			add => _onV27Changed += value;
			remove => _onV27Changed -= value;
		}
		[SyncVar]
		private int _v29;
		private Action<int>? _onV29Changed;
		public event Action<int> OnV29Changed
		{
			add => _onV29Changed += value;
			remove => _onV29Changed -= value;
		}
		[SyncVar]
		private int _v30;
		private Action<int>? _onV30Changed;
		public event Action<int> OnV30Changed
		{
			add => _onV30Changed += value;
			remove => _onV30Changed -= value;
		}
		[SyncVar]
		private int _v31;
		private Action<int>? _onV31Changed;
		public event Action<int> OnV31Changed
		{
			add => _onV31Changed += value;
			remove => _onV31Changed -= value;
		}
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void ft15();
		[SyncRpc]
		public partial void f22();
		[SyncRpc]
		public partial void f24(int a);
		[SyncRpc]
		public partial void f28(int a);
		public override bool IsDirtyReliable => false;
		public override bool IsDirtyUnreliable => false;
		public override void ClearDirtyReliable() { }
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer) { }
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
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
					_onV0Changed?.Invoke(_v0);
				}
				if (dirtyReliable_0[1])
				{
					if (!reader.TryReadInt32(out _v1)) return false;
					_onV1Changed?.Invoke(_v1);
				}
				if (dirtyReliable_0[2])
				{
					if (!reader.TryReadInt32(out _v2)) return false;
					_onV2Changed?.Invoke(_v2);
				}
				if (dirtyReliable_0[3])
				{
					if (!reader.TryReadInt32(out _v3)) return false;
					_onV3Changed?.Invoke(_v3);
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadInt32(out _v4)) return false;
					_onV4Changed?.Invoke(_v4);
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadInt32(out _v5)) return false;
					_onV5Changed?.Invoke(_v5);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadInt32(out _v6)) return false;
					_onV6Changed?.Invoke(_v6);
				}
				if (dirtyReliable_0[7])
				{
					if (!_v7.TryDeserializeSyncReliable(reader)) return false;
					_onV7Changed?.Invoke(_v7);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadInt32(out _v8)) return false;
					_onV8Changed?.Invoke(_v8);
				}
				if (dirtyReliable_1[1])
				{
					if (!reader.TryReadInt32(out _v9)) return false;
					_onV9Changed?.Invoke(_v9);
				}
				if (dirtyReliable_1[2])
				{
					if (!reader.TryReadInt32(out _v10)) return false;
					_onV10Changed?.Invoke(_v10);
				}
				if (dirtyReliable_1[3])
				{
					if (!reader.TryReadInt32(out _v11)) return false;
					_onV11Changed?.Invoke(_v11);
				}
				if (dirtyReliable_1[4])
				{
					if (!reader.TryReadInt32(out _v12)) return false;
					_onV12Changed?.Invoke(_v12);
				}
				if (dirtyReliable_1[5])
				{
					if (!reader.TryReadInt32(out _v13)) return false;
					_onV13Changed?.Invoke(_v13);
				}
				if (dirtyReliable_1[6])
				{
					if (!reader.TryReadInt32(out _v14)) return false;
					_onV14Changed?.Invoke(_v14);
				}
				if (dirtyReliable_1[7])
				{
					if (!reader.TryReadInt32(out _v16)) return false;
					_onV16Changed?.Invoke(_v16);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					if (!reader.TryReadInt32(out _v17)) return false;
					_onV17Changed?.Invoke(_v17);
				}
				if (dirtyReliable_2[1])
				{
					if (!reader.TryReadInt32(out _v18)) return false;
					_onV18Changed?.Invoke(_v18);
				}
				if (dirtyReliable_2[2])
				{
					if (!_v19.TryDeserializeSyncReliable(reader)) return false;
					_onV19Changed?.Invoke(_v19);
				}
				if (dirtyReliable_2[3])
				{
					if (!_v20.TryDeserializeSyncReliable(reader)) return false;
					_onV20Changed?.Invoke(_v20);
				}
				if (dirtyReliable_2[4])
				{
					if (!reader.TryReadInt32(out _v21)) return false;
					_onV21Changed?.Invoke(_v21);
				}
				if (dirtyReliable_2[5])
				{
					if (!_v23.TryDeserializeSyncReliable(reader)) return false;
					_onV23Changed?.Invoke(_v23);
				}
				if (dirtyReliable_2[6])
				{
					if (!reader.TryReadInt32(out _v25)) return false;
					_onV25Changed?.Invoke(_v25);
				}
				if (dirtyReliable_2[7])
				{
					if (!reader.TryReadInt32(out _v26)) return false;
					_onV26Changed?.Invoke(_v26);
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyReliable_3 = reader.ReadBitmaskByte();
				if (dirtyReliable_3[0])
				{
					if (!reader.TryReadInt32(out _v27)) return false;
					_onV27Changed?.Invoke(_v27);
				}
				if (dirtyReliable_3[1])
				{
					if (!reader.TryReadInt32(out _v29)) return false;
					_onV29Changed?.Invoke(_v29);
				}
				if (dirtyReliable_3[2])
				{
					if (!reader.TryReadInt32(out _v30)) return false;
					_onV30Changed?.Invoke(_v30);
				}
				if (dirtyReliable_3[3])
				{
					if (!reader.TryReadInt32(out _v31)) return false;
					_onV31Changed?.Invoke(_v31);
				}
				if (dirtyReliable_3[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						ft15();
					}
				}
				if (dirtyReliable_3[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						f22();
					}
				}
				if (dirtyReliable_3[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						f24(a);
					}
				}
				if (dirtyReliable_3[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						f28(a);
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
				if (!_v19.TryDeserializeSyncUnreliable(reader)) return false;
				_onV19Changed?.Invoke(_v19);
			}
			if (dirtyUnreliable_0[1])
			{
				if (!_v20.TryDeserializeSyncUnreliable(reader)) return false;
				_onV20Changed?.Invoke(_v20);
			}
			return true;
		}
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out _v0)) return false;
			_onV0Changed?.Invoke(_v0);
			if (!reader.TryReadInt32(out _v1)) return false;
			_onV1Changed?.Invoke(_v1);
			if (!reader.TryReadInt32(out _v2)) return false;
			_onV2Changed?.Invoke(_v2);
			if (!reader.TryReadInt32(out _v3)) return false;
			_onV3Changed?.Invoke(_v3);
			if (!reader.TryReadInt32(out _v4)) return false;
			_onV4Changed?.Invoke(_v4);
			if (!reader.TryReadInt32(out _v5)) return false;
			_onV5Changed?.Invoke(_v5);
			if (!reader.TryReadInt32(out _v6)) return false;
			_onV6Changed?.Invoke(_v6);
			if (!_v7.TryDeserializeEveryProperty(reader)) return false;
			_onV7Changed?.Invoke(_v7);
			if (!reader.TryReadInt32(out _v8)) return false;
			_onV8Changed?.Invoke(_v8);
			if (!reader.TryReadInt32(out _v9)) return false;
			_onV9Changed?.Invoke(_v9);
			if (!reader.TryReadInt32(out _v10)) return false;
			_onV10Changed?.Invoke(_v10);
			if (!reader.TryReadInt32(out _v11)) return false;
			_onV11Changed?.Invoke(_v11);
			if (!reader.TryReadInt32(out _v12)) return false;
			_onV12Changed?.Invoke(_v12);
			if (!reader.TryReadInt32(out _v13)) return false;
			_onV13Changed?.Invoke(_v13);
			if (!reader.TryReadInt32(out _v14)) return false;
			_onV14Changed?.Invoke(_v14);
			if (!reader.TryReadInt32(out _v16)) return false;
			_onV16Changed?.Invoke(_v16);
			if (!reader.TryReadInt32(out _v17)) return false;
			_onV17Changed?.Invoke(_v17);
			if (!reader.TryReadInt32(out _v18)) return false;
			_onV18Changed?.Invoke(_v18);
			if (!_v19.TryDeserializeEveryProperty(reader)) return false;
			_onV19Changed?.Invoke(_v19);
			if (!_v20.TryDeserializeEveryProperty(reader)) return false;
			_onV20Changed?.Invoke(_v20);
			if (!reader.TryReadInt32(out _v21)) return false;
			_onV21Changed?.Invoke(_v21);
			if (!_v23.TryDeserializeEveryProperty(reader)) return false;
			_onV23Changed?.Invoke(_v23);
			if (!reader.TryReadInt32(out _v25)) return false;
			_onV25Changed?.Invoke(_v25);
			if (!reader.TryReadInt32(out _v26)) return false;
			_onV26Changed?.Invoke(_v26);
			if (!reader.TryReadInt32(out _v27)) return false;
			_onV27Changed?.Invoke(_v27);
			if (!reader.TryReadInt32(out _v29)) return false;
			_onV29Changed?.Invoke(_v29);
			if (!reader.TryReadInt32(out _v30)) return false;
			_onV30Changed?.Invoke(_v30);
			if (!reader.TryReadInt32(out _v31)) return false;
			_onV31Changed?.Invoke(_v31);
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
			_v14 = 0;
			_v16 = 0;
			_v17 = 0;
			_v18 = 0;
			_v19.InitializeRemoteProperties();
			_v20.InitializeRemoteProperties();
			_v21 = 0;
			_v23.InitializeRemoteProperties();
			_v25 = 0;
			_v26 = 0;
			_v27 = 0;
			_v29 = 0;
			_v30 = 0;
			_v31 = 0;
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
					reader.Ignore(4);
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
					_v19.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_2[3])
				{
					_v20.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_2[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[5])
				{
					_v23.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_2[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyReliable_3 = reader.ReadBitmaskByte();
				if (dirtyReliable_3[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[4])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_3[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_3[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_3[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
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
					reader.Ignore(4);
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
					ZTest_InnerObjectTarget.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_2[3])
				{
					ZTest_InnerObject.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_2[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[5])
				{
					SyncList<UserId>.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_2[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyReliable_3 = reader.ReadBitmaskByte();
				if (dirtyReliable_3[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[1])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_3[4])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_3[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_3[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_3[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				_v19.IgnoreSyncUnreliable(reader);
			}
			if (dirtyUnreliable_0[1])
			{
				_v20.IgnoreSyncUnreliable(reader);
			}
		}
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				ZTest_InnerObjectTarget.IgnoreSyncStaticUnreliable(reader);
			}
			if (dirtyUnreliable_0[1])
			{
				ZTest_InnerObject.IgnoreSyncStaticUnreliable(reader);
			}
		}
	}
}
#pragma warning restore CS0649
