using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using CT.Common.Serialization;
using Newtonsoft.Json;

namespace CT.Common.Gameplay
{
	public struct InteractorSize : IPacketSerializable, IEquatable<InteractorSize>
	{
		public InteractorColliderShapeType ShapeType;
		public float Value0;
		public float Value1;

		[JsonIgnore]
		public float Width
		{
			get => Value0;
			set => Value0 = value;
		}

		[JsonIgnore]
		public float Height
		{
			get => Value1;
			set => Value1 = value;
		}

		[JsonIgnore]
		public Vector2 Size
		{
			get => new Vector2(Width, Height);
			set
			{
				Width = value.X;
				Height = value.Y;
			}
		}

		[JsonIgnore]
		public float RadiusInner
		{
			get => Value0;
			set => Value0 = value;
		}

		[JsonIgnore]
		public float RadiusOuter
		{
			get => Value1;
			set => Value1 = value;
		}

		[JsonIgnore]
		public float Radius
		{
			get => Value0;
			set => Value0 = value;
		}

		[JsonIgnore]
		public int SerializeSize => getSerializeSizeBy(ShapeType);

		public static bool operator ==(InteractorSize lhs, InteractorSize rhs)
		{
			return lhs.Width == rhs.Width && lhs.Height == rhs.Height;
		}

		public static bool operator !=(InteractorSize lhs, InteractorSize rhs)
		{
			return lhs.Width != rhs.Width || lhs.Height != rhs.Height;
		}

		public override bool Equals(object? obj)
		{
			return obj is not InteractorSize other ? false : this == other;
		}

		public bool Equals(InteractorSize other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return (Width, Height).GetHashCode();
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(ShapeType);
			switch (ShapeType)
			{
				case InteractorColliderShapeType.Box:
				case InteractorColliderShapeType.Donut:
					writer.Put(Value0);
					writer.Put(Value1);
					break;

				case InteractorColliderShapeType.Circle:
					writer.Put(Value0);
					break;

				case InteractorColliderShapeType.None:
				default:
					break;
			}
		}

		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.TryReadInteractorColliderShapeType(out ShapeType)) return false;
			switch (ShapeType)
			{
				case InteractorColliderShapeType.Box:
				case InteractorColliderShapeType.Donut:
					if (!reader.TryReadSingle(out Value0)) return false;
					if (!reader.TryReadSingle(out Value1)) return false;
					return true;

				case InteractorColliderShapeType.Circle:
					if (!reader.TryReadSingle(out Value0)) return false;
					return true;

				case InteractorColliderShapeType.None:
				default:
					return true;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int getSerializeSizeBy(InteractorColliderShapeType shapeType)
		{
			switch (shapeType)
			{
				case InteractorColliderShapeType.Box:
				case InteractorColliderShapeType.Donut:
					return sizeof(float) * 2 + sizeof(InteractorColliderShapeType);

				case InteractorColliderShapeType.Circle:
					return sizeof(float) + sizeof(InteractorColliderShapeType);

				case InteractorColliderShapeType.None:
				default:
					return sizeof(InteractorColliderShapeType);
			}
		}

		public void Ignore(IPacketReader reader)
		{
			IgnoreStatic(reader);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IgnoreStatic(IPacketReader reader)
		{
			if (!reader.TryReadInteractorColliderShapeType(out var shapeType)) return;
			reader.Ignore(getSerializeSizeBy(shapeType));
		}
	}
}
