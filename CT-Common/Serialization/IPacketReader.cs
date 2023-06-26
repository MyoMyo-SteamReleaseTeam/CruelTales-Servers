using System;
#if NET
using System.Numerics;
#elif UNITY_2021
using UnityEngine;
#endif

using CT.Common.DataType;

namespace CT.Common.Serialization
{
	public interface IPacketReader
	{
		public ArraySegment<byte> ByteSegment { get; }
		public int Size { get; }
		public bool IsReadEnd { get; }
		public int ReadPosition { get; }
		public int Capacity { get; }
		public bool CanRead(int readSize);
		public bool CanRead<T>(T serializeObject) where T : IPacketSerializable;
		public void ResetReader();
		public void SetReadPosition(int position);
		public bool TryReadTo<T>(in T serializeObject) where T : IPacketSerializable;
		public void IgnoreAll();
		public void Ignore(int count);
		public bool TryRead<T>(out T value) where T : IPacketSerializable, new();
		public int CopyToWriter(IPacketWriter writer);
		public bool PeekBool();
		public bool PeekBoolean();
		public byte PeekByte();
		public sbyte PeekSByte();
		public short PeekInt16();
		public ushort PeekUInt16();
		public int PeekInt32();
		public uint PeekUInt32();
		public long PeekInt64();
		public ulong PeekUInt64();
		public float PeekSingle();
		public double PeekDouble();
		public bool ReadBool();
		public bool ReadBoolean();
		public byte ReadByte();
		public sbyte ReadSByte();
		public short ReadInt16();
		public ushort ReadUInt16();
		public int ReadInt32();
		public uint ReadUInt32();
		public long ReadInt64();
		public ulong ReadUInt64();
		public float ReadSingle();
		public double ReadDouble();
		public NetString ReadNetString();
		public NetStringShort ReadNetStringShort();
		public void ReadBytesCopy(ArraySegment<byte> dest, int offset);
		public byte[] ReadBytes();
		public bool TryPeekBool(out bool value);
		public bool TryPeekBoolean(out bool value);
		public bool TryPeekByte(out byte value);
		public bool TryPeekSByte(out sbyte value);
		public bool TryPeekInt16(out short value);
		public bool TryPeekUInt16(out ushort value);
		public bool TryPeekInt32(out int value);
		public bool TryPeekUInt32(out uint value);
		public bool TryPeekInt64(out long value);
		public bool TryPeekUInt64(out ulong value);
		public bool TryPeekSingle(out float value);
		public bool TryPeekDouble(out double value);
		public bool TryReadBool(out bool value);
		public bool TryReadBoolean(out bool value);
		public bool TryReadByte(out byte value);
		public bool TryReadSByte(out sbyte value);
		public bool TryReadInt16(out short value);
		public bool TryReadUInt16(out ushort value);
		public bool TryReadInt32(out int value);
		public bool TryReadUInt32(out uint value);
		public bool TryReadInt64(out long value);
		public bool TryReadUInt64(out ulong value);
		public bool TryReadSingle(out float value);
		public bool TryReadDouble(out double value);
		public bool TryReadNetString(out string value);
		public bool TryReadNetStringShort(out string value);

		#if UNITY_2021
		public bool TryReadVector2(out UnityEngine.Vector2 value);
		public bool TryReadVector3(out UnityEngine.Vector3 value);
		#endif
		public bool TryReadVector2(out System.Numerics.Vector2 value);
		public bool TryReadVector3(out System.Numerics.Vector3 value);
	}
}
