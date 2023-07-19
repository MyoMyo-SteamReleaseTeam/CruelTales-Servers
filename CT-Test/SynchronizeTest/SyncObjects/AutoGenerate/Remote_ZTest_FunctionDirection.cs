/*
 * Generated File : Remote_ZTest_FunctionDirection
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
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
	public partial class ZTest_FunctionDirection
	{
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_FromClientVoid();
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_FromServerArg(int a, int b);
		[SyncRpc]
		public partial void Server_FromServerVoid();
		[SyncRpc]
		public partial void Server_FromServerArg(int a, int b);
		private BitmaskByte _dirtyReliable_0 = new();
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
		public partial void Client_FromClientVoid()
		{
			Client_FromClientVoidCallstackCount++;
			_dirtyReliable_0[0] = true;
		}
		private byte Client_FromClientVoidCallstackCount = 0;
		public partial void Client_FromServerArg(int a, int b)
		{
			Client_FromServerArgCallstack.Add((a, b));
			_dirtyReliable_0[1] = true;
		}
		private List<(int a, int b)> Client_FromServerArgCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			Client_FromClientVoidCallstackCount = 0;
			Client_FromServerArgCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				writer.Put((byte)Client_FromClientVoidCallstackCount);
			}
			if (_dirtyReliable_0[1])
			{
				byte count = (byte)Client_FromServerArgCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_FromServerArgCallstack[i];
					writer.Put(arg.a);
					writer.Put(arg.b);
				}
			}
		}
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer) { }
		public override void InitializeMasterProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Server_FromServerVoid();
				}
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int a)) return false;
					if (!reader.TryReadInt32(out int b)) return false;
					Server_FromServerArg(a, b);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader) => true;
		public override void InitializeRemoteProperties() { }
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(4);
				}
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[1])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
					reader.Ignore(4);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649