using System;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class ZTest_Parent : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public partial void Client_P1(NetworkPlayer player)
		{
		}

		protected virtual partial void Client_p2(NetworkPlayer player, int a, int b)
		{
		}
	}

	public partial class ZTest_Child : ZTest_Parent
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public partial void Client_C3(NetworkPlayer player)
		{
		}

		private partial void Client_c4(NetworkPlayer player)
		{ 
		}
	}

	public partial class ZTest_ChildChild : ZTest_Child
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public partial void Client_CC5(NetworkPlayer player)
		{
		}

		private partial void Client_cc6(NetworkPlayer player)
		{
		}
	}

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