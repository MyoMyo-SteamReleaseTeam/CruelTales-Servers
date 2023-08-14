/*
 * Generated File : Master_ZTest_Value32Target
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
	public partial class ZTest_Value32Target
	{
		[SyncVar]
		private int _v0;
		[SyncVar]
		private int _v1;
		[SyncVar]
		private int _v2;
		[SyncVar]
		private int _v3;
		[SyncVar]
		private int _v4;
		[SyncVar]
		private int _v5;
		[SyncVar]
		private int _v6;
		[SyncObject]
		private readonly SyncList<UserId> _v7;
		[SyncVar]
		private int _v8;
		[SyncVar]
		private int _v9;
		[SyncVar]
		private int _v10;
		[SyncVar]
		private int _v11;
		[SyncVar]
		private int _v12;
		[SyncVar]
		private int _v13;
		[SyncVar]
		private int _v14;
		[SyncVar]
		private int _v16;
		[SyncVar]
		private int _v17;
		[SyncVar]
		private int _v18;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObjectTarget _v19;
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObject _v20;
		[SyncVar]
		private int _v21;
		[SyncObject]
		private readonly SyncList<UserId> _v23;
		[SyncVar]
		private int _v25;
		[SyncVar]
		private int _v26;
		[SyncVar]
		private int _v27;
		[SyncVar]
		private int _v29;
		[SyncVar]
		private int _v30;
		[SyncVar]
		private int _v31;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void ft15(NetworkPlayer player);
		[SyncRpc]
		public partial void f22();
		[SyncRpc]
		public partial void f24(int a);
		[SyncRpc]
		public partial void f28(int a);
		public ZTest_Value32Target()
		{
			_v7 = new(this);
			_v19 = new(this);
			_v20 = new(this);
			_v23 = new(this);
		}
		private BitmaskByte _dirtyReliable_0 = new();
		private BitmaskByte _dirtyReliable_1 = new();
		private BitmaskByte _dirtyReliable_2 = new();
		private BitmaskByte _dirtyReliable_3 = new();
		private BitmaskByte _dirtyUnreliable_0 = new();
		public int V0
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
		public int V1
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
		public int V2
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
		public int V3
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
		public int V4
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
		public int V6
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
		private int V8
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
		private int V9
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
		private int V10
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
		private int V11
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
		private int V12
		{
			get => _v12;
			set
			{
				if (_v12 == value) return;
				_v12 = value;
				_dirtyReliable_1[4] = true;
				MarkDirtyReliable();
			}
		}
		private int V13
		{
			get => _v13;
			set
			{
				if (_v13 == value) return;
				_v13 = value;
				_dirtyReliable_1[5] = true;
				MarkDirtyReliable();
			}
		}
		public int V14
		{
			get => _v14;
			set
			{
				if (_v14 == value) return;
				_v14 = value;
				_dirtyReliable_1[6] = true;
				MarkDirtyReliable();
			}
		}
		public int V16
		{
			get => _v16;
			set
			{
				if (_v16 == value) return;
				_v16 = value;
				_dirtyReliable_1[7] = true;
				MarkDirtyReliable();
			}
		}
		public int V17
		{
			get => _v17;
			set
			{
				if (_v17 == value) return;
				_v17 = value;
				_dirtyReliable_2[0] = true;
				MarkDirtyReliable();
			}
		}
		public int V18
		{
			get => _v18;
			set
			{
				if (_v18 == value) return;
				_v18 = value;
				_dirtyReliable_2[1] = true;
				MarkDirtyReliable();
			}
		}
		public ZTest_InnerObjectTarget V19 => _v19;
		public ZTest_InnerObject V20 => _v20;
		public int V21
		{
			get => _v21;
			set
			{
				if (_v21 == value) return;
				_v21 = value;
				_dirtyReliable_2[4] = true;
				MarkDirtyReliable();
			}
		}
		public int V25
		{
			get => _v25;
			set
			{
				if (_v25 == value) return;
				_v25 = value;
				_dirtyReliable_2[6] = true;
				MarkDirtyReliable();
			}
		}
		public int V26
		{
			get => _v26;
			set
			{
				if (_v26 == value) return;
				_v26 = value;
				_dirtyReliable_2[7] = true;
				MarkDirtyReliable();
			}
		}
		public int V27
		{
			get => _v27;
			set
			{
				if (_v27 == value) return;
				_v27 = value;
				_dirtyReliable_3[0] = true;
				MarkDirtyReliable();
			}
		}
		private int V29
		{
			get => _v29;
			set
			{
				if (_v29 == value) return;
				_v29 = value;
				_dirtyReliable_3[1] = true;
				MarkDirtyReliable();
			}
		}
		private int V30
		{
			get => _v30;
			set
			{
				if (_v30 == value) return;
				_v30 = value;
				_dirtyReliable_3[2] = true;
				MarkDirtyReliable();
			}
		}
		private int V31
		{
			get => _v31;
			set
			{
				if (_v31 == value) return;
				_v31 = value;
				_dirtyReliable_3[3] = true;
				MarkDirtyReliable();
			}
		}
		public partial void ft15(NetworkPlayer player)
		{
			ft15Callstack.Add(player);
			_dirtyReliable_3[4] = true;
			MarkDirtyReliable();
		}
		private TargetVoidCallstack<NetworkPlayer> ft15Callstack = new(8);
		public partial void f22()
		{
			f22CallstackCount++;
			_dirtyReliable_3[5] = true;
			MarkDirtyReliable();
		}
		private byte f22CallstackCount = 0;
		public partial void f24(int a)
		{
			f24iCallstack.Add(a);
			_dirtyReliable_3[6] = true;
			MarkDirtyReliable();
		}
		private List<int> f24iCallstack = new(4);
		public partial void f28(int a)
		{
			f28iCallstack.Add(a);
			_dirtyReliable_3[7] = true;
			MarkDirtyReliable();
		}
		private List<int> f28iCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			_v7.ClearDirtyReliable();
			_dirtyReliable_1.Clear();
			_dirtyReliable_2.Clear();
			_v19.ClearDirtyReliable();
			_v20.ClearDirtyReliable();
			_v23.ClearDirtyReliable();
			_dirtyReliable_3.Clear();
			ft15Callstack.Clear();
			f22CallstackCount = 0;
			f24iCallstack.Clear();
			f28iCallstack.Clear();
		}
		public override void ClearDirtyUnreliable()
		{
			_isDirtyUnreliable = false;
			_dirtyUnreliable_0.Clear();
			_v19.ClearDirtyUnreliable();
			_v20.ClearDirtyUnreliable();
		}
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte masterDirty = new BitmaskByte();
			_dirtyReliable_0[7] = _v7.IsDirtyReliable;
			masterDirty[0] = _dirtyReliable_0.AnyTrue();
			masterDirty[1] = _dirtyReliable_1.AnyTrue();
			_dirtyReliable_2[2] = _v19.IsDirtyReliable;
			_dirtyReliable_2[3] = _v20.IsDirtyReliable;
			_dirtyReliable_2[5] = _v23.IsDirtyReliable;
			masterDirty[2] = _dirtyReliable_2.AnyTrue();
			masterDirty[3] = _dirtyReliable_3.AnyTrue();
			int masterDirty_pos = writer.OffsetSize(sizeof(byte));
			if (masterDirty[0])
			{
				_dirtyReliable_0.Serialize(writer);
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
					_v7.SerializeSyncReliable(writer);
				}
			}
			if (masterDirty[1])
			{
				_dirtyReliable_1.Serialize(writer);
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
					writer.Put(_v10);
				}
				if (_dirtyReliable_1[3])
				{
					writer.Put(_v11);
				}
				if (_dirtyReliable_1[4])
				{
					writer.Put(_v12);
				}
				if (_dirtyReliable_1[5])
				{
					writer.Put(_v13);
				}
				if (_dirtyReliable_1[6])
				{
					writer.Put(_v14);
				}
				if (_dirtyReliable_1[7])
				{
					writer.Put(_v16);
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = _dirtyReliable_2;
				int dirtyReliable_2_pos = writer.OffsetSize(sizeof(byte));
				if (_dirtyReliable_2[0])
				{
					writer.Put(_v17);
				}
				if (_dirtyReliable_2[1])
				{
					writer.Put(_v18);
				}
				if (_dirtyReliable_2[2])
				{
					int curSize = writer.Size;
					_v19.SerializeSyncReliable(player, writer);
					if (writer.Size == curSize)
					{
						dirtyReliable_2[2] = false;
					}
				}
				if (_dirtyReliable_2[3])
				{
					_v20.SerializeSyncReliable(player, writer);
				}
				if (_dirtyReliable_2[4])
				{
					writer.Put(_v21);
				}
				if (_dirtyReliable_2[5])
				{
					_v23.SerializeSyncReliable(writer);
				}
				if (_dirtyReliable_2[6])
				{
					writer.Put(_v25);
				}
				if (_dirtyReliable_2[7])
				{
					writer.Put(_v26);
				}
				if (dirtyReliable_2.AnyTrue())
				{
					writer.PutTo(dirtyReliable_2, dirtyReliable_2_pos);
				}
				else
				{
					writer.SetSize(dirtyReliable_2_pos);
					masterDirty[2] = false;
				}
			}
			if (masterDirty[3])
			{
				BitmaskByte dirtyReliable_3 = _dirtyReliable_3;
				int dirtyReliable_3_pos = writer.OffsetSize(sizeof(byte));
				if (_dirtyReliable_3[0])
				{
					writer.Put(_v27);
				}
				if (_dirtyReliable_3[1])
				{
					writer.Put(_v29);
				}
				if (_dirtyReliable_3[2])
				{
					writer.Put(_v30);
				}
				if (_dirtyReliable_3[3])
				{
					writer.Put(_v31);
				}
				if (_dirtyReliable_3[4])
				{
					int ft15Count = ft15Callstack.GetCallCount(player);
					if (ft15Count > 0)
					{
						writer.Put((byte)ft15Count);
					}
					else
					{
						dirtyReliable_3[4] = false;
					}
				}
				if (_dirtyReliable_3[5])
				{
					writer.Put((byte)f22CallstackCount);
				}
				if (_dirtyReliable_3[6])
				{
					byte count = (byte)f24iCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = f24iCallstack[i];
						writer.Put(arg);
					}
				}
				if (_dirtyReliable_3[7])
				{
					byte count = (byte)f28iCallstack.Count;
					writer.Put(count);
					for (int i = 0; i < count; i++)
					{
						var arg = f28iCallstack[i];
						writer.Put(arg);
					}
				}
				if (dirtyReliable_3.AnyTrue())
				{
					writer.PutTo(dirtyReliable_3, dirtyReliable_3_pos);
				}
				else
				{
					writer.SetSize(dirtyReliable_3_pos);
					masterDirty[3] = false;
				}
			}
			if (masterDirty.AnyTrue())
			{
				writer.PutTo(masterDirty, masterDirty_pos);
			}
			else
			{
				writer.SetSize(masterDirty_pos);
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyUnreliable_0[0] = _v19.IsDirtyUnreliable;
			_dirtyUnreliable_0[1] = _v20.IsDirtyUnreliable;
			BitmaskByte dirtyUnreliable_0 = _dirtyUnreliable_0;
			int dirtyUnreliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyUnreliable_0[0])
			{
				int curSize = writer.Size;
				_v19.SerializeSyncUnreliable(player, writer);
				if (writer.Size == curSize)
				{
					dirtyUnreliable_0[0] = false;
				}
			}
			if (_dirtyUnreliable_0[1])
			{
				_v20.SerializeSyncUnreliable(player, writer);
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
			_v7.SerializeEveryProperty(writer);
			writer.Put(_v8);
			writer.Put(_v9);
			writer.Put(_v10);
			writer.Put(_v11);
			writer.Put(_v12);
			writer.Put(_v13);
			writer.Put(_v14);
			writer.Put(_v16);
			writer.Put(_v17);
			writer.Put(_v18);
			_v19.SerializeEveryProperty(writer);
			_v20.SerializeEveryProperty(writer);
			writer.Put(_v21);
			_v23.SerializeEveryProperty(writer);
			writer.Put(_v25);
			writer.Put(_v26);
			writer.Put(_v27);
			writer.Put(_v29);
			writer.Put(_v30);
			writer.Put(_v31);
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
			_v7.InitializeMasterProperties();
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
			_v19.InitializeMasterProperties();
			_v20.InitializeMasterProperties();
			_v21 = 0;
			_v23.InitializeMasterProperties();
			_v25 = 0;
			_v26 = 0;
			_v27 = 0;
			_v29 = 0;
			_v30 = 0;
			_v31 = 0;
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
