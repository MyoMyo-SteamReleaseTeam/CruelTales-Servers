/*
 * Generated File : Remote_ZTest_Value16NonTarget
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using CT.Common;
using CT.Common.DataType;
using CT.Common.Exceptions;
using CT.Common.Gameplay;
using CT.Common.Quantization;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools;
using CT.Common.DataType.Input;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay.PlayerCharacterStates;
using CT.Common.Gameplay.Players;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Collections;
using CT.Common.Tools.ConsoleHelper;
using CT.Common.Tools.Data;
using CT.Common.Tools.FSM;
using CT.Common.Tools.GetOpt;
using CT.Common.Tools.SharpJson;


namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_Value16NonTarget
	{
		[SyncVar]
		private byte _v0;
		public byte V0 => _v0;
		private Action<byte>? _onV0Changed;
		public event Action<byte> OnV0Changed
		{
			add => _onV0Changed += value;
			remove => _onV0Changed -= value;
		}
		[SyncVar]
		private sbyte _v1;
		public sbyte V1 => _v1;
		private Action<sbyte>? _onV1Changed;
		public event Action<sbyte> OnV1Changed
		{
			add => _onV1Changed += value;
			remove => _onV1Changed -= value;
		}
		[SyncVar]
		private ushort _v2;
		public ushort V2 => _v2;
		private Action<ushort>? _onV2Changed;
		public event Action<ushort> OnV2Changed
		{
			add => _onV2Changed += value;
			remove => _onV2Changed -= value;
		}
		[SyncVar]
		private short _v3;
		public short V3 => _v3;
		private Action<short>? _onV3Changed;
		public event Action<short> OnV3Changed
		{
			add => _onV3Changed += value;
			remove => _onV3Changed -= value;
		}
		[SyncVar]
		private uint _v4;
		public uint V4 => _v4;
		private Action<uint>? _onV4Changed;
		public event Action<uint> OnV4Changed
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
		private ulong _v6;
		public ulong V6 => _v6;
		private Action<ulong>? _onV6Changed;
		public event Action<ulong> OnV6Changed
		{
			add => _onV6Changed += value;
			remove => _onV6Changed -= value;
		}
		[SyncVar]
		private long _v7;
		public long V7 => _v7;
		private Action<long>? _onV7Changed;
		public event Action<long> OnV7Changed
		{
			add => _onV7Changed += value;
			remove => _onV7Changed -= value;
		}
		[SyncVar]
		private float _v8;
		public float V8 => _v8;
		private Action<float>? _onV8Changed;
		public event Action<float> OnV8Changed
		{
			add => _onV8Changed += value;
			remove => _onV8Changed -= value;
		}
		[SyncVar]
		private double _v9;
		public double V9 => _v9;
		private Action<double>? _onV9Changed;
		public event Action<double> OnV9Changed
		{
			add => _onV9Changed += value;
			remove => _onV9Changed -= value;
		}
		[SyncVar]
		private NetString _v10 = new();
		public NetString V10 => _v10;
		private Action<NetString>? _onV10Changed;
		public event Action<NetString> OnV10Changed
		{
			add => _onV10Changed += value;
			remove => _onV10Changed -= value;
		}
		[SyncVar]
		private NetStringShort _v11 = new();
		public NetStringShort V11 => _v11;
		private Action<NetStringShort>? _onV11Changed;
		public event Action<NetStringShort> OnV11Changed
		{
			add => _onV11Changed += value;
			remove => _onV11Changed -= value;
		}
		[SyncObject]
		private readonly SyncList<NetString> _v12 = new();
		public SyncList<NetString> V12 => _v12;
		private Action<SyncList<NetString>>? _onV12Changed;
		public event Action<SyncList<NetString>> OnV12Changed
		{
			add => _onV12Changed += value;
			remove => _onV12Changed -= value;
		}
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObjectTarget _v13 = new();
		public ZTest_InnerObjectTarget V13 => _v13;
		private Action<ZTest_InnerObjectTarget>? _onV13Changed;
		public event Action<ZTest_InnerObjectTarget> OnV13Changed
		{
			add => _onV13Changed += value;
			remove => _onV13Changed -= value;
		}
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObject _v14 = new();
		public ZTest_InnerObject V14 => _v14;
		private Action<ZTest_InnerObject>? _onV14Changed;
		public event Action<ZTest_InnerObject> OnV14Changed
		{
			add => _onV14Changed += value;
			remove => _onV14Changed -= value;
		}
		[SyncVar(SyncType.Unreliable)]
		private byte _uv0;
		public byte Uv0 => _uv0;
		private Action<byte>? _onUv0Changed;
		public event Action<byte> OnUv0Changed
		{
			add => _onUv0Changed += value;
			remove => _onUv0Changed -= value;
		}
		[SyncVar(SyncType.Unreliable)]
		private sbyte _uv1;
		public sbyte Uv1 => _uv1;
		private Action<sbyte>? _onUv1Changed;
		public event Action<sbyte> OnUv1Changed
		{
			add => _onUv1Changed += value;
			remove => _onUv1Changed -= value;
		}
		[SyncRpc]
		public partial void f0();
		public override bool IsDirtyReliable => false;
		public override bool IsDirtyUnreliable => false;
		public override void ClearDirtyReliable() { }
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer) { }
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void InitializeMasterProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					if (!reader.TryReadByte(out _v0)) return false;
					_onV0Changed?.Invoke(_v0);
				}
				if (dirtyReliable_0[1])
				{
					if (!reader.TryReadSByte(out _v1)) return false;
					_onV1Changed?.Invoke(_v1);
				}
				if (dirtyReliable_0[2])
				{
					if (!reader.TryReadUInt16(out _v2)) return false;
					_onV2Changed?.Invoke(_v2);
				}
				if (dirtyReliable_0[3])
				{
					if (!reader.TryReadInt16(out _v3)) return false;
					_onV3Changed?.Invoke(_v3);
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadUInt32(out _v4)) return false;
					_onV4Changed?.Invoke(_v4);
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadInt32(out _v5)) return false;
					_onV5Changed?.Invoke(_v5);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadUInt64(out _v6)) return false;
					_onV6Changed?.Invoke(_v6);
				}
				if (dirtyReliable_0[7])
				{
					if (!reader.TryReadInt64(out _v7)) return false;
					_onV7Changed?.Invoke(_v7);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadSingle(out _v8)) return false;
					_onV8Changed?.Invoke(_v8);
				}
				if (dirtyReliable_1[1])
				{
					if (!reader.TryReadDouble(out _v9)) return false;
					_onV9Changed?.Invoke(_v9);
				}
				if (dirtyReliable_1[2])
				{
					if (!_v10.TryDeserialize(reader)) return false;
					_onV10Changed?.Invoke(_v10);
				}
				if (dirtyReliable_1[3])
				{
					if (!_v11.TryDeserialize(reader)) return false;
					_onV11Changed?.Invoke(_v11);
				}
				if (dirtyReliable_1[4])
				{
					if (!_v12.TryDeserializeSyncReliable(reader)) return false;
					_onV12Changed?.Invoke(_v12);
				}
				if (dirtyReliable_1[5])
				{
					if (!_v13.TryDeserializeSyncReliable(reader)) return false;
					_onV13Changed?.Invoke(_v13);
				}
				if (dirtyReliable_1[6])
				{
					if (!_v14.TryDeserializeSyncReliable(reader)) return false;
					_onV14Changed?.Invoke(_v14);
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
				_onV13Changed?.Invoke(_v13);
			}
			if (dirtyUnreliable_0[1])
			{
				if (!_v14.TryDeserializeSyncUnreliable(reader)) return false;
				_onV14Changed?.Invoke(_v14);
			}
			if (dirtyUnreliable_0[2])
			{
				if (!reader.TryReadByte(out _uv0)) return false;
				_onUv0Changed?.Invoke(_uv0);
			}
			if (dirtyUnreliable_0[3])
			{
				if (!reader.TryReadSByte(out _uv1)) return false;
				_onUv1Changed?.Invoke(_uv1);
			}
			return true;
		}
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadByte(out _v0)) return false;
			_onV0Changed?.Invoke(_v0);
			if (!reader.TryReadSByte(out _v1)) return false;
			_onV1Changed?.Invoke(_v1);
			if (!reader.TryReadUInt16(out _v2)) return false;
			_onV2Changed?.Invoke(_v2);
			if (!reader.TryReadInt16(out _v3)) return false;
			_onV3Changed?.Invoke(_v3);
			if (!reader.TryReadUInt32(out _v4)) return false;
			_onV4Changed?.Invoke(_v4);
			if (!reader.TryReadInt32(out _v5)) return false;
			_onV5Changed?.Invoke(_v5);
			if (!reader.TryReadUInt64(out _v6)) return false;
			_onV6Changed?.Invoke(_v6);
			if (!reader.TryReadInt64(out _v7)) return false;
			_onV7Changed?.Invoke(_v7);
			if (!reader.TryReadSingle(out _v8)) return false;
			_onV8Changed?.Invoke(_v8);
			if (!reader.TryReadDouble(out _v9)) return false;
			_onV9Changed?.Invoke(_v9);
			if (!_v10.TryDeserialize(reader)) return false;
			_onV10Changed?.Invoke(_v10);
			if (!_v11.TryDeserialize(reader)) return false;
			_onV11Changed?.Invoke(_v11);
			if (!_v12.TryDeserializeEveryProperty(reader)) return false;
			_onV12Changed?.Invoke(_v12);
			if (!_v13.TryDeserializeEveryProperty(reader)) return false;
			_onV13Changed?.Invoke(_v13);
			if (!_v14.TryDeserializeEveryProperty(reader)) return false;
			_onV14Changed?.Invoke(_v14);
			if (!reader.TryReadByte(out _uv0)) return false;
			_onUv0Changed?.Invoke(_uv0);
			if (!reader.TryReadSByte(out _uv1)) return false;
			_onUv1Changed?.Invoke(_uv1);
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
