﻿/*
 * Generated File : Remote_ZTest_Value16
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
using CTC.Networks.Synchronizations;
using CTS.Instance.SyncObjects;

#if UNITY_2021
using UnityEngine;
#endif

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_Value16
	{
		public override NetworkObjectType Type => NetworkObjectType.ZTest_Value16;
		[SyncVar]
		private int _v0;
		public event Action<int>? OnV0Changed;
		[SyncVar]
		private int _v1;
		public event Action<int>? OnV1Changed;
		[SyncVar]
		private int _v2;
		public event Action<int>? OnV2Changed;
		[SyncVar]
		private NetString _v4 = new();
		public event Action<NetString>? OnV4Changed;
		[SyncVar]
		private NetStringShort _v5 = new();
		public event Action<NetStringShort>? OnV5Changed;
		[SyncVar]
		private byte _v6;
		public event Action<byte>? OnV6Changed;
		[SyncVar]
		private int _v7;
		public event Action<int>? OnV7Changed;
		[SyncVar]
		private ushort _v8;
		public event Action<ushort>? OnV8Changed;
		[SyncVar]
		private int _v9;
		public event Action<int>? OnV9Changed;
		[SyncVar]
		private byte _v10;
		public event Action<byte>? OnV10Changed;
		[SyncVar]
		private int _v12;
		public event Action<int>? OnV12Changed;
		[SyncVar]
		private short _v13;
		public event Action<short>? OnV13Changed;
		[SyncVar]
		private int _v15;
		public event Action<int>? OnV15Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv0;
		public event Action<int>? OnUv0Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv1;
		public event Action<int>? OnUv1Changed;
		[SyncVar(SyncType.Unreliable)]
		private ulong _uv2;
		public event Action<ulong>? OnUv2Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv4;
		public event Action<int>? OnUv4Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv5;
		public event Action<int>? OnUv5Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv6;
		public event Action<int>? OnUv6Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv7;
		public event Action<int>? OnUv7Changed;
		[SyncVar(SyncType.Unreliable)]
		private ushort _uv8;
		public event Action<ushort>? OnUv8Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv9;
		public event Action<int>? OnUv9Changed;
		[SyncVar(SyncType.Unreliable)]
		private float _uv10;
		public event Action<float>? OnUv10Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv12;
		public event Action<int>? OnUv12Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv13;
		public event Action<int>? OnUv13Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv15;
		public event Action<int>? OnUv15Changed;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void f3(int a);
		[SyncRpc]
		public partial void f14(int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf3(int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf14(int a);
		public override bool IsDirtyReliable => false;
		public override bool IsDirtyUnreliable => false;
		public override void ClearDirtyReliable() { }
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer) { }
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer) { }
		public override void InitializeProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
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
					if (!_v4.TryDeserialize(reader)) return false;
					OnV4Changed?.Invoke(_v4);
				}
				if (dirtyReliable_0[4])
				{
					if (!_v5.TryDeserialize(reader)) return false;
					OnV5Changed?.Invoke(_v5);
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadByte(out _v6)) return false;
					OnV6Changed?.Invoke(_v6);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadInt32(out _v7)) return false;
					OnV7Changed?.Invoke(_v7);
				}
				if (dirtyReliable_0[7])
				{
					if (!reader.TryReadUInt16(out _v8)) return false;
					OnV8Changed?.Invoke(_v8);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadInt32(out _v9)) return false;
					OnV9Changed?.Invoke(_v9);
				}
				if (dirtyReliable_1[1])
				{
					if (!reader.TryReadByte(out _v10)) return false;
					OnV10Changed?.Invoke(_v10);
				}
				if (dirtyReliable_1[2])
				{
					if (!reader.TryReadInt32(out _v12)) return false;
					OnV12Changed?.Invoke(_v12);
				}
				if (dirtyReliable_1[3])
				{
					if (!reader.TryReadInt16(out _v13)) return false;
					OnV13Changed?.Invoke(_v13);
				}
				if (dirtyReliable_1[4])
				{
					if (!reader.TryReadInt32(out _v15)) return false;
					OnV15Changed?.Invoke(_v15);
				}
				if (dirtyReliable_1[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						f3(a);
					}
				}
				if (dirtyReliable_1[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						f14(a);
					}
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0.AnyTrue())
			{
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
					if (!reader.TryReadUInt64(out _uv2)) return false;
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
					if (!reader.TryReadUInt16(out _uv8)) return false;
					OnUv8Changed?.Invoke(_uv8);
				}
			}
			BitmaskByte dirtyUnreliable_1 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_1.AnyTrue())
			{
				if (dirtyUnreliable_1[0])
				{
					if (!reader.TryReadInt32(out _uv9)) return false;
					OnUv9Changed?.Invoke(_uv9);
				}
				if (dirtyUnreliable_1[1])
				{
					if (!reader.TryReadSingle(out _uv10)) return false;
					OnUv10Changed?.Invoke(_uv10);
				}
				if (dirtyUnreliable_1[2])
				{
					if (!reader.TryReadInt32(out _uv12)) return false;
					OnUv12Changed?.Invoke(_uv12);
				}
				if (dirtyUnreliable_1[3])
				{
					if (!reader.TryReadInt32(out _uv13)) return false;
					OnUv13Changed?.Invoke(_uv13);
				}
				if (dirtyUnreliable_1[4])
				{
					if (!reader.TryReadInt32(out _uv15)) return false;
					OnUv15Changed?.Invoke(_uv15);
				}
				if (dirtyUnreliable_1[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						uf3(a);
					}
				}
				if (dirtyUnreliable_1[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						uf14(a);
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
			if (!_v4.TryDeserialize(reader)) return false;
			OnV4Changed?.Invoke(_v4);
			if (!_v5.TryDeserialize(reader)) return false;
			OnV5Changed?.Invoke(_v5);
			if (!reader.TryReadByte(out _v6)) return false;
			OnV6Changed?.Invoke(_v6);
			if (!reader.TryReadInt32(out _v7)) return false;
			OnV7Changed?.Invoke(_v7);
			if (!reader.TryReadUInt16(out _v8)) return false;
			OnV8Changed?.Invoke(_v8);
			if (!reader.TryReadInt32(out _v9)) return false;
			OnV9Changed?.Invoke(_v9);
			if (!reader.TryReadByte(out _v10)) return false;
			OnV10Changed?.Invoke(_v10);
			if (!reader.TryReadInt32(out _v12)) return false;
			OnV12Changed?.Invoke(_v12);
			if (!reader.TryReadInt16(out _v13)) return false;
			OnV13Changed?.Invoke(_v13);
			if (!reader.TryReadInt32(out _v15)) return false;
			OnV15Changed?.Invoke(_v15);
			if (!reader.TryReadInt32(out _uv0)) return false;
			OnUv0Changed?.Invoke(_uv0);
			if (!reader.TryReadInt32(out _uv1)) return false;
			OnUv1Changed?.Invoke(_uv1);
			if (!reader.TryReadUInt64(out _uv2)) return false;
			OnUv2Changed?.Invoke(_uv2);
			if (!reader.TryReadInt32(out _uv4)) return false;
			OnUv4Changed?.Invoke(_uv4);
			if (!reader.TryReadInt32(out _uv5)) return false;
			OnUv5Changed?.Invoke(_uv5);
			if (!reader.TryReadInt32(out _uv6)) return false;
			OnUv6Changed?.Invoke(_uv6);
			if (!reader.TryReadInt32(out _uv7)) return false;
			OnUv7Changed?.Invoke(_uv7);
			if (!reader.TryReadUInt16(out _uv8)) return false;
			OnUv8Changed?.Invoke(_uv8);
			if (!reader.TryReadInt32(out _uv9)) return false;
			OnUv9Changed?.Invoke(_uv9);
			if (!reader.TryReadSingle(out _uv10)) return false;
			OnUv10Changed?.Invoke(_uv10);
			if (!reader.TryReadInt32(out _uv12)) return false;
			OnUv12Changed?.Invoke(_uv12);
			if (!reader.TryReadInt32(out _uv13)) return false;
			OnUv13Changed?.Invoke(_uv13);
			if (!reader.TryReadInt32(out _uv15)) return false;
			OnUv15Changed?.Invoke(_uv15);
			return true;
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
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
					NetString.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[4])
				{
					NetStringShort.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(2);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[1])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[3])
				{
					reader.Ignore(2);
				}
				if (dirtyReliable_1[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[6])
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
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
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
					NetString.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[4])
				{
					NetStringShort.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(2);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[1])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[3])
				{
					reader.Ignore(2);
				}
				if (dirtyReliable_1[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_1[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[6])
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
			if (dirtyUnreliable_0.AnyTrue())
			{
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
					reader.Ignore(8);
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
					reader.Ignore(2);
				}
			}
			BitmaskByte dirtyUnreliable_1 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_1.AnyTrue())
			{
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
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_1[6])
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
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0.AnyTrue())
			{
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
					reader.Ignore(8);
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
					reader.Ignore(2);
				}
			}
			BitmaskByte dirtyUnreliable_1 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_1.AnyTrue())
			{
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
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyUnreliable_1[6])
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
