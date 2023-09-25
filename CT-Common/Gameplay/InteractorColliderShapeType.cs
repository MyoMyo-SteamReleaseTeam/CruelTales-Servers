using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public enum InteractorColliderShapeType : byte
	{
		None = 0,
		Box,
		Circle,
		Donut,
	}

	public static class InteractorColliderShapeTypeExtension
	{
		public static void Put(this IPacketWriter writer, InteractorColliderShapeType value)
		{
			writer.Put((byte)value);
		}

		public static bool TryReadInteractorColliderShapeType(this IPacketReader reader,
															  out InteractorColliderShapeType value)
		{
			if (!reader.TryReadByte(out byte readValue))
			{
				value = 0;
				return false;
			}

			value = (InteractorColliderShapeType)readValue;
			return true;
		}
	}
}
