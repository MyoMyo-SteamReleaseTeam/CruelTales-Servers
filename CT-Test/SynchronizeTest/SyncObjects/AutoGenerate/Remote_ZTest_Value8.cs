/*
 * Generated File : Remote_ZTest_Value8
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
#if UNITY_2021
using UnityEngine;
#endif

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_Value8
	{
		public override NetworkObjectType Type => NetworkObjectType.ZTest_Value8;
		[SyncVar]
		private NetString _v0 = new();
		public event Action<NetString>? OnV0Changed;
		[SyncVar]
		private NetStringShort _v1 = new();
		public event Action<NetStringShort>? OnV1Changed;
		[SyncVar]
		private byte _v2;
		public event Action<byte>? OnV2Changed;
		[SyncVar]
		private sbyte _v4;
		public event Action<sbyte>? OnV4Changed;
		[SyncVar]
		private ushort _v5;
		public event Action<ushort>? OnV5Changed;
		[SyncVar]
		private short _v6;
		public event Action<short>? OnV6Changed;
		[SyncVar]
		private int _v7;
		public event Action<int>? OnV7Changed;
		[SyncVar(SyncType.Unreliable)]
		private uint _uv0;
		public event Action<uint>? OnUv0Changed;
		[SyncVar(SyncType.Unreliable)]
		private long _uv1;
		public event Action<long>? OnUv1Changed;
		[SyncVar(SyncType.Unreliable)]
		private ulong _uv2;
		public event Action<ulong>? OnUv2Changed;
		[SyncVar(SyncType.Unreliable)]
		private float _uv4;
		public event Action<float>? OnUv4Changed;
		[SyncVar(SyncType.Unreliable)]
		private double _uv5;
		public event Action<double>? OnUv5Changed;
		[SyncVar(SyncType.Unreliable)]
		private UserId _uv6 = new();
		public event Action<UserId>? OnUv6Changed;
		[SyncVar(SyncType.Unreliable)]
		private int _uv7;
		public event Action<int>? OnUv7Changed;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void f3(int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uf3(int a);
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
			if (dirtyReliable_0[0])
			{
				if (!_v0.TryDeserialize(reader)) return false;
				OnV0Changed?.Invoke(_v0);
			}
			if (dirtyReliable_0[1])
			{
				if (!_v1.TryDeserialize(reader)) return false;
				OnV1Changed?.Invoke(_v1);
			}
			if (dirtyReliable_0[2])
			{
				if (!reader.TryReadByte(out _v2)) return false;
				OnV2Changed?.Invoke(_v2);
			}
			if (dirtyReliable_0[3])
			{
				if (!reader.TryReadSByte(out _v4)) return false;
				OnV4Changed?.Invoke(_v4);
			}
			if (dirtyReliable_0[4])
			{
				if (!reader.TryReadUInt16(out _v5)) return false;
				OnV5Changed?.Invoke(_v5);
			}
			if (dirtyReliable_0[5])
			{
				if (!reader.TryReadInt16(out _v6)) return false;
				OnV6Changed?.Invoke(_v6);
			}
			if (dirtyReliable_0[6])
			{
				if (!reader.TryReadInt32(out _v7)) return false;
				OnV7Changed?.Invoke(_v7);
			}
			if (dirtyReliable_0[7])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int a)) return false;
					f3(a);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				if (!reader.TryReadUInt32(out _uv0)) return false;
				OnUv0Changed?.Invoke(_uv0);
			}
			if (dirtyUnreliable_0[1])
			{
				if (!reader.TryReadInt64(out _uv1)) return false;
				OnUv1Changed?.Invoke(_uv1);
			}
			if (dirtyUnreliable_0[2])
			{
				if (!reader.TryReadUInt64(out _uv2)) return false;
				OnUv2Changed?.Invoke(_uv2);
			}
			if (dirtyUnreliable_0[3])
			{
				if (!reader.TryReadSingle(out _uv4)) return false;
				OnUv4Changed?.Invoke(_uv4);
			}
			if (dirtyUnreliable_0[4])
			{
				if (!reader.TryReadDouble(out _uv5)) return false;
				OnUv5Changed?.Invoke(_uv5);
			}
			if (dirtyUnreliable_0[5])
			{
				if (!_uv6.TryDeserialize(reader)) return false;
				OnUv6Changed?.Invoke(_uv6);
			}
			if (dirtyUnreliable_0[6])
			{
				if (!reader.TryReadInt32(out _uv7)) return false;
				OnUv7Changed?.Invoke(_uv7);
			}
			if (dirtyUnreliable_0[7])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int a)) return false;
					uf3(a);
				}
			}
			return true;
		}
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_v0.TryDeserialize(reader)) return false;
			OnV0Changed?.Invoke(_v0);
			if (!_v1.TryDeserialize(reader)) return false;
			OnV1Changed?.Invoke(_v1);
			if (!reader.TryReadByte(out _v2)) return false;
			OnV2Changed?.Invoke(_v2);
			if (!reader.TryReadSByte(out _v4)) return false;
			OnV4Changed?.Invoke(_v4);
			if (!reader.TryReadUInt16(out _v5)) return false;
			OnV5Changed?.Invoke(_v5);
			if (!reader.TryReadInt16(out _v6)) return false;
			OnV6Changed?.Invoke(_v6);
			if (!reader.TryReadInt32(out _v7)) return false;
			OnV7Changed?.Invoke(_v7);
			if (!reader.TryReadUInt32(out _uv0)) return false;
			OnUv0Changed?.Invoke(_uv0);
			if (!reader.TryReadInt64(out _uv1)) return false;
			OnUv1Changed?.Invoke(_uv1);
			if (!reader.TryReadUInt64(out _uv2)) return false;
			OnUv2Changed?.Invoke(_uv2);
			if (!reader.TryReadSingle(out _uv4)) return false;
			OnUv4Changed?.Invoke(_uv4);
			if (!reader.TryReadDouble(out _uv5)) return false;
			OnUv5Changed?.Invoke(_uv5);
			if (!_uv6.TryDeserialize(reader)) return false;
			OnUv6Changed?.Invoke(_uv6);
			if (!reader.TryReadInt32(out _uv7)) return false;
			OnUv7Changed?.Invoke(_uv7);
			return true;
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				NetString.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[1])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[4])
			{
				reader.Ignore(2);
			}
			if (dirtyReliable_0[5])
			{
				reader.Ignore(2);
			}
			if (dirtyReliable_0[6])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[7])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				NetString.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[1])
			{
				NetStringShort.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[4])
			{
				reader.Ignore(2);
			}
			if (dirtyReliable_0[5])
			{
				reader.Ignore(2);
			}
			if (dirtyReliable_0[6])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[7])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				reader.Ignore(4);
			}
			if (dirtyUnreliable_0[1])
			{
				reader.Ignore(8);
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
				reader.Ignore(8);
			}
			if (dirtyUnreliable_0[5])
			{
				UserId.IgnoreStatic(reader);
			}
			if (dirtyUnreliable_0[6])
			{
				reader.Ignore(4);
			}
			if (dirtyUnreliable_0[7])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				reader.Ignore(4);
			}
			if (dirtyUnreliable_0[1])
			{
				reader.Ignore(8);
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
				reader.Ignore(8);
			}
			if (dirtyUnreliable_0[5])
			{
				UserId.IgnoreStatic(reader);
			}
			if (dirtyUnreliable_0[6])
			{
				reader.Ignore(4);
			}
			if (dirtyUnreliable_0[7])
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
#pragma warning restore CS0649
