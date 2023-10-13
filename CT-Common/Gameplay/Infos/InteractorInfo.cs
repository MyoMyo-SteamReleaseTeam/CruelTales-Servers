using System;
using System.Numerics;
using CT.Common.Gameplay.RedHood;
using Newtonsoft.Json;

namespace CT.Common.Gameplay.Infos
{
	[Serializable]
	public struct InteractorInfo
	{
		// Properties
		public InteractorType InteractorType;
		public InteractionBehaviourType BehaviourType;
		public Vector2 Position;
		public InteractorSize Size;
		public float ProgressTime;
		public float Cooltime;

		// Addition Variables
		public int IntValue;
		public byte ByteValue;
		public Vector2 VectorValue;

		#region Mission

		[JsonIgnore]
		public RedHoodMission RedHoodMission
		{
			get => (RedHoodMission)ByteValue;
			set => ByteValue = (byte)value;
		}

		#endregion

		#region Teleporter

		/// <summary>텔레포트로 이동한 뒤의 Section입니다.</summary>
		[JsonIgnore]
		public byte SectionTo
		{
			get => (byte)IntValue;
			set => IntValue = value;
		}

		[JsonIgnore]
		public TeleporterShapeType TeleporterShape
		{
			get => (TeleporterShapeType)ByteValue;
			set => ByteValue = (byte)value;
		}

		[JsonIgnore]
		public Vector2 Destination
		{
			get => VectorValue;
			set => VectorValue = value;
		}

		#endregion
	}
}
