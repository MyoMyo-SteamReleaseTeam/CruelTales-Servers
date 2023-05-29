/*
 * Generated File : Master_TestCube
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
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	[Serializable]
	public partial class TestCube
	{
		public override NetworkObjectType Type => NetworkObjectType.TestCube;
		[SyncVar]
		private float _r;
		[SyncVar]
		private float _g;
		[SyncVar]
		private float _b;
		[SyncRpc]
		public partial void TestRPC(long someMessage);
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
		public float R
		{
			get => _r;
			set
			{
				if (_r == value) return;
				_r = value;
				_dirtyReliable_0[0] = true;
			}
		}
		public float G
		{
			get => _g;
			set
			{
				if (_g == value) return;
				_g = value;
				_dirtyReliable_0[1] = true;
			}
		}
		public float B
		{
			get => _b;
			set
			{
				if (_b == value) return;
				_b = value;
				_dirtyReliable_0[2] = true;
			}
		}
		public partial void TestRPC(long someMessage)
		{
			TestRPCCallstack.Add(someMessage);
			_dirtyReliable_0[3] = true;
		}
		private List<long> TestRPCCallstack = new(8);
		public override void ClearDirtyReliable()
		{
			_dirtyReliable_0.Clear();
			TestRPCCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				writer.Put(_r);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put(_g);
			}
			if (_dirtyReliable_0[2])
			{
				writer.Put(_b);
			}
			if (_dirtyReliable_0[3])
			{
				byte count = (byte)TestRPCCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = TestRPCCallstack[i];
					writer.Put(arg);
				}
			}
		}
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put(_r);
			writer.Put(_g);
			writer.Put(_b);
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader) => true;
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public override void IgnoreSyncReliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticReliable(IPacketReader reader) { }
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
