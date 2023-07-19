using System.Numerics;
using System.Runtime.CompilerServices;

public static class VectorSwizzleExtension
{
	#region Vector2 to Vector2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _xx(this Vector2 v) => new Vector2(v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _yx(this Vector2 v) => new Vector2(v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _0x(this Vector2 v) => new Vector2(0, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _1x(this Vector2 v) => new Vector2(1, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _xy(this Vector2 v) => new Vector2(v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _yy(this Vector2 v) => new Vector2(v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _0y(this Vector2 v) => new Vector2(0, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _1y(this Vector2 v) => new Vector2(1, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _x0(this Vector2 v) => new Vector2(v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _y0(this Vector2 v) => new Vector2(v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _x1(this Vector2 v) => new Vector2(v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _y1(this Vector2 v) => new Vector2(v.Y, 1);
	#endregion

	#region Vector2 to Vector3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xxx(this Vector2 v) => new Vector3(v.X, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xxy(this Vector2 v) => new Vector3(v.X, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xx0(this Vector2 v) => new Vector3(v.X, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xx1(this Vector2 v) => new Vector3(v.X, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yxx(this Vector2 v) => new Vector3(v.Y, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yxy(this Vector2 v) => new Vector3(v.Y, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yx0(this Vector2 v) => new Vector3(v.Y, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yx1(this Vector2 v) => new Vector3(v.Y, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0xx(this Vector2 v) => new Vector3(0, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0xy(this Vector2 v) => new Vector3(0, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0x0(this Vector2 v) => new Vector3(0, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0x1(this Vector2 v) => new Vector3(0, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1xx(this Vector2 v) => new Vector3(1, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1xy(this Vector2 v) => new Vector3(1, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1x0(this Vector2 v) => new Vector3(1, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1x1(this Vector2 v) => new Vector3(1, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xyx(this Vector2 v) => new Vector3(v.X, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xyy(this Vector2 v) => new Vector3(v.X, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xy0(this Vector2 v) => new Vector3(v.X, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xy1(this Vector2 v) => new Vector3(v.X, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yyx(this Vector2 v) => new Vector3(v.Y, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yyy(this Vector2 v) => new Vector3(v.Y, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yy0(this Vector2 v) => new Vector3(v.Y, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yy1(this Vector2 v) => new Vector3(v.Y, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0yx(this Vector2 v) => new Vector3(0, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0yy(this Vector2 v) => new Vector3(0, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0y0(this Vector2 v) => new Vector3(0, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0y1(this Vector2 v) => new Vector3(0, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1yx(this Vector2 v) => new Vector3(1, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1yy(this Vector2 v) => new Vector3(1, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1y0(this Vector2 v) => new Vector3(1, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1y1(this Vector2 v) => new Vector3(1, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x0x(this Vector2 v) => new Vector3(v.X, 0, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x0y(this Vector2 v) => new Vector3(v.X, 0, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x00(this Vector2 v) => new Vector3(v.X, 0, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x01(this Vector2 v) => new Vector3(v.X, 0, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y0x(this Vector2 v) => new Vector3(v.Y, 0, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y0y(this Vector2 v) => new Vector3(v.Y, 0, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y00(this Vector2 v) => new Vector3(v.Y, 0, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y01(this Vector2 v) => new Vector3(v.Y, 0, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x1x(this Vector2 v) => new Vector3(v.X, 1, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x1y(this Vector2 v) => new Vector3(v.X, 1, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x10(this Vector2 v) => new Vector3(v.X, 1, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x11(this Vector2 v) => new Vector3(v.X, 1, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y1x(this Vector2 v) => new Vector3(v.Y, 1, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y1y(this Vector2 v) => new Vector3(v.Y, 1, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y10(this Vector2 v) => new Vector3(v.Y, 1, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y11(this Vector2 v) => new Vector3(v.Y, 1, 1);
	#endregion

