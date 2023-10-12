#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using CT.Common;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncObjectDefinition]
	public class CostumeSet
	{
		[SyncVar] public int Back;
		[SyncVar] public int Tail;
		[SyncVar] public int Cheek;
		[SyncVar] public int Dress;
		[SyncVar] public int Eyes;
		[SyncVar] public int Eyebrow;
		[SyncVar] public int FaceAcc;
		[SyncVar] public int Hair;
		[SyncVar] public int HairAcc;
		[SyncVar] public int HairHelmet;
		[SyncVar] public int Headgear;
		[SyncVar] public int Lips;
		[SyncVar] public int Nose;
		[SyncVar] public int Shoes;
		[SyncVar] public int Hammer;
		[SyncVar] public NetColor SkinColor;
		[SyncVar] public NetColor HairColor;
		[SyncVar] public NetColor EyesColor;
	}
}
#pragma warning restore IDE0051