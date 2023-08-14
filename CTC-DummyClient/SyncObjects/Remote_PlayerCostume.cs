/*
 * Generated File : Remote_PlayerCostume
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
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class PlayerCostume : IRemoteSynchronizable
	{
		[SyncVar]
		private int _head;
		public int Head => _head;
		private Action<int>? _onHeadChanged;
		public event Action<int> OnHeadChanged
		{
			add => _onHeadChanged += value;
			remove => _onHeadChanged -= value;
		}
		[SyncVar]
		private int _hair;
		public int Hair => _hair;
		private Action<int>? _onHairChanged;
		public event Action<int> OnHairChanged
		{
			add => _onHairChanged += value;
			remove => _onHairChanged -= value;
		}
		[SyncVar]
		private int _body;
		public int Body => _body;
		private Action<int>? _onBodyChanged;
		public event Action<int> OnBodyChanged
		{
			add => _onBodyChanged += value;
			remove => _onBodyChanged -= value;
		}
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public PlayerCostume(IDirtyable owner)
		{
			_owner = owner;
		}
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
		public void ClearDirtyReliable() { }
		public void ClearDirtyUnreliable() { }
		public void SerializeSyncReliable(IPacketWriter writer) { }
		public void SerializeSyncUnreliable(IPacketWriter writer) { }
		public void InitializeMasterProperties() { }
		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!reader.TryReadInt32(out _head)) return false;
				_onHeadChanged?.Invoke(_head);
			}
			if (dirtyReliable_0[1])
			{
				if (!reader.TryReadInt32(out _hair)) return false;
				_onHairChanged?.Invoke(_hair);
			}
			if (dirtyReliable_0[2])
			{
				if (!reader.TryReadInt32(out _body)) return false;
				_onBodyChanged?.Invoke(_body);
			}
			return true;
		}
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out _head)) return false;
			_onHeadChanged?.Invoke(_head);
			if (!reader.TryReadInt32(out _hair)) return false;
			_onHairChanged?.Invoke(_hair);
			if (!reader.TryReadInt32(out _body)) return false;
			_onBodyChanged?.Invoke(_body);
			return true;
		}
		public void InitializeRemoteProperties()
		{
			_head = 0;
			_hair = 0;
			_body = 0;
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
				reader.Ignore(4);
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
				reader.Ignore(4);
			}
		}
		public void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
