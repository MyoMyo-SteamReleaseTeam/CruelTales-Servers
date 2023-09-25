using CT.Common.Gameplay.Infos;

namespace CT.Common.Gameplay
{
	public static class InteractorConst
	{
		public static InteractorInfo TableInteractorInfo = new()
		{
			InteractorType = InteractorType.Mission,
			BehaviourType = InteractionBehaviourType.Tigger,
			Size = new InteractorSize()
			{
				ShapeType = InteractorColliderShapeType.Circle,
				Radius = 4.0f
			},
			Cooltime = 0.5f,
		};

		public static InteractorInfo FieldItemInteractorInfo = new()
		{
			InteractorType = InteractorType.FieldItem,
			BehaviourType = InteractionBehaviourType.Tigger,
			Size = new InteractorSize()
			{
				ShapeType = InteractorColliderShapeType.Circle,
				Radius = 2.0f
			},
		};
	}
}
