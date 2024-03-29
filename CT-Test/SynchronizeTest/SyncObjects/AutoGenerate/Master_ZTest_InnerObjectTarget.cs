/*
 * Generated File : Master_ZTest_InnerObjectTarget
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
	public partial class ZTest_InnerObjectTarget : IMasterSynchronizable
	{
		[SyncVar]
		private int _v0;
		[SyncVar(SyncType.Unreliable)]
		private int _uv1;
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void f1(NetworkPlayer player, NetStringShort a);
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public ZTest_InnerObjectTarget()
		{
		}
		public ZTest_InnerObjectTarget(IDirtyable owner)
		{
			_owner = owner;
		}
		private BitmaskByte _dirtyReliable_0 = new();
		private BitmaskByte _dirtyUnreliable_0 = new();
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
		public partial void f1(NetworkPlayer player, NetStringShort a)
		{
			f1NCallstack.Add(player, a);
			_dirtyReliable_0[1] = true;
			MarkDirtyReliable();
		}
		private TargetCallstack<NetworkPlayer, NetStringShort> f1NCallstack = new(8);
		public int Uv1
		{
			get => _uv1;
			set
			{
				if (_uv1 == value) return;
				_uv1 = value;
				_dirtyUnreliable_0[0] = true;
				MarkDirtyUnreliable();
			}
		}
		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			f1NCallstack.Clear();
		}
		public void ClearDirtyUnreliable()
		{
			_isDirtyUnreliable = false;
			_dirtyUnreliable_0.Clear();
		}
		public void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				writer.Put(_v0);
			}
			if (_dirtyReliable_0[1])
			{
				int f1NCount = f1NCallstack.GetCallCount(player);
				if (f1NCount > 0)
				{
					var f1NcallList = f1NCallstack.GetCallList(player);
					writer.Put((byte)f1NCount);
					for (int i = 0; i < f1NCount; i++)
					{
						var arg = f1NcallList[i];
						arg.Serialize(writer);
					}
				}
				else
				{
					dirtyReliable_0[1] = false;
				}
			}
			if (dirtyReliable_0.AnyTrue())
			{
				writer.PutTo(dirtyReliable_0, dirtyReliable_0_pos);
			}
			else
			{
				writer.SetSize(dirtyReliable_0_pos);
			}
		}
		public void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyUnreliable_0.Serialize(writer);
			if (_dirtyUnreliable_0[0])
			{
				writer.Put(_uv1);
			}
		}
		public void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put(_v0);
			writer.Put(_uv1);
		}
		public void InitializeMasterProperties()
		{
			_v0 = 0;
			_uv1 = 0;
		}
		public bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader) => true;
		public bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public void InitializeRemoteProperties() { }
		public void IgnoreSyncReliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticReliable(IPacketReader reader) { }
		public void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
