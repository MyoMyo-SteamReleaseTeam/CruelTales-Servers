using CT.Common.Synchronizations;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	public class DirtyableMockup : IDirtyable
	{
		public bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;

		public bool _isDirtyUnreliable;
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
