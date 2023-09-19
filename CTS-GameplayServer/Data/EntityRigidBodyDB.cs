using System;
using System.Collections.Generic;
using CTS.Instance.SyncObjects;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;

namespace CTS.Instance.Data
{
	public static class EntityRigidBodyDB
	{
		private static Dictionary<NetworkObjectType, Func<KaRigidBody>> _rigidBodyByType = new()
		{
			{ NetworkObjectType.PlayerCharacter, () => new CircleRigidBody(0.25f, false, PhysicsLayerMask.Player) },
			{ NetworkObjectType.WolfCharacter, () => new CircleRigidBody(0.25f, false, PhysicsLayerMask.Player) },
			{ NetworkObjectType.RedHoodCharacter, () => new CircleRigidBody(0.25f, false, PhysicsLayerMask.Player) },
			{ NetworkObjectType.NormalCharacter, () => new CircleRigidBody(0.25f, false, PhysicsLayerMask.Player) },
			{ NetworkObjectType.TestCube, () => new CircleRigidBody(0.5f, false, PhysicsLayerMask.Player) },
		};

		public static KaRigidBody CreateRigidBodyBy(NetworkObjectType objectType)
		{
			if (_rigidBodyByType.TryGetValue(objectType, out var rigidBodyFunc))
				return rigidBodyFunc();

			return new BoxAABBRigidBody(1, 1, true, PhysicsLayerMask.System);
		}
	}
}
