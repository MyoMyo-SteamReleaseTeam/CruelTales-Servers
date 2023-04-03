using System;
using System.Collections.Generic;
using CT.Network.Core;
using CT.Network.DataType;
using CT.Network.Serialization;
using CT.Tools.Collections;

namespace CTS.Instance.Gameplay
{
	public class Stat
	{
		public int Hp;
		public float Speed;
	}

	public class Enemy
	{
		public bool IsAlive;
		public float X;
		public float Y;

		public float ValocityX = 0;
		public float ValocityY = 0;

		public float AliveTime = 0;
		public event Action? OnDestroy;
		public byte[] Allocation = new byte[100];

		private Stat _stat = new Stat();

		public void Initialize(float x, float y, float aliveTime = 1.0f)
		{
			IsAlive = true;
			X = x;
			Y = y;

			AliveTime = aliveTime;

			_stat.Hp = 100;
			_stat.Speed = 3.0f;
		}

		public void Move(float valx, float valy)
		{
			ValocityX = valx * _stat.Speed;
			ValocityY = valy * _stat.Speed;
		}

		public void Update(float deltaTime)
		{
			if (!IsAlive)
				return;

			X += ValocityX;
			Y += ValocityY;

			AliveTime -= deltaTime;
			if (AliveTime < 0)
			{
				IsAlive = false;
				OnDestroy?.Invoke();
			}
		}
	}

	public class GameInstance
	{
		private static int IdCounter = 1;
		public int Id { get; private set; }

		private BidirectionalMap<ClientToken, NetSession> _session;

		private List<Enemy> _enemies = new();
		private int MaxEnemySize = 100;

		private int _updateStress;
		private readonly int _userCount;
		private readonly int _receiveSize;
		private readonly int _sendSize;

		private readonly byte[] _receiveBuffer;
		private readonly byte[] _sendBuffer;

		private readonly Random _random = new Random();
		private int _randomStressFactor = 70;

		public GameInstance(ServerOption serverOption)
		{
			Id = IdCounter++;

			_userCount = serverOption.UserCount;

			_updateStress = serverOption.UpdateStress * _userCount;

			_receiveSize = serverOption.ReadSize * _userCount;
			_sendSize = serverOption.WriteSize * _userCount;

			_receiveBuffer = new byte[_receiveSize];
			_sendBuffer = new byte[_sendSize];

			// Create enemies
			for (int i = 0; i < MaxEnemySize; i++)
			{
				Enemy e = new Enemy();
				e.Initialize(10, 10, aliveTime: (float)(_random.NextDouble() * 10));
				_enemies.Add(e);
			}
		}

		public void ReadPackets()
		{
			byte value = 0;
			for (int u = 0; u < _userCount; u++)
			{
				for (int i = 0; i < _receiveSize; i++)
				{
					value += _receiveBuffer[i];
				}
			}
		}

		public void Update(float delta)
		{
			float randomRatio = _random.Next(_randomStressFactor, 100) / 100.0f;
			int stressCount = (int)(_updateStress * randomRatio);

			// Update enemies
			foreach (var e in _enemies)
			{
				if (!e.IsAlive)
				{
					if (_random.Next(10) == 1)
					{
						e.Initialize(0, 0, (float)(_random.NextDouble() * 5));
					}
					else
					{
						continue;
					}
				}

				e.Update(delta);
			}

			// User stress
			int a = 0;
			int multiply = 1;
			for (int u = 0; u < _userCount; u++)
			{
				for (int i = 0; i < stressCount; i++)
				{
					a++;
					multiply *= a;
				}
			}
		}

		public void WritePackets()
		{
			for (int u = 0; u < _userCount; u++)
			{
				for (int i = 0; i < _sendSize; i++)
				{
					_sendBuffer[i] = (byte)(i % 256);
				}
			}
		}

		public void OnPacketRecevied(ClientToken token, PacketReader reader)
		{

		}
	}
}
