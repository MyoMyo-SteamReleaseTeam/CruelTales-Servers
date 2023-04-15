using System.Runtime.InteropServices;
using CT.Common.DataType.Input;

namespace CTS.Instance.Gameplay
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ClientInputJob
	{
		[FieldOffset(0)] public InputType Type;
		[FieldOffset(4)] public InputMovementData InputMovement;
	}
}
