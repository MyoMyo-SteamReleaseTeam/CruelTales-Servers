/*
 * Generated File : Remote_ZTest_InnerTest
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


namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_InnerTest : IRemoteSynchronizable
	{
		[SyncVar(dir: SyncDirection.FromRemote)]
		private float _c;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_Test();
		[SyncVar]
		private int _a;
		public int A => _a;
		private Action<int>? _onAChanged;
		public event Action<int> OnAChanged
		{
			add => _onAChanged += value;
			remove => _onAChanged -= value;
		}
		[SyncVar]
		private float _b;
		public float B => _b;
		private Action<float>? _onBChanged;
		public event Action<float> OnBChanged
		{
			add => _onBChanged += value;
			remove => _onBChanged -= value;
		}
		[SyncRpc]
		public partial void Server_Test();
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void Server_TestTarget();
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public ZTest_InnerTest(IDirtyable owner)
		{
			_owner = owner;
		}
		private BitmaskByte _dirtyReliable_0 = new();
		protected bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		public void MarkDirtyReliable()
		{
			_isDirtyReliable = true;
			_owner.MarkDirtyReliable();
		}
		protected bool _isDirtyUnreliable;
		public bool IsDirtyUnreliable => _isDirtyUnreliable;
		public void MarkDirtyUnreliable()
		{
			_isDirtyUnreliable = true;
			_owner.MarkDirtyUnreliable();
		}
		public float C
		{
			get => _c;
			set
			{
				if (_c == value) return;
				_c = value;
				_dirtyReliable_0[0] = true;
				MarkDirtyReliable();
			}
		}
		public partial void Client_Test()
		{
			Client_TestCallstackCount++;
			_dirtyReliable_0[1] = true;
			MarkDirtyReliable();
		}
		private byte Client_TestCallstackCount = 0;
		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Client_TestCallstackCount = 0;
		}
		public void ClearDirtyUnreliable() { }
		public void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				writer.Put(_c);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put((byte)Client_TestCallstackCount);
			}
		}
		public void SerializeSyncUnreliable(IPacketWriter writer) { }
		public void InitializeMasterProperties()
		{
			_c = 0;
		}
		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!reader.TryReadInt32(out _a)) return false;
				_onAChanged?.Invoke(_a);
			}
			if (dirtyReliable_0[1])
			{
				if (!reader.TryReadSingle(out _b)) return false;
				_onBChanged?.Invoke(_b);
			}
			if (dirtyReliable_0[2])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Server_Test();
				}
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Server_TestTarget();
				}
			}
			return true;
		}
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out _a)) return false;
			_onAChanged?.Invoke(_a);
			if (!reader.TryReadSingle(out _b)) return false;
			_onBChanged?.Invoke(_b);
			return true;
		}
		public void InitializeRemoteProperties()
		{
			_a = 0;
			_b = 0;
		}
		public void IgnoreSyncReliable(IPacketReader reader)
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
				reader.Ignore(1);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(1);
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
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
				reader.Ignore(1);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(1);
			}
		}
		public void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
