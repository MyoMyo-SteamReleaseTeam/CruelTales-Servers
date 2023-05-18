using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class PlayerCharacter : MasterNetworkObject
	{
		public override PartitionType Visibility => PartitionType.View;

		public override VisibilityAuthority Target => VisibilityAuthority.Faction;
	}
}
