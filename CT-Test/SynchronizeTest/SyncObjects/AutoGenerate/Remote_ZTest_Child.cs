/*
 * Generated File : Remote_ZTest_Child
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
	public partial class ZTest_Child
	{
		[SyncVar(dir: SyncDirection.FromRemote)]
		protected int _field_Client_C3;
		[SyncVar(dir: SyncDirection.FromRemote)]
		protected int _field_Client_C4;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_C3();
		[SyncRpc(dir: SyncDirection.FromRemote)]
		protected partial void Client_c4();
		[SyncVar]
		protected int _field_Server_C3;
		public int Field_Server_C3 => _field_Server_C3;
		protected Action<int>? _onField_Server_C3Changed;
		public event Action<int> OnField_Server_C3Changed
		{
			add => _onField_Server_C3Changed += value;
			remove => _onField_Server_C3Changed -= value;
		}
		[SyncVar]
		protected int _field_Server_C4;
		protected Action<int>? _onField_Server_C4Changed;
		public event Action<int> OnField_Server_C4Changed
		{
			add => _onField_Server_C4Changed += value;
			remove => _onField_Server_C4Changed -= value;
		}
		[SyncVar]
		protected int _space_2;
		public int Space_2 => _space_2;
		protected Action<int>? _onSpace_2Changed;
		public event Action<int> OnSpace_2Changed
		{
			add => _onSpace_2Changed += value;
			remove => _onSpace_2Changed -= value;
		}
		[SyncVar]
		protected int _space_3;
		public int Space_3 => _space_3;
		protected Action<int>? _onSpace_3Changed;
		public event Action<int> OnSpace_3Changed
		{
			add => _onSpace_3Changed += value;
			remove => _onSpace_3Changed -= value;
		}
		[SyncRpc]
		public virtual partial void Server_C3();
		[SyncRpc(SyncType.ReliableTarget)]
		protected virtual partial void Server_c4();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _dirtyReliable_0.AnyTrue();
				return isDirty;
			}
		}
		public override bool IsDirtyUnreliable => false;
		public int Field_Client_C3
		{
			get => _field_Client_C3;
			set
			{
				if (_field_Client_C3 == value) return;
				_field_Client_C3 = value;
				_dirtyReliable_0[4] = true;
			}
		}
		protected int Field_Client_C4
		{
			get => _field_Client_C4;
			set
			{
				if (_field_Client_C4 == value) return;
				_field_Client_C4 = value;
				_dirtyReliable_0[5] = true;
			}
		}
		public partial void Client_C3()
		{
			Client_C3CallstackCount++;
			_dirtyReliable_0[6] = true;
		}
		protected byte Client_C3CallstackCount = 0;
		protected partial void Client_c4()
		{
			Client_c4CallstackCount++;
			_dirtyReliable_0[7] = true;
		}
		protected byte Client_c4CallstackCount = 0;
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			Client_P1CallstackCount = 0;
			Client_p2iiCallstack.Clear();
			Client_C3CallstackCount = 0;
			Client_c4CallstackCount = 0;
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				writer.Put(_field_Client_P1);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put(_field_Client_P2);
			}
			if (_dirtyReliable_0[2])
			{
				writer.Put((byte)Client_P1CallstackCount);
			}
			if (_dirtyReliable_0[3])
			{
				byte count = (byte)Client_p2iiCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_p2iiCallstack[i];
					writer.Put(arg.a);
					writer.Put(arg.b);
				}
			}
			if (_dirtyReliable_0[4])
			{
				writer.Put(_field_Client_C3);
			}
			if (_dirtyReliable_0[5])
			{
				writer.Put(_field_Client_C4);
			}
			if (_dirtyReliable_0[6])
			{
				writer.Put((byte)Client_C3CallstackCount);
			}
			if (_dirtyReliable_0[7])
			{
				writer.Put((byte)Client_c4CallstackCount);
			}
		}
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void InitializeMasterProperties()
		{
			_field_Client_P1 = 0;
			_field_Client_P2 = 0;
			_field_Client_C3 = 0;
			_field_Client_C4 = 0;
		}
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					if (!reader.TryReadInt32(out _field_Server_P1)) return false;
					_onField_Server_P1Changed?.Invoke(_field_Server_P1);
				}
				if (dirtyReliable_0[1])
				{
					if (!reader.TryReadSingle(out _field_Server_P2)) return false;
					_onField_Server_P2Changed?.Invoke(_field_Server_P2);
				}
				if (dirtyReliable_0[2])
				{
					if (!reader.TryReadInt32(out _space_1)) return false;
					_onSpace_1Changed?.Invoke(_space_1);
				}
				if (dirtyReliable_0[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Server_P1();
					}
				}
				if (dirtyReliable_0[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadInt32(out int a)) return false;
						if (!reader.TryReadInt32(out int b)) return false;
						Server_p2(a, b);
					}
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadInt32(out _field_Server_C3)) return false;
					_onField_Server_C3Changed?.Invoke(_field_Server_C3);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadInt32(out _field_Server_C4)) return false;
					_onField_Server_C4Changed?.Invoke(_field_Server_C4);
				}
				if (dirtyReliable_0[7])
				{
					if (!reader.TryReadInt32(out _space_2)) return false;
					_onSpace_2Changed?.Invoke(_space_2);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					if (!reader.TryReadInt32(out _space_3)) return false;
					_onSpace_3Changed?.Invoke(_space_3);
				}
				if (dirtyReliable_1[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Server_C3();
					}
				}
				if (dirtyReliable_1[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Server_c4();
					}
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out _field_Server_P1)) return false;
			_onField_Server_P1Changed?.Invoke(_field_Server_P1);
			if (!reader.TryReadSingle(out _field_Server_P2)) return false;
			_onField_Server_P2Changed?.Invoke(_field_Server_P2);
			if (!reader.TryReadInt32(out _space_1)) return false;
			_onSpace_1Changed?.Invoke(_space_1);
			if (!reader.TryReadInt32(out _field_Server_C3)) return false;
			_onField_Server_C3Changed?.Invoke(_field_Server_C3);
			if (!reader.TryReadInt32(out _field_Server_C4)) return false;
			_onField_Server_C4Changed?.Invoke(_field_Server_C4);
			if (!reader.TryReadInt32(out _space_2)) return false;
			_onSpace_2Changed?.Invoke(_space_2);
			if (!reader.TryReadInt32(out _space_3)) return false;
			_onSpace_3Changed?.Invoke(_space_3);
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_field_Server_P1 = 0;
			_field_Server_P2 = 0;
			_space_1 = 0;
			_field_Server_C3 = 0;
			_field_Server_C4 = 0;
			_space_2 = 0;
			_space_3 = 0;
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
					reader.Ignore(1);
				}
				if (dirtyReliable_0[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
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
					reader.Ignore(4);
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
					reader.Ignore(4);
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
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
					reader.Ignore(4);
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
					reader.Ignore(1);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public new static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
