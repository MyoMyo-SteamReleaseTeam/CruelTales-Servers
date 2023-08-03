using System.Diagnostics.CodeAnalysis;
using KaNet.Physics;

namespace PhysicsTester.KaNetPhysics
{
	internal class KaEntityManager
	{
		public Dictionary<int, KaEntity>.ValueCollection Entities => _entityById.Values;

		// Entities
		private Dictionary<int, KaEntity> _entityById = new();
		private List<KaEntity> _entityRemovalList = new();
		private Stack<int> _idStack = new();
		private int _idCounter = 0;

		private int getNewId()
		{
			return _idStack.Count > 0 ? _idStack.Pop() : ++_idCounter;
		}

		public void Update()
		{
			if (_entityRemovalList.Count > 0)
			{
				for (int i = 0; i < _entityRemovalList.Count; ++i)
				{
					KaEntity entity = _entityRemovalList[i];
					_idStack.Push(entity.ID);
					_entityById.Remove(entity.ID);
					entity.World.RemoveRigidBody(entity.Body);
				}

				_entityRemovalList.Clear();
			}
		}

		public void AddEntity(KaEntity entity)
		{
			int newId = getNewId();
			entity.SetID(newId);
			_entityById.Add(entity.ID, entity);
		}

		public void RemoveEntity(KaEntity entity)
		{
			_entityRemovalList.Add(entity);
		}

		public bool TryGetEntity(int id, [MaybeNullWhen(false)] out KaEntity entity)
		{
			return _entityById.TryGetValue(id, out entity);
		}

		public void Clear()
		{
			foreach (KaEntity entity in this.Entities)
			{
				entity.World.RemoveRigidBody(entity.Body);
			}

			_idCounter = 0;
			_idStack.Clear();
			_entityById.Clear();
		}
	}
}
