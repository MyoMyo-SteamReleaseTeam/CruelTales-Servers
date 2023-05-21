#nullable enable
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	[StructLayout(LayoutKind.Sequential)]
	public class NetworkTransform : IUpdatable
	{
		private Vector3 _position;
		public Vector3 Position
		{
			get => _position;
			set => _position = value;
		}
		private Vector3 _velocity;
		public Vector3 Velocity
		{
			get => _velocity;
			set
			{
				if (_velocity == value)
					return;

				_velocity = value;
				IsDirty = true;
			}
		}

		// Check dirty
		//private Vector3 _previousPosition;
		private bool _isTeleported;
		public bool IsDirty { get; private set; }

		public Action<bool, Vector3, Vector3>? OnChanged;

		public NetworkTransform()
		{
#if NET
			Position = Vector3.Zero;
			Velocity = Vector3.Zero;
#else
			Position = Vector3.zero;
			Velocity = Vector3.zero;
#endif
		}

		public void Update(float deltaTime)
		{
			//_previousPosition = Position;
			Position += Velocity * deltaTime;
			//IsDirty = Position != _previousPosition;
		}

		/// <summary>위치를 변경합니다. 해당 위치로 순간이동합니다.</summary>
		public void SetPosition(Vector3 position)
		{
			Position = position;
			IsDirty = true;
			_isTeleported = true;
		}

		/// <summary>움직임을 변경합니다.</summary>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		public void OnMovement(Vector3 position, Vector3 velocity)
		{
			Position = position;
			Velocity = velocity;
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(_isTeleported);
			writer.Put(Position);
			writer.Put(Velocity);
		}

		public void SerializeSpawnData(IPacketWriter writer)
		{
			writer.Put(Position);
			writer.Put(Velocity);
		}

		public void Deserialize(IPacketReader reader)
		{
			bool isTeleport = reader.ReadBool();
			Position = reader.ReadVector3();
			Velocity = reader.ReadVector3();
			OnChanged?.Invoke(isTeleport, Position, Velocity);
		}

		public void ClearDirty()
		{
			_isTeleported = false;
			IsDirty = false;
		}
	}
}

#nullable disable