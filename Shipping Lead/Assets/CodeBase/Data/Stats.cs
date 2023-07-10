using System;

namespace CodeBase.Data
{
	[Serializable]
	public class Stats
	{
		public float MaxMovementSpeed;
		public float MovementAccelaration;
		public float MovementDeacclaration;
		public float AirLinearDrag;
		public float JumpForce;
		public float FallMultiplier;
		public float LowMultiplier;
		public float JumpDelay;
		public int ExtraJumps;
		public float Damage;
		public float DamageRadius;
		public float VelocityPower;
	}
}