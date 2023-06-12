using System;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public partial class ZTest_FuntionObject : IMasterSynchronizable
	{

	}

	public partial class ZTest_InnerObject : IMasterSynchronizable
	{

	}

	public partial class ZTest_Value8 : MasterNetworkObject
	{
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority VisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value16 : MasterNetworkObject
	{
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority VisibilityAuthority => throw new NotImplementedException();
	}

	public partial class ZTest_Value32 : MasterNetworkObject
	{
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority VisibilityAuthority => throw new NotImplementedException();
	}
}