	#region Vector3 to Vector2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _xx(this Vector3 v) => new Vector2(v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _yx(this Vector3 v) => new Vector2(v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _zx(this Vector3 v) => new Vector2(v.Z, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _0x(this Vector3 v) => new Vector2(0, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _1x(this Vector3 v) => new Vector2(1, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _xy(this Vector3 v) => new Vector2(v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _yy(this Vector3 v) => new Vector2(v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _zy(this Vector3 v) => new Vector2(v.Z, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _0y(this Vector3 v) => new Vector2(0, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _1y(this Vector3 v) => new Vector2(1, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _xz(this Vector3 v) => new Vector2(v.X, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _yz(this Vector3 v) => new Vector2(v.Y, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _zz(this Vector3 v) => new Vector2(v.Z, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _0z(this Vector3 v) => new Vector2(0, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _1z(this Vector3 v) => new Vector2(1, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _x0(this Vector3 v) => new Vector2(v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _y0(this Vector3 v) => new Vector2(v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _z0(this Vector3 v) => new Vector2(v.Z, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _x1(this Vector3 v) => new Vector2(v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _y1(this Vector3 v) => new Vector2(v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 _z1(this Vector3 v) => new Vector2(v.Z, 1);
	#endregion

	#region Vector3 to Vector3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xxx(this Vector3 v) => new Vector3(v.X, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xxy(this Vector3 v) => new Vector3(v.X, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xxz(this Vector3 v) => new Vector3(v.X, v.X, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xx0(this Vector3 v) => new Vector3(v.X, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xx1(this Vector3 v) => new Vector3(v.X, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yxx(this Vector3 v) => new Vector3(v.Y, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yxy(this Vector3 v) => new Vector3(v.Y, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yxz(this Vector3 v) => new Vector3(v.Y, v.X, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yx0(this Vector3 v) => new Vector3(v.Y, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yx1(this Vector3 v) => new Vector3(v.Y, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zxx(this Vector3 v) => new Vector3(v.Z, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zxy(this Vector3 v) => new Vector3(v.Z, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zxz(this Vector3 v) => new Vector3(v.Z, v.X, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zx0(this Vector3 v) => new Vector3(v.Z, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zx1(this Vector3 v) => new Vector3(v.Z, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0xx(this Vector3 v) => new Vector3(0, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0xy(this Vector3 v) => new Vector3(0, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0xz(this Vector3 v) => new Vector3(0, v.X, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0x0(this Vector3 v) => new Vector3(0, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0x1(this Vector3 v) => new Vector3(0, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1xx(this Vector3 v) => new Vector3(1, v.X, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1xy(this Vector3 v) => new Vector3(1, v.X, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1xz(this Vector3 v) => new Vector3(1, v.X, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1x0(this Vector3 v) => new Vector3(1, v.X, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1x1(this Vector3 v) => new Vector3(1, v.X, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xyx(this Vector3 v) => new Vector3(v.X, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xyy(this Vector3 v) => new Vector3(v.X, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xyz(this Vector3 v) => new Vector3(v.X, v.Y, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xy0(this Vector3 v) => new Vector3(v.X, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xy1(this Vector3 v) => new Vector3(v.X, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yyx(this Vector3 v) => new Vector3(v.Y, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yyy(this Vector3 v) => new Vector3(v.Y, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yyz(this Vector3 v) => new Vector3(v.Y, v.Y, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yy0(this Vector3 v) => new Vector3(v.Y, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yy1(this Vector3 v) => new Vector3(v.Y, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zyx(this Vector3 v) => new Vector3(v.Z, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zyy(this Vector3 v) => new Vector3(v.Z, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zyz(this Vector3 v) => new Vector3(v.Z, v.Y, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zy0(this Vector3 v) => new Vector3(v.Z, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zy1(this Vector3 v) => new Vector3(v.Z, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0yx(this Vector3 v) => new Vector3(0, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0yy(this Vector3 v) => new Vector3(0, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0yz(this Vector3 v) => new Vector3(0, v.Y, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0y0(this Vector3 v) => new Vector3(0, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0y1(this Vector3 v) => new Vector3(0, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1yx(this Vector3 v) => new Vector3(1, v.Y, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1yy(this Vector3 v) => new Vector3(1, v.Y, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1yz(this Vector3 v) => new Vector3(1, v.Y, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1y0(this Vector3 v) => new Vector3(1, v.Y, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1y1(this Vector3 v) => new Vector3(1, v.Y, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xzx(this Vector3 v) => new Vector3(v.X, v.Z, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xzy(this Vector3 v) => new Vector3(v.X, v.Z, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xzz(this Vector3 v) => new Vector3(v.X, v.Z, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xz0(this Vector3 v) => new Vector3(v.X, v.Z, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _xz1(this Vector3 v) => new Vector3(v.X, v.Z, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yzx(this Vector3 v) => new Vector3(v.Y, v.Z, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yzy(this Vector3 v) => new Vector3(v.Y, v.Z, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yzz(this Vector3 v) => new Vector3(v.Y, v.Z, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yz0(this Vector3 v) => new Vector3(v.Y, v.Z, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _yz1(this Vector3 v) => new Vector3(v.Y, v.Z, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zzx(this Vector3 v) => new Vector3(v.Z, v.Z, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zzy(this Vector3 v) => new Vector3(v.Z, v.Z, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zzz(this Vector3 v) => new Vector3(v.Z, v.Z, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zz0(this Vector3 v) => new Vector3(v.Z, v.Z, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _zz1(this Vector3 v) => new Vector3(v.Z, v.Z, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0zx(this Vector3 v) => new Vector3(0, v.Z, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0zy(this Vector3 v) => new Vector3(0, v.Z, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0zz(this Vector3 v) => new Vector3(0, v.Z, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0z0(this Vector3 v) => new Vector3(0, v.Z, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _0z1(this Vector3 v) => new Vector3(0, v.Z, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1zx(this Vector3 v) => new Vector3(1, v.Z, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1zy(this Vector3 v) => new Vector3(1, v.Z, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1zz(this Vector3 v) => new Vector3(1, v.Z, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1z0(this Vector3 v) => new Vector3(1, v.Z, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _1z1(this Vector3 v) => new Vector3(1, v.Z, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x0x(this Vector3 v) => new Vector3(v.X, 0, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x0y(this Vector3 v) => new Vector3(v.X, 0, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x0z(this Vector3 v) => new Vector3(v.X, 0, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x00(this Vector3 v) => new Vector3(v.X, 0, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x01(this Vector3 v) => new Vector3(v.X, 0, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y0x(this Vector3 v) => new Vector3(v.Y, 0, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y0y(this Vector3 v) => new Vector3(v.Y, 0, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y0z(this Vector3 v) => new Vector3(v.Y, 0, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y00(this Vector3 v) => new Vector3(v.Y, 0, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y01(this Vector3 v) => new Vector3(v.Y, 0, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z0x(this Vector3 v) => new Vector3(v.Z, 0, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z0y(this Vector3 v) => new Vector3(v.Z, 0, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z0z(this Vector3 v) => new Vector3(v.Z, 0, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z00(this Vector3 v) => new Vector3(v.Z, 0, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z01(this Vector3 v) => new Vector3(v.Z, 0, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x1x(this Vector3 v) => new Vector3(v.X, 1, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x1y(this Vector3 v) => new Vector3(v.X, 1, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x1z(this Vector3 v) => new Vector3(v.X, 1, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x10(this Vector3 v) => new Vector3(v.X, 1, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _x11(this Vector3 v) => new Vector3(v.X, 1, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y1x(this Vector3 v) => new Vector3(v.Y, 1, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y1y(this Vector3 v) => new Vector3(v.Y, 1, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y1z(this Vector3 v) => new Vector3(v.Y, 1, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y10(this Vector3 v) => new Vector3(v.Y, 1, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _y11(this Vector3 v) => new Vector3(v.Y, 1, 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z1x(this Vector3 v) => new Vector3(v.Z, 1, v.X);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z1y(this Vector3 v) => new Vector3(v.Z, 1, v.Y);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z1z(this Vector3 v) => new Vector3(v.Z, 1, v.Z);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z10(this Vector3 v) => new Vector3(v.Z, 1, 0);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 _z11(this Vector3 v) => new Vector3(v.Z, 1, 1);
	#endregion
}