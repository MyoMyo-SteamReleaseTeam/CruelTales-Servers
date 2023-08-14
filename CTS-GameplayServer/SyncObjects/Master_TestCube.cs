/*
 * Generated File : Master_TestCube
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
	public partial class TestCube
	{
		public override NetworkObjectType Type => NetworkObjectType.TestCube;
		[SyncVar]
		private float _r;
		[SyncVar]
		private float _g;
		[SyncVar]
		private float _b;
		[SyncVar(SyncType.ColdData)]
		private float _animationTime;
		[SyncRpc]
		public partial void TestRPC(long someMessage);
		public TestCube()
		{
		}
		private BitmaskByte _dirtyReliable_0 = new();
		public float R
		{
			get => _r;
			set
			{
				if (_r == value) return;
				_r = value;
				_dirtyReliable_0[0] = true;
				MarkDirtyReliable();
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
				MarkDirtyReliable();
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
				MarkDirtyReliable();
			}
		}
		public partial void TestRPC(long someMessage)
		{
			TestRPClCallstack.Add(someMessage);
			_dirtyReliable_0[3] = true;
			MarkDirtyReliable();
		}
		private List<long> TestRPClCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			TestRPClCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
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
				byte count = (byte)TestRPClCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = TestRPClCallstack[i];
					writer.Put(arg);
				}
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			writer.Put(_r);
			writer.Put(_g);
			writer.Put(_b);
			writer.Put(_animationTime);
		}
		public override void InitializeMasterProperties()
		{
			_r = 0;
			_g = 0;
			_b = 0;
			_animationTime = 0;
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
