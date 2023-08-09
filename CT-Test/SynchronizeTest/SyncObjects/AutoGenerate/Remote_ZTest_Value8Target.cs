/*
 * Generated File : Remote_ZTest_Value8Target
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
	public partial class ZTest_Value8Target
	{
		[SyncVar]
		private NetString _v0 = new();
		public NetString V0 => _v0;
		private Action<NetString>? _onV0Changed;
		public event Action<NetString> OnV0Changed
		{
			add => _onV0Changed += value;
			remove => _onV0Changed -= value;
		}
		[SyncVar]
		private NetStringShort _v1 = new();
		public NetStringShort V1 => _v1;
		private Action<NetStringShort>? _onV1Changed;
		public event Action<NetStringShort> OnV1Changed
		{
			add => _onV1Changed += value;
			remove => _onV1Changed -= value;
		}
		[SyncVar]
		private TestEnumType _v2;
		public TestEnumType V2 => _v2;
		private Action<TestEnumType>? _onV2Changed;
		public event Action<TestEnumType> OnV2Changed
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
		[SyncObject]
		private readonly SyncList<UserId> _v4 = new();
		public SyncList<UserId> V4 => _v4;
		private Action<SyncList<UserId>>? _onV4Changed;
		public event Action<SyncList<UserId>> OnV4Changed
		{
			add => _onV4Changed += value;
			remove => _onV4Changed -= value;
		}
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObjectTarget _v5 = new();
		public ZTest_InnerObjectTarget V5 => _v5;
		private Action<ZTest_InnerObjectTarget>? _onV5Changed;
		public event Action<ZTest_InnerObjectTarget> OnV5Changed
		{
			add => _onV5Changed += value;
			remove => _onV5Changed -= value;
		}
		[SyncVar(SyncType.Unreliable)]
		private int _uv0;
		public int Uv0 => _uv0;
		private Action<int>? _onUv0Changed;
		public event Action<int> OnUv0Changed
		{
			add => _onUv0Changed += value;
			remove => _onUv0Changed -= value;
		}
		[SyncVar(SyncType.Unreliable)]
		private int _uv1;
		public int Uv1 => _uv1;
		private Action<int>? _onUv1Changed;
		public event Action<int> OnUv1Changed
		{
			add => _onUv1Changed += value;
			remove => _onUv1Changed -= value;
		}
		[SyncVar(SyncType.Unreliable)]
		private int _uv2;
		public int Uv2 => _uv2;
		private Action<int>? _onUv2Changed;
		public event Action<int> OnUv2Changed
		{
			add => _onUv2Changed += value;
			remove => _onUv2Changed -= value;
		}
		[SyncVar(SyncType.Unreliable)]
		private int _uv3;
		public int Uv3 => _uv3;
		private Action<int>? _onUv3Changed;
		public event Action<int> OnUv3Changed
		{
			add => _onUv3Changed += value;
			remove => _onUv3Changed -= value;
		}
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void ft0(NetString v0, NetStringShort v1, TestEnumType v2, int v3);
		[SyncRpc]
		public partial void f1();
		[SyncRpc(SyncType.Unreliable)]
		public partial void uf0(int a, byte b);
		[SyncRpc(SyncType.Unreliable)]
		public partial void uf1(int a, double b);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uft2();
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
			if (dirtyReliable_0[0])
			{
				if (!_v0.TryDeserialize(reader)) return false;
				_onV0Changed?.Invoke(_v0);
			}
			if (dirtyReliable_0[1])
			{
				if (!_v1.TryDeserialize(reader)) return false;
				_onV1Changed?.Invoke(_v1);
			}
			if (dirtyReliable_0[2])
			{
				if (!reader.TryReadInt32(out var _v2Value)) return false;
				_v2 = (TestEnumType)_v2Value;
				_onV2Changed?.Invoke(_v2);
			}
			if (dirtyReliable_0[3])
			{
				if (!reader.TryReadInt32(out _v3)) return false;
				_onV3Changed?.Invoke(_v3);
			}
			if (dirtyReliable_0[4])
			{
				if (!_v4.TryDeserializeSyncReliable(reader)) return false;
				_onV4Changed?.Invoke(_v4);
			}
			if (dirtyReliable_0[5])
			{
				if (!_v5.TryDeserializeSyncReliable(reader)) return false;
				_onV5Changed?.Invoke(_v5);
			}
			if (dirtyReliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetString v0 = new();
					if (!v0.TryDeserialize(reader)) return false;
					NetStringShort v1 = new();
					if (!v1.TryDeserialize(reader)) return false;
					if (!reader.TryReadInt32(out var v2Value)) return false;
					TestEnumType v2 = (TestEnumType)v2Value;
					if (!reader.TryReadInt32(out int v3)) return false;
					ft0(v0, v1, v2, v3);
				}
			}
			if (dirtyReliable_0[7])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					f1();
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				if (!_v5.TryDeserializeSyncUnreliable(reader)) return false;
				_onV5Changed?.Invoke(_v5);
			}
			if (dirtyUnreliable_0[1])
			{
				if (!reader.TryReadInt32(out _uv0)) return false;
				_onUv0Changed?.Invoke(_uv0);
			}
			if (dirtyUnreliable_0[2])
			{
				if (!reader.TryReadInt32(out _uv1)) return false;
				_onUv1Changed?.Invoke(_uv1);
			}
			if (dirtyUnreliable_0[3])
			{
				if (!reader.TryReadInt32(out _uv2)) return false;
				_onUv2Changed?.Invoke(_uv2);
			}
			if (dirtyUnreliable_0[4])
			{
				if (!reader.TryReadInt32(out _uv3)) return false;
				_onUv3Changed?.Invoke(_uv3);
			}
			if (dirtyUnreliable_0[5])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int a)) return false;
					if (!reader.TryReadByte(out byte b)) return false;
					uf0(a, b);
				}
			}
			if (dirtyUnreliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int a)) return false;
					if (!reader.TryReadDouble(out double b)) return false;
					uf1(a, b);
				}
			}
			if (dirtyUnreliable_0[7])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					uft2();
				}
			}
			return true;
		}
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_v0.TryDeserialize(reader)) return false;
			_onV0Changed?.Invoke(_v0);
			if (!_v1.TryDeserialize(reader)) return false;
			_onV1Changed?.Invoke(_v1);
			if (!reader.TryReadInt32(out var _v2Value)) return false;
			_v2 = (TestEnumType)_v2Value;
			_onV2Changed?.Invoke(_v2);
			if (!reader.TryReadInt32(out _v3)) return false;
			_onV3Changed?.Invoke(_v3);
			if (!_v4.TryDeserializeEveryProperty(reader)) return false;
			_onV4Changed?.Invoke(_v4);
			if (!_v5.TryDeserializeEveryProperty(reader)) return false;
			_onV5Changed?.Invoke(_v5);
			if (!reader.TryReadInt32(out _uv0)) return false;
			_onUv0Changed?.Invoke(_uv0);
			if (!reader.TryReadInt32(out _uv1)) return false;
			_onUv1Changed?.Invoke(_uv1);
			if (!reader.TryReadInt32(out _uv2)) return false;
			_onUv2Changed?.Invoke(_uv2);
			if (!reader.TryReadInt32(out _uv3)) return false;
			_onUv3Changed?.Invoke(_uv3);
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_v0 = new();
			_v1 = new();
			_v2 = (TestEnumType)0;
			_v3 = 0;
			_v4.InitializeRemoteProperties();
			_v5.InitializeRemoteProperties();
			_uv0 = 0;
			_uv1 = 0;
			_uv2 = 0;
			_uv3 = 0;
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
				reader.Ignore(4);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[4])
			{
				_v4.IgnoreSyncReliable(reader);
			}
			if (dirtyReliable_0[5])
			{
				_v5.IgnoreSyncReliable(reader);
			}
			if (dirtyReliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetString.IgnoreStatic(reader);
					NetStringShort.IgnoreStatic(reader);
					reader.Ignore(4);
					reader.Ignore(4);
				}
			}
			if (dirtyReliable_0[7])
			{
				reader.Ignore(1);
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
				reader.Ignore(4);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[4])
			{
				SyncList<UserId>.IgnoreSyncStaticReliable(reader);
			}
			if (dirtyReliable_0[5])
			{
				ZTest_InnerObjectTarget.IgnoreSyncStaticReliable(reader);
			}
			if (dirtyReliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetString.IgnoreStatic(reader);
					NetStringShort.IgnoreStatic(reader);
					reader.Ignore(4);
					reader.Ignore(4);
				}
			}
			if (dirtyReliable_0[7])
			{
				reader.Ignore(1);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				_v5.IgnoreSyncUnreliable(reader);
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
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(1);
				}
			}
			if (dirtyUnreliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(8);
				}
			}
			if (dirtyUnreliable_0[7])
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
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(1);
				}
			}
			if (dirtyUnreliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(8);
				}
			}
			if (dirtyUnreliable_0[7])
			{
				reader.Ignore(1);
			}
		}
	}
}
#pragma warning restore CS0649
