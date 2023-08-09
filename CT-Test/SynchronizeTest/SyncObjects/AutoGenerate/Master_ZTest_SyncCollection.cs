/*
 * Generated File : Master_ZTest_SyncCollection
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
using CT.Common.DataType;
using CT.Common.DataType.Input;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class ZTest_SyncCollection
	{
		[SyncObject]
		private readonly SyncList<UserId> _userIdList = new();
		[SyncObject(SyncType.ReliableOrUnreliable)]
		private readonly ZTest_InnerObjectTarget _syncObj = new();
		private BitmaskByte _dirtyReliable_0 = new();
		private BitmaskByte _dirtyUnreliable_0 = new();
		public override bool IsDirtyReliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _userIdList.IsDirtyReliable;
				isDirty |= _syncObj.IsDirtyReliable;
				isDirty |= _dirtyReliable_0.AnyTrue();
				return isDirty;
			}
		}
		public override bool IsDirtyUnreliable
		{
			get
			{
				bool isDirty = false;
				isDirty |= _syncObj.IsDirtyUnreliable;
				isDirty |= _dirtyUnreliable_0.AnyTrue();
				return isDirty;
			}
		}
		public SyncList<UserId> UserIdList => _userIdList;
		public ZTest_InnerObjectTarget SyncObj => _syncObj;
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			_userIdList.ClearDirtyReliable();
			_syncObj.ClearDirtyReliable();
		}
		public override void ClearDirtyUnreliable()
		{
			_dirtyUnreliable_0.Clear();
			_syncObj.ClearDirtyUnreliable();
		}
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0[0] = _userIdList.IsDirtyReliable;
			_dirtyReliable_0[1] = _syncObj.IsDirtyReliable;
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_userIdList.SerializeSyncReliable(writer);
			}
			if (_dirtyReliable_0[1])
			{
				int curSize = writer.Size;
				_syncObj.SerializeSyncReliable(player, writer);
				if (writer.Size == curSize)
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
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyUnreliable_0[0] = _syncObj.IsDirtyUnreliable;
			BitmaskByte dirtyUnreliable_0 = _dirtyUnreliable_0;
			int dirtyUnreliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyUnreliable_0[0])
			{
				int curSize = writer.Size;
				_syncObj.SerializeSyncUnreliable(player, writer);
				if (writer.Size == curSize)
				{
					dirtyUnreliable_0[0] = false;
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
			_userIdList.SerializeEveryProperty(writer);
			_syncObj.SerializeEveryProperty(writer);
		}
		public override void InitializeMasterProperties()
		{
			_userIdList.InitializeMasterProperties();
			_syncObj.InitializeMasterProperties();
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
