using CT.Common.Synchronizations;

namespace CT.Test.Gameplays
{
	public class MokupDirtyable : IDirtyable
	{
		private bool _isDirtyReliable;
		private bool _isDirtyUnreliable;

		public bool IsDirtyReliable => _isDirtyReliable;

		public bool IsDirtyUnreliable => _isDirtyUnreliable;

		public void BindOwner(IDirtyable owner)
		{
		}

		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
		}

		public void ClearDirtyUnreliable()
		{
			_isDirtyUnreliable = false;
		}

		public void MarkDirtyReliable()
		{
			_isDirtyReliable = true;
		}

		public void MarkDirtyUnreliable()
		{
			_isDirtyUnreliable = true;
		}
	}
}
