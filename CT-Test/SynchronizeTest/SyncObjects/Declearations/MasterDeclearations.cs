using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Common.DataType.Synchronizations;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Synchronizations;

namespace CTS.Instance.SyncObjects
{
	public static class TestHelper
	{
		public static void AddOneTo(NetworkPlayer player, Dictionary<NetworkPlayer, int> table)
		{
			if (!table.ContainsKey(player))
			{
				table.Add(player, 1);
				return;
			}

			table[player]++;
		}
	}

	public partial class ZTest_Parent : MasterNetworkObject
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public Dictionary<NetworkPlayer, int> Client_P1_CountTable = new();
		public Dictionary<NetworkPlayer, (int a, int b)> Client_P2_Table = new();

		public float Field_Server_P2_Public
		{
			get => Field_Server_P2;
			set => Field_Server_P2 = value;
		}

		public float Field_Client_P2_Public => _field_Client_P2;

		public partial void Client_P1(NetworkPlayer player)
		{
			TestHelper.AddOneTo(player, Client_P1_CountTable);
		}

		protected virtual partial void Client_p2(NetworkPlayer player, int a, int b)
		{
			if (!Client_P2_Table.ContainsKey(player))
			{
				Client_P2_Table.Add(player, (a, b));
				return;
			}

			var v = Client_P2_Table[player];
			v.a += a;
			v.b += b;
			Client_P2_Table[player] = v;
		}

		public void Server_P2_Public(NetworkPlayer player, int a, int b) => this.Server_p2(player, a, b);
	}

	public partial class ZTest_Child : ZTest_Parent
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public Dictionary<NetworkPlayer, int> Client_C3_CountTable = new();
		public Dictionary<NetworkPlayer, int> Client_C4_CountTable = new();

		public int Field_Server_C4_Public
		{
			get => Field_Server_C4;
			set => Field_Server_C4 = value;
		}

		public float Field_Client_C4_Public => _field_Client_C4;

		public partial void Client_C3(NetworkPlayer player)
		{
			TestHelper.AddOneTo(player, Client_C3_CountTable);
		}

		protected partial void Client_c4(NetworkPlayer player)
		{
			TestHelper.AddOneTo(player, Client_C4_CountTable);
		}

		public void Server_C4_Public(NetworkPlayer player) => this.Server_c4(player);
	}

	public partial class ZTest_ChildChild : ZTest_Child
	{
		public override NetworkObjectType Type => throw new NotImplementedException();
		public override VisibilityType Visibility => throw new NotImplementedException();
		public override VisibilityAuthority InitialVisibilityAuthority => throw new NotImplementedException();

		public Dictionary<NetworkPlayer, int> Client_CC5_CountTable = new();
		public Dictionary<NetworkPlayer, int> Client_CC6_CountTable = new();

		public int Field_Server_CC6_Public
		{
			get => Field_Server_CC6;
			set => Field_Server_CC6 = value;
		}

		public float Field_Client_CC6_Public => _field_Client_CC6;

		public partial void Client_CC5(NetworkPlayer player)
		{
			TestHelper.AddOneTo(player, Client_CC5_CountTable);
		}

		protected partial void Client_cc6(NetworkPlayer player)
		{
			TestHelper.AddOneTo(player, Client_CC6_CountTable);
		}

		public void Server_CC6_Public(NetworkPlayer player) => this.Server_cc6(player);
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