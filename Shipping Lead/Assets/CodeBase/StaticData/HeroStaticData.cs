using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "HeroData", menuName = "StaticData/Hero")]
	public class HeroStaticData : ScriptableObject
	{
		[Header("HP")]
		[Range(1, 100)]
		public int Hp;

		[Header("Damage")]
		[Range(1, 50)]
		public float Damage;
		[Range(0.5f, 5)]
		public float Cleavage;

		[Header("Movement")]
		[Range(1, 100)]
		public float MovementAccelaration;
		[Range(1, 10)]
		public float MovementDeacclaration;
		[Range(1, 100)]
		public float MaxMoveSpeed;
		[Range(1, 5)]
		public float VelocityPower;

		[Header("Jump")]
		[Range(5, 25)]
		public float JumpForce;
		[Range(1, 10)]
		public float AirLinearDrag;
		[Range(5, 25)]
		public float FallMultiplier;
		[Range(5, 25)]
		public float LowJumpMultiplier;
		[Range(1, 5)]
		public int ExtraJumps;


		public GameObject Prefab;
	}
}