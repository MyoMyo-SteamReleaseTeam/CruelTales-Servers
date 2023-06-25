/*
 * Generated File : Remote_ZTest_Value32
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
	public partial class ZTest_Value32
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
		private int _v4;
		public int V4 => _v4;
		public event Action<int>? OnV4Changed;
		[SyncVar]
		private int _v5;
		public int V5 => _v5;
		public event Action<int>? OnV5Changed;
		[SyncObject]
		private SyncList<UserId> _v6 = new();
		public event Action<SyncList<UserId>>? OnV6Changed;
		[SyncVar]
		private int _v7;
		public int V7 => _v7;
		public event Action<int>? OnV7Changed;
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
		private int _v12;
		public event Action<int>? OnV12Changed;
		[SyncVar]
		private int _v13;
		public event Action<int>? OnV13Changed;
		[SyncVar]
		private int _v15;
		public int V15 => _v15;
		public event Action<int>? OnV15Changed;
		[SyncVar]
		private int _v16;
		public int V16 => _v16;
		public event Action<int>? OnV16Changed;
		[SyncVar]
		private int _v18;
		public int V18 => _v18;
		public event Action<int>? OnV18Changed;
		[SyncObject]
		private ZTest_InnerObject _v19 = new();
		public event Action<ZTest_InnerObject>? OnV19Changed;
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
		[SyncVar]
		private int _v25;
		public int V25 => _v25;
		public event Action<int>? OnV25Changed;
		[SyncVar]
		private int _v26;
		public int V26 => _v26;
		public event Action<int>? OnV26Changed;
		[SyncVar]
		private int _v27;
		public int V27 => _v27;
		public event Action<int>? OnV27Changed;
		[SyncVar]
		private int _v29;
		public event Action<int>? OnV29Changed;
		[SyncVar]
		private int _v30;
		public event Action<int>? OnV30Changed;
		[SyncVar]
		private int _v31;
		public event Action<int>? OnV31Changed;
		[SyncVar]
		private int _v32;
		public event Action<int>? OnV32Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv0;
		public int Uv0 => _uv0;
		public event Action<int>? OnUv0Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv1;
		public int Uv1 => _uv1;
		public event Action<int>? OnUv1Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv2;
		public int Uv2 => _uv2;
		public event Action<int>? OnUv2Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv4;
		public int Uv4 => _uv4;
		public event Action<int>? OnUv4Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv5;
		public int Uv5 => _uv5;
		public event Action<int>? OnUv5Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv6;
		public int Uv6 => _uv6;
		public event Action<int>? OnUv6Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv7;
		public int Uv7 => _uv7;
		public event Action<int>? OnUv7Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv8;
		public int Uv8 => _uv8;
		public event Action<int>? OnUv8Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv10;
		public int Uv10 => _uv10;
		public event Action<int>? OnUv10Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv13;
		public int Uv13 => _uv13;
		public event Action<int>? OnUv13Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv15;
		public int Uv15 => _uv15;
		public event Action<int>? OnUv15Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv16;
		public int Uv16 => _uv16;
		public event Action<int>? OnUv16Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv18;
		public event Action<int>? OnUv18Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv19;
		public event Action<int>? OnUv19Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv20;
		public event Action<int>? OnUv20Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv21;
		public event Action<int>? OnUv21Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv23;
		public int Uv23 => _uv23;
		public event Action<int>? OnUv23Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv25;
		public int Uv25 => _uv25;
		public event Action<int>? OnUv25Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv26;
		public int Uv26 => _uv26;
		public event Action<int>? OnUv26Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv27;
		public int Uv27 => _uv27;
		public event Action<int>? OnUv27Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv29;
		public event Action<int>? OnUv29Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv30;
		public event Action<int>? OnUv30Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv31;
		public event Action<int>? OnUv31Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv32;
		public event Action<int>? OnUv32Changed;
		[SyncRpc]
		public partial void f3(int a);
		[SyncRpc]
		public partial void f14();
		[SyncRpc]
		public partial void f17(int a);
		[SyncRpc]
		public partial void f22();
		[SyncRpc]
		public partial void f24(int a);
		[SyncRpc]
		private partial void f28(int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf3(int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf9();
		[SyncRpc(SyncType.Unreliable)]
		public partial void uf12(int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf14(int a, float b);
		[SyncRpc(SyncType.Unreliable)]
		private partial void uf17(int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf22(byte a, int b, uint c);
		[SyncRpc(SyncType.Unreliable)]
		public partial void uf24();
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf28(int a);
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
					if (!reader.TryReadInt32(out _v4)) return false;
					OnV4Changed?.Invoke(_v4);
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadInt32(out _v5)) return false;
					OnV5Changed?.Invoke(_v5);
				}
				if (dirtyReliable_0[5])
				{
					if (!_v6.TryDeserializeSyncReliable(reader)) return false;
					OnV6Changed?.Invoke(_v6);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadInt32(out _v7)) return false;
					OnV7Changed?.Invoke(_v7);
				}
				if (dirtyReliable_0[7])
				{
					if (!reader.TryReadInt32(out _v8)) return false;
					OnV8Changed?.Invoke(_v8);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadInt32(out _v9)) return false;
					OnV9Changed?.Invoke(_v9);
				}
				if (dirtyReliable_1[1])
				{
					if (!reader.TryReadInt32(out _v10)) return false;
					OnV10Changed?.Invoke(_v10);
				}
				if (dirtyReliable_1[2])
				{
					if (!reader.TryReadInt32(out _v12)) return false;
					OnV12Changed?.Invoke(_v12);
				}
				if (dirtyReliable_1[3])
				{
					if (!reader.TryReadInt32(out _v13)) return false;
					OnV13Changed?.Invoke(_v13);
				}
				if (dirtyReliable_1[4])
				{
					if (!reader.TryReadInt32(out _v15)) return false;
					OnV15Changed?.Invoke(_v15);
				}
				if (dirtyReliable_1[5])
				{
					if (!reader.TryReadInt32(out _v16)) return false;
					OnV16Changed?.Invoke(_v16);
				}
				if (dirtyReliable_1[6])
				{
					if (!reader.TryReadInt32(out _v18)) return false;
					OnV18Changed?.Invoke(_v18);
				}
				if (dirtyReliable_1[7])
				{
					if (!_v19.TryDeserializeSyncReliable(reader)) return false;
					OnV19Changed?.Invoke(_v19);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					if (!reader.TryReadInt32(out _v20)) return false;
					OnV20Changed?.Invoke(_v20);
				}
				if (dirtyReliable_2[1])
				{
					if (!reader.TryReadInt32(out _v21)) return false;
					OnV21Changed?.Invoke(_v21);
				}
				if (dirtyReliable_2[2])
				{
					if (!_v23.TryDeserializeSyncReliable(reader)) return false;
					OnV23Changed?.Invoke(_v23);
				}
				if (dirtyReliable_2[3])
				{
					if (!reader.TryReadInt32(out _v25)) return false;
					OnV25Changed?.Invoke(_v25);
				}
				if (dirtyReliable_2[4])
				{
					if (!reader.TryReadInt32(out _v26)) return false;
					OnV26Changed?.Invoke(_v26);
				}
				if (dirtyReliable_2[5])
				{
					if (!reader.TryReadInt32(out _v27)) return false;
					OnV27Changed?.Invoke(_v27);
				}
				if (dirtyReliable_2[6])
				{
					if (!reader.TryReadInt32(out _v29)) return false;
					OnV29Changed?.Invoke(_v29);
				}
				if (dirtyReliable_2[7])
				{
					if (!reader.TryReadInt32(out _v30)) return false;
					OnV30Changed?.Invoke(_v30);
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyReliable_3 = reader.ReadBitmaskByte();
				if (dirtyReliable_3[0])
				{
					if (!reader.TryReadInt32(out _v31)) return false;
					OnV31Changed?.Invoke(_v31);
				}
				if (dirtyReliable_3[1])
				{
					if (!reader.TryReadInt32(out _v32)) return false;
					OnV32Changed?.Invoke(_v32);
				}
				if (dirtyReliable_3[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						f3(a);
					}
				}
				if (dirtyReliable_3[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						f14();
					}
				}
				if (dirtyReliable_3[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						f17(a);
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
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_0[0])
				{
					if (!reader.TryReadInt32(out _uv0)) return false;
					OnUv0Changed?.Invoke(_uv0);
				}
				if (dirtyUnreliable_0[1])
				{
					if (!reader.TryReadInt32(out _uv1)) return false;
					OnUv1Changed?.Invoke(_uv1);
				}
				if (dirtyUnreliable_0[2])
				{
					if (!reader.TryReadInt32(out _uv2)) return false;
					OnUv2Changed?.Invoke(_uv2);
				}
				if (dirtyUnreliable_0[3])
				{
					if (!reader.TryReadInt32(out _uv4)) return false;
					OnUv4Changed?.Invoke(_uv4);
				}
				if (dirtyUnreliable_0[4])
				{
					if (!reader.TryReadInt32(out _uv5)) return false;
					OnUv5Changed?.Invoke(_uv5);
				}
				if (dirtyUnreliable_0[5])
				{
					if (!reader.TryReadInt32(out _uv6)) return false;
					OnUv6Changed?.Invoke(_uv6);
				}
				if (dirtyUnreliable_0[6])
				{
					if (!reader.TryReadInt32(out _uv7)) return false;
					OnUv7Changed?.Invoke(_uv7);
				}
				if (dirtyUnreliable_0[7])
				{
					if (!reader.TryReadInt32(out _uv8)) return false;
					OnUv8Changed?.Invoke(_uv8);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyUnreliable_1 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_1[0])
				{
					if (!reader.TryReadInt32(out _uv10)) return false;
					OnUv10Changed?.Invoke(_uv10);
				}
				if (dirtyUnreliable_1[1])
				{
					if (!reader.TryReadInt32(out _uv13)) return false;
					OnUv13Changed?.Invoke(_uv13);
				}
				if (dirtyUnreliable_1[2])
				{
					if (!reader.TryReadInt32(out _uv15)) return false;
					OnUv15Changed?.Invoke(_uv15);
				}
				if (dirtyUnreliable_1[3])
				{
					if (!reader.TryReadInt32(out _uv16)) return false;
					OnUv16Changed?.Invoke(_uv16);
				}
				if (dirtyUnreliable_1[4])
				{
					if (!reader.TryReadInt32(out _uv18)) return false;
					OnUv18Changed?.Invoke(_uv18);
				}
				if (dirtyUnreliable_1[5])
				{
					if (!reader.TryReadInt32(out _uv19)) return false;
					OnUv19Changed?.Invoke(_uv19);
				}
				if (dirtyUnreliable_1[6])
				{
					if (!reader.TryReadInt32(out _uv20)) return false;
					OnUv20Changed?.Invoke(_uv20);
				}
				if (dirtyUnreliable_1[7])
				{
					if (!reader.TryReadInt32(out _uv21)) return false;
					OnUv21Changed?.Invoke(_uv21);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyUnreliable_2 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_2[0])
				{
					if (!reader.TryReadInt32(out _uv23)) return false;
					OnUv23Changed?.Invoke(_uv23);
				}
				if (dirtyUnreliable_2[1])
				{
					if (!reader.TryReadInt32(out _uv25)) return false;
					OnUv25Changed?.Invoke(_uv25);
				}
				if (dirtyUnreliable_2[2])
				{
					if (!reader.TryReadInt32(out _uv26)) return false;
					OnUv26Changed?.Invoke(_uv26);
				}
				if (dirtyUnreliable_2[3])
				{
					if (!reader.TryReadInt32(out _uv27)) return false;
					OnUv27Changed?.Invoke(_uv27);
				}
				if (dirtyUnreliable_2[4])
				{
					if (!reader.TryReadInt32(out _uv29)) return false;
					OnUv29Changed?.Invoke(_uv29);
				}
				if (dirtyUnreliable_2[5])
				{
					if (!reader.TryReadInt32(out _uv30)) return false;
					OnUv30Changed?.Invoke(_uv30);
				}
				if (dirtyUnreliable_2[6])
				{
					if (!reader.TryReadInt32(out _uv31)) return false;
					OnUv31Changed?.Invoke(_uv31);
				}
				if (dirtyUnreliable_2[7])
				{
					if (!reader.TryReadInt32(out _uv32)) return false;
					OnUv32Changed?.Invoke(_uv32);
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyUnreliable_3 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_3[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						uf3(a);
					}
				}
				if (dirtyUnreliable_3[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						uf9();
					}
				}
				if (dirtyUnreliable_3[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						uf12(a);
					}
				}
				if (dirtyUnreliable_3[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						if (!reader.TryReadSingle(out float b)) return false;
						uf14(a, b);
					}
				}
				if (dirtyUnreliable_3[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						uf17(a);
					}
				}
				if (dirtyUnreliable_3[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadByte(out byte a)) return false;
						if (!reader.TryReadInt32(out int b)) return false;
						if (!reader.TryReadUInt32(out uint c)) return false;
						uf22(a, b, c);
					}
				}
				if (dirtyUnreliable_3[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						uf24();
					}
				}
				if (dirtyUnreliable_3[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						uf28(a);
					}
				}
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
			if (!reader.TryReadInt32(out _v4)) return false;
			OnV4Changed?.Invoke(_v4);
			if (!reader.TryReadInt32(out _v5)) return false;
			OnV5Changed?.Invoke(_v5);
			if (!_v6.TryDeserializeEveryProperty(reader)) return false;
			OnV6Changed?.Invoke(_v6);
			if (!reader.TryReadInt32(out _v7)) return false;
			OnV7Changed?.Invoke(_v7);
			if (!reader.TryReadInt32(out _v8)) return false;
			OnV8Changed?.Invoke(_v8);
			if (!reader.TryReadInt32(out _v9)) return false;
			OnV9Changed?.Invoke(_v9);
			if (!reader.TryReadInt32(out _v10)) return false;
			OnV10Changed?.Invoke(_v10);
			if (!reader.TryReadInt32(out _v12)) return false;
			OnV12Changed?.Invoke(_v12);
			if (!reader.TryReadInt32(out _v13)) return false;
			OnV13Changed?.Invoke(_v13);
			if (!reader.TryReadInt32(out _v15)) return false;
			OnV15Changed?.Invoke(_v15);
			if (!reader.TryReadInt32(out _v16)) return false;
			OnV16Changed?.Invoke(_v16);
			if (!reader.TryReadInt32(out _v18)) return false;
			OnV18Changed?.Invoke(_v18);
			if (!_v19.TryDeserializeEveryProperty(reader)) return false;
			OnV19Changed?.Invoke(_v19);
			if (!reader.TryReadInt32(out _v20)) return false;
			OnV20Changed?.Invoke(_v20);
			if (!reader.TryReadInt32(out _v21)) return false;
			OnV21Changed?.Invoke(_v21);
			if (!_v23.TryDeserializeEveryProperty(reader)) return false;
			OnV23Changed?.Invoke(_v23);
			if (!reader.TryReadInt32(out _v25)) return false;
			OnV25Changed?.Invoke(_v25);
			if (!reader.TryReadInt32(out _v26)) return false;
			OnV26Changed?.Invoke(_v26);
			if (!reader.TryReadInt32(out _v27)) return false;
			OnV27Changed?.Invoke(_v27);
			if (!reader.TryReadInt32(out _v29)) return false;
			OnV29Changed?.Invoke(_v29);
			if (!reader.TryReadInt32(out _v30)) return false;
			OnV30Changed?.Invoke(_v30);
			if (!reader.TryReadInt32(out _v31)) return false;
			OnV31Changed?.Invoke(_v31);
			if (!reader.TryReadInt32(out _v32)) return false;
			OnV32Changed?.Invoke(_v32);
			if (!reader.TryReadInt32(out _uv0)) return false;
			OnUv0Changed?.Invoke(_uv0);
			if (!reader.TryReadInt32(out _uv1)) return false;
			OnUv1Changed?.Invoke(_uv1);
			if (!reader.TryReadInt32(out _uv2)) return false;
			OnUv2Changed?.Invoke(_uv2);
			if (!reader.TryReadInt32(out _uv4)) return false;
			OnUv4Changed?.Invoke(_uv4);
			if (!reader.TryReadInt32(out _uv5)) return false;
			OnUv5Changed?.Invoke(_uv5);
			if (!reader.TryReadInt32(out _uv6)) return false;
			OnUv6Changed?.Invoke(_uv6);
			if (!reader.TryReadInt32(out _uv7)) return false;
			OnUv7Changed?.Invoke(_uv7);
			if (!reader.TryReadInt32(out _uv8)) return false;
			OnUv8Changed?.Invoke(_uv8);
			if (!reader.TryReadInt32(out _uv10)) return false;
			OnUv10Changed?.Invoke(_uv10);
			if (!reader.TryReadInt32(out _uv13)) return false;
			OnUv13Changed?.Invoke(_uv13);
			if (!reader.TryReadInt32(out _uv15)) return false;
			OnUv15Changed?.Invoke(_uv15);
			if (!reader.TryReadInt32(out _uv16)) return false;
			OnUv16Changed?.Invoke(_uv16);
			if (!reader.TryReadInt32(out _uv18)) return false;
			OnUv18Changed?.Invoke(_uv18);
			if (!reader.TryReadInt32(out _uv19)) return false;
			OnUv19Changed?.Invoke(_uv19);
			if (!reader.TryReadInt32(out _uv20)) return false;
			OnUv20Changed?.Invoke(_uv20);
			if (!reader.TryReadInt32(out _uv21)) return false;
			OnUv21Changed?.Invoke(_uv21);
			if (!reader.TryReadInt32(out _uv23)) return false;
			OnUv23Changed?.Invoke(_uv23);
			if (!reader.TryReadInt32(out _uv25)) return false;
			OnUv25Changed?.Invoke(_uv25);
			if (!reader.TryReadInt32(out _uv26)) return false;
			OnUv26Changed?.Invoke(_uv26);
			if (!reader.TryReadInt32(out _uv27)) return false;
			OnUv27Changed?.Invoke(_uv27);
			if (!reader.TryReadInt32(out _uv29)) return false;
			OnUv29Changed?.Invoke(_uv29);
			if (!reader.TryReadInt32(out _uv30)) return false;
			OnUv30Changed?.Invoke(_uv30);
			if (!reader.TryReadInt32(out _uv31)) return false;
			OnUv31Changed?.Invoke(_uv31);
			if (!reader.TryReadInt32(out _uv32)) return false;
			OnUv32Changed?.Invoke(_uv32);
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_v0 = 0;
			_v1 = 0;
			_v2 = 0;
			_v4 = 0;
			_v5 = 0;
			_v6.InitializeRemoteProperties();
			_v7 = 0;
			_v8 = 0;
			_v9 = 0;
			_v10 = 0;
			_v12 = 0;
			_v13 = 0;
			_v15 = 0;
			_v16 = 0;
			_v18 = 0;
			_v19.InitializeRemoteProperties();
			_v20 = 0;
			_v21 = 0;
			_v23.InitializeRemoteProperties();
			_v25 = 0;
			_v26 = 0;
			_v27 = 0;
			_v29 = 0;
			_v30 = 0;
			_v31 = 0;
			_v32 = 0;
			_uv0 = 0;
			_uv1 = 0;
			_uv2 = 0;
			_uv4 = 0;
			_uv5 = 0;
			_uv6 = 0;
			_uv7 = 0;
			_uv8 = 0;
			_uv10 = 0;
			_uv13 = 0;
			_uv15 = 0;
			_uv16 = 0;
			_uv18 = 0;
			_uv19 = 0;
			_uv20 = 0;
			_uv21 = 0;
			_uv23 = 0;
			_uv25 = 0;
			_uv26 = 0;
			_uv27 = 0;
			_uv29 = 0;
			_uv30 = 0;
			_uv31 = 0;
			_uv32 = 0;
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
					_v6.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(4);
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
					_v19.IgnoreSyncReliable(reader);
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
					_v23.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_2[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[5])
				{
					reader.Ignore(4);
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
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_3[3])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_3[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
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
					SyncList<UserId>.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(4);
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
					ZTest_InnerObject.IgnoreSyncStaticReliable(reader);
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
					SyncList<UserId>.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_2[3])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_2[5])
				{
					reader.Ignore(4);
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
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_3[3])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_3[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
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
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_0[0])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[1])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[2])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[3])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[4])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[5])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyUnreliable_1 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_1[0])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[1])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[2])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[3])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[4])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[5])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[6])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyUnreliable_2 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_2[0])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[1])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[2])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[3])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[4])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[5])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[6])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyUnreliable_3 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_3[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[1])
				{
					reader.Ignore(1);
				}
				if (dirtyUnreliable_3[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(1);
						reader.Ignore(4);
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[6])
				{
					reader.Ignore(1);
				}
				if (dirtyUnreliable_3[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
			}
		}
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_0[0])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[1])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[2])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[3])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[4])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[5])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_0[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyUnreliable_1 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_1[0])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[1])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[2])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[3])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[4])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[5])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[6])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_1[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyUnreliable_2 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_2[0])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[1])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[2])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[3])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[4])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[5])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[6])
				{
					reader.Ignore(4);
				}
				if (dirtyUnreliable_2[7])
				{
					reader.Ignore(4);
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyUnreliable_3 = reader.ReadBitmaskByte();
				if (dirtyUnreliable_3[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[1])
				{
					reader.Ignore(1);
				}
				if (dirtyUnreliable_3[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(1);
						reader.Ignore(4);
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_3[6])
				{
					reader.Ignore(1);
				}
				if (dirtyUnreliable_3[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
			}
		}
	}
}
#pragma warning restore CS0649
