/*
 * Generated File : Master_ZTest_Value16Target
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class ZTest_Value16Target
	{
		[SyncVar]
		private byte _v0;
		[SyncVar]
		private sbyte _v1;
		[SyncVar]
		private ushort _v2;
		[SyncVar]
		private short _v3;
		[SyncVar]
		private uint _v4;
		[SyncVar]
		private int _v5;
		[SyncVar]
		private ulong _v6;
		[SyncVar]
		private long _v7;
		[SyncVar]
		private float _v8;
		[SyncVar]
		private double _v9;
		[SyncVar]
		private NetString _v10 = new();
		[SyncVar]
		private NetStringShort _v11 = new();
		[SyncObject]
		private readonly SyncList<NetString> _v12;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObjectTarget _v13;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObject _v14;
		[SyncVar(SyncType.Unreliable)]
		private byte _uv0;
		[SyncVar(SyncType.Unreliable)]
		private sbyte _uv1;
		[SyncRpc]
		public partial void f0();
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uft1(NetworkPlayer player);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uft2(NetworkPlayer player, int a);
		[SyncRpc(SyncType.UnreliableTarget)]
		public partial void uft3(NetworkPlayer player, NetString a, int b);
		public ZTest_Value16Target()
		{
			_v12 = new(this);
			_v13 = new(this);
			_v14 = new(this);
		}
		private BitmaskByte _dirtyReliable_0 = new();
		private BitmaskByte _dirtyReliable_1 = new();
		private BitmaskByte _dirtyUnreliable_0 = new();
		public byte V0
		{
			get => _v0;
			set
			{
				if (_v0 == value) return;
				_v0 = value;
				_dirtyReliable_0[0] = true;
				MarkDirtyReliable();
			}
		}
		public sbyte V1
		{
			get => _v1;
			set
			{
				if (_v1 == value) return;
				_v1 = value;
				_dirtyReliable_0[1] = true;
				MarkDirtyReliable();
			}
		}
		public ushort V2
		{
			get => _v2;
			set
			{
				if (_v2 == value) return;
				_v2 = value;
				_dirtyReliable_0[2] = true;
				MarkDirtyReliable();
			}
		}
		public short V3
		{
			get => _v3;
			set
			{
				if (_v3 == value) return;
				_v3 = value;
				_dirtyReliable_0[3] = true;
				MarkDirtyReliable();
			}
		}
		public uint V4
		{
			get => _v4;
			set
			{
				if (_v4 == value) return;
				_v4 = value;
				_dirtyReliable_0[4] = true;
				MarkDirtyReliable();
			}
		}
		public int V5
		{
			get => _v5;
			set
			{
				if (_v5 == value) return;
				_v5 = value;
				_dirtyReliable_0[5] = true;
				MarkDirtyReliable();
			}
		}
		public ulong V6
		{
			get => _v6;
			set
			{
				if (_v6 == value) return;
				_v6 = value;
				_dirtyReliable_0[6] = true;
				MarkDirtyReliable();
			}
		}
		public long V7
		{
			get => _v7;
			set
			{
				if (_v7 == value) return;
				_v7 = value;
				_dirtyReliable_0[7] = true;
				MarkDirtyReliable();
			}
		}
		public float V8
		{
			get => _v8;
			set
			{
				if (_v8 == value) return;
				_v8 = value;
				_dirtyReliable_1[0] = true;
				MarkDirtyReliable();
			}
		}
		public double V9
		{
			get => _v9;
			set
			{
				if (_v9 == value) return;
				_v9 = value;
				_dirtyReliable_1[1] = true;
				MarkDirtyReliable();
			}
		}
		public NetString V10
		{
			get => _v10;
			set
			{
				if (_v10 == value) return;
				_v10 = value;
				_dirtyReliable_1[2] = true;
				MarkDirtyReliable();
			}
		}
		public NetStringShort V11
		{
			get => _v11;
			set
			{
				if (_v11 == value) return;
				_v11 = value;
				_dirtyReliable_1[3] = true;
				MarkDirtyReliable();
			}
		}
		public ZTest_InnerObjectTarget V13 => _v13;
		public ZTest_InnerObject V14 => _v14;
		public partial void f0()
		{
			f0CallstackCount++;
			_dirtyReliable_1[7] = true;
			MarkDirtyReliable();
		}
		private byte f0CallstackCount = 0;
		public byte Uv0
		{
			get => _uv0;
			set
			{
				if (_uv0 == value) return;
				_uv0 = value;
				_dirtyUnreliable_0[2] = true;
				MarkDirtyUnreliable();
			}
		}
		public sbyte Uv1
		{
			get => _uv1;
			set
			{
				if (_uv1 == value) return;
				_uv1 = value;
				_dirtyUnreliable_0[3] = true;
				MarkDirtyUnreliable();
			}
		}
		public partial void uft1(NetworkPlayer player)
		{
			uft1Callstack.Add(player);
			_dirtyUnreliable_0[4] = true;
			MarkDirtyUnreliable();
		}
		private TargetVoidCallstack<NetworkPlayer> uft1Callstack = new(8);
		public partial void uft2(NetworkPlayer player, int a)
		{
			uft2iCallstack.Add(player, a);
			_dirtyUnreliable_0[5] = true;
			MarkDirtyUnreliable();
		}
		private TargetCallstack<NetworkPlayer, int> uft2iCallstack = new(8);
		public partial void uft3(NetworkPlayer player, NetString a, int b)
		{
			uft3NiCallstack.Add(player, (a, b));
			_dirtyUnreliable_0[6] = true;
			MarkDirtyUnreliable();
		}
		private TargetCallstack<NetworkPlayer, (NetString a, int b)> uft3NiCallstack = new(8);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			_dirtyReliable_1.Clear();
			_v12.ClearDirtyReliable();
			_v13.ClearDirtyReliable();
			_v14.ClearDirtyReliable();
			f0CallstackCount = 0;
		}
		public override void ClearDirtyUnreliable()
		{
			_isDirtyUnreliable = false;
			_dirtyUnreliable_0.Clear();
			_v13.ClearDirtyUnreliable();
			_v14.ClearDirtyUnreliable();
			uft1Callstack.Clear();
			uft2iCallstack.Clear();
			uft3NiCallstack.Clear();
		}
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			int originSize = writer.Size;
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0.AnyTrue())
			{
				if (_dirtyReliable_0[0])
				{
					writer.Put(_v0);
				}
				if (_dirtyReliable_0[1])
				{
					writer.Put(_v1);
				}
				if (_dirtyReliable_0[2])
				{
					writer.Put(_v2);
				}
				if (_dirtyReliable_0[3])
				{
					writer.Put(_v3);
				}
				if (_dirtyReliable_0[4])
				{
					writer.Put(_v4);
				}
				if (_dirtyReliable_0[5])
				{
					writer.Put(_v5);
				}
				if (_dirtyReliable_0[6])
				{
					writer.Put(_v6);
				}
				if (_dirtyReliable_0[7])
				{
					writer.Put(_v7);
				}
			}
			_dirtyReliable_1[4] = _v12.IsDirtyReliable;
			_dirtyReliable_1[5] = _v13.IsDirtyReliable;
			_dirtyReliable_1[6] = _v14.IsDirtyReliable;
			BitmaskByte dirtyReliable_1 = _dirtyReliable_1;
			int dirtyReliable_1_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_1.AnyTrue())
			{
				if (_dirtyReliable_1[0])
				{
					writer.Put(_v8);
				}
				if (_dirtyReliable_1[1])
				{
					writer.Put(_v9);
				}
				if (_dirtyReliable_1[2])
				{
					_v10.Serialize(writer);
				}
				if (_dirtyReliable_1[3])
				{
					_v11.Serialize(writer);
				}
				if (_dirtyReliable_1[4])
				{
					_v12.SerializeSyncReliable(writer);
				}
				if (_dirtyReliable_1[5])
				{
					int curSize = writer.Size;
					_v13.SerializeSyncReliable(player, writer);
					if (writer.Size == curSize)
					{
						dirtyReliable_1[5] = false;
					}
				}
				if (_dirtyReliable_1[6])
				{
					_v14.SerializeSyncReliable(player, writer);
				}
				if (_dirtyReliable_1[7])
				{
					writer.Put((byte)f0CallstackCount);
				}
			}
			writer.PutTo(dirtyReliable_1, dirtyReliable_1_pos);
			if (writer.Size == originSize + 2)
			{
				writer.SetSize(originSize);
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyUnreliable_0[0] = _v13.IsDirtyUnreliable;
			_dirtyUnreliable_0[1] = _v14.IsDirtyUnreliable;
			BitmaskByte dirtyUnreliable_0 = _dirtyUnreliable_0;
			int dirtyUnreliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyUnreliable_0[0])
			{
				int curSize = writer.Size;
				_v13.SerializeSyncUnreliable(player, writer);
				if (writer.Size == curSize)
				{
					dirtyUnreliable_0[0] = false;
				}
			}
			if (_dirtyUnreliable_0[1])
			{
				_v14.SerializeSyncUnreliable(player, writer);
			}
			if (_dirtyUnreliable_0[2])
			{
				writer.Put(_uv0);
			}
			if (_dirtyUnreliable_0[3])
			{
				writer.Put(_uv1);
			}
			if (_dirtyUnreliable_0[4])
			{
				int uft1Count = uft1Callstack.GetCallCount(player);
				if (uft1Count > 0)
				{
					writer.Put((byte)uft1Count);
				}
				else
				{
					dirtyUnreliable_0[4] = false;
				}
			}
			if (_dirtyUnreliable_0[5])
			{
				int uft2iCount = uft2iCallstack.GetCallCount(player);
				if (uft2iCount > 0)
				{
					var uft2icallList = uft2iCallstack.GetCallList(player);
					writer.Put((byte)uft2iCount);
					for (int i = 0; i < uft2iCount; i++)
					{
						var arg = uft2icallList[i];
						writer.Put(arg);
					}
				}
				else
				{
					dirtyUnreliable_0[5] = false;
				}
			}
			if (_dirtyUnreliable_0[6])
			{
				int uft3NiCount = uft3NiCallstack.GetCallCount(player);
				if (uft3NiCount > 0)
				{
					var uft3NicallList = uft3NiCallstack.GetCallList(player);
					writer.Put((byte)uft3NiCount);
					for (int i = 0; i < uft3NiCount; i++)
					{
						var arg = uft3NicallList[i];
						arg.a.Serialize(writer);
						writer.Put(arg.b);
					}
				}
				else
				{
					dirtyUnreliable_0[6] = false;
				}
			}
			if (dirtyUnreliable_0.AnyTrue())
			{
				writer.PutTo(dirtyUnreliable_0, dirtyUnreliable_0_pos);
			}
			else
			{
				writer.SetSize(dirtyUnreliable_0_pos);
			}
		}
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put(_v0);
			writer.Put(_v1);
			writer.Put(_v2);
			writer.Put(_v3);
			writer.Put(_v4);
			writer.Put(_v5);
			writer.Put(_v6);
			writer.Put(_v7);
			writer.Put(_v8);
			writer.Put(_v9);
			_v10.Serialize(writer);
			_v11.Serialize(writer);
			_v12.SerializeEveryProperty(writer);
			_v13.SerializeEveryProperty(writer);
			_v14.SerializeEveryProperty(writer);
			writer.Put(_uv0);
			writer.Put(_uv1);
		}
		public override void InitializeMasterProperties()
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
			_v12.InitializeMasterProperties();
			_v13.InitializeMasterProperties();
			_v14.InitializeMasterProperties();
			_uv0 = 0;
			_uv1 = 0;
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader) => true;
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public override void InitializeRemoteProperties() { }
		public override void IgnoreSyncReliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticReliable(IPacketReader reader) { }
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
