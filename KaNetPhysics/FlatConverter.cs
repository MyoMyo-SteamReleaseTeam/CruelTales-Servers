using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlatPhysics
{
	public static class FlatConverter
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ToVector2(FlatVector v)
		{
			return new Vector2(v.X, v.Y);
		}

	}
}
