/*
 * Generated File : Remote_ZTest_Value16NonTarget
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
	public partial class ZTest_Value16NonTarget
	{
		[SyncVar]
		private byte _v0;
		public byte V0 => _v0;
		public event Action<byte>? OnV0Changed;
		[SyncVar]
		private sbyte _v1;
		public sbyte V1 => _v1;
		public event Action<sbyte>? OnV1Changed;
		[SyncVar]
		private ushort _v2;
		public ushort V2 => _v2;
		public event Action<ushort>? OnV2Changed;
		[SyncVar]
		private short _v3;
		public short V3 => _v3;
		public event Action<short>? OnV3Changed;
		[SyncVar]
		private uint _v4;
		public uint V4 => _v4;
		public event Action<uint>? OnV4Changed;
		[SyncVar]
		private int _v5;
		public int V5 => _v5;
		public event Action<int>? OnV5Changed;
		[SyncVar]
		private ulong _v6;
		public ulong V6 => _v6;
		public event Action<ulong>? OnV6Changed;
		[SyncVar]
		private long _v7;
		public long V7 => _v7;
		public event Action<long>? OnV7Changed;
		[SyncVar]
		private float _v8;
		public float V8 => _v8;
		public event Action<float>? OnV8Changed;
		[SyncVar]
		private double _v9;
		public double V9 => _v9;
		public event Action<double>? OnV9Changed;
		[SyncVar]
		private NetString _v10 = new();
		public NetString V10 => _v10;
		public event Action<NetString>? OnV10Changed;
		[SyncVar]
		private NetStringShort _v11 = new();
		public NetStringShort V11 => _v11;
		public event Action<NetStringShort>? OnV11Changed;
		[SyncObject]
		private readonly SyncList<NetString> _v12 = new();
		public SyncList<NetString> V12 => _v12;
		public event Action<SyncList<NetString>>? OnV12Changed;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObjectTarget _v13 = new();
		public ZTest_InnerObjectTarget V13 => _v13;
		public event Action<ZTest_InnerObjectTarget>? OnV13Changed;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObject _v14 = new();
		public ZTest_InnerObject V14 => _v14;
		public event Action<ZTest_InnerObject>? OnV14Changed;
		[SyncVar(SyncType.Unreliable)]
		private byte _uv0;
		public byte Uv0 => _uv0;
		public event Action<byte>? OnUv0Changed;
		[SyncVar(SyncType.Unreliable)]
		private sbyte _uv1;
		public sbyte Uv1 => _uv1;
		public event Action<sbyte>? OnUv1Changed;
		[SyncRpc]
		public partial void f0();
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
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					if (!reader.TryReadByte(out _v0)) return false;
					OnV0Changed?.Invoke(_v0);
				}
				if (dirtyReliable_0[1])
				{
					if (!reader.TryReadSByte(out _v1)) return false;
					OnV1Changed?.Invoke(_v1);
				}
				if (dirtyReliable_0[2])
				{
					if (!reader.TryReadUInt16(out _v2)) return false;
					OnV2Changed?.Invoke(_v2);
				}
				if (dirtyReliable_0[3])
				{
					if (!reader.TryReadInt16(out _v3)) return false;
					OnV3Changed?.Invoke(_v3);
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadUInt32(out _v4)) return false;
					OnV4Changed?.Invoke(_v4);
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadInt32(out _v5)) return false;
					OnV5Changed?.Invoke(_v5);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadUInt64(out _v6)) return false;
					OnV6Changed?.Invoke(_v6);
				}
				if (dirtyReliable_0[7])
				{
					if (!reader.TryReadInt64(out _v7)) return false;
					OnV7Changed?.Invoke(_v7);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadSingle(out _v8)) return false;
					OnV8Changed?.Invoke(_v8);
				}
				if (dirtyReliable_1[1])
				{
					if (!reader.TryReadDouble(out _v9)) return false;
					OnV9Changed?.Invoke(_v9);
				}
				if (dirtyReliable_1[2])
				{
					if (!_v10.TryDeserialize(reader)) return false;
					OnV10Changed?.Invoke(_v10);
				}
				if (dirtyReliable_1[3])
				{
					if (!_v11.TryDeserialize(reader)) return false;
					OnV11Changed?.Invoke(_v11);
				}
				if (dirtyReliable_1[4])
				{
					if (!_v12.TryDeserializeSyncReliable(reader)) return false;
					OnV12Changed?.Invoke(_v12);
				}
				if (dirtyReliable_1[5])
				{
					if (!_v13.TryDeserializeSyncReliable(reader)) return false;
					OnV13Changed?.Invoke(_v13);
				}
				if (dirtyReliable_1[6])
				{
					if (!_v14.TryDeserializeSyncReliable(reader)) return false;
					OnV14Changed?.Invoke(_v14);
				}
				if (dirtyReliable_1[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						f0();
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
				if (!_v13.TryDeserializeSyncUnreliable(reader)) return false;
				OnV13Changed?.Invoke(_v13);
			}
			if (dirtyUnreliable_0[1])
			{
				if (!_v14.TryDeserializeSyncUnreliable(reader)) return false;
				OnV14Changed?.Invoke(_v14);
			}
			if (dirtyUnreliable_0[2])
			{
				if (!reader.TryReadByte(out _uv0)) return false;
				OnUv0Changed?.Invoke(_uv0);
			}
			if (dirtyUnreliable_0[3])
			{
				if (!reader.TryReadSByte(out _uv1)) return false;
				OnUv1Changed?.Invoke(_uv1);
			}
			return true;
		}
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadByte(out _v0)) return false;
			OnV0Changed?.Invoke(_v0);
			if (!reader.TryReadSByte(out _v1)) return false;
			OnV1Changed?.Invoke(_v1);
			if (!reader.TryReadUInt16(out _v2)) return false;
			OnV2Changed?.Invoke(_v2);
			if (!reader.TryReadInt16(out _v3)) return false;
			OnV3Changed?.Invoke(_v3);
			if (!reader.TryReadUInt32(out _v4)) return false;
			OnV4Changed?.Invoke(_v4);
			if (!reader.TryReadInt32(out _v5)) return false;
			OnV5Changed?.Invoke(_v5);
			if (!reader.TryReadUInt64(out _v6)) return false;
			OnV6Changed?.Invoke(_v6);
			if (!reader.TryReadInt64(out _v7)) return false;
			OnV7Changed?.Invoke(_v7);
			if (!reader.TryReadSingle(out _v8)) return false;
			OnV8Changed?.Invoke(_v8);
			if (!reader.TryReadDouble(out _v9)) return false;
			OnV9Changed?.Invoke(_v9);
			if (!_v10.TryDeserialize(reader)) return false;
			OnV10Changed?.Invoke(_v10);
			if (!_v11.TryDeserialize(reader)) return false;
			OnV11Changed?.Invoke(_v11);
			if (!_v12.TryDeserializeEveryProperty(reader)) return false;
			OnV12Changed?.Invoke(_v12);
			if (!_v13.TryDeserializeEveryProperty(reader)) return false;
			OnV13Changed?.Invoke(_v13);
			if (!_v14.TryDeserializeEveryProperty(reader)) return false;
			OnV14Changed?.Invoke(_v14);
			if (!reader.TryReadByte(out _uv0)) return false;
			OnUv0Changed?.Invoke(_uv0);
			if (!reader.TryReadSByte(out _uv1)) return false;
			OnUv1Changed?.Invoke(_uv1);
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
			_v7 = 0;
			_v8 = 0;
			_v9 = 0;
			_v10 = new();
			_v11 = new();
			_v12.InitializeRemoteProperties();
			_v13.InitializeRemoteProperties();
			_v14.InitializeRemoteProperties();
			_uv0 = 0;
			_uv1 = 0;
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[1])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[2])
				{
					reader.Ignore(2);
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(2);
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
					reader.Ignore(8);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(8);
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
					reader.Ignore(8);
				}
				if (dirtyReliable_1[2])
				{
					NetString.IgnoreStatic(reader);
				}
				if (dirtyReliable_1[3])
				{
					NetStringShort.IgnoreStatic(reader);
				}
				if (dirtyReliable_1[4])
				{
					_v12.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_1[5])
				{
					_v13.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_1[6])
				{
					_v14.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_1[7])
				{
					reader.Ignore(1);
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
					reader.Ignore(1);
				}
				if (dirtyReliable_0[1])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[2])
				{
					reader.Ignore(2);
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(2);
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
					reader.Ignore(8);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(8);
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
					reader.Ignore(8);
				}
				if (dirtyReliable_1[2])
				{
					NetString.IgnoreStatic(reader);
				}
				if (dirtyReliable_1[3])
				{
					NetStringShort.IgnoreStatic(reader);
				}
				if (dirtyReliable_1[4])
				{
					SyncList<NetString>.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_1[5])
				{
					ZTest_InnerObjectTarget.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_1[6])
				{
					ZTest_InnerObject.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_1[7])
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
				_v13.IgnoreSyncUnreliable(reader);
			}
			if (dirtyUnreliable_0[1])
			{
				_v14.IgnoreSyncUnreliable(reader);
			}
			if (dirtyUnreliable_0[2])
			{
				reader.Ignore(1);
			}
			if (dirtyUnreliable_0[3])
			{
				reader.Ignore(1);
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
			if (dirtyUnreliable_0[2])
			{
				reader.Ignore(1);
			}
			if (dirtyUnreliable_0[3])
			{
				reader.Ignore(1);
			}
		}
	}
}
#pragma warning restore CS0649
