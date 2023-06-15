using System;
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

	public partial class ZTest_Value8 : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority VisibilityAuthority => throw new NotImplementedException();

		public void Call_uf5(NetworkPlayer player)
		{
			uf5(player);
		}
	}

	public partial class ZTest_Value16 : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority VisibilityAuthority => throw new NotImplementedException();

		public void SetV13(short value)
		{
			V13 = value;
		}
	}

	public partial class ZTest_Value32 : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority VisibilityAuthority => throw new NotImplementedException();

		public void CallF28(int value) => f28(value);
	}
}