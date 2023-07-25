/*
 * Generated File : Remote_ZTest_InnerObjectTarget
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
#if UNITY_2021
using UnityEngine;
#endif

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class ZTest_InnerObjectTarget : IRemoteSynchronizable
	{
		[SyncVar]
		private int _v0;
		public int V0 => _v0;
		private Action<int>? _onV0Changed;
		public event Action<int> OnV0Changed
		{
			add => _onV0Changed += value;
			remove => _onV0Changed -= value;
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
		[SyncRpc(SyncType.ReliableTarget)]
		public partial void f1(NetStringShort a);
		public bool IsDirtyReliable => false;
		public bool IsDirtyUnreliable => false;
		public void ClearDirtyReliable() { }
		public void ClearDirtyUnreliable() { }
		public void SerializeSyncReliable(IPacketWriter writer) { }
		public void SerializeSyncUnreliable(IPacketWriter writer) { }
		public void SerializeEveryProperty(IPacketWriter writer) { }
		public void InitializeMasterProperties() { }
		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!reader.TryReadInt32(out _v0)) return false;
				_onV0Changed?.Invoke(_v0);
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort a = new();
					if (!a.TryDeserialize(reader)) return false;
					f1(a);
				}
			}
			return true;
		}
		public bool TryDeserializeSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				if (!reader.TryReadInt32(out _uv1)) return false;
				_onUv1Changed?.Invoke(_uv1);
			}
			return true;
		}
		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadInt32(out _v0)) return false;
			_onV0Changed?.Invoke(_v0);
			if (!reader.TryReadInt32(out _uv1)) return false;
			_onUv1Changed?.Invoke(_uv1);
			return true;
		}
		public void InitializeRemoteProperties()
		{
			_v0 = 0;
			_uv1 = 0;
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
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
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
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					NetStringShort.IgnoreStatic(reader);
				}
			}
		}
		public void IgnoreSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				reader.Ignore(4);
			}
		}
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				reader.Ignore(4);
			}
		}
	}
}
#pragma warning restore CS0649
