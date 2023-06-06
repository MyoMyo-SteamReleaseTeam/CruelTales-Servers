/*
 * Generated File : Remote_TestCube
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[Serializable]
	public partial class TestCube
	{
		public override NetworkObjectType Type => NetworkObjectType.TestCube;
		[SyncVar]
		private float _r;
		public event Action<float>? OnRChanged;
		[SyncVar]
		private float _g;
		public event Action<float>? OnGChanged;
		[SyncVar]
		private float _b;
		public event Action<float>? OnBChanged;
		[SyncVar(SyncType.ColdData)]
		private float _animationTime;
		public event Action<float>? OnAnimationTimeChanged;
		[SyncRpc]
		public partial void TestRPC(long someMessage);
		public override bool IsDirtyReliable => false;
		public override bool IsDirtyUnreliable => false;
		public override void ClearDirtyReliable() { }
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer) { }
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer) { }
		public override void InitializeProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!reader.TryReadSingle(out _r)) return false;
				OnRChanged?.Invoke(_r);
			}
			if (dirtyReliable_0[1])
			{
				if (!reader.TryReadSingle(out _g)) return false;
				OnGChanged?.Invoke(_g);
			}
			if (dirtyReliable_0[2])
			{
				if (!reader.TryReadSingle(out _b)) return false;
				OnBChanged?.Invoke(_b);
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt64(out long someMessage)) return false;
					TestRPC(someMessage);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadSingle(out _r)) return false;
			OnRChanged?.Invoke(_r);
			if (!reader.TryReadSingle(out _g)) return false;
			OnGChanged?.Invoke(_g);
			if (!reader.TryReadSingle(out _b)) return false;
			OnBChanged?.Invoke(_b);
			if (!reader.TryReadSingle(out _animationTime)) return false;
			OnAnimationTimeChanged?.Invoke(_animationTime);
			return true;
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
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
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(8);
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
				reader.Ignore(4);
			}
			if (dirtyReliable_0[2])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[3])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(8);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
