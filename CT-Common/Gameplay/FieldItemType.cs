using CT.Common.Tools;

namespace CT.Common.Gameplay
{
	public enum FieldItemBaseType
	{
		None = 0,
		RedHood,
		Dueoksini,
	}

	public enum FieldItemType : uint
	{
		None = 0,

		RedHood,

		Dueoksini,
		Dueoksini_Rice,
		Dueoksini_Kimchi,
		Dueoksini_TaroSoup,
		Dueoksini_Japchae,
		Dueoksini_Jeon,
		Dueoksini_Yukjeon,
		Dueoksini_RawMeat,
		Dueoksini_SteamedSeaBream,
		Dueoksini_Galbijjim,
		Dueoksini_Gujeolpan,
		Dueoksini_Sinseonro,
	}

	public static class FieldItemTypeExtension
	{
		private static readonly PartialEnumTable _enumTable = new()
		{
			{ (int)FieldItemType.None, (int)FieldItemBaseType.None },
			{ (int)FieldItemType.RedHood, (int)FieldItemBaseType.RedHood },
			{ (int)FieldItemType.Dueoksini, (int)FieldItemBaseType.Dueoksini },
		};

		public static FieldItemBaseType GetBaseType(this FieldItemType value)
		{
			return (FieldItemBaseType)_enumTable.GetBaseTypeIndex((int)value);
		}

		public static bool IsBaseType(this FieldItemType value, FieldItemBaseType baseType)
		{
			return _enumTable.IsMatch((int)value, (int)baseType);
		}
	}
}
