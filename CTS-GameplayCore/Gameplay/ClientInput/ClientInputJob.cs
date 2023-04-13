using System.Runtime.InteropServices;
using CT.Network.DataType.Input;

namespace CTS.Instance.Gameplay.ClientInput
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ClientInputJob
	{
		[FieldOffset(0)] public InputType Type;
		[FieldOffset(4)] public InputMovementData InputMovement;
	}
}
