using System;
using CT.Common.Gameplay;
using CTS.Instance.Gameplay;

namespace CTS.Instance.SyncObjects
{
	public partial class FieldItem : Interactor
	{
		private Action _onFreezingEnd;

		public override void Constructor()
		{
			base.Constructor();
			_onFreezingEnd = onFreezingEnd;
		}

		public void InitializeAs(FieldItemType itemType)
		{
			ItemType = itemType;
		}

		public override void OnCreated()
		{
			base.OnCreated();
			Interactable = false;
			StartCoroutine(_onFreezingEnd, 1.0f);
		}

		private void onFreezingEnd()
		{
			Interactable = true;
		}

		public override void OnInteracted(NetworkPlayer player, PlayerCharacter playerCharacter)
		{
			if (!IsAlive)
				return;

			if (playerCharacter.FieldItem != FieldItemType.None)
			{
				playerCharacter.DropItem();
			}

			playerCharacter.FieldItem = ItemType;
			Destroy();
		}
	}
}