using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class RedHoodMissionInteractor : Interactor
	{
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.Users;
	}
}