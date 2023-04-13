using System.Runtime.InteropServices;
using CT.Network.DataType.Input;

namespace CT.Network.Runtimes
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ClientInputJob
	{
		[FieldOffset(0)] InputType Type;
		[FieldOffset(4)] InputMovementData InputMovement;
	}
}
