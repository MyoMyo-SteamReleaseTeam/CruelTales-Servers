namespace CT.Definitions
{
	public static class Constant
	{
		public const string PLAYER_COUNT = "8";

		/*
		 * 동기화 객체 Collection의 최대 Capcacity를 설정합니다.
		 * Collection은 최대 Capacity를 넘을 수 없습니다.
		 */
		public const string MAX_CAPACITY_BY_PLAYER = "maxCapacity: 8";
		public const string MAX_CAPACITY_BY_PLAYER_MULTIPLE = "maxCapacity: 16";

		/*
		 * 동기화 Collection의 Capcacity를 설정합니다.
		 * Collection의 Item 개수가 Capacity를 넘는 경우 GC가 발생될 수 있습니다.
		 */
		public const string CAPACITY_BY_PLAYER = "capacity: 8";
		public const string CAPACITY_BY_PLAYER_MULTIPLE = "capacity: 16";
	}
}
