using System;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class ZTest_InnerObject : IMasterSynchronizable
	{

	}

	public partial class ZTest_FunctionDirection : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public partial void Client_FromClientVoid(NetworkPlayer player)
		{
		}

		public partial void Client_FromServerArg(NetworkPlayer player, int a, int b)
		{
		}
	}

	public partial class ZTest_SyncCollection : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public SyncList<UserId> UserIdList => _userIdList;
	}

	public partial class ZTest_Value8Target : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value8NonTarget : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value16NonTarget : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value16Target : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value32Target : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value32NonTarget : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();
	}
}