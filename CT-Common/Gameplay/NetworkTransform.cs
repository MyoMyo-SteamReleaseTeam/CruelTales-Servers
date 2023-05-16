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
		public Vector3 Position { get; private set; }
		public Vector3 Velocity { get; private set; }

		// Check dirty
		private Vector3 _previousPosition;
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
			_previousPosition = Position;
			Position += Velocity * deltaTime;
			IsDirty = Position != _previousPosition;
		}

		public void SetPosition(Vector3 position)
		{
			Position = position;
			IsDirty = true;
			_isTeleported = true;
		}

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