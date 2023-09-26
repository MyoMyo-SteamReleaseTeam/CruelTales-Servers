/*
 * Generated File : Master_CameraController
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
using CT.Common.Gameplay.Infos;
using CT.Common.Gameplay.MiniGames;
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
	public partial class CameraController
	{
		public override NetworkObjectType Type => NetworkObjectType.CameraController;
		[SyncVar]
		private NetworkIdentity _targetId = new();
		[SyncVar]
		private float _followSpeed;
		[SyncRpc]
		public partial void Server_MoveTo(Vector2 position);
		[SyncRpc]
		public partial void Server_LookAt(Vector2 position);
		[SyncRpc]
		public partial void Server_LookAt(Vector2 position, float time);
		[SyncRpc]
		public partial void Server_Shake();
		[SyncRpc]
		public partial void Server_Zoom(float zoom);
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_CannotFindBindTarget(NetworkPlayer player);
		public CameraController()
		{
		}
		private BitmaskByte _dirtyReliable_0 = new();
		public NetworkIdentity TargetId
		{
			get => _targetId;
			set
			{
				if (_targetId == value) return;
				_targetId = value;
				_dirtyReliable_0[0] = true;
				MarkDirtyReliable();
			}
		}
		public float FollowSpeed
		{
			get => _followSpeed;
			set
			{
				if (_followSpeed == value) return;
				_followSpeed = value;
				_dirtyReliable_0[1] = true;
				MarkDirtyReliable();
			}
		}
		public partial void Server_MoveTo(Vector2 position)
		{
			Server_MoveToVCallstack.Add(position);
			_dirtyReliable_0[2] = true;
			MarkDirtyReliable();
		}
		private List<Vector2> Server_MoveToVCallstack = new(4);
		public partial void Server_LookAt(Vector2 position)
		{
			Server_LookAtVCallstack.Add(position);
			_dirtyReliable_0[3] = true;
			MarkDirtyReliable();
		}
		private List<Vector2> Server_LookAtVCallstack = new(4);
		public partial void Server_LookAt(Vector2 position, float time)
		{
			Server_LookAtVfCallstack.Add((position, time));
			_dirtyReliable_0[4] = true;
			MarkDirtyReliable();
		}
		private List<(Vector2 position, float time)> Server_LookAtVfCallstack = new(4);
		public partial void Server_Shake()
		{
			Server_ShakeCallstackCount++;
			_dirtyReliable_0[5] = true;
			MarkDirtyReliable();
		}
		private byte Server_ShakeCallstackCount = 0;
		public partial void Server_Zoom(float zoom)
		{
			Server_ZoomfCallstack.Add(zoom);
			_dirtyReliable_0[6] = true;
			MarkDirtyReliable();
		}
		private List<float> Server_ZoomfCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_MoveToVCallstack.Clear();
			Server_LookAtVCallstack.Clear();
			Server_LookAtVfCallstack.Clear();
			Server_ShakeCallstackCount = 0;
			Server_ZoomfCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				_targetId.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put(_followSpeed);
			}
			if (_dirtyReliable_0[2])
			{
				byte count = (byte)Server_MoveToVCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_MoveToVCallstack[i];
					arg.Serialize(writer);
				}
			}
			if (_dirtyReliable_0[3])
			{
				byte count = (byte)Server_LookAtVCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_LookAtVCallstack[i];
					arg.Serialize(writer);
				}
			}
			if (_dirtyReliable_0[4])
			{
				byte count = (byte)Server_LookAtVfCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_LookAtVfCallstack[i];
					writer.Put(arg.position);
					writer.Put(arg.time);
				}
			}
			if (_dirtyReliable_0[5])
			{
				writer.Put((byte)Server_ShakeCallstackCount);
			}
			if (_dirtyReliable_0[6])
			{
				byte count = (byte)Server_ZoomfCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_ZoomfCallstack[i];
					writer.Put(arg);
				}
			}
		}
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			_targetId.Serialize(writer);
			writer.Put(_followSpeed);
		}
		public override void InitializeMasterProperties()
		{
			_targetId = new();
			_followSpeed = 0;
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Client_CannotFindBindTarget(player);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader) => true;
		public override void InitializeRemoteProperties() { }
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
