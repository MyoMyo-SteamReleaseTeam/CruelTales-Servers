/*
 * Generated File : Master_NormalCharacter
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
	public partial class NormalCharacter
	{
		public override NetworkObjectType Type => NetworkObjectType.NormalCharacter;
		public NormalCharacter()
		{
		}
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Server_OnAnimationChangedDCallstack.Clear();
			Server_OnAnimationChangedDPCallstack.Clear();
			Server_OnProxyDirectionChangedPCallstack.Clear();
			Server_OrderTestiCallstack.Clear();
			Server_BroadcastOrderTestiiCallstack.Clear();
			Server_TestPositionTickByTickVCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer)
		{
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_userId.Serialize(writer);
			}
			if (_dirtyReliable_0[1])
			{
				byte count = (byte)Server_OnAnimationChangedDCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_OnAnimationChangedDCallstack[i];
					writer.Put((byte)arg);
				}
			}
			if (_dirtyReliable_0[2])
			{
				byte count = (byte)Server_OnAnimationChangedDPCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_OnAnimationChangedDPCallstack[i];
					writer.Put((byte)arg.state);
					writer.Put((byte)arg.direction);
				}
			}
			if (_dirtyReliable_0[3])
			{
				byte count = (byte)Server_OnProxyDirectionChangedPCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_OnProxyDirectionChangedPCallstack[i];
					writer.Put((byte)arg);
				}
			}
			if (_dirtyReliable_0[4])
			{
				int Server_OrderTestiCount = Server_OrderTestiCallstack.GetCallCount(player);
				if (Server_OrderTestiCount > 0)
				{
					var Server_OrderTesticallList = Server_OrderTestiCallstack.GetCallList(player);
					writer.Put((byte)Server_OrderTestiCount);
					for (int i = 0; i < Server_OrderTestiCount; i++)
					{
						var arg = Server_OrderTesticallList[i];
						writer.Put(arg);
					}
				}
				else
				{
					dirtyReliable_0[4] = false;
				}
			}
			if (_dirtyReliable_0[5])
			{
				byte count = (byte)Server_BroadcastOrderTestiiCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_BroadcastOrderTestiiCallstack[i];
					writer.Put(arg.userId);
					writer.Put(arg.fromSever);
				}
			}
			if (_dirtyReliable_0[6])
			{
				byte count = (byte)Server_TestPositionTickByTickVCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Server_TestPositionTickByTickVCallstack[i];
					arg.Serialize(writer);
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
		public override void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer) { }
		public override void SerializeEveryProperty(IPacketWriter writer)
		{
			_userId.Serialize(writer);
			writer.Put((byte)_animationState);
			writer.Put((byte)_proxyDirection);
			writer.Put(_animationTime);
		}
		public override void InitializeMasterProperties()
		{
			_userId = new();
			_animationState = (DokzaAnimationState)0;
			_proxyDirection = (ProxyDirection)0;
			_animationTime = 0;
		}
		public override bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadInt32(out int fromClient)) return false;
					Client_RequestTest(player, fromClient);
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					InputData inputData = new();
					if (!inputData.TryDeserialize(reader)) return false;
					Client_RequestInput(player, inputData);
				}
			}
			return true;
		}
		public override void InitializeRemoteProperties() { }
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public new static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(4);
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					InputData.IgnoreStatic(reader);
				}
			}
		}
		public new static void IgnoreSyncStaticUnreliable(IPacketReader reader)
		{
			BitmaskByte dirtyUnreliable_0 = reader.ReadBitmaskByte();
			if (dirtyUnreliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					InputData.IgnoreStatic(reader);
				}
			}
		}
	}
}
#pragma warning restore CS0649
