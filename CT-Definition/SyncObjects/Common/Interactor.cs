#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Synchronizations;

namespace CT.Definitions.SyncObjects
{
	[SyncNetworkObjectDefinition(capacity: 16)]
	public class Interactor
	{
		[SyncVar]
		public InteractionBehaviourType BehaviourType;

		[SyncVar]
		public InteractorSize Size;

		[SyncVar]
		public float ProgressTime;

		[SyncVar]
		public float Cooltime;

		[SyncVar]
		public bool Interactable;

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_TryInteract() { }

		[SyncRpc(dir: SyncDirection.FromRemote)]
		public void Client_TryCancel() { }

		[SyncRpc]
		public void Server_InteractResult(InteractResultType result) { }
	}

	[SyncNetworkObjectDefinition(capacity: 16)]
	public class Teleporter : Interactor
	{
		[SyncVar]
		public TeleporterShapeType TeleporterShape;
	}

	[SyncNetworkObjectDefinition(capacity: 64)]
	public class FieldItem : Interactor
	{
		[SyncVar]
		public FieldItemType ItemType;
	}
}
#pragma warning restore IDE0051