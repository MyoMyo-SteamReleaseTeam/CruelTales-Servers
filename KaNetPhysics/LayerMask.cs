using System;
using System.Collections.Generic;

namespace KaNet.Physics
{
	[Serializable]
	public struct LayerMask
	{
		public PhysicsLayerMask Mask;
		public PhysicsLayerMask Flags;

		public LayerMask(PhysicsLayerMask type, PhysicsLayerMask flags)
		{
			Mask = type;
			Flags = flags;
		}
	}

	[Flags]
	public enum PhysicsLayerMask : ushort
	{
		None = 0,

		Environment		= 1,		// 1
		Invisible		= 1 << 1,	// 2 
		Player			= 1 << 2,	// 3 
		Item			= 1 << 3,	// 4
		LayerMask_5		= 1 << 4,	// 5
		LayerMask_6		= 1 << 5,	// 6
		LayerMask_7		= 1 << 6,	// 7
	}

	public static class LayerMaskHelper
	{
		private const int MASK_COUNT = 8;

		private static List<List<byte>> _layerMatrix = new()
		{
			new List<byte>() /* None			/ 0 */ { 1, 1, 1, 1, 1, 1, 1, 1 },

												//////   7  6  5  4  3  2  1
			new List<byte>() /* Environment		/ 1 */ { 0, 0, 0, 1, 1, 0, 0 },
			new List<byte>() /* Invisible		/ 2 */ { 0, 0, 0, 1, 1, 0 },
			new List<byte>() /* Player			/ 3 */ { 0, 0, 0, 0, 0 },
			new List<byte>() /* Item			/ 4 */ { 0, 0, 0, 1 },
			new List<byte>() /* LayerMask_5		/ 5 */ { 0, 0, 0 },
			new List<byte>() /* LayerMask_6		/ 6 */ { 0, 0 },
			new List<byte>() /* LayerMask_7		/ 7 */ { 0 },
		};

		private static List<PhysicsLayerMask> _layerMaskEnums = new();
		private static Dictionary<PhysicsLayerMask, PhysicsLayerMask> _flagsByLayerMask = new();

		public static void Initialize()
		{
			// Initialize layer mask
			Span<PhysicsLayerMask> layerMaskFlagArray = stackalloc PhysicsLayerMask[MASK_COUNT];

			foreach (PhysicsLayerMask mask in Enum.GetValues(typeof(PhysicsLayerMask)))
			{
				_layerMaskEnums.Add(mask);
				layerMaskFlagArray[0] |= mask;
			}

			for (int y = 0; y < MASK_COUNT; y++)
			{
				for (int x = 0; x < MASK_COUNT - y; x++)
				{
					if (_layerMatrix[y][x] != 0)
					{
						layerMaskFlagArray[y] |= _layerMaskEnums[(MASK_COUNT - x - 1)];
						layerMaskFlagArray[(MASK_COUNT - x - 1)] |= _layerMaskEnums[y];
					}
				}
			}

			for (int i = 0; i < MASK_COUNT; i++)
			{
				_flagsByLayerMask.Add(_layerMaskEnums[i], layerMaskFlagArray[i]);
			}
		}

		public static LayerMask GetLayerMask(PhysicsLayerMask layerMask)
		{
			return new LayerMask(layerMask, _flagsByLayerMask[layerMask]);
		}
	}
}
