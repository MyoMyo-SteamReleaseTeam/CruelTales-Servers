using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class RedHoodMissionInfo : MasterNetworkObject
	{
		public override VisibilityAuthority InitialVisibilityAuthority => VisibilityAuthority.Owner;
		public override VisibilityType Visibility => VisibilityType.Global;
	}
}