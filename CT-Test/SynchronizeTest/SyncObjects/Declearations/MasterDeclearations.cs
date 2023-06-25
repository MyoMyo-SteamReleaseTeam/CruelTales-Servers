using System;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class ZTest_FuntionObject : IMasterSynchronizable
	{

	}

	public partial class ZTest_InnerObject : IMasterSynchronizable
	{

	}

	public partial class ZTest_SyncCollection : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public SyncList<UserId> UserIdList => _userIdList;
	}

	public partial class ZTest_Value8 : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public void Call_uf5(NetworkPlayer player)
		{
			uf5(player);
		}
	}

	public partial class ZTest_Value16 : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value32 : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public void CallF28(int value) => f28(value);
	}
}