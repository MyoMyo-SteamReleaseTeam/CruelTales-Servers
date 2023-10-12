using CT.Common.Gameplay;
#if CT_SERVER
using CTS.Instance.SyncObjects;
using log4net;
#elif CT_CLIENT
using CTC.Networks.SyncObjects.SyncObjects;
using CT.Logger;
#endif

namespace CTS.Instance.ClientShared
{
	public static class CostumeSetExtension
	{
#if CT_SERVER

		public static void SetBy(this CostumeSet obj, SkinSet skinSet)
		{
			obj.Back = skinSet.Back;
			obj.Tail = skinSet.Tail;
			obj.Cheek = skinSet.Cheek;
			obj.Dress = skinSet.Dress;
			obj.Eyes = skinSet.Eyes;
			obj.Eyebrow = skinSet.Eyebrow;
			obj.FaceAcc = skinSet.FaceAcc;
			obj.Hair = skinSet.Hair;
			obj.HairAcc = skinSet.HairAcc;
			obj.HairHelmet = skinSet.HairHelmet;
			obj.Headgear = skinSet.Headgear;
			obj.Lips = skinSet.Lips;
			obj.Nose = skinSet.Nose;
			obj.Shoes = skinSet.Shoes;
			obj.Hammer = skinSet.Hammer;
		}

		public static void SetBy(this CostumeSet obj, CostumeSet costumeSet)
		{
			obj.Back = costumeSet.Back;
			obj.Tail = costumeSet.Tail;
			obj.Cheek = costumeSet.Cheek;
			obj.Dress = costumeSet.Dress;
			obj.Eyes = costumeSet.Eyes;
			obj.Eyebrow = costumeSet.Eyebrow;
			obj.FaceAcc = costumeSet.FaceAcc;
			obj.Hair = costumeSet.Hair;
			obj.HairAcc = costumeSet.HairAcc;
			obj.HairHelmet = costumeSet.HairHelmet;
			obj.Headgear = costumeSet.Headgear;
			obj.Lips = costumeSet.Lips;
			obj.Nose = costumeSet.Nose;
			obj.Shoes = costumeSet.Shoes;
			obj.Hammer = costumeSet.Hammer;
		}

#endif

		public static SkinSet GetSkinSet(this CostumeSet obj)
		{
			SkinSet skinSet = new SkinSet();
			skinSet.Back = obj.Back;
			skinSet.Tail = obj.Tail;
			skinSet.Cheek = obj.Cheek;
			skinSet.Dress = obj.Dress;
			skinSet.Eyes = obj.Eyes;
			skinSet.Eyebrow = obj.Eyebrow;
			skinSet.FaceAcc = obj.FaceAcc;
			skinSet.Hair = obj.Hair;
			skinSet.HairAcc = obj.HairAcc;
			skinSet.HairHelmet = obj.HairHelmet;
			skinSet.Headgear = obj.Headgear;
			skinSet.Lips = obj.Lips;
			skinSet.Nose = obj.Nose;
			skinSet.Shoes = obj.Shoes;
			skinSet.Hammer = obj.Hammer;
			return skinSet;
		}
	}
}
