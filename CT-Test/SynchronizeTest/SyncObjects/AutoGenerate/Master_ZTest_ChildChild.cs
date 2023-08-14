/*
 * Generated File : Master_ZTest_ChildChild
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
	public partial class ZTest_ChildChild
	{
		[SyncVar]
		protected int _field_Server_CC5;
		[SyncVar]
		protected int _space_4;
		[SyncVar]
		protected int _field_Server_CC6;
		[SyncVar]
		protected int _space_5;
		[SyncVar]
		protected int _space_6;
		[SyncRpc]
		public partial void Server_CC5();
		[SyncRpc(SyncType.ReliableTarget)]
		protected partial void Server_cc6(NetworkPlayer player);
		[SyncVar(dir: SyncDirection.FromRemote)]
		protected int _field_Client_CC5;
		public int Field_Client_CC5 => _field_Client_CC5;
		protected Action<int>? _onField_Client_CC5Changed;
		public event Action<int> OnField_Client_CC5Changed
		{
			add => _onField_Client_CC5Changed += value;
			remove => _onField_Client_CC5Changed -= value;
		}
		[SyncVar(dir: SyncDirection.FromRemote)]
		protected int _field_Client_CC6;
		protected Action<int>? _onField_Client_CC6Changed;
		public event Action<int> OnField_Client_CC6Changed
		{
			add => _onField_Client_CC6Changed += value;
			remove => _onField_Client_CC6Changed -= value;
		}
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_CC5(NetworkPlayer player);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		protected partial void Client_cc6(NetworkPlayer player);
		public ZTest_ChildChild()
		{
		}
		protected BitmaskByte _dirtyReliable_2 = new();
		public int Field_Server_CC5
		{
			get => _field_Server_CC5;
			set
			{
				if (_field_Server_CC5 == value) return;
				_field_Server_CC5 = value;
				_dirtyReliable_1[3] = true;
				MarkDirtyReliable();
			}
		}
		public int Space_4
		{
			get => _space_4;
			set
			{
				if (_space_4 == value) return;
				_space_4 = value;
				_dirtyReliable_1[4] = true;
				MarkDirtyReliable();
			}
		}
		protected int Field_Server_CC6
		{
			get => _field_Server_CC6;
			set
			{
				if (_field_Server_CC6 == value) return;
				_field_Server_CC6 = value;
				_dirtyReliable_1[5] = true;
				MarkDirtyReliable();
			}
		}
		public int Space_5
		{
			get => _space_5;
			set
			{
				if (_space_5 == value) return;
				_space_5 = value;
				_dirtyReliable_1[6] = true;
				MarkDirtyReliable();
			}
		}
		public int Space_6
		{
			get => _space_6;
			set
			{
				if (_space_6 == value) return;
				_space_6 = value;
				_dirtyReliable_1[7] = true;
				MarkDirtyReliable();
			}
		}
		public partial void Server_CC5()
		{
			Server_CC5CallstackCount++;
			_dirtyReliable_2[0] = true;
			MarkDirtyReliable();
		}
		protected byte Server_CC5CallstackCount = 0;
		protected partial void Server_cc6(NetworkPlayer player)
		{
			Server_cc6Callstack.Add(player);
			_dirtyReliable_2[1] = true;
			MarkDirtyReliable();
		}
		protected TargetVoidCallstack<NetworkPlayer> Server_cc6Callstack = new(8);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_P1CallstackCount = 0;
			Server_p2iiCallstack.Clear();
			_dirtyReliable_1.Clear();
			Server_C3CallstackCount = 0;
			Server_c4Callstack.Clear();
			_dirtyReliable_2.Clear();
			Server_CC5CallstackCount = 0;
			Server_cc6Callstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte masterDirty = new BitmaskByte();
			masterDirty[0] = _dirtyReliable_0.AnyTrue();
			masterDirty[1] = _dirtyReliable_1.AnyTrue();
			masterDirty[2] = _dirtyReliable_2.AnyTrue();
			int masterDirty_pos = writer.OffsetSize(sizeof(byte));
			if (masterDirty[0])
			{
				BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
				int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
				if (_dirtyReliable_0[0])
				{
					writer.Put(_field_Server_P1);
				}
				if (_dirtyReliable_0[1])
				{
					writer.Put(_field_Server_P2);
				}
				if (_dirtyReliable_0[2])
				{
					writer.Put(_space_1);
				}
				if (_dirtyReliable_0[3])
				{
					writer.Put((byte)Server_P1CallstackCount);
				}
				if (_dirtyReliable_0[4])
				{
					int Server_p2iiCount = Server_p2iiCallstack.GetCallCount(player);
					if (Server_p2iiCount > 0)
					{
						var Server_p2iicallList = Server_p2iiCallstack.GetCallList(player);
						writer.Put((byte)Server_p2iiCount);
						for (int i = 0; i < Server_p2iiCount; i++)
						{
							var arg = Server_p2iicallList[i];
							writer.Put(arg.a);
							writer.Put(arg.b);
						}
					}
					else
					{
						dirtyReliable_0[4] = false;
					}
				}
				if (_dirtyReliable_0[5])
				{
					writer.Put(_field_Server_C3);
				}
				if (_dirtyReliable_0[6])
				{
					writer.Put(_field_Server_C4);
				}
				if (_dirtyReliable_0[7])
				{
					writer.Put(_space_2);
				}
				if (dirtyReliable_0.AnyTrue())
				{
					writer.PutTo(dirtyReliable_0, dirtyReliable_0_pos);
				}
				else
				{
					writer.SetSize(dirtyReliable_0_pos);
					masterDirty[0] = false;
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = _dirtyReliable_1;
				int dirtyReliable_1_pos = writer.OffsetSize(sizeof(byte));
				if (_dirtyReliable_1[0])
				{
					writer.Put(_space_3);
				}
				if (_dirtyReliable_1[1])
				{
					writer.Put((byte)Server_C3CallstackCount);
				}
				if (_dirtyReliable_1[2])
				{
					int Server_c4Count = Server_c4Callstack.GetCallCount(player);
					if (Server_c4Count > 0)
					{
						writer.Put((byte)Server_c4Count);
					}
					else
					{
						dirtyReliable_1[2] = false;
					}
				}
				if (_dirtyReliable_1[3])
				{
					writer.Put(_field_Server_CC5);
				}
				if (_dirtyReliable_1[4])
				{
					writer.Put(_space_4);
				}
				if (_dirtyReliable_1[5])
				{
					writer.Put(_field_Server_CC6);
				}
				if (_dirtyReliable_1[6])
				{
					writer.Put(_space_5);
				}
				if (_dirtyReliable_1[7])
				{
					writer.Put(_space_6);
				}
				if (dirtyReliable_1.AnyTrue())
				{
					writer.PutTo(dirtyReliable_1, dirtyReliable_1_pos);
				}
				else
				{
					writer.SetSize(dirtyReliable_1_pos);
					masterDirty[1] = false;
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = _dirtyReliable_2;
				int dirtyReliable_2_pos = writer.OffsetSize(sizeof(byte));
				if (_dirtyReliable_2[0])
				{
					writer.Put((byte)Server_CC5CallstackCount);
				}
				if (_dirtyReliable_2[1])
				{
					int Server_cc6Count = Server_cc6Callstack.GetCallCount(player);
					if (Server_cc6Count > 0)
					{
						writer.Put((byte)Server_cc6Count);
					}
					else
					{
						dirtyReliable_2[1] = false;
					}
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
			if (masterDirty.AnyTrue())
			{
				writer.PutTo(masterDirty, masterDirty_pos);
			}
			else
			{
				writer.SetSize(masterDirty_pos);
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put(_field_Server_P1);
			writer.Put(_field_Server_P2);
			writer.Put(_space_1);
			writer.Put(_field_Server_C3);
			writer.Put(_field_Server_C4);
			writer.Put(_space_2);
			writer.Put(_space_3);
			writer.Put(_field_Server_CC5);
			writer.Put(_space_4);
			writer.Put(_field_Server_CC6);
			writer.Put(_space_5);
			writer.Put(_space_6);
		}
		public override void InitializeMasterProperties()
		{
			_field_Server_P1 = 0;
			_field_Server_P2 = 0;
			_space_1 = 0;
			_field_Server_C3 = 0;
			_field_Server_C4 = 0;
			_space_2 = 0;
			_space_3 = 0;
			_field_Server_CC5 = 0;
			_space_4 = 0;
			_field_Server_CC6 = 0;
			_space_5 = 0;
			_space_6 = 0;
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					if (!reader.TryReadInt32(out _field_Client_P1)) return false;
					_onField_Client_P1Changed?.Invoke(_field_Client_P1);
				}
				if (dirtyReliable_0[1])
				{
					if (!reader.TryReadSingle(out _field_Client_P2)) return false;
					_onField_Client_P2Changed?.Invoke(_field_Client_P2);
				}
				if (dirtyReliable_0[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Client_P1(player);
					}
				}
				if (dirtyReliable_0[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						if (!reader.TryReadInt32(out int b)) return false;
						Client_p2(player, a, b);
					}
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadInt32(out _field_Client_C3)) return false;
					_onField_Client_C3Changed?.Invoke(_field_Client_C3);
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadInt32(out _field_Client_C4)) return false;
					_onField_Client_C4Changed?.Invoke(_field_Client_C4);
				}
				if (dirtyReliable_0[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Client_C3(player);
					}
				}
				if (dirtyReliable_0[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Client_c4(player);
					}
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadInt32(out _field_Client_CC5)) return false;
					_onField_Client_CC5Changed?.Invoke(_field_Client_CC5);
				}
				if (dirtyReliable_1[1])
				{
					if (!reader.TryReadInt32(out _field_Client_CC6)) return false;
					_onField_Client_CC6Changed?.Invoke(_field_Client_CC6);
				}
				if (dirtyReliable_1[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Client_CC5(player);
					}
				}
				if (dirtyReliable_1[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Client_cc6(player);
					}
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public override void InitializeRemoteProperties()
		{
			_field_Client_P1 = 0;
			_field_Client_P2 = 0;
			_field_Client_C3 = 0;
			_field_Client_C4 = 0;
			_field_Client_CC5 = 0;
			_field_Client_CC6 = 0;
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
					reader.Ignore(1);
				}
				if (dirtyReliable_0[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
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
					reader.Ignore(1);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(1);
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
					reader.Ignore(4);
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_1[3])
				{
					reader.Ignore(1);
				}
			}
		}
		public new static void IgnoreSyncStaticReliable(IPacketReader reader)
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
					reader.Ignore(1);
				}
				if (dirtyReliable_0[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
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
					reader.Ignore(1);
				}
				if (dirtyReliable_0[7])
				{
					reader.Ignore(1);
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
					reader.Ignore(4);
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_1[3])
				{
					reader.Ignore(1);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public new static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
