namespace CT.Common.Synchronizations
{
	public interface IDirtyable
	{
		/// <summary>
		/// 소유자 객체를 바인딩합니다.
		/// </summary>
		public void BindOwner(IDirtyable owner);

		/// <summary>
		/// 동기화가 필요한 신뢰성 요소가 존재하는지 여부입니다.
		/// 일반적으로 내부 프로퍼티나 원격 함수가 호출되었을 때 true로 전환됩니다.
		/// </summary>
		public bool IsDirtyReliable { get; }

		/// <summary>
		/// 동기화가 필요한 비신뢰성 요소가 존재하는지 여부입니다.
		/// 일반적으로 내부 프로퍼티나 원격 함수가 호출되었을 때 true로 전환됩니다.
		/// </summary>
		public bool IsDirtyUnreliable { get; }

		/// <summary>
		/// 신뢰성 데이터 변경이 발생했을 때 호출합니다.
		/// </summary>
		public void MarkDirtyReliable();

		/// <summary>
		/// 비신뢰성 데이터 변경이 발생했을 때 호출합니다.
		/// </summary>
		public void MarkDirtyUnreliable();

		/// <summary>
		/// 신뢰성 Dirty Bits를 초기화합니다.
		/// </summary>
		public void ClearDirtyReliable();

		/// <summary>
		/// 비신뢰성 Dirty Bits를 초기화합니다.
		/// </summary>
		public void ClearDirtyUnreliable();
	}
}
